using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFluentUI
{
    public partial class Spinner : FluentUIComponentBase
    {
        [Parameter] public string? Label { get; set; }
        [Parameter] public SpinnerLabelPosition LabelPosition { get; set; } = SpinnerLabelPosition.Bottom;
        [Parameter] public SpinnerSize Size { get; set; } = SpinnerSize.Medium;
        [Parameter] public string? StatusMessage { get; set; }

        private string GetAriaLive()
        {
            return AriaLive switch
            {
                AriaLive.Polite => "polite",
                AriaLive.Assertive => "assertive",
                AriaLive.Off => "off",
                _ => "polite",
            };
        }

        private string GetPositionStyle()
        {
            return LabelPosition switch
            {
                SpinnerLabelPosition.Left => " ms-Spinner--left",
                SpinnerLabelPosition.Right => " ms-Spinner--right",
                SpinnerLabelPosition.Top => " ms-Spinner--top",
                _ => "",
            };
        }

        private string GetSpinnerSizeStyle()
        {
            return Size switch
            {
                SpinnerSize.Large => " ms-Spinner--large",
                SpinnerSize.Medium => " ms-Spinner--medium",
                SpinnerSize.Small => " ms-Spinner--small",
                SpinnerSize.XSmall => " ms-Spinner--xSmall",
                _ => " ms-Spinner--medium",
            };
        }
    }
}
