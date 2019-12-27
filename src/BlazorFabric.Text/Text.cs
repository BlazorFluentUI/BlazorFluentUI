
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.CompilerServices;
using System.Collections.Generic;

namespace BlazorFabric
{
    public class Text : FabricComponentBase
    {
        [Parameter] public string As { get; set; } = "span";
        [Parameter] public bool Block { get; set; }
        [Parameter] public bool NoWrap { get; set; }
        [Parameter] public TextType Variant { get; set; } = TextType.None;
        [Parameter] public IMsText CustomVariant { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }

        private MsText TextStyle;
        private ICollection<UniqueRule> CssRules = new HashSet<UniqueRule>();
        private UniqueRule msTextRule;

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            //base.BuildRenderTree(builder);

            builder.OpenComponent<ComponentStyle>(0);
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

            CreateTextStyle();

            msTextRule = new UniqueRule()
            {
                Selector = new ClassSelector() { SelectorName = "ms-text", UniqueName = true },
                Properties = TextStyle
            };
            CssRules.Add(msTextRule);

            base.OnInitialized();

        }

        private void CreateTextStyle()
        {
            TextStyle = new MsText();
            TextStyle.Display = Block ? (As == "td" ? "table-cell" : "block") : "inline";
            TextStyle.Color = CustomVariant?.Color ?? "inherit";
            TextStyle.WebkitFontSmoothing = CustomVariant?.WebkitFontSmoothing ?? "antialiased";
            TextStyle.MozOsxFontSmoothing = CustomVariant?.MozOsxFontSmoothing ?? "grayscale";
            TextStyle.FontFamily = CustomVariant?.FontFamily ?? "'Segoe UI', 'Segoe UI Web (West European)', 'Segoe UI', -apple-system, BlinkMacSystemFont, 'Roboto', 'Helvetica Neue', sans-serif";
            TextStyle.FontWeight = CustomVariant?.FontWeight ?? ((int)Variant > (int)TextType.Large ? "600" : "400");
            TextStyle.FontSize = CustomVariant?.FontSize ?? (Variant == TextType.None ? "inherit" : TextSizeMapper.TextSizeMap[Variant]);
            if (NoWrap)
            {
                TextStyle.WhiteSpace = "nowrap";
                TextStyle.Overflow = "hidden";
                TextStyle.TextOverflow = "ellipsis";
            }

        }
    }
}
