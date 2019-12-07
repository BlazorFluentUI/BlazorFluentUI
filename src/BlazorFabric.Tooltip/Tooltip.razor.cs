using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public partial class Tooltip : FabricComponentBase
    {
        [Parameter] public int BeakWidth { get; set; } = 16;
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public TooltipDelay Delay { get; set; } = TooltipDelay.Medium;
        [Parameter] public DirectionalHint DirectionalHint { get; set; } = DirectionalHint.TopCenter;
        [Parameter] public FabricComponentBase FabricComponentTarget { get; set; }
        [Parameter] public double MaxWidth { get; set; } = 364;
        [Parameter] public EventCallback<EventArgs> OnMouseEnter { get; set; }
        [Parameter] public EventCallback<EventArgs> OnMouseLeave { get; set; }
        

    }
}
