using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentUI
{
    public partial class ResponsiveCascader : ResponsiveComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
