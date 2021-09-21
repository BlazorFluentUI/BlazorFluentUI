using Microsoft.AspNetCore.Components;

namespace BlazorFluentUI.Demo.Shared
{
    public partial class NavMenu
    {
        [Parameter]
        public EventCallback<BlazorFluentUI.Routing.NavLink> OnLinkClicked { get; set; }

        bool collapseNavMenu = true;
        string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;
        void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }
    }
}