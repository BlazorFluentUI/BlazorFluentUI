using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public partial class ResponsiveCascader : ResponsiveFabricComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
