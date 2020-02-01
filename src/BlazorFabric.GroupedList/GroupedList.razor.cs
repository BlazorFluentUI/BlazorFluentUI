using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class GroupedList<TItem> : FabricComponentBase
    {
        //private IEnumerable<IGrouping<object, TItem>> groups;
        private bool _isGrouped;
        private IEnumerable<Group<TItem>> _groups;

        private const double COMPACT_ROW_HEIGHT = 32;
        private const double ROW_HEIGHT = 42;

        private BasicList<Group<TItem>> _mainList;

        [Parameter]
        public bool Compact { get; set; }

        [Parameter]
        public Func<Group<TItem>, int, double> GetGroupHeight { get; set; }

        [Parameter]
        public Func<TItem, object> GroupKeySelector { get; set; }

        [Parameter]
        public Func<TItem, MouseEventArgs, Task> ItemClicked { get; set; }

        [Parameter]
        public IEnumerable<TItem> ItemsSource { get; set; }

        [Parameter]
        public RenderFragment<TItem> ItemTemplate { get; set; }

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



        protected override Task OnParametersSetAsync()
        {
            if (GroupKeySelector != null && SubGroupSelector != null)
            {
                _isGrouped = true;
                int index = 0;
                if (ItemsSource != null)
                {
                    _groups = Group<TItem>.CreateGroups(ItemsSource, GroupKeySelector, SubGroupSelector, ref index, 0);
                }
            }
            return base.OnParametersSetAsync();
        }



       

        private double GetPageHeight(int itemIndex, ManualRectangle visibleRectangle, int itemCount)
        {
            if (GroupKeySelector != null && SubGroupSelector != null)
            {
                var pageGroup = _groups.ElementAtOrDefault(itemIndex);

                if (pageGroup != null)
                {
                    if (GetGroupHeight != null)
                    {
                        return GetGroupHeight(pageGroup, itemIndex);
                    }
                    else
                    {
                        return GetGroupHeightInternal(pageGroup, itemIndex);
                    }
                }
                else
                {
                    return 0;
                }
            }
            return 0;
        }

        private double GetGroupHeightInternal(Group<TItem> group, int itemIndex)
        {
            double rowHeight = Compact ? COMPACT_ROW_HEIGHT : ROW_HEIGHT;
            return rowHeight + (group.IsCollapsed ? 0 : rowHeight * GetGroupItemLimitInternal(group));
        }

        private int GetGroupItemLimitInternal(Group<TItem> group)
        {
            // going to ignore the property for now

            //default groupItemLimit
            return group.Count;
        }

        private string GetGroupKey(Group<TItem> group, int index)
        {
            return $"group-{(group != null && !string.IsNullOrEmpty(group.Key) ? group.Key : index.ToString())}";
        }

        private PageSpecification GetPageSpecification(int itemIndex, ManualRectangle manualRectangle)
        {
            var pageSpecification = new PageSpecification
            {
                Key = _groups != null ? _groups.ElementAt(itemIndex).Key : ""
            };
            return pageSpecification;
        }

    }
}
