using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class ProgressIndicator : FluentUIComponentBase
    {
        // if the percentComplete is near 0, don't animate it.
        // This prevents animations on reset to 0 scenarios
        const decimal ZERO_THRESHOLD = 0.01M;

        [Parameter] public string? AriaValueText { get; set; }
        [Parameter] public double BarHeight { get; set; } = 2;
        [Parameter] public string? Description { get; set; }
        [Parameter] public bool Indeterminate { get; set; } = false;
        [Parameter] public string? Label { get; set; }
        [Parameter] public RenderFragment<decimal>? RenderProgressTemplate { get; set; }
        [Parameter] public decimal PercentComplete { get; set; } = -1;
        [Parameter] public bool ProgressHidden { get; set; }

        protected string? AriaValueMin;
        protected string? AriaValueMax;
        protected string? AriaValueNow;


        private decimal _percent = -1;
        private const int marginBetweenText = 8;
        private const int textHeight = 18;
        //private bool isRTL = false;
        private Rule ProgressIndicatorItemProgressRule = new();
        private Rule ProgressIndicatorProgressTrackRule = new();
        private Rule ProgressIndicatorProgressBarRule = new();

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
                        $"background-color:{Theme?.Palette.NeutralLight}"
            };
            ProgressIndicatorProgressBarRule.Properties = new CssString()
            {
                Css = $"height:{BarHeight}px;"
                        // ToDo Implement RTL-Support
            };
        }
    }
}
