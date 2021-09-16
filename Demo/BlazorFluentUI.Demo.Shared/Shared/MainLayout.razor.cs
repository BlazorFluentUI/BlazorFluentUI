using BlazorFluentUI.Themes.Default;
using Microsoft.AspNetCore.Components;

namespace BlazorFluentUI.Demo.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject]
        public ThemeProvider? ThemeProvider { get; set; }

        private bool customTheme;
        private string? dark;
        private Rule ScrollbarRule = new Rule();
        //private ICollection<IRule> DarkThemeLocalRules { get; set; } = new System.Collections.Generic.List<IRule>();
        protected override void OnInitialized()
        {
            customTheme = ThemeProvider?.Theme.Palette.GetType() == typeof(DefaultPaletteDark);
            base.OnInitialized();
        }

        private Task SwitchTheme()
        {
            customTheme = !customTheme;
            if (customTheme)
            {
                var palette = new DefaultPaletteDark();
                ThemeProvider?.UpdateTheme(palette, new DefaultSemanticColorsDark(palette), new DefaultSemanticTextColorsDark(palette));
                dark = $"background-color: {ThemeProvider?.Theme.SemanticColors.BodyBackground}";
            }
            else
            {
                ThemeProvider?.UpdateTheme(new DefaultPalette());
                dark = "";
            }

            return Task.CompletedTask;
        }

        //private bool isMenuCollapsed = true;
        private bool isPanelOpen = false;
        [CascadingParameter]
        public ResponsiveMode CurrentMode { get; set; }

        void ShowMenu()
        {
            isPanelOpen = true;
        }

        void HideMenu()
        {
            isPanelOpen = false;
        }

        void OnNavLinkClick(BlazorFluentUI.Routing.NavLink linkBase)
        {
            isPanelOpen = false;
        }
    }
}