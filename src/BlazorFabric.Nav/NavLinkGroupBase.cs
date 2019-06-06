using BlazorFabric.BaseComponent;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric.Nav
{
    public class NavLinkGroupBase: FabricComponentBase
    {
        [Parameter] protected bool CollapseByDefault { get; set; }
        [Parameter] protected RenderFragment ChildContent { get; set; }
        [Parameter] protected string Name { get; set; }

        [CascadingParameter] protected string ExpandButtonAriaLabel { get; set; }

        [Parameter] protected EventCallback<NavLinkGroupBase> OnClick { get; set; }

        protected bool isExpanded = true;

        protected async Task ClickHandler(UIMouseEventArgs args)
        {
            this.isExpanded = !this.isExpanded;
            await OnClick.InvokeAsync(this);
            //return Task.CompletedTask;
        }

        protected override Task OnInitAsync()
        {
            return base.OnInitAsync();
        }

        protected override Task OnParametersSetAsync()
        {
            isExpanded = !this.CollapseByDefault;
            return base.OnParametersSetAsync();
        }

    }
}
