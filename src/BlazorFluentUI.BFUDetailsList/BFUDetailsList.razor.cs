using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class BFUDetailsList<TItem> : BFUComponentBase
    {
        [Parameter]
        public CheckboxVisibility CheckboxVisibility { get; set; } = CheckboxVisibility.OnHover;

        [Parameter]
        public IEnumerable<BFUDetailsRowColumn<TItem>> Columns { get; set; }

        [Parameter]
        public bool Compact { get; set; }

        [Parameter]
        public bool DisableSelectionZone { get; set; }

        [Parameter]
        public bool EnterModalSelectionOnTouch { get; set; }

        [Parameter]
        public RenderFragment FooterTemplate { get; set; }

        [Parameter]
        public Func<TItem, string> GroupTitleSelector { get; set; }

        [Parameter]
        public RenderFragment HeaderTemplate { get; set; }

        [Parameter]
        public bool IsHeaderVisible { get; set; } = true;

        [Parameter]
        public bool IsVirtualizing { get; set; } = true;

        [Parameter]
        public IEnumerable<TItem> ItemsSource { get; set; }

        [Parameter]
        public DetailsListLayoutMode LayoutMode { get; set; }

        [Parameter]
        public EventCallback<TItem> OnItemContextMenu { get; set; }

        [Parameter]
        public EventCallback<TItem> OnItemInvoked { get; set; }

        [Parameter]
        public EventCallback<ColumnResizedArgs<TItem>> OnColumnResized { get; set; }

        [Parameter]
        public RenderFragment<TItem> RowTemplate { get; set; }

        [Parameter]
        public Selection<TItem> Selection { get; set; } = new Selection<TItem>();

        [Parameter]
        public EventCallback<Selection<TItem>> SelectionChanged { get; set; }

        [Parameter]
        public SelectionMode SelectionMode { get; set; }

        [Parameter]
        public bool SelectionPreservedOnEmptyClick { get; set; }

        [Parameter]
        public Func<TItem, IEnumerable<TItem>> SubGroupSelector { get; set; }


        //State
        int focusedItemIndex;
        double _lastWidth = -1;
        SelectionMode _lastSelectionMode;
        Viewport _lastViewport;
        Viewport _viewport;
        private IEnumerable<BFUDetailsRowColumn<TItem>> _adjustedColumns = Enumerable.Empty<BFUDetailsRowColumn<TItem>>();
        const double MIN_COLUMN_WIDTH = 100;

        Dictionary<string, double> _columnOverrides = new Dictionary<string, double>();

        BFUGroupedList<TItem> groupedList;
        BFUList<TItem> list;
        BFUSelectionZone<TItem> selectionZone;

        protected bool isAllSelected;
        private bool shouldRender = true;

        private IReadOnlyDictionary<string, object> lastParameters = null;

        protected SelectAllVisibility selectAllVisibility = SelectAllVisibility.None;

        public void ForceUpdate()
        {
            groupedList?.ForceUpdate();
        }

        protected override bool ShouldRender()
        {
            if (!shouldRender)
            {
                shouldRender = true;
                return false;
            }
            return true;
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            shouldRender = false;

            var dictParameters = parameters.ToDictionary();
            if (lastParameters == null)
            {
                shouldRender = true;
            }
            else
            {
                var differences = dictParameters.Where(entry =>
                {
                    return !lastParameters[entry.Key].Equals(entry.Value);
                }
                ).ToDictionary(entry => entry.Key, entry => entry.Value);

                if (differences.Count > 0)
                {
                    shouldRender = true;
                }
            }
            lastParameters = dictParameters;

            if (_viewport != null && _viewport != _lastViewport)
            {
                AdjustColumns(
                    parameters.GetValueOrDefault<IEnumerable<TItem>>("ItemsSource"),
                    parameters.GetValueOrDefault<DetailsListLayoutMode>("LayoutMode"),
                    parameters.GetValueOrDefault<SelectionMode>("SelectionMode"),
                    parameters.GetValueOrDefault<CheckboxVisibility>("CheckboxVisibility"),
                    parameters.GetValueOrDefault<IEnumerable<BFUDetailsRowColumn<TItem>>>("Columns"),
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


        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            return base.OnAfterRenderAsync(firstRender);
        }

        private void OnHeaderKeyDown(KeyboardEventArgs keyboardEventArgs)
        {
            // this was attached in the ms-DetailsList-headerWrapper div.  When holding Ctrl nothing happens (since it's a meta key), but if you click while holding Ctrl, a large number of keydown events is sent to this handler and freezes the UI. 
        }

        private void OnContentKeyDown(KeyboardEventArgs keyboardEventArgs)
        {
            // this was attached in the ms-DetailsList-contentWrapper div.  When holding Ctrl nothing happens (since it's a meta key), but if you click while holding Ctrl, a large number of keydown events is sent to this handler and freezes the UI. 
        }

        private bool ShouldAllBeSelected()
        {
            if (SubGroupSelector == null)
            {
                return Selection.SelectedItems.Count() == ItemsSource.Count() && ItemsSource.Any();
            }
            else
            {
                //source is grouped... need to recursively select them all.
                var flattenedItems = ItemsSource?.SelectManyRecursive(x => SubGroupSelector(x));
                if (flattenedItems == null)
                    return false;

                return flattenedItems.Count() == Selection.SelectedItems.Count() && flattenedItems.Any();
            }
        }

        private void OnAllSelected()
        {
            if (SubGroupSelector == null)
            {
                if (Selection.SelectedItems.Count() != this.ItemsSource.Count())
                {
                    selectionZone.AddItems(ItemsSource);
                }
                else
                {
                    selectionZone.ClearSelection();
                }
            }
            else
            {
                //source is grouped... need to recursively select them all.
                var flattenedItems = this.ItemsSource?.SelectManyRecursive(x => SubGroupSelector(x));
                if (flattenedItems.Count() != Selection.SelectedItems.Count())
                {
                    selectionZone.AddItems(flattenedItems);
                }
                else
                {
                    selectionZone.ClearSelection();
                }
            }
        }

        private void ViewportChangedHandler(Viewport viewport)
        {
            _lastViewport = _viewport;
            _viewport = viewport;
            //Debug.WriteLine($"Viewport changed: {viewport.ScrollWidth}");
            if (_viewport != null)
                AdjustColumns(ItemsSource, LayoutMode, SelectionMode, CheckboxVisibility, Columns, true);
        }

        private void AdjustColumns(IEnumerable<TItem> newItems, DetailsListLayoutMode newLayoutMode, SelectionMode newSelectionMode, CheckboxVisibility newCheckboxVisibility, IEnumerable<BFUDetailsRowColumn<TItem>> newColumns, bool forceUpdate, int resizingColumnIndex = -1)
        {
            _adjustedColumns = GetAdjustedColumns(newItems, newLayoutMode, newSelectionMode, newCheckboxVisibility, newColumns, forceUpdate, resizingColumnIndex);
        }

        private IEnumerable<BFUDetailsRowColumn<TItem>> GetAdjustedColumns(IEnumerable<TItem> newItems, DetailsListLayoutMode newLayoutMode, SelectionMode newSelectionMode, CheckboxVisibility newCheckboxVisibility, IEnumerable<BFUDetailsRowColumn<TItem>> newColumns, bool forceUpdate, int resizingColumnIndex)
        {
            var columns = Columns.EmptyIfNull();
            var lastWidth = _lastWidth;
            var lastSelectionMode = _lastSelectionMode;

            if (!forceUpdate && _lastViewport.ScrollWidth == _viewport.ScrollWidth && SelectionMode == newSelectionMode && (Columns == null || newColumns == Columns))
                return Enumerable.Empty<BFUDetailsRowColumn<TItem>>();

            // skipping default column builder... user must provide columns always

            IEnumerable<BFUDetailsRowColumn<TItem>> adjustedColumns = null;

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
                    adjustedColumns = GetJustifiedColumnsAfterResize(newColumns, newCheckboxVisibility, newSelectionMode, _viewport.ScrollWidth, resizingColumnIndex);
                }
                else
                {
                    adjustedColumns = GetJustifiedColumns(newColumns, newCheckboxVisibility, newSelectionMode, _viewport.ScrollWidth, resizingColumnIndex);
                }

                foreach (var col in adjustedColumns)
                {
                    _columnOverrides[col.Key] = col.CalculatedWidth;
                }
            }



            return adjustedColumns;
        }

        private IEnumerable<BFUDetailsRowColumn<TItem>> GetFixedColumns(IEnumerable<BFUDetailsRowColumn<TItem>> newColumns)
        {
            foreach (var col in newColumns)
            {
                col.CalculatedWidth = !double.IsNaN(col.MaxWidth) ? col.MaxWidth : (!double.IsNaN(col.MinWidth) ? col.MinWidth : MIN_COLUMN_WIDTH);
            }
            return newColumns;
        }

        private IEnumerable<BFUDetailsRowColumn<TItem>> GetJustifiedColumnsAfterResize(IEnumerable<BFUDetailsRowColumn<TItem>> newColumns, CheckboxVisibility newCheckboxVisibility, SelectionMode newSelectionMode, double viewportWidth, int resizingColumnIndex)
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
            var fixedWidth = fixedColumns.Aggregate<BFUDetailsRowColumn<TItem>, double, double>(0, (total, column) => total + GetPaddedWidth(column, ++count == 0), x => x);

            var remainingColumns = newColumns.Skip(resizingColumnIndex).Take(newColumns.Count() - resizingColumnIndex);
            var remainingWidth = viewportWidth - fixedWidth;

            var adjustedColumns = GetJustifiedColumns(remainingColumns, newCheckboxVisibility, newSelectionMode, remainingWidth, resizingColumnIndex);

            return Enumerable.Concat(fixedColumns, adjustedColumns);
        }

        private IEnumerable<BFUDetailsRowColumn<TItem>> GetJustifiedColumns(IEnumerable<BFUDetailsRowColumn<TItem>> newColumns, CheckboxVisibility newCheckboxVisibility, SelectionMode newSelectionMode, double viewportWidth, int resizingColumnIndex)
        {
            var rowCheckWidth = newSelectionMode != SelectionMode.None && newCheckboxVisibility != CheckboxVisibility.Hidden ? 48 : 0;  //DetailsRowCheckbox width
            var groupExpandedWidth = 0; //skipping this for now.
            double totalWidth = 0;
            var availableWidth = viewportWidth - (rowCheckWidth + groupExpandedWidth);
            int count = 0;

            System.Collections.Generic.List<BFUDetailsRowColumn<TItem>> adjustedColumns = new System.Collections.Generic.List<BFUDetailsRowColumn<TItem>>();
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

        private double GetPaddedWidth(BFUDetailsRowColumn<TItem> column, bool isFirst)
        {
            return column.CalculatedWidth +
                    BFUDetailsRow<TItem>.CellLeftPadding +
                    BFUDetailsRow<TItem>.CellRightPadding +
                    (column.IsPadded ? BFUDetailsRow<TItem>.CellExtraRightPadding : 0);
        }

        private void OnColumnResizedInternal(ColumnResizedArgs<TItem> columnResizedArgs)
        {
            OnColumnResized.InvokeAsync(columnResizedArgs);

            _columnOverrides[columnResizedArgs.Column.Key] = columnResizedArgs.NewWidth;
            AdjustColumns(ItemsSource, LayoutMode, SelectionMode, CheckboxVisibility, Columns, true, columnResizedArgs.ColumnIndex);
        }

        private void OnColumnAutoResized(ItemContainer<BFUDetailsRowColumn<TItem>> itemContainer)
        {
            // TO-DO - will require measuring row cells, jsinterop
        }

        public void SortData(BFUDetailsRowColumn<TItem> Column)
        {
            if (Column.IsSorted)
            {
                Column.IsSortedDescending = !Column.IsSortedDescending;
            }
            else
            {
                Column.IsSorted = true;
                Column.IsSortedDescending = false;

            }

            foreach (var col in Columns)
            {
                if (col != Column)
                {
                    col.IsSorted = false;
                    col.IsSortedDescending = false;
                }
            }


            if (Column.IsSortedDescending)
            {
                this.ItemsSource = this.ItemsSource.OrderByDescending(Column.FieldSelector);
            }
            else
            {
                this.ItemsSource = this.ItemsSource.OrderBy(Column.FieldSelector);
            }



            StateHasChanged();

        }
    }
}
