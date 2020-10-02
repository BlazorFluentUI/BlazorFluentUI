using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using DynamicData.Binding;
using DynamicData.Cache;
using DynamicData.Aggregation;
using System.Reactive.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reactive.Subjects;
using System.Reactive;
using System.Runtime.CompilerServices;

namespace BlazorFluentUI
{
    public partial class BFUGroupedListAuto<TItem,TKey> : BFUComponentBase, IDisposable
    {
        //private IEnumerable<IGrouping<object, TItem>> groups;
        //private bool _isGrouped;
        private BFUList<IGroupedListItem3<TItem>> listReference;

        private ReadOnlyObservableCollection<IGroupedListItem3<TItem>> dataItems;

        //private IEnumerable<Group<TItem,TKey>> _groups;

        private const double COMPACT_ROW_HEIGHT = 32;
        private const double ROW_HEIGHT = 42;

        private SourceCache<TItem, TKey> sourceCache;
        //private SourceList<TItem> sourceList;
        private IDisposable sourceListSubscription;

        //private TItem _rootGroup;
        private IEnumerable<TItem> _itemsSource;

        private IDisposable _selectionSubscription;
        private IDisposable _transformedDisposable;

        [CascadingParameter]
        public BFUSelectionZone<TItem> SelectionZone { get; set; }

        private IEnumerable<BFUDetailsRowColumn<TItem>> _columns;
      
        /// <summary>
        /// This is intended to be populated only when GroupedList is rendered under DetailsList.
        /// </summary>
        [Parameter]
        public IEnumerable<BFUDetailsRowColumn<TItem>> Columns { get => _columns; set { if (_columns == value) return; else { _columns = value; OnPropertyChanged(); } } }


        [Parameter]
        public bool Compact { get; set; }

        /// <summary>
        /// GetKey must get a key that can be transformed into a unique string because the key will be written as HTML.  You can leave this null if your ItemsSource implements IList as the index will be used as a key.  
        /// </summary>
        [Parameter]
        public Func<TItem, TKey> GetKey { get; set; }

        [Parameter]
        public IList<Func<TItem, object>>? GroupBy { get; set; }

        [Parameter]
        public bool IsVirtualizing { get; set; } = true;

        [Parameter]
        public Func<TItem, MouseEventArgs, Task>? ItemClicked { get; set; }

        [Parameter]
        public IList<TItem>? ItemsSource { get; set; }

        [Parameter]
        public bool GroupSortDescending { get; set; }

        [Parameter]
        public RenderFragment<IndexedItem<IGroupedListItem3<TItem>>>? ItemTemplate { get; set; }

        [Parameter]
        public EventCallback<GroupedListCollection<TItem>> OnGeneratedListItems { get; set; }

        [Parameter]
        public EventCallback<bool> OnGroupExpandedChanged { get; set; }

        [Parameter]
        public Func<bool> OnShouldVirtualize { get; set; } = () => true;

        [Parameter]
        public EventCallback<Viewport> OnViewportChanged { get; set; }

        [Parameter]
        public Selection<TItem> Selection { get; set; }

        [Parameter]
        public SelectionMode SelectionMode { get; set; } = SelectionMode.Single;

        [Parameter]
        public IList<Func<TItem, object>>? SortBy { get; set; } = null;

        [Parameter]
        public IList<bool>? SortDescending { get; set; }

        [Parameter]
        public Func<TItem, IEnumerable<TItem>> SubGroupSelector { get; set; }


        private Func<TItem, object> getKeyInternal;
        private IDisposable sourceCacheSubscription;
        private ReadOnlyObservableCollection<IGroupedListItem3<TItem>> groupedUIListItems;

