using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentUI
{
    public partial class ResponsiveWrapper : ResponsiveComponentBase
    {
        [Parameter]
        public RenderFragment<ResponsiveMode> ChildContent { get; set; }


    }
}
