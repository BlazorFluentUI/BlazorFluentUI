using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class DetailsList<TItem> : FluentUIComponentBase, IAsyncDisposable
    {
        [Parameter]
        public CheckboxVisibility CheckboxVisibility { get; set; } = CheckboxVisibility.OnHover;

        [Parameter]
        public IEnumerable<DetailsRowColumn<TItem>> Columns { get; set; }

        [Parameter]
        public bool Compact { get; set; }

        [Parameter]
        public bool DisableSelectionZone { get; set; }

        [Parameter]
        public bool EnterModalSelectionOnTouch { get; set; }

        [Parameter]
        public RenderFragment FooterTemplate { get; set; }

        [Parameter]
        public Func<TItem, object> GetKey { get; set; }

        [Parameter]
        public Func<TItem, string> GroupTitleSelector { get; set; }

        [Parameter]
        public RenderFragment HeaderTemplate { get; set; }

        [Parameter]
        public bool IsHeaderVisible { get; set; } = true;

        [Parameter]
        public bool IsVirtualizing { get; set; } = true;

        // Selection.SetItems will crash if a null source is passed in... but we need to clear it if the source is null, so pass in an empty list.
        // Also, the internal List<TItem> will also crash if a null itemssource is passed to it.
        private IList<TItem> _itemsSource = new System.Collections.Generic.List<TItem>();
        [Parameter]
        public IList<TItem> ItemsSource { get => _itemsSource; set { if (value == null) { _itemsSource = new System.Collections.Generic.List<TItem>(); } else { _itemsSource = value; } } }

        [Parameter]
        public DetailsListLayoutMode LayoutMode { get; set; }

        [Parameter]
        public EventCallback<TItem> OnItemContextMenu { get; set; }

        [Parameter]
        public EventCallback<TItem> OnItemInvoked { get; set; }

        [Parameter]
        public EventCallback<ColumnResizedArgs<TItem>> OnColumnResized { get; set; }

        [Parameter]
        public RenderFragment<IndexedItem<TItem>> RowTemplate { get; set; }


        [Parameter]
        public Selection<TItem> Selection { get; set; } 

        [Parameter]
        public SelectionMode SelectionMode { get; set; }

        [Parameter]
        public bool SelectionPreservedOnEmptyClick { get; set; }

        [Parameter]
        public Func<TItem, IEnumerable<TItem>> SubGroupSelector { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }


        //State
        int focusedItemIndex;
        double _lastWidth = -1;
        SelectionMode _lastSelectionMode;
        Viewport _lastViewport;
        Viewport _viewport;
        private IEnumerable<DetailsRowColumn<TItem>> _adjustedColumns = Enumerable.Empty<DetailsRowColumn<TItem>>();
        const double MIN_COLUMN_WIDTH = 100;


        Dictionary<string, double> _columnOverrides = new Dictionary<string, double>();

        private Selection<TItem> _selection = new Selection<TItem>();

        GroupedList<TItem,object> groupedList;
        List<TItem> list;
        SelectionZone<TItem> selectionZone;

        protected bool isAllSelected;
        private bool shouldRender = true;

        private IReadOnlyDictionary<string, object> lastParameters = null;

        protected SelectAllVisibility selectAllVisibility = SelectAllVisibility.None;
        private DotNetObjectReference<DetailsList<TItem>> selfReference;
        private int _viewportRegistration;

        public DetailsList()
        {
            Selection = new Selection<TItem>();

        }

        public void ForceUpdate()
        {
            groupedList?.ForceUpdate();
        }

        protected override bool ShouldRender()
        {
            //if (!shouldRender)
            //{
            //    shouldRender = true;
            //    return false;
            //}
            return true;
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {

            if (_viewport != null && _viewport != _lastViewport)
            {
                AdjustColumns(
                    parameters.GetValueOrDefault<IEnumerable<TItem>>("ItemsSource"),
                    parameters.GetValueOrDefault<DetailsListLayoutMode>("LayoutMode"),
                    parameters.GetValueOrDefault<SelectionMode>("SelectionMode"),
                    parameters.GetValueOrDefault<CheckboxVisibility>("CheckboxVisibility"),
                    parameters.GetValueOrDefault<IEnumerable<DetailsRowColumn<TItem>>>("Columns"),
                    true
                    );
            }

            var selectionMode = parameters.GetValueOrDefault<SelectionMode>("SelectionMode");
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
                        Selection = new Selection<TItem>();
                        Selection.GetKey = GetKey;
                    }
                    _selection = Selection;

                    if (Selection.GetKey == null)
                        Selection.GetKey = GetKey;

                    Selection.SetItems(ItemsSource);
                }
                else
                {
                    if (Selection.GetItems() != ItemsSource)
                        Selection.SetItems(ItemsSource, false);
                }

                if (Selection.SelectionMode != SelectionMode)
                    Selection.SelectionMode = SelectionMode;
            }

            await base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                selfReference = DotNetObjectReference.Create(this);
                _viewportRegistration = await JSRuntime.InvokeAsync<int>("FluentUIBaseComponent.addViewport", selfReference, RootElementReference, true);
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private void OnHeaderKeyDown(KeyboardEventArgs keyboardEventArgs)
        {
            // this was attached in the ms-DetailsList-headerWrapper div.  When holding Ctrl nothing happens (since it's a meta key), but if you click while holding Ctrl, a large number of keydown events is sent to this handler and freezes the UI. 
        }

        private void OnContentKeyDown(KeyboardEventArgs keyboardEventArgs)
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
            //Debug.WriteLine($"Viewport changed: {viewport.ScrollWidth}");
            if (_viewport != null)
            {
                AdjustColumns(ItemsSource, LayoutMode, SelectionMode, CheckboxVisibility, Columns, true);
                InvokeAsync(StateHasChanged);
            }
        }

        private void AdjustColumns(IEnumerable<TItem> newItems, DetailsListLayoutMode newLayoutMode, SelectionMode newSelectionMode, CheckboxVisibility newCheckboxVisibility, IEnumerable<DetailsRowColumn<TItem>> newColumns, bool forceUpdate, int resizingColumnIndex = -1)
        {
            _adjustedColumns = GetAdjustedColumns(newItems, newLayoutMode, newSelectionMode, newCheckboxVisibility, newColumns, forceUpdate, resizingColumnIndex);
        }

        private IEnumerable<DetailsRowColumn<TItem>> GetAdjustedColumns(IEnumerable<TItem> newItems, DetailsListLayoutMode newLayoutMode, SelectionMode newSelectionMode, CheckboxVisibility newCheckboxVisibility, IEnumerable<DetailsRowColumn<TItem>> newColumns, bool forceUpdate, int resizingColumnIndex)
        {
            var columns = Columns.EmptyIfNull();
            var lastWidth = _lastWidth;
            var lastSelectionMode = _lastSelectionMode;

            if (!forceUpdate && _lastViewport.Width == _viewport.Width && SelectionMode == newSelectionMode && (Columns == null || newColumns == Columns))
                return Enumerable.Empty<DetailsRowColumn<TItem>>();

            // skipping default column builder... user must provide columns always

            IEnumerable<DetailsRowColumn<TItem>> adjustedColumns = null;

            if (LayoutMode == DetailsListLayoutMode.FixedColumns)
            {
                adjustedColumns = GetFixedColumns(newColumns);

                foreach (var col in adjustedColumns)
                    _columnOverrides[col.Key] = col.CalculatedWidth;
            }
            else
            {
                if (resizingColumnIndex != -1)
                {
                    adjustedColumns = GetJustifiedColumnsAfterResize(newColumns, newCheckboxVisibility, newSelectionMode, _viewport.Width, resizingColumnIndex);
                }
                else
                {
                    adjustedColumns = GetJustifiedColumns(newColumns, newCheckboxVisibility, newSelectionMode, _viewport.Width, resizingColumnIndex);
                }

                foreach (var col in adjustedColumns)
                {
                    _columnOverrides[col.Key] = col.CalculatedWidth;
                }
            }



            return adjustedColumns;
        }

        private IEnumerable<DetailsRowColumn<TItem>> GetFixedColumns(IEnumerable<DetailsRowColumn<TItem>> newColumns)
        {
            foreach (var col in newColumns)
            {
                col.CalculatedWidth = !double.IsNaN(col.MaxWidth) ? col.MaxWidth : (!double.IsNaN(col.MinWidth) ? col.MinWidth : MIN_COLUMN_WIDTH);
            }
            return newColumns;
        }

        private IEnumerable<DetailsRowColumn<TItem>> GetJustifiedColumnsAfterResize(IEnumerable<DetailsRowColumn<TItem>> newColumns, CheckboxVisibility newCheckboxVisibility, SelectionMode newSelectionMode, double viewportWidth, int resizingColumnIndex)
        {
            var fixedColumns = newColumns.Take(resizingColumnIndex);
            foreach (var col in fixedColumns)
            {
                if (_columnOverrides.TryGetValue(col.Key, out var overridenWidth))
                    col.CalculatedWidth = overridenWidth;
                else
                    col.CalculatedWidth = double.NaN;
            }

            int count = 0;
            var fixedWidth = fixedColumns.Aggregate<DetailsRowColumn<TItem>, double, double>(0, (total, column) => total + GetPaddedWidth(column, ++count == 0), x => x);

            var remainingColumns = newColumns.Skip(resizingColumnIndex).Take(newColumns.Count() - resizingColumnIndex);
            var remainingWidth = viewportWidth - fixedWidth;

            var adjustedColumns = GetJustifiedColumns(remainingColumns, newCheckboxVisibility, newSelectionMode, remainingWidth, resizingColumnIndex);

            return Enumerable.Concat(fixedColumns, adjustedColumns);
        }

        private IEnumerable<DetailsRowColumn<TItem>> GetJustifiedColumns(IEnumerable<DetailsRowColumn<TItem>> newColumns, CheckboxVisibility newCheckboxVisibility, SelectionMode newSelectionMode, double viewportWidth, int resizingColumnIndex)
        {
            var rowCheckWidth = newSelectionMode != SelectionMode.None && newCheckboxVisibility != CheckboxVisibility.Hidden ? 48 : 0;  //DetailsRowCheckbox width
            var groupExpandedWidth = 0; //skipping this for now.
            double totalWidth = 0;
            var availableWidth = viewportWidth - (rowCheckWidth + groupExpandedWidth);
            int count = 0;

            System.Collections.Generic.List<DetailsRowColumn<TItem>> adjustedColumns = new System.Collections.Generic.List<DetailsRowColumn<TItem>>();
            foreach (var col in newColumns)
            {
                adjustedColumns.Add(col);
                col.CalculatedWidth = !double.IsNaN(col.MinWidth) ? col.MinWidth : 100;
                if (_columnOverrides.TryGetValue(col.Key, out var overridenWidth))
                    col.CalculatedWidth = overridenWidth;

                var isFirst = count + resizingColumnIndex == 0;
                totalWidth += GetPaddedWidth(col, isFirst);
            }

            var lastIndex = adjustedColumns.Count() - 1;

            // Shrink or remove collapsable columns.
            while (lastIndex > 0 && totalWidth > availableWidth)
            {
                var col = adjustedColumns.ElementAt(lastIndex);
                var minWidth = !double.IsNaN(col.MinWidth) ? col.MinWidth : 100;
                var overflowWidth = totalWidth - availableWidth;

                if (col.CalculatedWidth - minWidth >= overflowWidth || !col.IsCollapsible)
                {
                    var originalWidth = col.CalculatedWidth;
                    col.CalculatedWidth = Math.Max(col.CalculatedWidth - overflowWidth, minWidth);
                    totalWidth -= originalWidth - col.CalculatedWidth;
                }
                else
                {
                    totalWidth -= GetPaddedWidth(col, false);
                    adjustedColumns.RemoveRange(lastIndex, 1);
                }
                lastIndex--;
            }

            //Then expand columns starting at the beginning, until we've filled the width.
            for (var i = 0; i < adjustedColumns.Count && totalWidth < availableWidth; i++)
            {
                var col = adjustedColumns[i];
                var isLast = i == adjustedColumns.Count - 1;
                var hasOverrides = _columnOverrides.TryGetValue(col.Key, out var overrides);
                if (hasOverrides && !isLast)
                    continue;

                var spaceLeft = availableWidth - totalWidth;
                double increment = 0;
                if (isLast)
                    increment = spaceLeft;
                else
                {
                    var maxWidth = col.MaxWidth;
                    var minWidth = !double.IsNaN(col.MinWidth) ? col.MinWidth : (!double.IsNaN(col.MaxWidth) ? col.MaxWidth : 100);
                    increment = !double.IsNaN(maxWidth) ? Math.Min(spaceLeft, maxWidth - minWidth) : spaceLeft;
                }

                col.CalculatedWidth += increment;
                totalWidth += increment;
            }

            return adjustedColumns;
        }

        private double GetPaddedWidth(DetailsRowColumn<TItem> column, bool isFirst)
        {
            return column.CalculatedWidth +
                    DetailsRow<TItem>.CELL_LEFT_PADDING +
                    DetailsRow<TItem>.CELL_RIGHT_PADDING +
                    (column.IsPadded ? DetailsRow<TItem>.CELL_EXTRA_RIGHT_PADDING : 0);
        }

        private void OnColumnResizedInternal(ColumnResizedArgs<TItem> columnResizedArgs)
        {
            OnColumnResized.InvokeAsync(columnResizedArgs);

            _columnOverrides[columnResizedArgs.Column.Key] = columnResizedArgs.NewWidth;
            AdjustColumns(ItemsSource, LayoutMode, SelectionMode, CheckboxVisibility, Columns, true, columnResizedArgs.ColumnIndex);
        }

        private void OnColumnAutoResized(ItemContainer<DetailsRowColumn<TItem>> itemContainer)
        {
            // TO-DO - will require measuring row cells, jsinterop
        }

        public async ValueTask DisposeAsync()
        {
            if (_viewportRegistration != -1)
            {
                await JSRuntime.InvokeVoidAsync("FluentUIBaseComponent.removeViewport", _viewportRegistration);
            }
            selfReference?.Dispose();
        }
    }
}
