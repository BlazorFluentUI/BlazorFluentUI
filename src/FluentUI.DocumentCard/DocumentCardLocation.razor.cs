using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class DocumentCardLocation : FluentUIComponentBase
    {
        /// <summary>
        /// Text for the location of the document.
        /// </summary>
        [Parameter]
        public string? Location { get; set; }

        /// <summary>
        /// URL to navigate to for this location.
        /// </summary>
        [Parameter]
        public string? LocationHref { get; set; }

        /// <summary>
        /// Function to call when the location is clicked.
        /// </summary>
        public EventCallback? OnClick { get; set; }

        private ICollection<IRule> DocumentCardLocationRules { get; set; } = new List<IRule>();

        private Rule RootRule = new Rule();
        private Rule RootHoverRule = new Rule();

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
            RootRule.Selector = new ClassSelector() { SelectorName = $"ms-DocumentCardLocation" };
            DocumentCardLocationRules.Add(RootRule);

            RootHoverRule.Selector = new ClassSelector() { SelectorName = $"ms-DocumentCardLocation:hover" };
            DocumentCardLocationRules.Add(RootHoverRule);
        }

        private void SetStyle()
        {
            RootRule.Properties = new CssString()
            {
                Css = $"color: {Theme.Palette.ThemePrimary};" +
                    $"display: block;" +
                    $"font-size: {Theme.FontStyle.FontSize.Small};" +
                    $"font-weight: {Theme.FontStyle.FontWeight.SemiBold};" +
                    "overflow:hidde;" +
                    "padding: 8px 16px;" +
                    "position: relative;" +
                    "text-decoration: none;" +
                    "text-overflow: ellipsis;" +
                    "white-space:nowrap;"
            };

            RootHoverRule.Properties = new CssString()
            {
                Css = $"color: {Theme.Palette.ThemePrimary};" +
                "cursor: pointer;"
            };
        }
    }
}
