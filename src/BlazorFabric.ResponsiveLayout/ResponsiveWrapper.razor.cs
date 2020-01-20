using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public partial class ResponsiveWrapper : ResponsiveFabricComponentBase
    {
        [Parameter]
        public RenderFragment<ResponsiveMode> ChildContent { get; set; }


    }
}
