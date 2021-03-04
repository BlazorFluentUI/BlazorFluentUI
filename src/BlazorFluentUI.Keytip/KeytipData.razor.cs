using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFluentUI
{
    public partial class KeytipData : FluentUIComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool Disabled { get; set; }
    }
}
