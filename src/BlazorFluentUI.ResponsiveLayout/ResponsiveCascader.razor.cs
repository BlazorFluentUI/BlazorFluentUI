using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFluentUI
{
    public partial class ResponsiveCascader : ResponsiveComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
