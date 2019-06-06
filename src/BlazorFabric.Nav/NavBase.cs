using BlazorFabric.BaseComponent;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric.Nav
{
    public class NavBase: FabricComponentBase
    {
        [Parameter] protected RenderFragment ChildContent { get; set; }
        [Parameter] protected string AriaLabel { get; set; }
        [Parameter] protected string ExpandButtonAriaLabel { get; set; }

        [Parameter] protected bool IsOnTop { get; set; }
                
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
