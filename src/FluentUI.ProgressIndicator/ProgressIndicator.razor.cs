using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class ProgressIndicator : FluentUIComponentBase
    {
        // if the percentComplete is near 0, don't animate it.
        // This prevents animations on reset to 0 scenarios
        const decimal ZERO_THRESHOLD = 0.01M;

        [Parameter] public string AriaValueText { get; set; }
        [Parameter] public double BarHeight { get; set; } = 2;
        [Parameter] public string Description { get; set; }
        [Parameter] public bool Indeterminate { get; set; } = false;
        [Parameter] public string Label { get; set; }
        [Parameter] public RenderFragment<decimal> RenderProgressTemplate { get; set; }
        [Parameter] public decimal PercentComplete { get; set; } = -1;
        [Parameter] public bool ProgressHidden { get; set; }

        protected string AriaValueMin;
        protected string AriaValueMax;
        protected string AriaValueNow;


        private decimal _percent = -1;
        private const int marginBetweenText = 8;
        private const int textHeight = 18;
        private bool isRTL = false;
        private Rule ProgressIndicatorItemProgressRule = new Rule();
        private Rule ProgressIndicatorProgressTrackRule = new Rule();
        private Rule ProgressIndicatorProgressBarRule = new Rule();

        private ICollection<IRule> ProgressIndicatorLocalRules { get; set; } = new List<IRule>();

        protected override void OnInitialized()
        {
            CreateLocalCss();
            SetStyle();
            base.OnInitialized();
        }

        protected override void OnThemeChanged()
        {
            SetStyle();
            base.OnThemeChanged();
        }

        protected override Task OnParametersSetAsync()
        {
            if (PercentComplete >= 0)
            {
                _percent = Math.Min(100, Math.Max(0, PercentComplete * 100));
            }
            else
            {
                _percent = -1;
            }

            AriaValueMin = _percent >= 0 ? null : "0";
            AriaValueMax = _percent >= 0 ? null : "100";
            AriaValueNow = _percent >= 0 ? null : _percent.ToString();

            return base.OnParametersSetAsync();
        }

        protected string GetWidthByPercentage()
        {
            string styles = "";

            if (_percent >= 0)
            {
                styles += $"width: {_percent.ToCssValue()}%;";
                if (PercentComplete < ZERO_THRESHOLD)
                {
                    styles += "transition: none;";
                }
            }

            return styles;
        }

        private void CreateLocalCss()
        {
            ProgressIndicatorItemProgressRule.Selector = new ClassSelector() { SelectorName = "ms-ProgressIndicator-itemProgress" };
            ProgressIndicatorProgressTrackRule.Selector = new ClassSelector() { SelectorName = "ms-ProgressIndicator-progressTrack" };
            ProgressIndicatorProgressBarRule.Selector = new ClassSelector() { SelectorName = "ms-ProgressIndicator-progressBar" };
            ProgressIndicatorLocalRules.Add(ProgressIndicatorItemProgressRule);
            ProgressIndicatorLocalRules.Add(ProgressIndicatorProgressTrackRule);
            ProgressIndicatorLocalRules.Add(ProgressIndicatorProgressBarRule);
        }

        private void SetStyle()
        {
            ProgressIndicatorItemProgressRule.Properties = new CssString()
            {
                Css = $"position:relative;" +
                            $"overflow:hidden;" +
                            $"height:{BarHeight}px;" +
                            $"padding:{marginBetweenText}px 0;"
            };
            ProgressIndicatorProgressTrackRule.Properties = new CssString()
            {
                Css = $"position:absolute;" +
                        $"width:100%;" +
                        $"height:{BarHeight}px;" +
                        $"background-color:{Theme.Palette.NeutralLight}"
            };
            ProgressIndicatorProgressBarRule.Properties = new CssString()
            {
                Css = $"height:{BarHeight}px;"
                        // ToDo Implement RTL-Support
            };
        }

        //private ICollection<IRule> CreateGlobalCss()
        //{
        //    var progressIndicatorGlobalRules = new HashSet<IRule>();
        //    progressIndicatorGlobalRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = "@keyframes IndeterminateProgress" },
        //        Properties = new CssString()
        //        {
        //            Css = "0%{left:-30%;} 100%{left:100%;}"
        //        }
        //    });
        //    progressIndicatorGlobalRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = "@keyframes IndeterminateProgressRTL" },
        //        Properties = new CssString()
        //        {
        //            Css = "100%{right:-30%;} 0%{right:100%;}"
        //        }
        //    });
        //    progressIndicatorGlobalRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-ProgressIndicator-itemName" },
        //        Properties = new CssString()
        //        {
        //            Css = $"overflow:hidden;" +
        //                    $"text-overflow:ellipsis;" +
        //                    $"white-space:nowrap;" +
        //                    $"color:var(--semanticTextColors.BodyText};" +
        //                    $"padding-top:{marginBetweenText / 2}px;" +
        //                    $"line-height:{textHeight + 2}px;"
        //}
        //    });
        //    progressIndicatorGlobalRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = ".ms-ProgressIndicator-itemDescription" },
        //        Properties = new CssString()
        //        {
        //            Css = $"color:var(--semanticTextColors.BodySubtext};" +
        //                    $"font-size:{Theme.FontStyle.FontSize.Small};" +
        //                    $"line-height:{textHeight}px;"
        //        }
        //    });
        //    progressIndicatorGlobalRules.Add(new Rule()
        //    {
        //        Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
        //        Properties = new CssString()
        //        {
        //            Css = ".ms-ProgressIndicator-progressTrack{border-bottom:1px solid WindowText;} .ms-ProgressIndicator-progressBar{background-color:WindowText;}"
        //        }
        //    });
        //    return progressIndicatorGlobalRules;
        //}

    }
}
