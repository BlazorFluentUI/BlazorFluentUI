using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BlazorFluentUI
{
    public partial class BFUCheck : BFUComponentBase
    {
        [Parameter]
        public bool Checked { get; set; }

        [Parameter]
        public bool UseFastIcons { get; set; }

    }
}
