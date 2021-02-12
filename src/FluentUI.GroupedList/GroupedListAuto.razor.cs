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
using System.Collections.Specialized;

namespace FluentUI
{
    public partial class GroupedListAuto<TItem,TKey> : FluentUIComponentBase, IDisposable
    {
        //private IEnumerable<IGrouping<object, TItem>> groups;
        //private bool _isGrouped;
        private List<IGroupedListItem3<TItem>> listReference;

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
        public SelectionZone<TItem> SelectionZone { get; set; }

        private IEnumerable<DetailsRowColumn<TItem>> _columns;
      
        /// <summary>
        /// This is intended to be populated only when GroupedList is rendered under DetailsList.
        /// </summary>
        [Parameter]
        public IEnumerable<DetailsRowColumn<TItem>> Columns { get => _columns; set { if (_columns == value) return; else { _columns = value; OnPropertyChanged(); } } }


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

        //[Parameter]
        //public EventCallback<GroupedListCollection<TItem>> OnGeneratedListItems { get; set; }

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

        //[Parameter]
        //public Func<TItem, IEnumerable<TItem>> SubGroupSelector { get; set; }


        private Func<TItem, object> getKeyInternal;
        private IDisposable sourceCacheSubscription;
        private ReadOnlyObservableCollection<IGroupedListItem3<TItem>> groupedUIListItems;

        private IList<bool>? _sortDescending;
        private IList<Func<TItem, object>>? _sortBy;
        private BehaviorSubject<IComparer<TItem>> sortExpressionComparer;// = new BehaviorSubject<IComparer<TItem>>(new SortExpressionComparer<TItem>());
        private BehaviorSubject<IComparer<IGroupedListItem3<TItem>>> subGroupSortExpressionComparer;// = new BehaviorSubject<IComparer<IGroupedListItem3<TItem>>>(new SortExpressionComparer<IGroupedListItem3<TItem>>());
        private bool _groupSortDescending;

        private Subject<Unit> resorter = new Subject<Unit>();

        Dictionary<HeaderItem3<TItem,TKey>, IDisposable> headerSubscriptions = new Dictionary<HeaderItem3<TItem, TKey>, IDisposable>();

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

        //public bool ShouldAllBeSelected()
        //{
        //    return false;
        //    //return SelectionZone.Selection.SelectedKeys.Count() == groupedUIListItems.Count() && groupedUIListItems.Any();
        //}

        //private string GetKeyForHeader(GroupedListItem2<TItem> header)
        //{
        //    return string.Join(',', header.Children.Select(x => getKeyInternal(x.Item)).ToArray());
        //}

        private void OnHeaderClicked(IndexedItem<IGroupedListItem3<TItem>> indexedItem)
        {
            // should check for other callback, but it doesn't exist for now.  Do the OnHeaderToggled as a default action.

            OnHeaderToggled(indexedItem);
        }

        /// <summary>
        /// OnHeaderToggled is the action for clicking the select all button AND the default action for header clicking if the callback isn't set externally
        /// </summary>
        /// <param name="indexedItem"></param>
        private async Task OnHeaderToggled(IndexedItem<IGroupedListItem3<TItem>> indexedItem)
        {
            if (Selection != null)
            {
                var header = (indexedItem.Item as HeaderItem3<TItem, TKey>);

                Selection.ToggleRangeSelected(header.GroupIndex, header.Count);
            }
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


            if (SortBy != _sortBy || SortDescending != _sortDescending || (SortBy != null && _sortBy != null && !SortBy.SequenceEqual(_sortBy)) || (SortDescending != null && _sortDescending != null && !SortDescending.SequenceEqual(_sortDescending)))
            {
                _sortBy = SortBy;
                _sortDescending = SortDescending;

                IComparer<TItem> sortExpression = null;
                IComparer<IGroupedListItem3<TItem>> subGroupListSortExpression = null;
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
                                sortExpression = (sortExpression as SortExpressionComparer<TItem>).ThenByDescending(GroupBy[i].ConvertToIComparable());
                            else
                                sortExpression = (sortExpression as SortExpressionComparer<TItem>).ThenByAscending(GroupBy[i].ConvertToIComparable());
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
                                sortExpression = (sortExpression as SortExpressionComparer<TItem>).ThenByDescending(sortBy[j].ConvertToIComparable());
                            else
                                sortExpression = (sortExpression as SortExpressionComparer<TItem>).ThenByAscending(sortBy[j].ConvertToIComparable());
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
                                subGroupListSortExpression = (subGroupListSortExpression as SortExpressionComparer<IGroupedListItem3<TItem>>).ThenByDescending(x => sortBy[j](x.Item) as IComparable);
                            else
                                subGroupListSortExpression = (subGroupListSortExpression as SortExpressionComparer<IGroupedListItem3<TItem>>).ThenByAscending(x => sortBy[j](x.Item) as IComparable);
                        }
                    }
                }

                if (sortExpression == null)  // need to sort by **something** or else DD will use any ordering after grouping and it won't match selection sort.  Going to use original list order.
                {
                    sortExpression = new OriginalSortComparer<TItem>(ItemsSource); //new SortExpressionComparer<TItem>();
                    subGroupListSortExpression = new OriginalGroupSortComparer<TItem>(ItemsSource); //new SortExpression<IGroupedListItem3<TItem>>((x,y)=> x.Item);
                }

