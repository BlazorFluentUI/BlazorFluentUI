using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class DocumentCardStatus : FluentUIComponentBase
    {
        /// <summary>
        /// Describes DocumentCard status icon.
        /// </summary>
        [Parameter]
        public string? StatusIcon { get; set; }

        /// <summary>
        /// Describe status information. Required field.
        /// </summary>
        [Parameter]
        public string? Status { get; set; }

        private ICollection<IRule> DocumentCardStatusRules { get; set; } = new List<IRule>();

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
            DocumentCardStatusRules.Add(RootRule);
        }

        private void SetStyle()
        {
            RootRule.Properties = new CssString()
            {
                Css = $"margin: 8px 16px;" +
                    $"color: {Theme.Palette.NeutralPrimary};" +
                    $"background-color:  {Theme.Palette.NeutralLighter};" +
                    $"height: 32px;"
            };
        }
    }
}
