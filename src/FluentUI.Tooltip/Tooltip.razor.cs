using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentUI
{
    public partial class Tooltip : FluentUIComponentBase
    {
        [Parameter] public int BeakWidth { get; set; } = 16;
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public TooltipDelay Delay { get; set; } = TooltipDelay.Medium;
        [Parameter] public DirectionalHint DirectionalHint { get; set; } = DirectionalHint.TopCenter;
        [Parameter] public FluentUIComponentBase FabricComponentTarget { get; set; }
        [Parameter] public double MaxWidth { get; set; } = 364;
        [Parameter] public EventCallback<EventArgs> OnMouseEnter { get; set; }
        [Parameter] public EventCallback<EventArgs> OnMouseLeave { get; set; }

        private ICollection<IRule> TooltipLocalRules { get; set; } = new List<IRule>();

        private Rule TooltipRule = new Rule();
        private Rule TooltipAfterRule = new Rule();
        private double TooltipGabSpace;

        protected override void OnInitialized()
        {
            CreateLocalCss();
            SetStyle();
            base.OnInitialized();
        }

        protected override void OnThemeChanged()
        {
            SetStyle();
        }

        private void CreateLocalCss()
        {
            TooltipRule.Selector = new ClassSelector() { SelectorName = "ms-Tooltip" };
            TooltipAfterRule.Selector = new ClassSelector() { SelectorName = "ms-Tooltip", PseudoElement = PseudoElements.After };
            TooltipLocalRules.Add(TooltipRule);
            TooltipLocalRules.Add(TooltipAfterRule);
        }



        private void SetStyle()
        {
            TooltipGabSpace = -(Math.Sqrt((BeakWidth * BeakWidth) / 2) + 0);
            TooltipRule.Properties = new CssString()
            {
                Css = $"background:var(--semanticColors.MenuBackground);" +
                            $"box-shadow:{Theme.Effects.Elevation8};" +
                            $"padding:8px;" +
                            $"max-width:{MaxWidth}px;"
            };
            TooltipAfterRule.Properties = new CssString()
            {
                Css = $"content:'';" +
                        $"position:absolute;" +
                        $"bottom:{TooltipGabSpace}px;" +
                        $"left:{TooltipGabSpace}px;" +
                        $"right:{TooltipGabSpace}px;" +
                        $"top:{TooltipGabSpace}px;" +
                        $"z-index:0;"
            };
        }
    }
}