        private IList<bool>? _sortDescending;
        private IList<Func<TItem, object>>? _sortBy;
        private BehaviorSubject<IComparer<TItem>> sortExpressionComparer = new BehaviorSubject<IComparer<TItem>>(new SortExpressionComparer<TItem>());
        private BehaviorSubject<IComparer<IGroupedListItem3<TItem>>> subGroupSortExpressionComparer = new BehaviorSubject<IComparer<IGroupedListItem3<TItem>>>(new SortExpressionComparer<IGroupedListItem3<TItem>>());
        private bool _groupSortDescending;
        private BehaviorSubject<IComparer<GroupedListItem2<TItem>>> _groupSort = new BehaviorSubject<IComparer<GroupedListItem2<TItem>>>(SortExpressionComparer<GroupedListItem2<TItem>>.Ascending(x => x));
        private Subject<Unit> resorter = new Subject<Unit>();


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        protected override Task OnInitializedAsync()
        {

            return base.OnInitializedAsync();
        }

        public void ToggleSelectAll()
        {
            //if (SelectionZone.Selection.SelectedKeys.Count() != this.groupedUIListItems.Count())
            //{
            //    //selectionZone.AddItems(ItemsSource);
            //    var list = new HashSet<object>();
            //    for (var i = 0; i < groupedUIListItems.Count(); i++)
            //    {
            //        if (groupedUIListItems[i] is HeaderItem<TItem>)
            //        {
            //            list.Add(string.Join(',', groupedUIListItems[i].Children.Select(x => getKeyInternal(x.Item)).ToArray()));
            //        }
            //        else
            //        {
            //            list.Add(getKeyInternal(groupedUIListItems[i].Item));
            //        }
            //    }
            //    SelectionZone.AddKeys(list);
            //}
            //else
            //{
            //    SelectionZone.ClearSelection();
            //}
        }

        public bool ShouldAllBeSelected()
        {
            return false;
            //return SelectionZone.Selection.SelectedKeys.Count() == groupedUIListItems.Count() && groupedUIListItems.Any();
        }

        private string GetKeyForHeader(GroupedListItem2<TItem> header)
        {
            return string.Join(',', header.Children.Select(x => getKeyInternal(x.Item)).ToArray());
        }

        private void OnHeaderClicked(IndexedItem<IGroupedListItem3<TItem>> indexedItem)
        {
            //if (SelectionZone != null)
            //{
            //    // Doesn't seem to be any difference in the behavior for clicking the Header vs the checkmark in the header.
            //    //does selection contain this item already?
            //    var headerKey = GetKeyForHeader(indexedItem.Item);
            //    if (SelectionZone.Selection.SelectedKeys.Contains(headerKey))
            //    {
            //        var listToDeselect = new List<object>();
            //        //deselect it and all possible children
            //        listToDeselect.Add(headerKey);
            //        if (dataItems.Count - 1 > indexedItem.Index)  // there are more items to check
            //        {
            //            for (var i = indexedItem.Index + 1; i < dataItems.Count; i++)
            //            {
            //                if (dataItems[i].Depth > indexedItem.Item.Depth)
            //                {
            //                    listToDeselect.Add(dataItems[i].Key);
            //                }
            //                else
            //                    break;
            //            }
            //        }
            //        SelectionZone.RemoveKeys(listToDeselect);
                    
            //    }
            //    else
            //    {
            //        var listToSelect = new List<object>();
            //        //select it and all possible children
            //        listToSelect.Add(headerKey);
            //        if (dataItems.Count - 1 > indexedItem.Index)  // there are more items to check
            //        {
            //            for (var i = indexedItem.Index + 1; i < dataItems.Count; i++)
            //            {
            //                if (dataItems[i].Depth > indexedItem.Item.Depth)
            //                {
            //                    listToSelect.Add(dataItems[i].Key);
            //                }
            //                else
            //                    break;
            //            }
            //        }
            //        SelectionZone.AddKeys(listToSelect);
            //        //select it and all children
            //        //var items = SubGroupSelector(headerItem.Item)?.RecursiveSelect<TItem, TItem>(r => SubGroupSelector(r), i => i).Append(headerItem.Item);
            //        //SelectionZone.AddItems(items);
            //    }
            //}
        }

