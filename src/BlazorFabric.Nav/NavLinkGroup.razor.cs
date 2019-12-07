using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class NavLinkGroup : FabricComponentBase
    {
        [Parameter] public bool CollapseByDefault { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public string Name { get; set; }

        [CascadingParameter] protected string ExpandButtonAriaLabel { get; set; }

        [Parameter] public EventCallback<NavLinkGroup> OnClick { get; set; }

        protected bool isExpanded = true;

        protected async Task ClickHandler(MouseEventArgs args)
        {
            this.isExpanded = !this.isExpanded;
            await OnClick.InvokeAsync(this);
            //return Task.CompletedTask;
        }

        protected override Task OnInitializedAsync()
        {
            //System.Diagnostics.Debug.WriteLine("Initializing NavLinkGroupBase");
            return base.OnInitializedAsync();
        }

        protected override Task OnParametersSetAsync()
        {
            isExpanded = !this.CollapseByDefault;
            return base.OnParametersSetAsync();
        }

    }
}
