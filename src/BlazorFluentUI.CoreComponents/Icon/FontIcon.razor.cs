using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    [Obsolete]
    public partial class FontIcon : FluentUIComponentBase
    {
        private string? icon;

        [Parameter]
        public string? IconName { get; set; }
        [Parameter] public string? IconSrc { get; set; }

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
