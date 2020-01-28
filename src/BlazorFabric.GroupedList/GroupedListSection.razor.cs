using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class GroupedListSection<TItem> : FabricComponentBase
    {
        private bool isCollapsed;
        private IEnumerable<TItem> itemsHaveGroups;
        private IEnumerable<TItem> itemsWithoutGroups;

        [Parameter]
        public bool Compact { get; set; }

        [Parameter]
        public Func<TItem, object> GroupKeySelector { get; set; }

        [Parameter]
        public RenderFragment<TItem> ItemTemplate { get; set; }

        [Parameter]
        public string Name { get; set; }

        [Parameter]
        public SelectionMode SelectionMode { get; set; } = SelectionMode.Single;

        [Parameter]
        public Func<TItem, IEnumerable<TItem>> SubGroupSelector { get; set; }

        //[Parameter]
        //public IEnumerable<IGrouping<object, TItem>> Groups { get; set; }

        [Parameter]
        public IEnumerable<TItem> Items { get; set; }

        protected override Task OnParametersSetAsync()
        {
            if (GroupKeySelector != null && SubGroupSelector != null)
            {
                itemsHaveGroups = Items.Where(x=> SubGroupSelector.Invoke(x) != null);
                itemsWithoutGroups = Items.Except(itemsHaveGroups);
                
                //groups = Items.Select(SubGroupSelector);
            }
            return base.OnParametersSetAsync();
        }
    }
}
