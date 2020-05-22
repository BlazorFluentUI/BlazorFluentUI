using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using DynamicData.Aggregation;
using System.Reactive.Linq;
using System.Collections.ObjectModel;

namespace BlazorFluentUI
{
    public partial class BFUGroupedList<TItem> : BFUComponentBase, IDisposable
    {
        //private IEnumerable<IGrouping<object, TItem>> groups;
        //private bool _isGrouped;
        private BFUList<GroupedListItem<TItem>> listReference;

        private ReadOnlyObservableCollection<GroupedListItem<TItem>> dataItems;

        //private IEnumerable<Group<TItem,TKey>> _groups;

        private const double COMPACT_ROW_HEIGHT = 32;
        private const double ROW_HEIGHT = 42;


        //private TItem _rootGroup;
        private IEnumerable<TItem> _itemsSource;

        private IDisposable _selectionSubscription;
        private IDisposable _transformedDisposable;

        [CascadingParameter]
        public BFUSelectionZone<TItem> SelectionZone { get; set; }

        [Parameter]
        public bool Compact { get; set; }

        [Parameter]
        public Func<TItem, string> GroupTitleSelector { get; set; }

        [Parameter]
        public Func<TItem, MouseEventArgs, Task> ItemClicked { get; set; }

        [Parameter]
        public IEnumerable<TItem> ItemsSource { get; set; }

        //[Parameter]
        //public TItem RootGroup { get; set; }

        [Parameter]
        public RenderFragment<GroupedListItem<TItem>> ItemTemplate { get; set; }

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

        public void ForceUpdate()
        {
            _itemsSource = null;
            StateHasChanged();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (SubGroupSelector != null)
            {
                //if (ItemsSource != null && !ItemsSource.Equals(_itemsSource))
                //if (RootGroup != null && !RootGroup.Equals(_rootGroup))

                if (ItemsSource != null && !ItemsSource.Equals(_itemsSource))
                {
                    //dispose old subscriptions
                    _transformedDisposable?.Dispose();

                    _itemsSource = ItemsSource;
                    //_rootGroup = RootGroup;
                    if (_itemsSource != null)
                    //if (_rootGroup != null)
                    {
                        //var list = new System.Collections.Generic.List<TItem>();
                        //list.Add(_rootGroup);
                        
                        var changeSet = _itemsSource.AsObservableChangeSet();
                        System.Collections.Generic.List<HeaderItem<TItem>> headersList = new System.Collections.Generic.List<HeaderItem<TItem>>();
                        Dictionary<int, int> depthIndex = new Dictionary<int, int>();

                        var rootIndex = 0;
                        var transformedChangeSet = changeSet.TransformMany<GroupedListItem<TItem>, TItem>((x) =>

                        {
                            var header = new HeaderItem<TItem>(x, null, rootIndex++, 0, GroupTitleSelector);
                            headersList.Add(header);
                            var children = SubGroupSelector(x).RecursiveSelect<TItem, GroupedListItem<TItem>>(
                                                                                    r => SubGroupSelector(r),
                                                                                             (s, index, depth) =>
                                                                                             {
                                                                                                 if (!depthIndex.ContainsKey(depth))
                                                                                                     depthIndex[depth] = 0;
                                                                                                 var parent = headersList.FirstOrDefault(header => header.Depth == depth-1 && SubGroupSelector(header.Item).Contains(s));
                                                                                                 if (SubGroupSelector(s) == null || SubGroupSelector(s).Count() == 0)
                                                                                                 {
                                                                                                     var item = new PlainItem<TItem>(s, parent, index, depth);
                                                                                                     parent?.Children.Add(item);
                                                                                                     return item;
                                                                                                 }
                                                                                                 else
                                                                                                 {
                                                                                                     var header = new HeaderItem<TItem>(s, parent, index, depth, GroupTitleSelector);
                                                                                                     headersList.Add(header);
                                                                                                     parent?.Children.Add(header);
                                                                                                     return header;
                                                                                                 }
                                                                                             },
                                                                                             1);
                            return Enumerable.Repeat(header, 1).Concat(children);
                        });


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