        private void OnHeaderToggled(IndexedItem<IGroupedListItem3<TItem>> indexedItem)
        {
            //if (SelectionZone != null)
            //{
            //    // Doesn't seem to be any difference in the behavior for clicking the Header vs the checkmark in the header.
            //    //does selection contain this item already?
            //    var headerKey = GetKeyForHeader(indexedItem.Item);
            //    if (SelectionZone.Selection.SelectedKeys.Contains(headerKey))
            //    {
            //        var listToDeselect = new List<object>();
            //        //deselect it and all possible children
            //        listToDeselect.Add(headerKey);
            //        if (dataItems.Count - 1 > indexedItem.Index)  // there are more items to check
            //        {
            //            for (var i = indexedItem.Index + 1; i < dataItems.Count; i++)
            //            {
            //                if (dataItems[i].Depth > indexedItem.Item.Depth)
            //                {
            //                    listToDeselect.Add(dataItems[i].Key);
            //                }
            //                else
            //                    break;
            //            }
            //        }
            //        SelectionZone.RemoveKeys(listToDeselect);
            //        //deselect it and all children
            //        //var items = SubGroupSelector(headerItem.Item)?.RecursiveSelect<TItem, TItem>(r => SubGroupSelector(r), i => i).Append(headerItem.Item);
            //        //SelectionZone.RemoveItems(items);
            //    }
            //    else
            //    {
            //        var listToSelect = new List<object>();
            //        //select it and all possible children
            //        listToSelect.Add(headerKey);
            //        if (dataItems.Count - 1 > indexedItem.Index)  // there are more items to check
            //        {
            //            for (var i = indexedItem.Index + 1; i < dataItems.Count; i++)
            //            {
            //                if (dataItems[i].Depth > indexedItem.Item.Depth)
            //                {
            //                    listToSelect.Add(dataItems[i].Key);
            //                }
            //                else
            //                    break;
            //            }
            //        }
            //        SelectionZone.AddKeys(listToSelect);
            //        //select it and all children
            //        //var items = SubGroupSelector(headerItem.Item)?.RecursiveSelect<TItem, TItem>(r => SubGroupSelector(r), i => i).Append(headerItem.Item);
            //        //SelectionZone.AddItems(items);
            //    }
            //}
        }

        public void ForceUpdate()
        {
            _itemsSource = null;

            StateHasChanged();
        }

