using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using DynamicData.Cache;
using DynamicData.Aggregation;
using DynamicData.Operators;
using DynamicData.Experimental;
using System.Reactive.Linq;
using DynamicData.Tests;
using System.Collections.ObjectModel;

namespace BlazorFabric
{
    public partial class GroupedList<TItem> : FabricComponentBase
    {
        //private IEnumerable<IGrouping<object, TItem>> groups;
        private bool _isGrouped;
        private ReadOnlyObservableCollection<GroupedListItem<TItem>> dataItems;

        //private IEnumerable<Group<TItem,TKey>> _groups;

        private const double COMPACT_ROW_HEIGHT = 32;
        private const double ROW_HEIGHT = 42;

        //private List<Group<TItem, TKey>> _mainList;

        [Parameter]
        public bool Compact { get; set; }

        //[Parameter]
        //public Func<Group<TItem>, int, double> GetGroupHeight { get; set; }

        [Parameter]
        public Func<TItem, string> GroupTitleSelector { get; set; }

        [Parameter]
        public Func<TItem, MouseEventArgs, Task> ItemClicked { get; set; }

        [Parameter]
        public IEnumerable<TItem> ItemsSource { get; set; }

        [Parameter]
        public TItem RootGroup { get; set; }

        [Parameter]
        public RenderFragment<GroupedListItem<TItem>> ItemTemplate { get; set; }

        [Parameter]
        public EventCallback<bool> OnGroupExpandedChanged { get; set; }

        [Parameter]
        public Func<bool> OnShouldVirtualize { get; set; } = () => true;

        [Parameter]
        public TItem Selection { get; set; }

        [Parameter]
        public SelectionMode SelectionMode { get; set; } = SelectionMode.Single;

        [Parameter]
        public Func<TItem, IEnumerable<TItem>> SubGroupSelector { get; set; }


        //[Parameter] public Action OnScrollExternalEvent { get; set; }  //For nested lists like GroupedList, too many javascript scroll events bogs down the whole system
        //[Parameter] public ManualRectangle ExternalProvidedScrollDimensions { get; set; }  //For nested lists like GroupedList

        //IObservable<IChangeSet<ParentOwned<TItem, TKey>, TKey>> FlattenChangeSet(IObservable<IChangeSet<ParentOwned<TItem, TKey>, TKey>> node)
        //{
        //    var flattened = node.TransformMany(x=> SubGroupSelector(x.Item), GroupKeySelector)
        //        .Transform(x => new ParentOwned<TItem, TKey>(x, default(TKey), GroupKeySelector, SubGroupSelector));
        //    flattened = FlattenChangeSet(flattened);
        //    return node.Concat(flattened);
        //}

        int FindDepth(TItem item)
        {
            int depth = 0;
            var firstCollection = SubGroupSelector(RootGroup);
            if (firstCollection.Contains(item))
                return depth;
            else
            {
                return FindDepthRecursion(item, firstCollection, depth+1);
            }
        }
        int FindDepthRecursion(TItem item, IEnumerable<TItem> collection, int depth)
        {
            foreach (var subItem in collection)
            {
                var subCollection = SubGroupSelector(subItem);
                if (subCollection != null)
                {
                    if (subCollection.Contains(item))
                        break;
                    else
                    {
                        var result = FindDepthRecursion(item, subCollection, depth + 1);
                        if (result != -1)
                        {
                            depth = result;
                            break;
                        }
                    }
                }
            }
            return depth;
        }

