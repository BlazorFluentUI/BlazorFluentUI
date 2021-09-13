using Microsoft.AspNetCore.Components;

namespace BlazorFluentUI
{
    public partial class Icon : FluentUIComponentBase
    {
        [Parameter] public bool Filled { get; set; }
        [Parameter] public string? IconName { get; set; }
        [Parameter] public int IconSize { get; set; } = 24;
        [Parameter] public string? IconSrc { get; set; }
        [Parameter] public IconType IconType { get; set; }
        [Parameter] public bool UseFluentUISystemIcons { get; set; } = true;

        public string IconClassName
        {
            get
            {
                if (IconName == null)
                {
                    return "ms-Icon-placeHolder";
                }
                else
                {
                    if (!UseFluentUISystemIcons)
                    {
                        return IconName;
                    }
                    else
                    {
                        return $"icon-ic_fluent_{IconName}_{IconSize}_{(Filled ? "filled" : "regular")}";
                    }
                }
            }
        }
    }
}
