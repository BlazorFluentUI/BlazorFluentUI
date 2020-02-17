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
using System.Reactive.Subjects;

namespace BlazorFabric
{
    public partial class GroupedList<TItem> : FabricComponentBase
    {
        //private IEnumerable<IGrouping<object, TItem>> groups;
        //private bool _isGrouped;
        private List<GroupedListItem<TItem>> listReference;

        private ReadOnlyObservableCollection<GroupedListItem<TItem>> dataItems;

        //private IEnumerable<Group<TItem,TKey>> _groups;

        private const double COMPACT_ROW_HEIGHT = 32;
        private const double ROW_HEIGHT = 42;

        //private SourceCache<GroupedListItem<TItem>, string> selectedSourceCache = new SourceCache<GroupedListItem<TItem>, string>(x => x.Key);
        // This needs to be a sourcecache... and we'll just add and remove selected object by passing this cache to the items and letting them do it.
        // When this cache gets updated, we'll send out a notification for the component.

        //private BehaviorSubject<Selection<GroupedListItem<TItem>>> _internalSelectionSubject = new BehaviorSubject<Selection<GroupedListItem<TItem>>>(new Selection<GroupedListItem<TItem>>());
        //private Selection<GroupedListItem<TItem>> _internalSelection = new Selection<GroupedListItem<TItem>>();
        //private Selection<GroupedListItem<TItem>> internalSelection { get => _internalSelectionSubject.Value;
        //    set
        //    {
        //        _internalSelectionSubject.OnNext(value);
        //    }
        //}
        private TItem _rootGroup;

        private Selection<GroupedListItem<TItem>> internalSelection = new Selection<GroupedListItem<TItem>>();
        private IDisposable _selectionSubscription;

        //internal IEnumerable<GroupedListItem<TItem>> selectedItems { get => internalSelection.SelectedItems; set => internalSelection = new Selection<GroupedListItem<TItem>>(value); }

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
        public Selection<TItem> Selection { get; set; }
        
        [Parameter] 
        public EventCallback<Selection<TItem>> SelectionChanged { get; set; }

        [Parameter]
        public SelectionMode SelectionMode { get; set; } = SelectionMode.Single;

        [Parameter]
        public Func<TItem, IEnumerable<TItem>> SubGroupSelector { get; set; }



        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        private void OnSelectedChanged(Selection<GroupedListItem<TItem>> selection)
        {
            List<string> s = new List<string>();
            
            var finalList = new System.Collections.Generic.List<GroupedListItem<TItem>>(selection.SelectedItems);

            var itemsToAdd = selection.SelectedItems.Except(internalSelection.SelectedItems).ToList();
            var itemsToRemove = internalSelection.SelectedItems.Except(selection.SelectedItems).ToList();
            itemsToRemove.ForEach(x =>
            {
                var remove = GetChildrenRecursive(x);
                finalList.Remove(remove);
            });

            itemsToAdd.ForEach(x =>
            {                
                var add = GetChildrenRecursive(x);                
                finalList.Add(add);
            });

            //check to see if a header needs to be turned OFF because all of its children are *not* selected.
            restart:
            var headers = finalList.Where(x => x is HeaderItem<TItem>).Cast<HeaderItem<TItem>>().ToList();
            foreach (var header in headers)
            {
                if (header.Children.Except(finalList).Count() > 0)
                {
                    finalList.Remove(header);
                    //start loop over again, simplest way to start over is a goto statement.  This is needed when a header turns off, but it's parent needs to turn off, too.
                    goto restart;
                }
            }

            //check to see if a header needs to be turned ON because all of its children *are* selected.
            var potentialHeaders = finalList.Select(x => x.Parent).Where(x=> x!=null).Distinct().ToList();
            foreach (var header in potentialHeaders)
            {
                if (header.Children.Except(finalList).Count() == 0)
                    finalList.Add(header);
            }

            SelectionChanged.InvokeAsync(new Selection<TItem>(finalList.Select(x => x.Item)));
        }

        private System.Collections.Generic.List<GroupedListItem<TItem>> GetChildrenRecursive(GroupedListItem<TItem> item)
        {
            var groupedItems = new System.Collections.Generic.List<GroupedListItem<TItem>>();
            foreach (var child in item.Children)
            {
                groupedItems.Add(child);
                var subItems = GetChildrenRecursive(child);
                groupedItems.Add(subItems);
            }
            return groupedItems;
        }

