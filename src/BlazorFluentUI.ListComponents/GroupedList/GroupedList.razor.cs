using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

using DynamicData;
using DynamicData.Aggregation;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;


namespace BlazorFluentUI.Lists
{
    public partial class GroupedList<TItem,TKey> : FluentUIComponentBase, IDisposable
    {
        //private IEnumerable<IGrouping<object, TItem>> groups;
        //private bool _isGrouped;
        private List<IGroupedListItem3<TItem>>? listReference;

        private ObservableCollection<IGroupedListItem3<TItem>>? dataItems;

        //private IEnumerable<Group<TItem,TKey>> _groups;

        private const double COMPACT_ROW_HEIGHT = 32;
        private const double ROW_HEIGHT = 42;


        //private TItem _rootGroup;
        private IList<TItem>? _itemsSource;

        //private IDisposable? _selectionSubscription;
        //private IDisposable? _transformedDisposable;

        [CascadingParameter]
        public SelectionZone<TItem?>? SelectionZone { get; set; }

        [Parameter]
        public bool Compact { get; set; }

        /// <summary>
        /// GetKey must get a key that can be transformed into a unique string because the key will be written as HTML.  You can leave this null if your ItemsSource implements IList as the index will be used as a key.
        /// </summary>
        [Parameter]
        public Func<TItem, TKey>? GetKey { get; set; }

        [Parameter]
        public Func<TItem, string>? GroupTitleSelector { get; set; }

        [Parameter]
        public bool IsVirtualizing { get; set; } = true;

        [Parameter]
        public Func<TItem, MouseEventArgs, Task>? ItemClicked { get; set; }

        [Parameter]
        public IList<TItem>? ItemsSource { get; set; }

        [Parameter]
        public RenderFragment<IndexedItem<IGroupedListItem3<TItem>>>? ItemTemplate { get; set; }

        [Parameter]
        public EventCallback<bool> OnGroupExpandedChanged { get; set; }

        [Parameter]
        public Func<bool> OnShouldVirtualize { get; set; } = () => true;

        [Parameter]
        public Selection<TItem>? Selection { get; set; }

        [Parameter]
        public SelectionMode SelectionMode { get; set; } = SelectionMode.Single;

        [Parameter]
        public Func<TItem, IEnumerable<TItem>>? SubGroupSelector { get; set; }

#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
        private Dictionary<HeaderItem3<TItem, TKey>, IDisposable> headerSubscriptions = new();


        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        private void OnHeaderClicked(IndexedItem<IGroupedListItem3<TItem>> headerItem)
        {
            // should check for other callback, but it doesn't exist for now.  Do the OnHeaderToggled as a default action.
            OnHeaderToggled(headerItem);
        }

        private void OnHeaderToggled(IndexedItem<IGroupedListItem3<TItem>> headerItem)
        {
            if (Selection != null)
            {
                if (headerItem.Item is HeaderItem3<TItem, TKey> header)
                    Selection.ToggleRangeSelected(header.GroupIndex, header.Count);
            }
            //if (SelectionZone != null)
            //{
            //    // Doesn't seem to be any difference in the behavior for clicking the Header vs the checkmark in the header.
            //    //does selection contain this item already?
            //    if (SelectionZone.Selection.SelectedItems.Contains(headerItem.Item))
            //    {
            //        //deselect it and all children
            //        var items = SubGroupSelector(headerItem.Item)?.RecursiveSelect<TItem, TItem>(r => SubGroupSelector(r), i => i).Append(headerItem.Item);

            //        SelectionZone.RemoveItems(items);
            //    }
            //    else
            //    {
            //        //select it and all children
            //        var items = SubGroupSelector(headerItem.Item)?.RecursiveSelect<TItem, TItem>(r => SubGroupSelector(r), i => i).Append(headerItem.Item);
            //        SelectionZone.AddItems(items);
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
            if (GetKey == null)
                throw new Exception("Must have GetKey.");

            if (Selection != null)
            {
                Selection.SelectionMode = SelectionMode;
                Selection.GetKey = item => GetKey(item);
            }


            if (SubGroupSelector != null)
            {

                if (ItemsSource != null && !ItemsSource.Equals(_itemsSource))
                {
                    if (Selection != null)
                    {
                        Selection.SetItems(FlattenList(ItemsSource, SubGroupSelector), false);
                    }
                    _itemsSource = ItemsSource;

                    if (_itemsSource != null)
                    {
                        dataItems = new ObservableCollection<IGroupedListItem3<TItem>>();
                        int cummulativeCount = 0;
                        for (int i =0; i< _itemsSource.Count; i++)
                        {
                            HeaderItem3<TItem, TKey>? group = new(_itemsSource[i], 0, cummulativeCount, SubGroupSelector, GroupTitleSelector!);
                            dataItems.Add(group);
                            int subItemCount = GroupedList<TItem, TKey>.GetPlainItemsCount(_itemsSource[i], SubGroupSelector);
                            cummulativeCount += subItemCount;
                        }

                    }

                }
            }

            await base.OnParametersSetAsync();
        }

        IList<TItem> FlattenList(IEnumerable<TItem> groupedItems, Func<TItem,IEnumerable<TItem>> subGroupSelector)
        {
            IList<TItem> flattenedItems = new System.Collections.Generic.List<TItem>();

            foreach (TItem? item in groupedItems)
            {
                IEnumerable<TItem>? subItems = subGroupSelector(item);
                if (subItems == null || !subItems.Any())
                    flattenedItems.Add(item);
                else
                {
                    IList<TItem>? moreItems = FlattenList(subItems, subGroupSelector);
                    flattenedItems.AddRange(moreItems);
                }
            }

            return flattenedItems;
        }

        public static int GetPlainItemsCount(TItem item, Func<TItem, IEnumerable<TItem>> subgroupSelector)
        {
            IEnumerable<TItem>? subItems = subgroupSelector(item);
            if (subItems == null || !subItems.Any())
                return 1;
            else
            {
                int count = 0;
                foreach (TItem? subItem in subItems)
                {
                    int subcount = GroupedList<TItem,TKey>.GetPlainItemsCount(subItem, subgroupSelector);
                    count += subcount;
                }
                return count;
            }
        }


        public void Dispose()
        {
            foreach (KeyValuePair<HeaderItem3<TItem, TKey>, IDisposable> header in headerSubscriptions)
            {
                header.Value.Dispose();
            }

            //_transformedDisposable?.Dispose();
            //_selectionSubscription?.Dispose();
        }
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.

    }
}
