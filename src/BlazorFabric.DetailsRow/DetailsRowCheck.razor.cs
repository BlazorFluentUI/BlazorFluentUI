using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public partial class DetailsRowCheck : FabricComponentBase
    {
        [Parameter]
        public bool CanSelect { get; set; }

        [Parameter]
        public bool Checked { get; set; }

        [Parameter]
        public bool Compact { get; set; }

        [Parameter]
        public bool IsHeader { get; set; }

        [Parameter]
        public bool IsVisible { get; set; }

        [Parameter]
        public bool Selected { get; set; }

        [Parameter]
        public bool UseFastIcons { get; set; } = true;

    }
}
