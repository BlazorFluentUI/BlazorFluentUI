using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace FluentUI
{
    public partial class Check : FluentUIComponentBase
    {
        [Parameter]
        public bool Checked { get; set; }

        [Parameter]
        public bool UseFastIcons { get; set; }

    }
}
