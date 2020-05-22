using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFluentUI
{
    public partial class BFUResponsiveWrapper : BFUResponsiveComponentBase
    {
        [Parameter]
        public RenderFragment<ResponsiveMode> ChildContent { get; set; }


    }
}
