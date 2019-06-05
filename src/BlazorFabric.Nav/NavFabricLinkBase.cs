using BlazorFabric.BaseComponent;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric.Nav
{
    public class NavFabricLinkBase : FabricComponentBase
    {
        [Parameter] protected RenderFragment ChildContent { get; set; }  //LINKS

        [Parameter] protected string AriaLabel { get; set; }
        [Parameter] protected bool Disabled { get; set; }
        [Parameter] protected bool ForceAnchor { get; set; }
        [Parameter] protected string Icon { get; set; }
        [Parameter] protected bool IsButton { get; set; }
        [Parameter] protected bool IsExpanded { get; set; }
        [Parameter] protected string Name { get; set; }
        [Parameter] protected string Target { get; set; }  //link <a> target
        [Parameter] protected string Title { get; set; } //tooltip and ARIA
        [Parameter] protected string Url { get; set; }  
        
        [Parameter] EventCallback<NavFabricLinkBase> OnClick { get; set; }

        protected override Task OnInitAsync()
        {
            return base.OnInitAsync();
        }

        protected override Task OnParametersSetAsync()
        {
            return base.OnParametersSetAsync();
        }
    }
}