        private System.Collections.Generic.List<GroupedListItem<TItem>> GetS(IEnumerable<TItem> items)
        {
            var groupedItems = new System.Collections.Generic.List<GroupedListItem<TItem>>();
            foreach (var item in items)
            {
                var foundItem = dataItems.FirstOrDefault(x => x.Item.Equals(item));
                if (foundItem != null)
                    groupedItems.Add(foundItem);

                var moreItems = SubGroupSelector.Invoke(item);
                if (moreItems != null)
                    groupedItems.AddRange(GetS(moreItems));
            }
            return groupedItems;
        }

        protected override async Task OnParametersSetAsync()
        {
            if (Selection != null)
            {
                if (dataItems != null)
                {
                   
                    internalSelection = new Selection<GroupedListItem<TItem>>(dataItems.Where(x => Selection.SelectedItems.Contains(x.Item)));
                }

                else
                    //selectedSourceCache.Clear();
                    internalSelection = new Selection<GroupedListItem<TItem>>();
            }

            if (SelectionMode == SelectionMode.Single && internalSelection.SelectedItems.Count() > 1)
            {
                //selectedSourceCache.Clear();
                internalSelection.ClearSelection(); //new SysteList<GroupedListItem<TItem>>();
                //_shouldRender = true;
                //await SelectionChanged.InvokeAsync(new Selection<TItem>(selectedSourceCache.Items.Select(x => x.Item)));
                await SelectionChanged.InvokeAsync(new Selection<TItem>(internalSelection.SelectedItems.Select(x=>x.Item)));
            }
            else if (SelectionMode == SelectionMode.None && internalSelection.SelectedItems.Count() > 0)
            {
                //selectedSourceCache.Clear();
                internalSelection.ClearSelection(); //.SelectedItems = new List<GroupedListItem<TItem>>();
                //_shouldRender = true;
                //await SelectionChanged.InvokeAsync(new Selection<TItem>(selectedSourceCache.Items.Select(x => x.Item)));
                await SelectionChanged.InvokeAsync(new Selection<TItem>(internalSelection.SelectedItems.Select(x => x.Item)));
            }

            if (SubGroupSelector != null)
            {
                if (RootGroup != null && !RootGroup.Equals(_rootGroup))
                {
                    //dispose old subscriptions

                    _rootGroup = RootGroup;
                    if (_rootGroup != null)
                    {
                        var list = new System.Collections.Generic.List<TItem>();
                        list.Add(_rootGroup);
                        var changeSet = list.AsObservableChangeSet();
                        Dictionary<int, HeaderItem<TItem>> headers = new Dictionary<int, HeaderItem<TItem>>();
                        Dictionary<int, int> depthIndex = new Dictionary<int, int>();
                        var transformedChangeSet = changeSet.TransformMany<GroupedListItem<TItem>, TItem>(x => SubGroupSelector(x)?.RecursiveSelect<TItem, GroupedListItem<TItem>>(
                                                                                         r => SubGroupSelector(r),
                                                                                         (s, index, depth) =>
                                                                                         {
                                                                                             if (!depthIndex.ContainsKey(depth))
                                                                                                 depthIndex[depth] = 0;
                                                                                             headers.TryGetValue(depth - 1, out var parent);
                                                                                             if (SubGroupSelector(s) == null)
                                                                                             {
                                                                                                 Debug.WriteLine($"Creating ITEM: {depth}-{index}");
                                                                                                 var item = new PlainItem<TItem>(s, parent, index, depth);
                                                                                                 parent?.Children.Add(item);
                                                                                                 return item;
                                                                                             }
                                                                                             else
                                                                                             {
                                                                                                 Debug.WriteLine($"Creating HEADER: {depth}-{index}");
                                                                                                 var header = new HeaderItem<TItem>(s, parent, index, depth, GroupTitleSelector);
                                                                                                 headers[depth] = header;
                                                                                                 parent?.Children.Add(header);
                                                                                                 return header;
                                                                                             }
                                                                                         }));


                        var someDisposable = transformedChangeSet
                            .AutoRefreshOnObservable(x => x.IsVisibleObservable)
                            .Filter(x => x.IsVisible)
                            .Sort(new GroupedListItemComparer<TItem>())
                            .Do(x => Debug.WriteLine("Change occurred"))
                            .Bind(out dataItems)
                            .Subscribe();

                    }
                }
             
            }
            await base.OnParametersSetAsync();
        }



    }
}
