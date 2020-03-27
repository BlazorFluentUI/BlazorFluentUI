using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class Label : FabricComponentBase, IHasPreloadableGlobalStyle
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Whether the associated form field is required or not
        /// </summary>
        [Parameter]
        public bool Required { get; set; }

        /// <summary>
        /// Renders the label as disabled.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public string HtmlFor { get; set; }  //not being used for anything.

        public ICollection<Rule> CreateGlobalCss(ITheme theme)
        {
            var labelRules = new HashSet<Rule>();
            labelRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Label" }, Properties = new CssString() { Css = $"font-weight:{theme.FontStyle.FontWeight.SemiBold};color:{theme.SemanticTextColors.BodyText};box-sizing:border-box;box-shadow:none;margin:0;display:block;padding: 5px 0px;word-wrap:break-word;overflow-wrap:break-word;" } });
            labelRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Label--disabled" }, Properties = new CssString() { Css = $"color:{theme.SemanticTextColors.DisabledBodyText};" } });
            labelRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Label--required::after" }, Properties = new CssString() { Css = $"content:' *';color:{theme.SemanticTextColors.ErrorText};padding-right:12px;" } });
            labelRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" }, Properties = new CssString() { Css = ".ms-Label--disabled{color:GrayText;}" } });
            return labelRules;
        }
    }
}