                if (sortExpressionComparer == null)
                    sortExpressionComparer = new BehaviorSubject<IComparer<TItem>>(sortExpression);
                else
                    sortExpressionComparer.OnNext(sortExpression);
                
                if (subGroupSortExpressionComparer == null)
                    subGroupSortExpressionComparer = new BehaviorSubject<IComparer<IGroupedListItem3<TItem>>>(subGroupListSortExpression);
                else
                    subGroupSortExpressionComparer.OnNext(subGroupListSortExpression);
            }
            else if ((SortBy == null && _sortBy != null) || sortExpressionComparer == null)
            {
                //even if there's no sorting and we're using the original list's order, we need to apply grouping "sorting" for the selection list 
                IComparer<TItem> sortExpression = null;
             
                if (GroupBy != null)
                    sortExpression = new OriginalSortComparerPresortedByGroups<TItem>(ItemsSource, GroupBy, GroupSortDescending); //new SortExpressionComparer<TItem>();
                else
                    sortExpression = new OriginalSortComparer<TItem>(ItemsSource);
                //    sortExpression = (sortExpression as SortExpressionComparer<TItem>).Add(ThenByAscending(new OriginalSortComparer<TItem>(ItemsSource));

                var subGroupListSortExpression = new OriginalGroupSortComparer<TItem>(ItemsSource); //new SortExpression<IGroupedListItem3<TItem>>((x,y)=> x.Item);

                if (sortExpressionComparer == null)
                    sortExpressionComparer = new BehaviorSubject<IComparer<TItem>>(sortExpression);
                else
                    sortExpressionComparer.OnNext(sortExpression);

                if (subGroupSortExpressionComparer == null)
                    subGroupSortExpressionComparer = new BehaviorSubject<IComparer<IGroupedListItem3<TItem>>>(subGroupListSortExpression);
                else
                    subGroupSortExpressionComparer.OnNext(subGroupListSortExpression);
            }

            //if (GroupSortDescending != _groupSortDescending)
            //{
            //    _groupSortDescending = GroupSortDescending;
            //    if (_groupSortDescending)
            //        _groupSort.OnNext(SortExpressionComparer<GroupedListItem2<TItem>>.Descending(x => x));
            //    else
            //        _groupSort.OnNext(SortExpressionComparer<GroupedListItem2<TItem>>.Ascending(x => x));

            //}

            if (GroupBy != null)
            {
                if (ItemsSource != null && !ItemsSource.Equals(_itemsSource))
                {
                    if (_itemsSource is INotifyCollectionChanged)
                    {
                        (_itemsSource as INotifyCollectionChanged).CollectionChanged -= GroupedListAuto_CollectionChanged;
                    }

                    _itemsSource = ItemsSource;
                    CreateSourceCache();
                    
                    if (_itemsSource is INotifyCollectionChanged)
                    {
                        (_itemsSource as INotifyCollectionChanged).CollectionChanged += GroupedListAuto_CollectionChanged;
                    }

                    if (_itemsSource != null)
                        sourceCache.AddOrUpdate(_itemsSource);
                }
            }
           

            await base.OnParametersSetAsync();

        }

        private void GroupedListAuto_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    sourceCache.Edit(x =>
                    {
                        foreach (TItem item in e.NewItems)
                        {
                            x.AddOrUpdate(item);
                        }
                    });
                    break;
                case NotifyCollectionChangedAction.Remove:
                    sourceCache.Edit(x =>
                    {
                        foreach (TItem item in e.OldItems)
                        {
                            x.Remove(item);
                        }
                    });
                    break;
                case NotifyCollectionChangedAction.Reset:
                    sourceCache.Edit(x =>
                    {
                        x.Clear();
                        x.AddOrUpdate(_itemsSource);
                    });
                    break;
                case NotifyCollectionChangedAction.Replace:
                    sourceCache.Edit(x =>
                    {
                        foreach (TItem item in e.NewItems)
                        {
                            x.AddOrUpdate(item);
                        }
                    });
                    break;
            }
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

            Subject<Unit> reindexTrigger = new Subject<Unit>();

            ReplaySubject<IConnectableObservable<ISortedChangeSet<IGroupedListItem3<TItem>, object>>> futureGroups = new ReplaySubject<IConnectableObservable<ISortedChangeSet<IGroupedListItem3<TItem>, object>>>();

            var groupsPublished = published.Group(firstGrouping)
                .Sort(SortExpressionComparer<IGroup<TItem, TKey, object>>.Ascending(x => x.Key as IComparable))
                .Replay();

            
            groupsPublished
                .Transform(group =>
                {
                    return new HeaderItem3<TItem, TKey>(group, remainingGrouping, 0, groupsPublished, null, subGroupSortExpressionComparer, () => InvokeAsync(StateHasChanged), reindexTrigger) as IGroupedListItem3<TItem>;
                })
                .Sort(SortExpressionComparer<IGroupedListItem3<TItem>>.Ascending(x=>x.Name))
                .Bind(out groupedUIListItems)
                .Do(_ => {
                    //InvokeAsync(StateHasChanged);
                    })
                .Subscribe(x => { }, error => {
                    Debug.WriteLine(error.Message);
                });

            groupsPublished.Connect();

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

            foreach (var header in headerSubscriptions)
            {
                header.Value.Dispose();
            }
        }
    }
}
