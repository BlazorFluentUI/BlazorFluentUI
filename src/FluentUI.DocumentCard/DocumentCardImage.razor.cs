using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class DocumentCardImage : FluentUIComponentBase
    {
        private ICollection<IRule> DocumentCardImageRules { get; set; } = new List<IRule>();

        private Rule RootRule = new Rule();
        private Rule CenteredIconRule = new Rule();
        private Rule CenteredIconWrapperRule = new Rule();
        private Rule CornerIconRule = new Rule();
        private const string CenteredIconSize = "42px";
        private const string CornerIconSize = "32px";
        private bool ImageLoaded = false;

        [Parameter]
        public string? ImageSource { get; set; }

        [Parameter]
        public double Height { get; set; } = double.NaN;

        [Parameter]
        public double Width { get; set; } = double.NaN;

        [Parameter]
        public string? IconName { get; set; }
        [Parameter]
        public string? IconSrc { get; set; }

        [Parameter]
        public ImageFit ImageFit { get; set; } = ImageFit.Unset;

        public static Dictionary<string, string> GlobalClassNames = new Dictionary<string, string>()
        {
            {"root", "root"},
            {"centeredIcon", "centeredIcon"},
            {"centeredIconWrapper", "centeredIconWrapper"},
            {"cornerIcon", "cornerIcon" }
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

        private void LoadStateChanged(ImageLoadState imageLoadState)
        {
            ImageLoaded = imageLoadState != ImageLoadState.NotLoaded;
        }

        private void CreateLocalCss()
        {
            RootRule.Selector = new ClassSelector() { SelectorName = $"{GlobalClassNames["root"]}" };
            DocumentCardImageRules.Add(RootRule);

            CenteredIconRule.Selector = new ClassSelector() { SelectorName = $"{GlobalClassNames["centeredIcon"]}" };
            DocumentCardImageRules.Add(CenteredIconRule);
            CenteredIconWrapperRule.Selector = new ClassSelector() { SelectorName = $"{GlobalClassNames["centeredIconWrapper"]}" };
            DocumentCardImageRules.Add(CenteredIconWrapperRule);
            CornerIconRule.Selector = new ClassSelector() { SelectorName = $"{GlobalClassNames["cornerIcon"]}" };
            DocumentCardImageRules.Add(CornerIconRule);
        }

        private void SetStyle()
        {
            RootRule.Properties = new CssString()
            {
                Css = $"border-bottom: 1px solid {Theme.Palette.NeutralLight};" +
                    $"position: relative;" +
                    $"background-color: {Theme.Palette.NeutralLighterAlt};" +
                    $"overflow:hidden;" +
                    $"{(Height != double.NaN ? $"height:{Height}px;" : "")}" +
                    $"{(Width != double.NaN ? $"width:{Width}px;" : "")}"
            };

            CenteredIconRule.Properties = new CssString()
            {
                Css = $"height: {CenteredIconSize};" +
                    $"widtht: {CenteredIconSize};" +
                    $"font-size: {CenteredIconSize};"
            };

            CenteredIconWrapperRule.Properties = new CssString()
            {
                Css = $"display: flex;" +
                $"align-items: center;" +
                $"justify-content: center;" +
                "height:100%;" +
                "width: 100%;" +
                "position:absolute;" +
                "top:0;" +
                "left:0"
            };

            CornerIconRule.Properties = new CssString()
            {
                Css = $"left: 10px;" +
                $"bottom: 10px;" +
                $"height: {CornerIconSize};" +
                $"width:{CornerIconSize};" +
                $"font-size: {CornerIconSize};" +
                "position:absolute;" +
                "overflow:visible;"
            };
        }
    }
}
