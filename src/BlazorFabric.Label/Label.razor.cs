using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class Label : FabricComponentBase
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

        private ICollection<Rule> LabelRules { get; set; } = new List<Rule>();

        protected override void OnInitialized()
        {
            CreateCss();
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            CreateCss();
            base.OnParametersSet();
        }

        private void CreateCss()
        {
            LabelRules.Clear();
            LabelRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Label" }, Properties = new CssString() { Css = $"font-weight:{Theme.FontStyle.FontWeight.SemiBold};color:{Theme.SemanticTextColors.BodyText};box-sizing:border-box;box-shadow:none;margin:0;display:block;padding: 5px 0px;word-wrap:break-word;overflow-wrap:break-word;" } });
            LabelRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Label--disabled" }, Properties = new CssString() { Css = $"color:{Theme.SemanticTextColors.DisabledBodyText};" } });
            LabelRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = ".ms-Label--required::after" }, Properties = new CssString() { Css = $"content:' *';color:{Theme.SemanticTextColors.ErrorText};padding-right:12px;" } });
            LabelRules.Add(new Rule() { Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" }, Properties = new CssString() { Css = ".ms-Label--disabled{color:GrayText;}" } });
        }

    }
}