        protected override async Task OnParametersSetAsync()
        {
            //if (GetKey == null)
            //{
            //    if (!(ItemsSource is IList<TItem>))
            //    {
            //        throw new Exception("ItemsSource must either have GetKey set to point to a key value for each item OR ItemsSource must be an indexable list that implements IList.");
            //    }
            //    getKeyInternal = item => ItemsSource.IndexOf(item);
            //}
            //else
            //{
            //    getKeyInternal = GetKey;
            //}
            if (GetKey == null)
                throw new Exception("Must have GetKey.");


            if (SortBy != _sortBy || SortDescending != _sortDescending || (SortBy != null && _sortBy != null && !SortBy.SequenceEqual(_sortBy) ) || (SortDescending != null && _sortDescending != null && !SortDescending.SequenceEqual(_sortDescending)))
            {
                _sortBy = SortBy;
                _sortDescending = SortDescending;

                SortExpressionComparer<TItem> sortExpression = null;
                SortExpressionComparer<IGroupedListItem3<TItem>> subGroupListSortExpression = null;
                if (GroupBy != null)
                {
                    for (var i = 0; i < GroupBy.Count(); i++)
                    {
                        if (sortExpression == null)
                        {
                            if (GroupSortDescending)
                                sortExpression = SortExpressionComparer<TItem>.Descending(GroupBy[i].ConvertToIComparable());
                            else
                                sortExpression = SortExpressionComparer<TItem>.Ascending(GroupBy[i].ConvertToIComparable());
                        }
                        else
                        {
                            if (GroupSortDescending)
                                sortExpression = sortExpression.ThenByDescending(GroupBy[i].ConvertToIComparable());
                            else
                                sortExpression = sortExpression.ThenByAscending(GroupBy[i].ConvertToIComparable());
                        }
                    }
                }

                if (SortBy != null)
                {
                    var sortBy = SortBy.ToList();  //making a local copy
                    for (var i = 0; i < sortBy.Count(); i++)
                    {
                        var j = i;  //local copy.
                        if (sortExpression == null)
                        {
                            if (SortDescending[j])
                                sortExpression = SortExpressionComparer<TItem>.Descending(sortBy[j].ConvertToIComparable());
                            else
                                sortExpression = SortExpressionComparer<TItem>.Ascending(sortBy[j].ConvertToIComparable());
                        }
                        else
                        {
                            if (SortDescending[j])
                                sortExpression = sortExpression.ThenByDescending(sortBy[j].ConvertToIComparable());
                            else
                                sortExpression = sortExpression.ThenByAscending(sortBy[j].ConvertToIComparable());
                        }
                    }
                    for (var i = 0; i < sortBy.Count(); i++)
                    {
                        var j = i;  //local copy, necessary for the callback... not really for the above part.
                        if (subGroupListSortExpression == null)
                        {
                            if (SortDescending[j]) 
                                subGroupListSortExpression = SortExpressionComparer<IGroupedListItem3<TItem>>.Descending(x => sortBy[j](x.Item) as IComparable);
                            else
                                subGroupListSortExpression = SortExpressionComparer<IGroupedListItem3<TItem>>.Ascending(x => sortBy[j](x.Item) as IComparable);
                        }
                        else
                        {
                            if (SortDescending[j])
                                subGroupListSortExpression = subGroupListSortExpression.ThenByDescending(x => sortBy[j](x.Item) as IComparable);
                            else
                                subGroupListSortExpression = subGroupListSortExpression.ThenByAscending(x => sortBy[j](x.Item) as IComparable);
                        }
                    }
                }

                if (sortExpression == null)
                {
                    sortExpression = new SortExpressionComparer<TItem>();
                    subGroupListSortExpression = new SortExpressionComparer<IGroupedListItem3<TItem>>();
                }
                sortExpressionComparer.OnNext(sortExpression);
                subGroupSortExpressionComparer.OnNext(subGroupListSortExpression);
            }

            if (GroupSortDescending != _groupSortDescending)
            {
                _groupSortDescending = GroupSortDescending;
                if (_groupSortDescending)
                    _groupSort.OnNext(SortExpressionComparer<GroupedListItem2<TItem>>.Descending(x => x));
                else
                    _groupSort.OnNext(SortExpressionComparer<GroupedListItem2<TItem>>.Ascending(x => x));

            }

            if (GroupBy != null)
            {
                if (ItemsSource != null && !ItemsSource.Equals(_itemsSource))
                {
                    _itemsSource = ItemsSource;
                    CreateSourceCache();
                    
                    if (_itemsSource != null)
                        sourceCache.AddOrUpdate(_itemsSource);
                }
            }
            else if (SubGroupSelector != null)
            {
              
            }



            await base.OnParametersSetAsync();

        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            //Debug.WriteLine($"There are {groupedUIListItems.Count} items to render");
            return base.OnAfterRenderAsync(firstRender);
        }

