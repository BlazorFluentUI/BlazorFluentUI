using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class ProgressIndicator : FabricComponentBase
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

        protected string GetBarHeightStyles()
        {
            string styles = "";
            styles += $"height:{BarHeight}px;";

            return styles;
        }

        protected string GetProgressBarStyles()
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
            styles += GetBarHeightStyles();

            if (Indeterminate)
            {
                styles += "position:absolute;";
                styles += "min-width:33%;";
                styles += "background: linear-gradient(to right, var(--palette-NeutralLight) 0%, var(--palette-ThemePrimary) 50%, var(--palette-NeutralLight) 100%);";
                styles += "animation: IndeterminateProgress 3s infinite;";
            }
            else
            {
                styles += "transition: width .15s linear;";
            }

            return styles;
        }

   
    }
}
