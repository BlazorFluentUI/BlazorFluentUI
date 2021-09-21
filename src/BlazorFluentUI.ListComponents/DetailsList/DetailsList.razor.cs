using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BlazorFluentUI.Lists
{
    public partial class DetailsList<TItem> : FluentUIComponentBase, IAsyncDisposable
    {
        [Parameter]
        public CheckboxVisibility CheckboxVisibility { get; set; } = CheckboxVisibility.OnHover;

        [Parameter]
        public IEnumerable<IDetailsRowColumn<TItem>>? Columns { get; set; }

        [Parameter]
        public bool Compact { get; set; }

        [Parameter]
        public bool DisableSelectionZone { get; set; }

        [Parameter]
        public bool EnterModalSelectionOnTouch { get; set; }

        [Parameter]
        public RenderFragment? FooterTemplate { get; set; }

        [Parameter]
        public Func<TItem, object>? GetKey { get; set; }

        [Parameter]
        public Func<TItem, string>? GroupTitleSelector { get; set; }

        [Parameter]
        public RenderFragment? HeaderTemplate { get; set; }

        [Parameter]
        public bool IsHeaderVisible { get; set; } = true;

        [Parameter]
        public bool IsVirtualizing { get; set; } = true;

        // Selection.SetItems will crash if a null source is passed in... but we need to clear it if the source is null, so pass in an empty list.
        // Also, the internal List<TItem> will also crash if a null itemssource is passed to it.
        private IList<TItem> _itemsSource = new System.Collections.Generic.List<TItem>();
        [Parameter]
        public IList<TItem> ItemsSource
        {
            get => _itemsSource;
            set
            {
                if (value == null)
                {
                    _itemsSource = new System.Collections.Generic.List<TItem>();
                }
                else
                {
                    _itemsSource = value;
                }
            }
        }

        [Parameter]
        public DetailsListLayoutMode LayoutMode { get; set; }

        [Parameter]
        public EventCallback<TItem> OnItemContextMenu { get; set; }

        [Parameter]
        public EventCallback<TItem> OnItemInvoked { get; set; }

        [Parameter]
        public EventCallback<ColumnResizedArgs<TItem>> OnColumnResized { get; set; }

        [Parameter]
        public RenderFragment<IndexedItem<TItem>>? RowTemplate { get; set; }


        [Parameter]
        public Selection<TItem>? Selection { get; set; }

        [Parameter]
        public SelectionMode SelectionMode { get; set; }

        [Parameter]
        public bool SelectionPreservedOnEmptyClick { get; set; }

        [Parameter]
        public Func<TItem, IEnumerable<TItem>>? SubGroupSelector { get; set; }

        //State
        //int focusedItemIndex;
        //double _lastWidth = -1;
        //SelectionMode _lastSelectionMode;
        Viewport? _lastViewport;
        Viewport? _viewport;
        private IEnumerable<IDetailsRowColumn<TItem?>> _adjustedColumns = Enumerable.Empty<IDetailsRowColumn<TItem?>>();
        const double MIN_COLUMN_WIDTH = 100;
        readonly Dictionary<string, double> _columnOverrides = new();

        private Selection<TItem> _selection = new();

        GroupedList<TItem, object>? groupedList;
        //List<TItem> list;
        SelectionZone<TItem>? selectionZone;

        protected bool isAllSelected;
        private bool shouldRender = true;

        //private IReadOnlyDictionary<string, object> lastParameters = null;

        protected SelectAllVisibility selectAllVisibility = SelectAllVisibility.None;
        private DotNetObjectReference<DetailsList<TItem>>? selfReference;
        private int _viewportRegistration = -1;

        public DetailsList()
        {
            Selection = new Selection<TItem>();

        }

        public void ForceUpdate()
        {
            //groupedList?.ForceUpdate();
        }

        protected override bool ShouldRender()
        {
            if (shouldRender)
            {
                shouldRender = false;
                Debug.WriteLine("should render true!");
                return true;
            }
            Debug.WriteLine("should render false!");
            return false;
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {

            if (_viewport != null && _viewport != _lastViewport)
            {
                AdjustColumns(
                    parameters.GetValueOrDefault<IEnumerable<TItem>>("ItemsSource")!,
                    parameters.GetValueOrDefault<DetailsListLayoutMode>("LayoutMode"),
                    parameters.GetValueOrDefault<SelectionMode>("SelectionMode"),
                    parameters.GetValueOrDefault<CheckboxVisibility>("CheckboxVisibility"),
                    parameters.GetValueOrDefault<IEnumerable<IDetailsRowColumn<TItem>>>("Columns")!,
                    true
                    );
            }

            SelectionMode selectionMode = parameters.GetValueOrDefault<SelectionMode>("SelectionMode");
            if (selectionMode == SelectionMode.None)
            {
                selectAllVisibility = SelectAllVisibility.None;
            }
            else if (selectionMode == SelectionMode.Single)
            {
                selectAllVisibility = SelectAllVisibility.Hidden;
            }
            else if (selectionMode == SelectionMode.Multiple)
            {
                //disable if collapsed groups
                //TBD!

                selectAllVisibility = SelectAllVisibility.Visible;
            }

            if (parameters.GetValueOrDefault<CheckboxVisibility>("CheckboxVisibility") == CheckboxVisibility.Hidden)
            {
                selectAllVisibility = SelectAllVisibility.None;
            }

            //should render checks
            if (parameters.GetValueOrDefault<CheckboxVisibility>("CheckboxVisibility") != CheckboxVisibility ||
                parameters.GetValueOrDefault<bool>("Compact") != Compact ||
                parameters.GetValueOrDefault<bool>("EnterModalSelectionOnTouch") != EnterModalSelectionOnTouch ||
                parameters.GetValueOrDefault<bool>("DisableSelectionZone") != DisableSelectionZone ||
                parameters.GetValueOrDefault<bool>("IsHeaderVisible") != IsHeaderVisible ||
                parameters.GetValueOrDefault<bool>("IsVirtualizing") != IsVirtualizing ||
                parameters.GetValueOrDefault<DetailsListLayoutMode>("LayoutMode") != LayoutMode ||
                parameters.GetValueOrDefault<SelectionMode>("SelectionMode") != SelectionMode ||
                parameters.GetValueOrDefault<bool>("SelectionPreservedOnEmptyClick") != SelectionPreservedOnEmptyClick
                )
            {
                shouldRender = true;
            }
            return base.SetParametersAsync(parameters);
        }

        //private void DetailsList_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        //{
        //    // collection changed... rerun selection stuff without clearing selection
        //    //Selection?.SetItems(ItemsSource, false);
        //}

        protected override async Task OnParametersSetAsync()
        {
            if (!DisableSelectionZone)
            {
                if (Selection != _selection)
                {
                    if (Selection == null)
                    {
                        Selection = new Selection<TItem>
                        {
                            GetKey = GetKey!
                        };
                    }
                    _selection = Selection!;

                    if (Selection.GetKey == null)
                        Selection.GetKey = GetKey!;
                    if (ItemsSource != null)
                        Selection.SetItems(ItemsSource);
                }
                else
                {
                    if (ItemsSource != null && Selection.GetItems() != ItemsSource)
                        Selection.SetItems(ItemsSource, false);
                }

                if (Selection.SelectionMode != SelectionMode)
                    Selection.SelectionMode = SelectionMode;
            }

            await base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (baseModule == null)
                baseModule = await JSRuntime!.InvokeAsync<IJSObjectReference>("import", BasePath);

            if (firstRender)
            {
                selfReference = DotNetObjectReference.Create(this);
                _viewportRegistration = await baseModule.InvokeAsync<int>("addViewport", cancellationTokenSource.Token, selfReference, RootElementReference);
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private static void OnHeaderKeyDown(KeyboardEventArgs keyboardEventArgs)
        {
            // this was attached in the ms-DetailsList-headerWrapper div.  When holding Ctrl nothing happens (since it's a meta key), but if you click while holding Ctrl, a large number of keydown events is sent to this handler and freezes the UI.
        }

        private static void OnContentKeyDown(KeyboardEventArgs keyboardEventArgs)
        {
            // this was attached in the ms-DetailsList-contentWrapper div.  When holding Ctrl nothing happens (since it's a meta key), but if you click while holding Ctrl, a large number of keydown events is sent to this handler and freezes the UI.
        }


        //private void OnAllSelected()
        //{

        //}

        [JSInvokable]
        public void ViewportChanged(Viewport viewport)
        {
            _lastViewport = _viewport;
            _viewport = viewport;
            if (_viewport != null)
            {
                AdjustColumns(ItemsSource, LayoutMode, SelectionMode, CheckboxVisibility, Columns!, true);
                Debug.WriteLine($"Viewport changed: {viewport.Width}");
                shouldRender = true;
                InvokeAsync(StateHasChanged);
            }
        }

        private void AdjustColumns(IEnumerable<TItem?> newItems, DetailsListLayoutMode newLayoutMode, SelectionMode newSelectionMode, CheckboxVisibility newCheckboxVisibility, IEnumerable<IDetailsRowColumn<TItem?>> newColumns, bool forceUpdate, int resizingColumnIndex = -1)
        {
            _adjustedColumns = GetAdjustedColumns(newItems, newLayoutMode, newSelectionMode, newCheckboxVisibility, newColumns, forceUpdate, resizingColumnIndex);
            shouldRender = resizingColumnIndex > -1;
        }

        private IEnumerable<IDetailsRowColumn<TItem?>> GetAdjustedColumns(IEnumerable<TItem?> newItems, DetailsListLayoutMode newLayoutMode, SelectionMode newSelectionMode, CheckboxVisibility newCheckboxVisibility, IEnumerable<IDetailsRowColumn<TItem?>> newColumns, bool forceUpdate, int resizingColumnIndex)
        {

            if (!forceUpdate && _lastViewport?.Width == _viewport?.Width && SelectionMode == newSelectionMode && (Columns == null || newColumns == Columns))
                return Enumerable.Empty<IDetailsRowColumn<TItem?>>();

            // skipping default column builder... user must provide columns always

            IEnumerable<IDetailsRowColumn<TItem?>> adjustedColumns;

            if (LayoutMode == DetailsListLayoutMode.FixedColumns)
            {
                adjustedColumns = DetailsList<TItem?>.GetFixedColumns(newColumns);

                foreach (IDetailsRowColumn<TItem?> col in adjustedColumns)
                    _columnOverrides[col.Key!] = col.CalculatedWidth;
            }
            else
            {
                if (resizingColumnIndex != -1)
                {
                    adjustedColumns = GetJustifiedColumnsAfterResize(newColumns, newCheckboxVisibility, newSelectionMode, _viewport!.Width, resizingColumnIndex);
                }
                else
                {
                    adjustedColumns = GetJustifiedColumns(newColumns, newCheckboxVisibility, newSelectionMode, _viewport!.Width, resizingColumnIndex);
                }

                foreach (IDetailsRowColumn<TItem?> col in adjustedColumns)
                {
                    _columnOverrides[col.Key!] = col.CalculatedWidth;
                }
            }



            return adjustedColumns;
        }

        private static IEnumerable<IDetailsRowColumn<TItem?>> GetFixedColumns(IEnumerable<IDetailsRowColumn<TItem?>> newColumns)
        {
            foreach (IDetailsRowColumn<TItem?> col in newColumns)
            {
                col.CalculatedWidth = !double.IsNaN(col.MaxWidth) ? col.MaxWidth : (!double.IsNaN(col.MinWidth) ? col.MinWidth : MIN_COLUMN_WIDTH);
            }
            return newColumns;
        }

        private IEnumerable<IDetailsRowColumn<TItem?>> GetJustifiedColumnsAfterResize(IEnumerable<IDetailsRowColumn<TItem?>> newColumns, CheckboxVisibility newCheckboxVisibility, SelectionMode newSelectionMode, double viewportWidth, int resizingColumnIndex)
        {
            IEnumerable<IDetailsRowColumn<TItem?>> fixedColumns = newColumns.Take(resizingColumnIndex);
            foreach (IDetailsRowColumn<TItem?> col in fixedColumns)
            {
                if (_columnOverrides.TryGetValue(col.Key!, out double overridenWidth))
                    col.CalculatedWidth = overridenWidth;
                else
                    col.CalculatedWidth = double.NaN;
            }

            double fixedWidth = fixedColumns.Aggregate<IDetailsRowColumn<TItem?>, double, double>(0, (total, column) => total + DetailsList<TItem?>.GetPaddedWidth(column), x => x);

            IEnumerable<IDetailsRowColumn<TItem?>>? remainingColumns = newColumns.Skip(resizingColumnIndex).Take(newColumns.Count() - resizingColumnIndex);
            double remainingWidth = viewportWidth - fixedWidth;

            IEnumerable<IDetailsRowColumn<TItem?>>? adjustedColumns = GetJustifiedColumns(remainingColumns, newCheckboxVisibility, newSelectionMode, remainingWidth, resizingColumnIndex);

            return Enumerable.Concat(fixedColumns, adjustedColumns);
        }

        private IEnumerable<IDetailsRowColumn<TItem?>> GetJustifiedColumns(IEnumerable<IDetailsRowColumn<TItem?>> newColumns, CheckboxVisibility newCheckboxVisibility, SelectionMode newSelectionMode, double viewportWidth, int resizingColumnIndex)
        {
            int rowCheckWidth = newSelectionMode != SelectionMode.None && newCheckboxVisibility != CheckboxVisibility.Hidden ? 48 : 0;  //DetailsRowCheckbox width
            int groupExpandedWidth = 0; //skipping this for now.
            double totalWidth = 0;
            double availableWidth = viewportWidth - (rowCheckWidth + groupExpandedWidth);

            System.Collections.Generic.List<IDetailsRowColumn<TItem?>> adjustedColumns = new();
            foreach (IDetailsRowColumn<TItem?> col in newColumns)
            {
                adjustedColumns.Add(col);
                col.CalculatedWidth = !double.IsNaN(col.MinWidth) ? col.MinWidth : 100;
                if (_columnOverrides.TryGetValue(col.Key!, out double overridenWidth))
                    col.CalculatedWidth = overridenWidth;

                totalWidth += DetailsList<TItem>.GetPaddedWidth(col);
            }

            int lastIndex = adjustedColumns.Count - 1;

            // Shrink or remove collapsable columns.
            while (lastIndex > 0 && totalWidth > availableWidth)
            {
                IDetailsRowColumn<TItem?> col = adjustedColumns.ElementAt(lastIndex);
                double minWidth = !double.IsNaN(col.MinWidth) ? col.MinWidth : 100;
                double overflowWidth = totalWidth - availableWidth;

                if (col.CalculatedWidth - minWidth >= overflowWidth || !col.IsCollapsible)
                {
                    double originalWidth = col.CalculatedWidth;
                    col.CalculatedWidth = Math.Max(col.CalculatedWidth - overflowWidth, minWidth);
                    totalWidth -= originalWidth - col.CalculatedWidth;
                }
                else
                {
                    totalWidth -= DetailsList<TItem>.GetPaddedWidth(col);
                    adjustedColumns.RemoveRange(lastIndex, 1);
                }
                lastIndex--;
            }

            //Then expand columns starting at the beginning, until we've filled the width.
            for (int i = 0; i < adjustedColumns.Count && totalWidth < availableWidth; i++)
            {
                IDetailsRowColumn<TItem?> col = adjustedColumns[i];
                bool isLast = i == adjustedColumns.Count - 1;
                bool hasOverrides = _columnOverrides.TryGetValue(col.Key!, out _);
                if (hasOverrides && !isLast)
                    continue;

                double spaceLeft = availableWidth - totalWidth;
                double increment;
                if (isLast)
                    increment = spaceLeft;
                else
                {
                    double maxWidth = col.MaxWidth;
                    double minWidth = !double.IsNaN(col.MinWidth) ? col.MinWidth : (!double.IsNaN(col.MaxWidth) ? col.MaxWidth : 100);
                    increment = !double.IsNaN(maxWidth) ? Math.Min(spaceLeft, maxWidth - minWidth) : spaceLeft;
                }

                col.CalculatedWidth += increment;
                totalWidth += increment;
            }

            return adjustedColumns;
        }

        private static double GetPaddedWidth(IDetailsRowColumn<TItem?> column)
        {
            return column.CalculatedWidth +
                    DetailsRow<TItem>.CELL_LEFT_PADDING +
                    DetailsRow<TItem>.CELL_RIGHT_PADDING +
                    (column.IsPadded ? DetailsRow<TItem>.CELL_EXTRA_RIGHT_PADDING : 0);
        }

        private void OnColumnResizedInternal(ColumnResizedArgs<TItem> columnResizedArgs)
        {
            OnColumnResized.InvokeAsync(columnResizedArgs);

            _columnOverrides[columnResizedArgs.Column.Key!] = columnResizedArgs.NewWidth;
            AdjustColumns(ItemsSource, LayoutMode, SelectionMode, CheckboxVisibility, Columns!, true, columnResizedArgs.ColumnIndex);
        }

        private static void OnColumnAutoResized(ItemContainer<IDetailsRowColumn<TItem>> itemContainer)
        {
            // TO-DO - will require measuring row cells, jsinterop
        }

        public override async ValueTask DisposeAsync()
        {
            try
            {
                if (_viewportRegistration != -1)
                {
                    await baseModule!.InvokeVoidAsync("removeViewport", _viewportRegistration);
                    _viewportRegistration = -1;
                }
                selfReference?.Dispose();
                if (baseModule != null)
                    await baseModule.DisposeAsync();

                await base.DisposeAsync();
            }
            catch (TaskCanceledException)
            {
            }
        }
    }
}
