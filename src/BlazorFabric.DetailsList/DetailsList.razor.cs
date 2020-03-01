using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class DetailsList<TItem> : FabricComponentBase
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
        public Func<TItem, string> GroupTitleSelector { get; set; }

        [Parameter]
        public RenderFragment HeaderTemplate { get; set; }

        [Parameter]
        public bool IsHeaderVisible { get; set; } = true;

        [Parameter]
        public IEnumerable<TItem> ItemsSource { get; set; }

        [Parameter]
        public DetailsListLayoutMode LayoutMode { get; set; }

        [Parameter]
        public EventCallback<ItemContainer<TItem>> OnItemContextMenu { get; set; }

        [Parameter]
        public EventCallback<ItemContainer<TItem>> OnItemInvoked { get; set; }

        [Parameter]
        public EventCallback<ColumnResizedArgs<TItem>> OnColumnResized { get; set; }

        [Parameter]
        public RenderFragment<ItemContainer<TItem>> RowTemplate { get; set; }

        [Parameter]
        public Selection<TItem> Selection { get; set; }

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
        private IEnumerable<DetailsRowColumn<TItem>> _adjustedColumns = Enumerable.Empty<DetailsRowColumn<TItem>>();
        const double MIN_COLUMN_WIDTH = 100;

        Dictionary<string, double> _columnOverrides = new Dictionary<string, double>();

        GroupedList<TItem> groupedList;
        List<TItem> list;
        SelectionZone<TItem> selectionZone;

        public void ForceUpdate()
        {
            groupedList?.ForceUpdate();
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            bool shouldForceUpdates = false;
            
            var newLayoutMode = parameters.GetValueOrDefault<DetailsListLayoutMode>("LayoutMode");
            if (LayoutMode != newLayoutMode)
                shouldForceUpdates = true;

            if (parameters.GetValueOrDefault<CheckboxVisibility>("CheckboxVisibility") != CheckboxVisibility
                || parameters.GetValueOrDefault<IEnumerable<DetailsRowColumn<TItem>>>("Columns") != Columns
                || parameters.GetValueOrDefault<bool>("Compact") != Compact)
            {
                shouldForceUpdates = true;
            }

            if (_viewport != null)
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

            return base.SetParametersAsync(parameters);
        }


        protected override Task OnParametersSetAsync()
        {

            return base.OnParametersSetAsync();
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            return base.OnAfterRenderAsync(firstRender);
        }

        private void OnHeaderKeyDown(KeyboardEventArgs keyboardEventArgs)
        {

        }

        private void OnContentKeyDown(KeyboardEventArgs keyboardEventArgs)
        {

        }

        private bool ShouldAllBeSelected()
        {
            if (SubGroupSelector == null)
            {
                if (Selection.SelectedItems.Count() == this.ItemsSource.Count())
                    return true;
                else
                    return false;
            }
            else
            {
                //source is grouped... need to recursively select them all.
                var flattenedItems = this.ItemsSource?.SelectManyRecursive(x => SubGroupSelector(x));
                if (flattenedItems.Count() == Selection.SelectedItems.Count())
                    return true;
                else
                    return false;
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

        private void AdjustColumns(IEnumerable<TItem> newItems, DetailsListLayoutMode newLayoutMode, SelectionMode newSelectionMode, CheckboxVisibility newCheckboxVisibility, IEnumerable<DetailsRowColumn<TItem>> newColumns,bool forceUpdate,int resizingColumnIndex = -1)
        {
            _adjustedColumns = GetAdjustedColumns(newItems, newLayoutMode, newSelectionMode, newCheckboxVisibility, newColumns, forceUpdate, resizingColumnIndex);
        }

        private IEnumerable<DetailsRowColumn<TItem>> GetAdjustedColumns(IEnumerable<TItem> newItems, DetailsListLayoutMode newLayoutMode, SelectionMode newSelectionMode, CheckboxVisibility newCheckboxVisibility, IEnumerable<DetailsRowColumn<TItem>> newColumns, bool forceUpdate, int resizingColumnIndex)
        {
            var columns = Columns.EmptyIfNull();
            var lastWidth = _lastWidth;
            var lastSelectionMode = _lastSelectionMode;

            if (!forceUpdate && _lastViewport.ScrollWidth == _viewport.ScrollWidth && SelectionMode == newSelectionMode && (Columns == null || newColumns == Columns))
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
            var fixedWidth = fixedColumns.Aggregate<DetailsRowColumn<TItem>, double, double>(0, (total, column) =>  total + GetPaddedWidth(column, ++count == 0), x => x);

            var remainingColumns = newColumns.Skip(resizingColumnIndex).Take(newColumns.Count() - resizingColumnIndex);
            var remainingWidth = viewportWidth - fixedWidth;

            var adjustedColumns = GetJustifiedColumns(remainingColumns,newCheckboxVisibility,newSelectionMode, remainingWidth, resizingColumnIndex);

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

                if (col.CalculatedWidth - minWidth >=overflowWidth || !col.IsCollapsible)
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
            for (var i=0; i<adjustedColumns.Count && totalWidth < availableWidth; i++)
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
                    DetailsRow<TItem>.CellLeftPadding +
                    DetailsRow<TItem>.CellRightPadding +
                    (column.IsPadded ? DetailsRow<TItem>.CellExtraRightPadding : 0);
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
    }
}