        protected override Task OnParametersSetAsync()
        {
            if (SubGroupSelector != null)
            {
                _isGrouped = true;
                int index = 0;
                if (RootGroup != null)
                {
                    var list = new System.Collections.Generic.List<TItem>();
                    list.Add(RootGroup);
                    var changeSet = list.AsObservableChangeSet();
                    Dictionary<int, HeaderItem<TItem>> headers = new Dictionary<int, HeaderItem<TItem>>();
                    Dictionary<int, int> depthIndex = new Dictionary<int, int>();
                    var result = changeSet.TransformMany<GroupedListItem<TItem>,TItem>(x => SubGroupSelector(x)?.RecursiveSelect<TItem, GroupedListItem<TItem>>(
                                                                                    r => SubGroupSelector(r),
                                                                                    (s, index, depth) =>
                                                                                    {
                                                                                        if (!depthIndex.ContainsKey(depth))
                                                                                            depthIndex[depth] = 0;
                                                                                        headers.TryGetValue(depth-1, out var parent);
                                                                                        if (SubGroupSelector(s) == null)
                                                                                        {
                                                                                            Debug.WriteLine($"Creating ITEM: {depth}-{index}");
                                                                                            return new PlainItem<TItem>(s, parent, index, depth);
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            Debug.WriteLine($"Creating HEADER: {depth}-{index}");
                                                                                            var header = new HeaderItem<TItem>(s, parent, index, depth, GroupTitleSelector);
                                                                                            headers[depth] = header;
                                                                                            return header;
                                                                                        }
                                                                                    }))
                        .AutoRefreshOnObservable(x=>x.IsVisibleObservable)
                        .Filter(x=> x.IsVisible)
                        .Sort(new GroupedListItemComparer<TItem>())
                        .Bind(out dataItems)
                        .Subscribe();
                }

             
            }
            return base.OnParametersSetAsync();
        }

        //IObservable<IChangeSet<GroupedListItem<TItem, TKey>,TKey>> ProcessCache(IObservable<IChangeSet<TItem, TKey>> changeSet)
        //{
        //    var groups = changeSet.Filter(x => SubGroupSelector(x) != null);
        //    var transformedGroups = groups.Transform<GroupedListItem<TItem, TKey>, TItem, TKey>(x => new HeaderItem<TItem, TKey>(x));

        //    //need to setup recursion on this part and merge it.
        //    //var flattenedCache = groups.Flatten();

        //    //var processedSubItems = ProcessCache(flattenedCache);

        //    var transformedItems = changeSet.Filter(x => SubGroupSelector(x) == null).Transform<GroupedListItem<TItem, TKey>, TItem, TKey>(x => new PlainItem<TItem, TKey>(x));

        //    var combined = transformedGroups.Merge(transformedItems);
        //    return combined;
        //}

        //private void HandleListScrollerHeightChanged((double, object) details)
        //{
        //    Debug.WriteLine($"Height changed: {details.Item1} for {(int)details.Item2}");
            
        //}

        //private void HandleSectionHeightChanged(double height)
        //{
        //    //_mainList.TriggerRemeasure();
        //}




        //private double GetPageHeight(int itemIndex, ManualRectangle visibleRectangle, int itemCount)
        //{
        //    if (SubGroupSelector != null)
        //    {
        //        //var pageGroup = _groups.ElementAtOrDefault(itemIndex);

        //        //if (pageGroup != null)
        //        //{
        //        //    if (GetGroupHeight != null)
        //        //    {
        //        //        return GetGroupHeight(pageGroup, itemIndex);
        //        //    }
        //        //    else
        //        //    {
        //        //        return GetGroupHeightInternal(pageGroup, itemIndex);
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    return 0;
        //        //}
        //    }
        //    return 0;
        //}

        //private double GetGroupHeightInternal(Group<TItem> group, int itemIndex)
        //{
        //    double rowHeight = Compact ? COMPACT_ROW_HEIGHT : ROW_HEIGHT;
        //    return rowHeight + (group.IsCollapsed ? 0 : rowHeight * GetGroupItemLimitInternal(group));
        //}

        //private int GetGroupItemLimitInternal(Group<TItem> group)
        //{
        //    // going to ignore the property for now

        //    //default groupItemLimit
        //    return group.Count;
        //}

        //private string GetGroupKey(Group<TItem> group, int index)
        //{
        //    return $"group-{(group != null && !string.IsNullOrEmpty(group.Key) ? group.Key : index.ToString())}";
        //}

        //private PageSpecification GetPageSpecification(int itemIndex, ManualRectangle manualRectangle)
        //{
        //    var pageSpecification = new PageSpecification
        //    {
        //        //Key = _groups != null ? _groups.ElementAt(itemIndex).Key : ""
        //    };
        //    return pageSpecification;
        //}

    }
}
