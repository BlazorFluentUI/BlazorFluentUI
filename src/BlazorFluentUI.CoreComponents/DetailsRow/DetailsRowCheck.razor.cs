using BlazorFluentUI.Style;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFluentUI
{
    public partial class DetailsRowCheck : FluentUIComponentBase
    {
        [Parameter]
        public bool AnySelected { get; set; }

        [Parameter]
        public bool CanSelect { get; set; }

        [Parameter]
        public RenderFragment DetailsCheckboxTemplate { get; set; }

        [Parameter]
        public bool Checked { get; set; }

        [Parameter]
        public bool Compact { get; set; }

        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public bool IsFocusable { get; set; }

        [Parameter]
        public bool IsHeader { get; set; }

        [Parameter]
        public bool IsVisible { get; set; }

        [Parameter]
        public bool IsSelected { get; set; }

        [Parameter]
        public bool UseFastIcons { get; set; } = true;
    }
}
