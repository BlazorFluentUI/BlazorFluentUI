using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class FontIcon : FluentUIComponentBase
    {
        private string? icon;

        [Parameter]
        public string IconName { get; set; }
        [Parameter] public string? IconSrc { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> ExtraParameters { get; set; }

        protected override Task OnParametersSetAsync()
        {
            if (IconName != null)
            {
                MappedFontIcons.Icons.TryGetValue(IconName, out icon);
            }
            else
            {
                icon = IconSrc;
            }

            return base.OnParametersSetAsync();
        }
    }
}
