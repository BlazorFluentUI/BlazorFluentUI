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
        [Parameter] protected int Context { get; set; }
        
        [Parameter] protected RenderFragment ChildContent { get; set; }  //LINKS

        [Parameter] protected string AriaLabel { get; set; }
        [Parameter] protected bool Disabled { get; set; }
        [Parameter] protected bool ForceAnchor { get; set; }
        [Parameter] protected string Icon { get; set; }
        [Parameter] protected bool IsButton { get; set; }
        [Parameter] protected string Name { get; set; }
        [Parameter] protected string Target { get; set; }  //link <a> target
        [Parameter] protected string Title { get; set; } //tooltip and ARIA
        [Parameter] protected string Url { get; set; }

        [Parameter] protected int NestedDepth { get; set; }

        [Parameter] EventCallback<NavFabricLinkBase> OnClick { get; set; }

        protected bool isExpanded { get; set; }


        protected bool isSelected { get; set; }
        protected string depthClass = "";
       
        protected override Task OnInitAsync()
        {
            
            return base.OnInitAsync();
        }

        protected override Task OnParametersSetAsync()
        {
            switch (this.NestedDepth)
            {
                case 0:
                    depthClass = "";
                    break;
                case 1:
                    depthClass = "depth-one";
                    break;
                case 2:
                    depthClass = "depth-two";
                    break;
                case 3:
                    depthClass = "depth-three";
                    break;
                case 4:
                    depthClass = "depth-four";
                    break;
                case 5:
                    depthClass = "depth-five";
                    break;
                case 6:
                    depthClass = "depth-six";
                    break;
            }
            return base.OnParametersSetAsync();
        }

        protected async Task ClickHandler(UIMouseEventArgs args)
        {
            this.isExpanded = !this.isExpanded;
            await OnClick.InvokeAsync(this);
        }
    }
}
