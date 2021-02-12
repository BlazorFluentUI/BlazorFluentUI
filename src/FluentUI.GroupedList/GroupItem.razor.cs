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
    public partial class GroupItem<TItem, TKey> : FluentUIComponentBase, IAsyncDisposable
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

        [Parameter]
        public IList<Func<TItem, object>>? GroupBy { get; set; }

        [Parameter]
        public bool IsVirtualizing { get; set; } = true;

        [Parameter]
        public Func<TItem, MouseEventArgs, Task>? ItemClicked { get; set; }

        [Parameter]
        public ICollection<IGroupedListItem3<TItem>>? ItemsSource { get; set; }

        [Parameter]
        public bool GroupSortDescending { get; set; }

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

        [Parameter]
        public IList<Func<TItem, object>>? SortBy { get; set; } = null;

        [Parameter]
        public IList<bool>? SortDescending { get; set; }

        [Parameter]
        public int StartIndex { get; set; }

        
        private Func<TItem, object> getKeyInternal;
        private ICollection<IGroupedListItem3<TItem>> groupedUIListItems;

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
           

            //if (SubGroupSelector != null)
            {
                //if (ItemsSource != null && !ItemsSource.Equals(_itemsSource))
                //{
                //    //if (Selection != null)
                //    //{
                //    //    Selection.SetItems(ItemsSource);
                //    //}
                //    _itemsSource = ItemsSource;
                    
                //    //if (_itemsSource != null)
                //    //{
                //    //    dataItems = new ObservableCollection<IGroupedListItem3<TItem>>();
                //    //    int cummulativeCount = 0;
                //    //    for (var i = 0; i < _itemsSource.Count; i++)
                //    //    {
                //    //        var group = new HeaderItem3<TItem, TKey>(_itemsSource[i], i, cummulativeCount, SubGroupSelector);
                //    //        dataItems.Add(group);
                //    //        var subItemCount = GroupedList<TItem, TKey>.GetPlainItemsCount(_itemsSource[i], SubGroupSelector);
                //    //        cummulativeCount += subItemCount;
                //    //    }

                //    //}
                //}
            }
           


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
            _transformedDisposable?.Dispose();
            _selectionSubscription?.Dispose();
            return ValueTask.CompletedTask;
        }
    }
}