        private void CreateSourceCache()
        {
            sourceCacheSubscription?.Dispose();
            sourceCacheSubscription = null;

            sourceListSubscription?.Dispose();
            sourceListSubscription = null;

            if (_itemsSource == null)
            {
                return;
            }

            sourceCache = new SourceCache<TItem,TKey>(GetKey);



            var firstGrouping = GroupBy.FirstOrDefault();
            var remainingGrouping = GroupBy.Skip(1);

            var published = sourceCache.Connect()                
                .Publish();

            ReplaySubject<IConnectableObservable<ISortedChangeSet<IGroupedListItem3<TItem>, object>>> futureGroups = new ReplaySubject<IConnectableObservable<ISortedChangeSet<IGroupedListItem3<TItem>, object>>>();

            var groupsPublished = published.Group(firstGrouping)
                .Sort(SortExpressionComparer<IGroup<TItem, TKey, object>>.Ascending(x => x.Key as IComparable))
                .Replay();

                
                //.Sort(SortExpressionComparer<IGroupedListItem3>.Ascending(x => x.Name as IComparable))
                //.Replay();

            
            groupsPublished
                .Transform(group =>
                {
                    return new HeaderItem3<TItem, TKey>(group, remainingGrouping, 0, groupsPublished, null, subGroupSortExpressionComparer, () => InvokeAsync(StateHasChanged)) as IGroupedListItem3<TItem>;
                })
                //.Transform(x=>
                //{
                //    (x as HeaderItem3<TItem, TKey>).AddGroupAccumulator(groupsPublished);
                //    return x;
                //})
                .Bind(out groupedUIListItems)
                .Do(_ => {
                    InvokeAsync(StateHasChanged);
                    })
                //.Do(_ => Debug.WriteLine($"There are {groupedUIListItems.Count} items to render."))
                .Subscribe(x => { }, error => {
                    Debug.WriteLine(error.Message);
                });

            groupsPublished.Connect();

            //SortExpressionComparer<TItem> sortExpression = null;

            //if (GroupBy != null)
            //{
            //    for (var i = 0; i < GroupBy.Count(); i++)
            //    {
            //        if (sortExpression == null)
            //        {
            //            if (GroupSortDescending)
            //                sortExpression = SortExpressionComparer<TItem>.Descending(GroupBy[i].ConvertToIComparable());
            //            else
            //                sortExpression = SortExpressionComparer<TItem>.Ascending(GroupBy[i].ConvertToIComparable());
            //        }
            //        else
            //        {
            //            if (GroupSortDescending)
            //                sortExpression = sortExpression.ThenByDescending(GroupBy[i].ConvertToIComparable());
            //            else
            //                sortExpression = sortExpression.ThenByAscending(GroupBy[i].ConvertToIComparable());
            //        }
            //    }
            //}

            

            //if (SortBy != null)
            //{
            //    for (var i = 0; i < SortBy.Count(); i++)
            //    {
            //        if (sortExpression == null)
            //        {
            //            if (SortDescending[i])
            //                sortExpression = SortExpressionComparer<TItem>.Descending(SortBy[i].ConvertToIComparable());
            //            else
            //                sortExpression = SortExpressionComparer<TItem>.Ascending(SortBy[i].ConvertToIComparable());
            //        }
            //        else
            //        {
            //            if (SortDescending[i])
            //                sortExpression = sortExpression.ThenByDescending(SortBy[i].ConvertToIComparable());
            //            else
            //                sortExpression = sortExpression.ThenByAscending(SortBy[i].ConvertToIComparable());
            //        }
            //    }
            //}

            //if (sortExpression == null)
            //    sortExpression = new SortExpressionComparer<TItem>();

            published
                .Sort(sortExpressionComparer)
                .Bind(out var selectionItems)
                .Subscribe(x => { }, error =>
                {
                    Debug.WriteLine(error.Message);
                });

            sourceListSubscription = published.Connect();

            Selection?.SetItems(selectionItems);
        }


        //public void SelectAll()
        //{
        //    SelectionZone.AddItems(dataItems.)
        //}


        public void Dispose()
        {
            _transformedDisposable?.Dispose();
            _selectionSubscription?.Dispose();
        }
    }
}
