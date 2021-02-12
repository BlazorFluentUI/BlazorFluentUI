using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class DocumentCardLogo : FluentUIComponentBase
    {
        /// <summary>
        /// Describes DocumentCard Logo badge.
        /// </summary>
        [Parameter]
        public string? LogoIcon { get; set; }

        /// <summary>
        /// Describe Logo name, optional.
        /// </summary>
        [Parameter]
        public string? LogoName { get; set; }

        private ICollection<IRule> DocumentCardLogoRules { get; set; } = new List<IRule>();

        private Rule RootRule = new Rule();

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            SetStyle();
        }

        protected override Task OnInitializedAsync()
        {
            CreateLocalCss();
            SetStyle();
            return base.OnInitializedAsync();
        }

        protected override void OnThemeChanged()
        {
            SetStyle();
        }

        private void CreateLocalCss()
        {
            RootRule.Selector = new ClassSelector() { SelectorName = $"ms-DocumentCardLogo" };
            DocumentCardLogoRules.Add(RootRule);
        }

        private void SetStyle()
        {
            RootRule.Properties = new CssString()
            {
                Css = $"font-size: {Theme.FontStyle.FontSize.XxLargePlus};" +
                    $"color: {Theme.Palette.ThemePrimary};" +
                    "display:block;" +
                    "padding: 16px 16px 0 16px;"
            };
        }
    }
}
