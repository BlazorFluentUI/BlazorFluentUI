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

namespace FluentUI
{
    public partial class GroupItemAuto<TItem, TKey> : FluentUIComponentBase, IAsyncDisposable
    {
        //private IEnumerable<IGrouping<object, TItem>> groups;
        //private bool _isGrouped;
        private List<IGroupedListItem3<TItem>> listReference;

        //private ReadOnlyObservableCollection<IGroupedListItem3> dataItems;

        //private IEnumerable<Group<TItem,TKey>> _groups;

        private const double COMPACT_ROW_HEIGHT = 32;
        private const double ROW_HEIGHT = 42;

        //private SourceCache<TItem, TKey> sourceCache;
        //private SourceList<TItem> sourceList;
       // private IDisposable sourceListSubscription;

        //private TItem _rootGroup;
        //private IEnumerable<TItem> _itemsSource;

        private IDisposable _selectionSubscription;
        private IDisposable _transformedDisposable;

        [CascadingParameter]
        public SelectionZone<TItem> SelectionZone { get; set; }

        [Parameter]
        public bool Compact { get; set; }

        /// <summary>
        /// GetKey must get a key that can be transformed into a unique string because the key will be written as HTML.  You can leave this null if your ItemsSource implements IList as the index will be used as a key.  
        /// </summary>
        [Parameter]
        public Func<TItem, TKey> GetKey { get; set; }

        //[Parameter]
        //public IList<Func<TItem, object>>? GroupBy { get; set; }

        [Parameter]
        public bool IsVirtualizing { get; set; } = true;

        [Parameter]
        public Func<TItem, MouseEventArgs, Task>? ItemClicked { get; set; }

        [Parameter]
        public ICollection<IGroupedListItem3<TItem>>? ItemsSource { get; set; }

        //[Parameter]
        //public bool GroupSortDescending { get; set; }

        [Parameter]
        public RenderFragment<IndexedItem<IGroupedListItem3<TItem>>>? ItemTemplate { get; set; }

        //[Parameter]
        //public EventCallback<GroupedListCollection<TItem>> OnGeneratedListItems { get; set; }

        [Parameter]
        public EventCallback<bool> OnGroupExpandedChanged { get; set; }

        [Parameter]
        public Action<IndexedItem<IGroupedListItem3<TItem>>> OnHeaderClick { get; set; }

        [Parameter]
        public Action<IndexedItem<IGroupedListItem3<TItem>>> OnHeaderToggle { get; set; }

        [Parameter]
        public Func<bool> OnShouldVirtualize { get; set; } = () => true;

        [Parameter]
        public EventCallback<Viewport> OnViewportChanged { get; set; }

        [Parameter]
        public Selection<TItem> Selection { get; set; }

        [Parameter]
        public SelectionMode SelectionMode { get; set; } = SelectionMode.Single;

        //[Parameter]
        //public IList<Func<TItem, object>>? SortBy { get; set; } = null;

        //[Parameter]
        //public IList<bool>? SortDescending { get; set; }

        [Parameter]
        public int StartIndex { get; set; }

        
        private Func<TItem, object> getKeyInternal;
        private IDisposable sourceCacheSubscription;
        private ReadOnlyObservableCollection<IGroupedListItem3<TItem>> groupedUIListItems;


        Dictionary<HeaderItem3<TItem, TKey>, IDisposable> headerSubscriptions = new Dictionary<HeaderItem3<TItem, TKey>, IDisposable>();

        //private IList<bool>? _sortDescending;
        //private IList<Func<TItem, object>>? _sortBy;
        //private BehaviorSubject<IComparer<TItem>> sortExpressionComparer = new BehaviorSubject<IComparer<TItem>>(new SortExpressionComparer<TItem>());
        //private bool _groupSortDescending;
        //private BehaviorSubject<IComparer<GroupedListItem2<TItem>>> _groupSort = new BehaviorSubject<IComparer<GroupedListItem2<TItem>>>(SortExpressionComparer<GroupedListItem2<TItem>>.Ascending(x => x));
        //private Subject<Unit> resorter = new Subject<Unit>();


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

        //private string GetKeyForHeader(GroupedListItem2<TItem> header)
        //{
        //    return string.Join(',', header.Children.Select(x => getKeyInternal(x.Item)).ToArray());
        //}

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
            //_itemsSource = null;

            StateHasChanged();
        }

        protected override async Task OnParametersSetAsync()
        {

            if (GetKey == null)
                throw new Exception("Must have GetKey.");


            //if (SortBy != _sortBy || SortDescending != _sortDescending)
            //{
            //    _sortBy = SortBy;
            //    _sortDescending = SortDescending;
            //    if (SortBy != null)
            //    {
            //        var index = 0;

            //        foreach (var sortFunc in SortBy)
            //        {
            //            if (SortDescending != null && SortDescending.ElementAt(index) != null && SortDescending.ElementAt(index) == true)
            //                sortExpressionComparer.OnNext(SortExpressionComparer<TItem>.Descending(sortFunc.ConvertToIComparable()));
            //            else
            //                sortExpressionComparer.OnNext(SortExpressionComparer<TItem>.Ascending(sortFunc.ConvertToIComparable()));
            //            index++;
            //        }
            //    }
            //    else
            //    {
            //        if (ItemsSource != null && ItemsSource is IList<TItem>)
            //            sortExpressionComparer.OnNext(new OriginalSortComparer<TItem>((IList<TItem>)ItemsSource)); // if the original list is an IList (order matters) retain this original order when sorting hasn't been enabled.
            //        else
            //            sortExpressionComparer.OnNext(new SortExpressionComparer<TItem>());

            //    }

            //}

            //if (GroupSortDescending != _groupSortDescending)
            //{
            //    _groupSortDescending = GroupSortDescending;
            //    if (_groupSortDescending)
            //        _groupSort.OnNext(SortExpressionComparer<GroupedListItem2<TItem>>.Descending(x => x));
            //    else
            //        _groupSort.OnNext(SortExpressionComparer<GroupedListItem2<TItem>>.Ascending(x => x));

            //}

            //if (GroupBy != null)
            //{
            //    //if (ItemsSource != null && !ItemsSource.Equals(_itemsSource))
            //    //{
            //    //    //_itemsSource = ItemsSource;
            //    //    CreateSourceCache();

            //    //    if (_itemsSource != null)
            //    //        sourceCache.AddOrUpdate(_itemsSource);
            //    //}
            //}
           


            await base.OnParametersSetAsync();

        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            //Debug.WriteLine($"There are {groupedUIListItems.Count} items to render");
            return base.OnAfterRenderAsync(firstRender);
        }

        

        //public void SelectAll()
        //{
        //    SelectionZone.AddItems(dataItems.)
        //}



        public ValueTask DisposeAsync()
        {
            foreach (var header in headerSubscriptions)
            {
                header.Value.Dispose();
            }

            _transformedDisposable?.Dispose();
            _selectionSubscription?.Dispose();

            return ValueTask.CompletedTask;
        }
    }
}
