using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class DocumentCardActions : FluentUIComponentBase
    {
        /// <summary>
        ///  The number of views this document has received.
        /// </summary>
        [Parameter]
        public int Views { get; set; }

        /// <summary>
        /// The actions available for this document.
        /// </summary>
        [Parameter]
        public DocumentCardAction[]? Actions { get; set; }

        private ICollection<IRule> DocumentCardActionsRules { get; set; } = new List<IRule>();

        private Rule RootRule = new Rule();
        private Rule ActionRule = new Rule();
        private Rule ActionButtonRule = new Rule();
        private Rule ActionButtonHoverRule = new Rule();
        private Rule ViewsRule = new Rule();
        private Rule ViewsIconRule = new Rule();

        private const int ACTION_SIZE = 34;
        private const int HORIZONTAL_PADDING = 12;
        private const int VERTICAL_PADDING = 4;


        public static Dictionary<string, string> GlobalClassNames = new Dictionary<string, string>()
        {
            {"root", "ms-DocumentCardActions"},
            {"action", "ms-DocumentCardActions-action"},
            {"views", "ms-DocumentCardActions-views"}
        };

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
            RootRule.Selector = new ClassSelector() { SelectorName = $"{GlobalClassNames["root"]}" };
            DocumentCardActionsRules.Add(RootRule);

            ActionRule.Selector = new ClassSelector() { SelectorName = $"{GlobalClassNames["action"]}" };
            DocumentCardActionsRules.Add(ActionRule);
            ActionButtonRule.Selector = new CssStringSelector() { SelectorName = $".{GlobalClassNames["root"]} .ms-Button" };
            DocumentCardActionsRules.Add(ActionButtonRule);
            ActionButtonHoverRule.Selector = new CssStringSelector() { SelectorName = $".{GlobalClassNames["root"]} .ms-Button:hover .ms-Button-icon" };
            DocumentCardActionsRules.Add(ActionButtonHoverRule);
            ViewsRule.Selector = new ClassSelector() { SelectorName = $"{GlobalClassNames["views"]}" };
            DocumentCardActionsRules.Add(ViewsRule);
            ViewsIconRule.Selector = new ClassSelector() { SelectorName = $"viewsIcon" };
            DocumentCardActionsRules.Add(ViewsIconRule);
        }

        private void SetStyle()
        {
            RootRule.Properties = new CssString()
            {
                Css = $"height: {ACTION_SIZE}px;" +
                    $"padding: {VERTICAL_PADDING}px {HORIZONTAL_PADDING}px;" +
                    $"position: relative;"
            };

            ActionRule.Properties = new CssString()
            {
                Css = $"float: left;" +
                    $"margin-right: 4px;" +
                    $"color: {Theme.Palette.NeutralSecondary};" +
                    $"cursor: pointer"
            };

            ActionButtonRule.Properties = new CssString()
            {
                Css = $"font-size: {Theme.FontStyle.FontSize.MediumPlus};" +
                $"height: {ACTION_SIZE}px;" +
                $"width: {ACTION_SIZE}px;" 
            };
            ActionButtonHoverRule.Properties = new CssString()
            {
                Css = $"color: var(--semanticTextColors.ButtonText);" +
                $"cursor: pointer;" 
            };
            ViewsRule.Properties = new CssString()
            {
                Css = $"text-align: right;" +
                $"line-height: {ACTION_SIZE}px;"
            };
            ActionButtonRule.Properties = new CssString()
            {
                Css = $"margin-right: 8px;" +
                $"font-size: {Theme.FontStyle.FontSize.Medium};" +
                $"vertical-align: top;"
            };
        }
    }
}
