using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using DynamicData;
using DynamicData.Binding;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BlazorFluentUI.Lists
{
    public partial class DetailsListAuto<TItem> : FluentUIComponentBase, IAsyncDisposable
    {
        private IEnumerable<DetailsRowColumn<TItem>>? _columns;

        [Parameter]
        public CheckboxVisibility CheckboxVisibility { get; set; } = CheckboxVisibility.OnHover;

        [Parameter]
        public IEnumerable<DetailsRowColumn<TItem>> Columns { get => _columns!; set { if (_columns == value) return; else { _columns = value; OnPropertyChanged(); } } }

        [Parameter]
        public bool Compact { get; set; }

        [Parameter]
        public bool DisableSelectionZone { get; set; }

        [Parameter]
        public bool EnterModalSelectionOnTouch { get; set; }

        [Parameter]
        public RenderFragment? FooterTemplate { get; set; }

        /// <summary>
        /// GetKey must get a key that can be transformed into a unique string because the key will be written as HTML.  You can leave this null if your ItemsSource implements IList as the index will be used as a key.
        /// </summary>
        [Parameter]
        public Func<TItem, object>? GetKey { get; set; }

        [Parameter]
        public IList<Func<TItem, object>>? GroupBy { get; set; }


        //[Parameter]
        //public Func<TItem, string> GroupTitleSelector { get; set; }

        [Parameter]
        public RenderFragment? HeaderTemplate { get; set; }

        [Parameter]
        public bool IsHeaderVisible { get; set; } = true;

        [Parameter]
        public bool IsVirtualizing { get; set; } = true;

        [Parameter]
        public IEnumerable<TItem>? ItemsSource { get; set; }

        [Parameter]
        public DetailsListLayoutMode LayoutMode { get; set; }

        [Parameter]
        public EventCallback<TItem> OnItemContextMenu { get; set; }

        [Parameter]
        public EventCallback<TItem> OnItemInvoked { get; set; }

        [Parameter]
        public EventCallback<ColumnResizedArgs<TItem>> OnColumnResized { get; set; }

        [Parameter]
        public EventCallback<RowMountArgs<TItem>> OnRowDidMount { get; set; }

        [Parameter]
        public EventCallback<RowMountArgs<TItem>> OnRowWillUnmount { get; set; }

        [Parameter]
        public RenderFragment<IndexedItem<TItem>>? RowTemplate { get; set; }

        [Parameter]
        public Selection<TItem> Selection { get; set; } = new Selection<TItem>();

        //[Parameter]
        //public EventCallback<Selection<TItem>> SelectionChanged { get; set; }

        [Parameter]
        public SelectionMode SelectionMode { get; set; }

        [Parameter]
        public bool SelectionPreservedOnEmptyClick { get; set; }

        [Inject]
        private IJSRuntime? JSRuntime { get; set; }
        private const string BasePath = "./_content/BlazorFluentUI.CoreComponents/baseComponent.js";
        private IJSObjectReference? baseModule;

        private Selection<TItem> _selection = new();

        //State
        //int focusedItemIndex;
        //double _lastWidth = -1;
        //SelectionMode _lastSelectionMode;
        Viewport? _lastViewport;
        Viewport? _viewport;
        private IEnumerable<DetailsRowColumn<TItem>> _adjustedColumns = Enumerable.Empty<DetailsRowColumn<TItem>>();
        const double MIN_COLUMN_WIDTH = 100;

        Dictionary<string, double> _columnOverrides = new();

        GroupedListAuto<TItem, object>? groupedList;
        //List<TItem>? list;
        SelectionZone<TItem>? selectionZone;

        protected bool isAllSelected;
        //private bool shouldRender = true;

        //private IReadOnlyDictionary<string, object>? lastParameters = null;

        protected SelectAllVisibility selectAllVisibility = SelectAllVisibility.None;

        private SourceList<TItem>? sourceList;
        private ReadOnlyObservableCollection<TItem>? items;
        //private ReadOnlyObservableCollection<GroupedListItem2<TItem>> groupedUIItems;

        //private IObservable<Func<TItem, bool>>? DynamicDescriptionFilter;
        private IEnumerable<TItem>? itemsSource;
        private IDisposable? sourceCacheSubscription;
        private Subject<Unit> applyFilter = new();

        private Func<TItem, object>? getKeyInternal;

        private IList<Func<TItem, object>>? groupSortSelectors;
        private IList<bool>? groupSortDescendingList;
        private DotNetObjectReference<DetailsListAuto<TItem>>? selfReference;
        private int _viewportRegistration = -1;

        private Dictionary<object, DetailsRow<TItem>> _activeRows = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnColumnClick(DetailsRowColumn<TItem> column)
        {
            if (column.PropType!.GetInterface("IComparable") != null)
            {
                if (column.IsSorted && column.IsSortedDescending)
                {
                    column.IsSortedDescending = false;
                }
                else if (column.IsSorted)
                {
                    column.IsSortedDescending = true;
                }
                else
                {
                    column.IsSorted = true;
                }
            }
            foreach (DetailsRowColumn<TItem>? col in Columns)
            {
                if (col != column)
                {
                    col.IsSorted = false;
                    col.IsSortedDescending = false;
                }
            }
        }

        public void ForceUpdate()
        {
            //groupedList?.ForceUpdate();
        }

        //protected override bool ShouldRender()
        //{

        //    if (!shouldRender)
        //    {
        //        shouldRender = true;
        //        //return false;
        //    }
        //    return true;
        //}

        public override Task SetParametersAsync(ParameterView parameters)
        {
            if (_viewport != null && _viewport != _lastViewport)
            {
                AdjustColumns(
                    parameters.GetValueOrDefault<IEnumerable<TItem>>("ItemsSource")!,
                    parameters.GetValueOrDefault<DetailsListLayoutMode>("LayoutMode"),
                    parameters.GetValueOrDefault<SelectionMode>("SelectionMode"),
                    parameters.GetValueOrDefault<CheckboxVisibility>("CheckboxVisibility"),
                    parameters.GetValueOrDefault<IEnumerable<DetailsRowColumn<TItem>>>("Columns")!,
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

            //var subGroupSelector = parameters.GetValueOrDefault<Func<TItem, IEnumerable<TItem>>>("SubGroupSelector");



            return base.SetParametersAsync(parameters);
        }

        protected override Task OnParametersSetAsync()
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
                    _selection = Selection;

                    if (Selection.GetKey == null)
                        Selection.GetKey = GetKey!;
                }

                if (Selection.SelectionMode != SelectionMode)
                    Selection.SelectionMode = SelectionMode;
            }

            //Setup SourceCache to pull from GetKey or from IList index

            if (GetKey == null)
            {
                if (!(itemsSource is IList<TItem>))
                {
                    throw new Exception("ItemsSource must either have GetKey set to point to a key value for each item OR ItemsSource must be an indexable list that implements IList.");
                }
                getKeyInternal = item => itemsSource.IndexOf(item);

            }
            else
            {
                getKeyInternal = GetKey;
            }

            if (ItemsSource != itemsSource)
            {
                itemsSource = ItemsSource;
                CreateSourceCache();
                sourceList?.AddRange(itemsSource!);
            }

            return base.OnParametersSetAsync();
        }

        private void CreateSourceCache()
        {
            sourceCacheSubscription?.Dispose();
            sourceCacheSubscription = null;

            if (itemsSource == null)
            {
                return;
            }

            sourceList = new SourceList<TItem>();

            //Setup observable for INotifyPropertyChanged
            IObservable<PropertyChangedEventArgs>? propertyChanged = Observable.FromEvent<PropertyChangedEventHandler, PropertyChangedEventArgs>(
              handler =>
              {
                  void changed(object? sender, PropertyChangedEventArgs e) => handler(e);
                  return changed;
              },
              handler => PropertyChanged += handler,
              handler => PropertyChanged -= handler);

            //watch for changes to any properties and pick out changes to Columns, need to return an initial value in case Columns was already set.
            IObservable<PropertyChangedEventArgs>? columnsObservable = Observable.Return(new PropertyChangedEventArgs(nameof(Columns)))
                .Merge(propertyChanged)
                .Where(x => x.PropertyName == "Columns")
                .SelectMany(prop =>
                {
                    // now watch for changes to the Columns object properties and return an initial value so that any following logic can be setup
                    return Columns.Aggregate(Observable.Empty<PropertyChangedEventArgs>(), (x, y) => x.Merge(y.WhenPropertyChanged!));
                });


            //Setup filter expression observable

            IObservable<Func<TItem, bool>>? filterExpression = Observable.Return(new PropertyChangedEventArgs("FilterPredicate")).Merge(columnsObservable).Where(colProp => colProp.PropertyName == "FilterPredicate").Select(row =>
            {
                //remove isfiltered status on all columns
                foreach (DetailsRowColumn<TItem>? col in Columns)
                    col.IsFiltered = false;

                //get only columns with actual filter expressions (even if they don't succeed in filtering anything) and set isFiltered to true to show the icon
                System.Collections.Generic.List<DetailsRowColumn<TItem>>? columnsWithFilters = Columns
                  .Where(row => row.FilterPredicate != null).Select(x =>
                  {
                      x.IsFiltered = true;
                      return x;
                  }).ToList();

                //this is where the filter is run when dynamicdata needs it.
                return (Func<TItem, bool>)(item =>
                {
                    foreach (DetailsRowColumn<TItem>? col in columnsWithFilters)
                    {
                        if (!col.FilterPredicate!(col.FieldSelector!(item)))
                        {
                            //col.IsFiltered = true;
                            return false;
                        }
                    }
                    return true;
                });

            });



            IObservable<IComparer<TItem>>? sortExpression = Observable.Return(new PropertyChangedEventArgs("IsSorted")).Merge(columnsObservable).Where(colProp => colProp.PropertyName == "IsSorted" || colProp.PropertyName == "IsSortedDescending").Select(x =>
            {
                IEnumerable<DetailsRowColumn<TItem>>? sort = Columns.Where(x => x.IsSorted);

                IComparer<TItem> sortChain;
                if (sort.Count() > 1)
                {
                    DetailsRowColumn<TItem>? first = sort.Take(1).First();

                    IEnumerable<DetailsRowColumn<TItem>>? rest = sort.Skip(1);
                    sortChain = rest.Aggregate(first.IsSortedDescending ?
                        SortExpressionComparer<TItem>.Descending(first.FieldSelector!.ConvertToIComparable()) :
                        SortExpressionComparer<TItem>.Ascending(first.FieldSelector!.ConvertToIComparable()),
                        (x, y) => y.IsSortedDescending ?
                        x.ThenByDescending(y.FieldSelector!.ConvertToIComparable()) :
                        x.ThenByAscending(y.FieldSelector!.ConvertToIComparable()));
                }
                else if (sort.Count() == 1)
                {
                    DetailsRowColumn<TItem>? first = sort.Take(1).First();
                    sortChain = first.IsSortedDescending ?
                        SortExpressionComparer<TItem>.Descending(first.FieldSelector!.ConvertToIComparable()) :
                        SortExpressionComparer<TItem>.Ascending(first.FieldSelector!.ConvertToIComparable());
                }
                else
                {
                    if (itemsSource != null && itemsSource is IList<TItem> list1)
                        sortChain = new OriginalSortComparer<TItem>(list1); // if the original list is an IList (order matters) retain this original order when sorting hasn't been enabled.
                    else
                        sortChain = new SortExpressionComparer<TItem>();
                }

                return sortChain;
            });

            Observable.Return(new PropertyChangedEventArgs("IsSorted")).Merge(columnsObservable).Where(colProp => colProp.PropertyName == "IsSorted" || colProp.PropertyName == "IsSortedDescending").Select(x =>
            {
                IEnumerable<DetailsRowColumn<TItem>>? sort = Columns.Where(x => x.IsSorted);
                if (sort.Any())
                {
                    return sort.Select(x => x.FieldSelector).ToList();
                }
                else
                {
                    return null;
                }
            }).Subscribe(x =>
            {
                groupSortSelectors = x!;
            });

            Observable.Return(new PropertyChangedEventArgs("IsSorted")).Merge(columnsObservable).Where(colProp => colProp.PropertyName == "IsSorted" || colProp.PropertyName == "IsSortedDescending").Select(x =>
            {
                IEnumerable<DetailsRowColumn<TItem>>? sort = Columns.Where(x => x.IsSorted);
                if (sort.Any())
                {
                    return sort.Select(x => x.IsSortedDescending).ToList();
                }
                else
                {
                    return null;
                }
            }).Subscribe(x =>
            {
                groupSortDescendingList = x;
            });

            // bind sourceCache to renderable list
            IObservable<IChangeSet<TItem>>? preBindExpression = sourceList.Connect()
                .Filter(filterExpression)
               .Sort(sortExpression);


            sourceCacheSubscription = preBindExpression.Bind(out items)

                //.Throttle(TimeSpan.FromMilliseconds(100))
                .Do(x =>
                {
                    InvokeAsync(StateHasChanged);
                })
                .Subscribe();

            if (GroupBy == null)
            {
                Selection?.SetItems(items);
            }
        }


        //private void OnGroupedListGeneratedItems(GroupedListCollection<TItem> groupedListItems)
        //{
        //    selectionZone?.SetGroupedItemsSource(groupedListItems.GroupedListItems);
        //    //return Task.CompletedTask;
        //}

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            baseModule = await JSRuntime!.InvokeAsync<IJSObjectReference>("import", BasePath);

            if (firstRender)
            {
                selfReference = DotNetObjectReference.Create(this);
                _viewportRegistration = await baseModule.InvokeAsync<int>("addViewport", selfReference, RootElementReference);

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



        private static void OnAllSelected()
        {
            //if (GroupBy == null)
            //{
            //    if (Selection.SelectedKeys.Count() != this.items.Count())
            //    {
            //        //selectionZone.AddItems(ItemsSource);
            //        var list = new List<object>();
            //        for (var i = 0; i < items.Count(); i++)
            //        {
            //            list.Add(getKeyInternal(items[i]));
            //        }
            //        selectionZone?.AddKeys(list);
            //    }
            //    else
            //    {
            //        selectionZone?.ClearSelection();
            //    }
            //}
            //else
            //{
            //    groupedList?.ToggleSelectAll();

            //}
        }

        [JSInvokable]
        public void ViewportChanged(Viewport viewport)
        {
            _lastViewport = _viewport;
            _viewport = viewport;
            //Debug.WriteLine($"Viewport changed: {viewport.ScrollWidth}");
            if (_viewport != null)
            {
                AdjustColumns(items, LayoutMode, SelectionMode, CheckboxVisibility, Columns, true);
                InvokeAsync(StateHasChanged);
            }
        }

        private void AdjustColumns(IEnumerable<TItem>? newItems, DetailsListLayoutMode newLayoutMode, SelectionMode newSelectionMode, CheckboxVisibility newCheckboxVisibility, IEnumerable<DetailsRowColumn<TItem>> newColumns, bool forceUpdate, int resizingColumnIndex = -1)
        {
            _adjustedColumns = GetAdjustedColumns(newItems, newLayoutMode, newSelectionMode, newCheckboxVisibility, newColumns, forceUpdate, resizingColumnIndex);
        }

        private IEnumerable<DetailsRowColumn<TItem>> GetAdjustedColumns(IEnumerable<TItem>? newItems, DetailsListLayoutMode newLayoutMode, SelectionMode newSelectionMode, CheckboxVisibility newCheckboxVisibility, IEnumerable<DetailsRowColumn<TItem>> newColumns, bool forceUpdate, int resizingColumnIndex)
        {
            //IEnumerable<DetailsRowColumn<TItem>>? columns;
            if (!forceUpdate && _lastViewport?.Width == _viewport?.Width && SelectionMode == newSelectionMode && (Columns == null || newColumns == Columns))
                return Enumerable.Empty<DetailsRowColumn<TItem>>();

            // skipping default column builder... user must provide columns always

            IEnumerable<DetailsRowColumn<TItem>> adjustedColumns;

            if (LayoutMode == DetailsListLayoutMode.FixedColumns)
            {
                adjustedColumns = DetailsListAuto<TItem>.GetFixedColumns(newColumns);

                foreach (DetailsRowColumn<TItem>? col in adjustedColumns)
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

                foreach (DetailsRowColumn<TItem>? col in adjustedColumns)
                {
                    _columnOverrides[col.Key!] = col.CalculatedWidth;
                }
            }



            return adjustedColumns;
        }

        private static IEnumerable<DetailsRowColumn<TItem>> GetFixedColumns(IEnumerable<DetailsRowColumn<TItem>> newColumns)
        {
            foreach (DetailsRowColumn<TItem>? col in newColumns)
            {
                col.CalculatedWidth = !double.IsNaN(col.MaxWidth) ? col.MaxWidth : (!double.IsNaN(col.MinWidth) ? col.MinWidth : MIN_COLUMN_WIDTH);
            }
            return newColumns;
        }

        private IEnumerable<DetailsRowColumn<TItem>> GetJustifiedColumnsAfterResize(IEnumerable<DetailsRowColumn<TItem>> newColumns, CheckboxVisibility newCheckboxVisibility, SelectionMode newSelectionMode, double viewportWidth, int resizingColumnIndex)
        {
            IEnumerable<DetailsRowColumn<TItem>>? fixedColumns = newColumns.Take(resizingColumnIndex);
            foreach (DetailsRowColumn<TItem>? col in fixedColumns)
            {
                if (_columnOverrides.TryGetValue(col.Key!, out double overridenWidth))
                    col.CalculatedWidth = overridenWidth;
                else
                    col.CalculatedWidth = double.NaN;
            }

            double fixedWidth = fixedColumns.Aggregate<DetailsRowColumn<TItem>, double, double>(0, (total, column) => total + DetailsListAuto<TItem>.GetPaddedWidth(column), x => x);

            IEnumerable<DetailsRowColumn<TItem>>? remainingColumns = newColumns.Skip(resizingColumnIndex).Take(newColumns.Count() - resizingColumnIndex);
            double remainingWidth = viewportWidth - fixedWidth;

            IEnumerable<DetailsRowColumn<TItem>>? adjustedColumns = GetJustifiedColumns(remainingColumns, newCheckboxVisibility, newSelectionMode, remainingWidth, resizingColumnIndex);

            return Enumerable.Concat(fixedColumns, adjustedColumns);
        }

        private IEnumerable<DetailsRowColumn<TItem>> GetJustifiedColumns(IEnumerable<DetailsRowColumn<TItem>> newColumns, CheckboxVisibility newCheckboxVisibility, SelectionMode newSelectionMode, double viewportWidth, int resizingColumnIndex)
        {
            int rowCheckWidth = newSelectionMode != SelectionMode.None && newCheckboxVisibility != CheckboxVisibility.Hidden ? 48 : 0;  //DetailsRowCheckbox width
            int groupExpandedWidth = 0; //skipping this for now.
            double totalWidth = 0;
            double availableWidth = viewportWidth - (rowCheckWidth + groupExpandedWidth);

            System.Collections.Generic.List<DetailsRowColumn<TItem>> adjustedColumns = new();
            foreach (DetailsRowColumn<TItem>? col in newColumns)
            {
                adjustedColumns.Add(col);
                col.CalculatedWidth = !double.IsNaN(col.MinWidth) ? col.MinWidth : 100;
                if (_columnOverrides.TryGetValue(col.Key!, out double overridenWidth))
                    col.CalculatedWidth = overridenWidth;

                totalWidth += DetailsListAuto<TItem>.GetPaddedWidth(col);
            }

            int lastIndex = adjustedColumns.Count - 1;

            // Shrink or remove collapsable columns.
            while (lastIndex > 0 && totalWidth > availableWidth)
            {
                DetailsRowColumn<TItem>? col = adjustedColumns.ElementAt(lastIndex);
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
                    totalWidth -= DetailsListAuto<TItem>.GetPaddedWidth(col);
                    adjustedColumns.RemoveRange(lastIndex, 1);
                }
                lastIndex--;
            }

            //Then expand columns starting at the beginning, until we've filled the width.
            for (int i = 0; i < adjustedColumns.Count && totalWidth < availableWidth; i++)
            {
                DetailsRowColumn<TItem>? col = adjustedColumns[i];
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

        private static double GetPaddedWidth(DetailsRowColumn<TItem> column)
        {
            return column.CalculatedWidth +
                    DetailsRow<TItem>.CELL_LEFT_PADDING +
                    DetailsRow<TItem>.CELL_RIGHT_PADDING +
                    (column.IsPadded ? DetailsRow<TItem>.CELL_EXTRA_RIGHT_PADDING : 0);
        }

        private void OnColumnResizedInternal(ColumnResizedArgs<TItem> columnResizedArgs)
        {

            //columnResizedArgs.NewWidth = newCalculatedWidth;

            OnColumnResized.InvokeAsync(columnResizedArgs);

            _columnOverrides[columnResizedArgs.Column.Key!] = columnResizedArgs.NewWidth;
            AdjustColumns(items!, LayoutMode, SelectionMode, CheckboxVisibility, Columns, true, columnResizedArgs.ColumnIndex);
        }

        private void OnColumnAutoResized(ItemContainer<DetailsRowColumn<TItem>> itemContainer)
        {
            // TO-DO - will require measuring row cells, jsinterop
            double max = 0;
            int count = 0;
            int totalCount = _activeRows.Count;

            foreach (KeyValuePair<object, DetailsRow<TItem>> pair in _activeRows)
            {
                pair.Value.MeasureCell(itemContainer.Index, width =>
                {
                    max = Math.Max(max, width);
                    count++;
                    if (count == totalCount)
                    {
                        OnColumnResizedInternal(new ColumnResizedArgs<TItem>(itemContainer.Item!, itemContainer.Index, max));
                    }

                });
                //currentRow.measureCell(columnIndex, (width: number) => {
                //    max = Math.max(max, width);
                //    count++;
                //    if (count === totalCount)
                //    {
                //        this._onColumnResized(column, max, columnIndex);
                //    }
                //});
            }
        }

        private void OnRowDidMountInternal(DetailsRow<TItem> row)
        {
            object? key = GetKey!(row.Item!);
            if (_activeRows.ContainsKey(key))
            {
                _activeRows[key] = row;
            }
            else
            {
                _activeRows.Add(key, row);
            }
            OnRowDidMount.InvokeAsync(new RowMountArgs<TItem> { Row = row, Item = row.Item, Index = row.ItemIndex });
        }

        private void OnRowWillUnmountInternal(DetailsRow<TItem> row)
        {
            object? key = GetKey!(row.Item!);
            _activeRows.Remove(key);

            OnRowWillUnmount.InvokeAsync(new RowMountArgs<TItem> { Row = row, Item = row.Item, Index = row.ItemIndex });
        }

        public async ValueTask DisposeAsync()
        {
            if (_viewportRegistration != -1)
            {
                await baseModule!.InvokeVoidAsync("removeViewport", _viewportRegistration);
                _viewportRegistration = -1;
            }
            selfReference?.Dispose();
            if (baseModule != null)
                await baseModule.DisposeAsync();
        }
    }
}