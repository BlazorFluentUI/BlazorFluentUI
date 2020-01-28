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
        private bool isGrouped;

        [Parameter]
        public bool Compact { get; set; }

        [Parameter]
        public Func<TItem,object> GroupKeySelector { get; set; }

        [Parameter]
        public Func<TItem, IEnumerable<TItem>> SubGroupSelector { get; set; }

        //[Parameter]
        //public bool IsGrouped { get; set; }

        [Parameter] 
        public Func<TItem, MouseEventArgs, Task> ItemClicked { get; set; }

        [Parameter] 
        public IEnumerable<TItem> ItemsSource { get; set; }

        [Parameter] 
        public RenderFragment<TItem> ItemTemplate { get; set; }

        [Parameter]
        public EventCallback<bool> OnGroupExpandedChanged { get; set; }

        [Parameter]
        public TItem Selection { get; set; }

        [Parameter] 
        public SelectionMode SelectionMode { get; set; } = SelectionMode.Single;

        protected override Task OnParametersSetAsync()
        {
            if (GroupKeySelector != null && SubGroupSelector != null)
            {
                isGrouped = true;

            }       
            return base.OnParametersSetAsync();
        }
    }
}
