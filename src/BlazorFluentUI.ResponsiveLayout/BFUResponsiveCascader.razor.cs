using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFluentUI
{
    public partial class BFUResponsiveCascader : BFUResponsiveComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
