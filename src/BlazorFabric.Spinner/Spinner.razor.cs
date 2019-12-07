using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public partial class Spinner : FabricComponentBase
    {
        [Parameter] public string Label { get; set; }
        [Parameter] public SpinnerLabelPosition LabelPosition { get; set; } = SpinnerLabelPosition.Bottom;
        [Parameter] public SpinnerSize Size { get; set; } = SpinnerSize.Medium;
        [Parameter] public string StatusMessage { get; set; }

        protected string GetAriaLive()
        {
            switch (AriaLive)
            {
                case AriaLive.Polite:
                    return "polite";
                case AriaLive.Assertive:
                    return "assertive";
                case AriaLive.Off:
                    return "off";
                default:
                    return "polite";
            }
        }

        protected string GetPositionStyle()
        {
            switch (LabelPosition)
            {
                case SpinnerLabelPosition.Left:
                    return " ms-Spinner--left";
                case SpinnerLabelPosition.Right:
                    return " ms-Spinner--right";
                case SpinnerLabelPosition.Top:
                    return " ms-Spinner--top";
                default:
                    return "";
            }
        }

        protected string GetSpinnerSizeStyle()
        {
            switch (Size)
            {
                case SpinnerSize.Large:
                    return " ms-Spinner--large";
                case SpinnerSize.Medium:
                    return " ms-Spinner--medium";
                case SpinnerSize.Small:
                    return " ms-Spinner--small";
                case SpinnerSize.XSmall:
                    return " ms-Spinner--xSmall";
                default:
                    return " ms-Spinner--medium";
            }
        }

    }
}
