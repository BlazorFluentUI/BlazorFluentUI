using Microsoft.AspNetCore.Components;

namespace FluentUI
{
    public partial class StackItem : FluentUIComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public CssValue Grow { get; set; }
        [Parameter] public CssValue Shrink { get; set; }
        [Parameter] public bool DisableShrink { get; set; } = false;
        [Parameter] public Alignment Align { get; set; } = Alignment.Unset;
        [Parameter] public bool VerticalFill { get; set; } = true;
        [Parameter] public int? Order { get; set; }

        [Parameter] public StackItemTokens Tokens { get; set; } = new StackItemTokens();

        protected string GetStyles()
        {
            string style = "";

            if (Tokens.Margin != null)
                style += $"margin:{Tokens.Margin.AsLength};";
            if (Tokens.Padding != null)
                style += $"padding:{Tokens.Padding.AsLength};";
            style += $"height:{(VerticalFill ? "100%" : "auto")};";
            style += "width:auto;";

            if (Grow != null)
                style += $"flex-grow:{(Grow.AsBooleanTrueExplicit == true ? "1" : Grow.AsString)};";
            
            if (DisableShrink || (Grow != null && Shrink != null))
                style += "flex-shrink:0;";
            else if (Shrink != null)
                style += $"flex-shrink:{Shrink.AsString};";

            if (Align != Alignment.Unset)
                style += $"align-self:{CssUtils.AlignMap[Align]};";
            
            if (Order.HasValue)
                style += $"order:{Order.Value.ToString()};";

            return style;
        }
    }
}
