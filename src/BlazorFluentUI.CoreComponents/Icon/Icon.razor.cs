using Microsoft.AspNetCore.Components;

namespace BlazorFluentUI
{
    public partial class Icon : FluentUIComponentBase
    {
        [Parameter] public string? IconName { get; set; }
        [Parameter] public string? IconSrc { get; set; }
        [Parameter] public IconType IconType { get; set; }

    }
}
