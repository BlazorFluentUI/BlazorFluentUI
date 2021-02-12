using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace FluentUI
{
    public partial class DocumentCardDetails : FluentUIComponentBase
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        private ICollection<IRule> DocumentCardDetailsLocalRules { get; set; } = new List<IRule>();

        private Rule MainRule = new Rule();

        public static Dictionary<string, string> GlobalClassNames = new Dictionary<string, string>()
        {
            {"root", "ms-DocumentCardDetails"}
        };

        private void CreateLocalCss()
        {
            MainRule.Selector = new ClassSelector() { SelectorName = $"ms-DocumentCardDetails" };
            DocumentCardDetailsLocalRules.Add(MainRule);
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

        private void SetStyle()
        {
            MainRule.Properties = new CssString()
            {
                Css = $"display:flex;" +
                      $"flex-direction: column;" +
                      $"flex: 1;" +
                      $"justify-content: space-between;" +
                      $"overflow:hidden;"
            };
        }
    }
}
