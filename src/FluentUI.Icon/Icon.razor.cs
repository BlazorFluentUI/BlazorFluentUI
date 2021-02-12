using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace FluentUI
{
    public partial class Icon : FluentUIComponentBase
    {
        [Parameter] public string IconName { get; set; }
        [Parameter] public string? IconSrc { get; set; }
        [Parameter] public IconType IconType { get; set; }
        [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string,object> ExtraParameters { get; set; }
              
    }
}
