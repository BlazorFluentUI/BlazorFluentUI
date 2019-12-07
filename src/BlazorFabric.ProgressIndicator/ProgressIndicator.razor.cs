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
        const double ZERO_THRESHOLD = 0.01;

        [Parameter] public string AriaValueText { get; set; }
        [Parameter] public double BarHeight { get; set; } = 2;
        [Parameter] public string Description { get; set; }
        [Parameter] public bool Indeterminate { get; set; } = false;
        [Parameter] public string Label { get; set; }
        [Parameter] public RenderFragment<double> RenderProgressTemplate { get; set; }
        [Parameter] public double PercentComplete { get; set; }
        [Parameter] public bool ProgressHidden { get; set; }

        protected string AriaValueMin;
        protected string AriaValueMax;
        protected string AriaValueNow;


        private double _percent = double.NaN;

        protected override Task OnParametersSetAsync()
        {
            if (!double.IsNaN(PercentComplete))
            {
                _percent = Math.Min(100, Math.Max(0, PercentComplete * 100));
            }
            else
            {
                _percent = double.NaN;
            }


            AriaValueMin = double.IsNaN(_percent) ? null : "0";
            AriaValueMax = double.IsNaN(_percent) ? null : "100";
            AriaValueNow = double.IsNaN(_percent) ? null : Math.Floor(_percent).ToString();

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

            if (!double.IsNaN(_percent))
            {
                styles += $"width: {_percent}%;";
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
