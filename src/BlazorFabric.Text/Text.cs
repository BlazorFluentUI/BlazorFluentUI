
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.CompilerServices;
using System.Collections.Generic;

namespace BlazorFabric
{
    public class Text : FabricComponentBase
    {
        [CascadingParameter(Name = "Theme")]
        public ITheme Theme { get; set; }
        [Parameter] public string As { get; set; } = "span";
        [Parameter] public bool Block { get; set; }
        [Parameter] public bool NoWrap { get; set; }
        [Parameter] public TextType Variant { get; set; } = TextType.None;
        [Parameter] public IMsText CustomVariant { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }

        private ICollection<LocalRule> CssRules = new HashSet<LocalRule>();
        private LocalRule msTextRule;

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            //base.BuildRenderTree(builder);

            builder.OpenComponent<LocalCS>(0);
            builder.AddAttribute(1, "Rules", CssRules);
            builder.AddAttribute(2, "RulesChanged", EventCallback.Factory.Create(this, RuntimeHelpers.CreateInferredEventCallback(this, __value => CssRules = __value, CssRules)));
            builder.CloseComponent();

            builder.OpenElement(3, As);
            builder.AddAttribute(4, "class", $"{msTextRule.Selector.SelectorName}");
            builder.AddContent(5, ChildContent);
            builder.CloseElement();
        }

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
            CssRules.Clear();
            CssRules.Add(new LocalRule() { Selector = new ClassSelector() { SelectorName = "ms-text", UniqueName = true }, Properties = CreateTextStyle() });
        }

        private MsText CreateTextStyle()
        {
            var textStyle = new MsText();
            textStyle.Display = Block ? (As == "td" ? "table-cell" : "block") : "inline";
            textStyle.Color = CustomVariant?.Color ?? "inherit";
            textStyle.WebkitFontSmoothing = CustomVariant?.WebkitFontSmoothing ?? "antialiased";
            textStyle.MozOsxFontSmoothing = CustomVariant?.MozOsxFontSmoothing ?? "grayscale";
            textStyle.FontFamily = CustomVariant?.FontFamily ?? "'Segoe UI', 'Segoe UI Web (West European)', 'Segoe UI', -apple-system, BlinkMacSystemFont, 'Roboto', 'Helvetica Neue', sans-serif";
            textStyle.FontWeight = CustomVariant?.FontWeight ?? ((int)Variant > (int)TextType.Large ? Theme.FontStyle.FontWeight.SemiBold.ToString() : Theme.FontStyle.FontWeight.Regular.ToString());
            textStyle.FontSize = CustomVariant?.FontSize ?? (Variant == TextType.None ? "inherit" : TextSizeMapper.TextSizeMap[Variant]);
            if (NoWrap)
            {
                textStyle.WhiteSpace = "nowrap";
                textStyle.Overflow = "hidden";
                textStyle.TextOverflow = "ellipsis";
            }
            return textStyle;
        }
    }
}
