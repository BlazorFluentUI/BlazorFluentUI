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
    public partial class GroupedList<TItem> : FabricComponentBase, IDisposable
    {
        //private IEnumerable<IGrouping<object, TItem>> groups;
        //private bool _isGrouped;
        private List<GroupedListItem<TItem>> listReference;

        private ReadOnlyObservableCollection<GroupedListItem<TItem>> dataItems;

        //private IEnumerable<Group<TItem,TKey>> _groups;

        private const double COMPACT_ROW_HEIGHT = 32;
        private const double ROW_HEIGHT = 42;


        private TItem _rootGroup;

        private IDisposable _selectionSubscription;
        private IDisposable _transformedDisposable;

        [CascadingParameter]
        public SelectionZone<TItem> SelectionZone { get; set; }

        [Parameter]
        public bool Compact { get; set; }

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
        public SelectionMode SelectionMode { get; set; } = SelectionMode.Single;

        [Parameter]
        public Func<TItem, IEnumerable<TItem>> SubGroupSelector { get; set; }



        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        private void OnHeaderClicked(HeaderItem<TItem> headerItem)
        {
            // Doesn't seem to be any difference in the behavior for clicking the Header vs the checkmark in the header.
            //does selection contain this item already?
            if (Selection.SelectedItems.Contains(headerItem.Item))
            {
                //deselect it and all children
                var items = SubGroupSelector(headerItem.Item)?.RecursiveSelect<TItem, TItem>(r => SubGroupSelector(r), i => i).Append(headerItem.Item);
                SelectionZone.RemoveItems(items);
            }
            else
            {
                //select it and all children
                var items = SubGroupSelector(headerItem.Item)?.RecursiveSelect<TItem, TItem>(r => SubGroupSelector(r), i => i).Append(headerItem.Item); 
                SelectionZone.AddItems(items);
            }
        }

        private void OnHeaderToggled(HeaderItem<TItem> headerItem)
        {
            // Doesn't seem to be any difference in the behavior for clicking the Header vs the checkmark in the header.
            //does selection contain this item already?
            if (Selection.SelectedItems.Contains(headerItem.Item))
            {
                //deselect it and all children
                var items = SubGroupSelector(headerItem.Item)?.RecursiveSelect<TItem, TItem>(r => SubGroupSelector(r), i => i).Append(headerItem.Item);
                SelectionZone.RemoveItems(items);
            }
            else
            {
                //select it and all children
                var items = SubGroupSelector(headerItem.Item)?.RecursiveSelect<TItem, TItem>(r => SubGroupSelector(r), i => i).Append(headerItem.Item);
                SelectionZone.AddItems(items);
            }
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
            if (SubGroupSelector != null)
            {
                if (RootGroup != null && !RootGroup.Equals(_rootGroup))
                {
                    //dispose old subscriptions
                    _transformedDisposable?.Dispose();

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


                        _transformedDisposable = transformedChangeSet
                            .AutoRefreshOnObservable(x => x.IsVisibleObservable)
                            .Filter(x => x.IsVisible)
                            .Sort(new GroupedListItemComparer<TItem>())
                            .Bind(out dataItems)
                            .Subscribe();

                    }
                }
            }

            if (SelectionMode == SelectionMode.Single && Selection.SelectedItems.Count() > 1)
            {
                SelectionZone.ClearSelection(); 
            }
            else if (SelectionMode == SelectionMode.None && Selection.SelectedItems.Count() > 0)
            {
                Selection.ClearSelection(); 
            }
            else
            {
                bool hasChanged = false;
                //make a copy of list
                var selected = Selection.SelectedItems.ToList();
                //check to see if a header needs to be turned OFF because all of its children are *not* selected.
                restart:
                var headers = selected.Where(x => SubGroupSelector(x) != null && SubGroupSelector(x).Count() > 0).ToList();
                foreach (var header in headers)
                {
                    if (SubGroupSelector(header).Except(selected).Count() > 0)
                    {
                        hasChanged = true;
                        selected.Remove(header);
                        //start loop over again, simplest way to start over is a goto statement.  This is needed when a header turns off, but it's parent header needs to turn off, too.
                        goto restart;
                    }
                }

                //check to see if a header needs to be turned ON because all of its children *are* selected.
                var potentialHeaders = dataItems.Where(x=> selected.Contains(x.Item)).Select(x=>x.Parent).Where(x=>x!= null).Distinct().ToList();
                foreach (var header in potentialHeaders)
                {
                    if (header.Children.Select(x => x.Item).Except(selected).Count() == 0)
                    {
                        if (!selected.Contains(header.Item))
                        {
                            selected.Add(header.Item);
                            hasChanged = true;
                        }
                    }
                }

                if (hasChanged)
                {
                    SelectionZone.AddAndRemoveItems(selected.Except(Selection.SelectedItems).ToList(), Selection.SelectedItems.Except(selected).ToList());
                }
            }

            await base.OnParametersSetAsync();
        }

        public void Dispose()
        {
            _transformedDisposable?.Dispose();
            _selectionSubscription?.Dispose();
        }
    }
}
