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

        private ICollection<Rule> SpinnerRules { get; set; } = new List<Rule>();

        protected override void OnInitialized()
        {
            CreateCss();
            base.OnInitialized();
        }

        protected override void OnThemeChanged()
        {
            CreateCss();
            base.OnThemeChanged();
        }

        private string GetAriaLive()
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

        private string GetPositionStyle()
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

        private string GetSpinnerSizeStyle()
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
        private void CreateCss()
        {
            SpinnerRules.Clear();
            SpinnerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Spinner" },
                Properties = new CssString()
                {
                    Css = $"display:flex;" +
                            "flex-direction:column;" +
                            "align-items:center;" +
                            "justify-content:center;"
                }
            });

            SpinnerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Spinner--top" },
                Properties = new CssString()
                {
                    Css = $"flex-direction:column-reverse;"
                }
            });
            SpinnerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Spinner--left" },
                Properties = new CssString()
                {
                    Css = $"flex-direction:row-reverse;"
                }
            });
            SpinnerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Spinner--right" },
                Properties = new CssString()
                {
                    Css = "flex-direction:row;"
                }
            });
            SpinnerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Spinner-circle" },
                Properties = new CssString()
                {
                    Css = $"box-sizing:border-box;" +
                    $"border-radius:50%;" +
                    $"border:1.5px solid {Theme.Palette.ThemeLight};" +
                    $"border-top-color:{Theme.Palette.ThemePrimary};" +
                    $"animation-name:spinAnimation;" +
                    $"animation-duration:1.3s;" +
                    $"animation-iteration-count:infinite;" +
                    $"animation-timing-function:cubic-bezier(.53,.21,.29,.67);"
                }
            });
            SpinnerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Spinner--xSmall" },
                Properties = new CssString()
                {
                    Css = $"width:12px;" +
                            $"height:12px;"
                }
            });
            SpinnerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Spinner--small" },
                Properties = new CssString()
                {
                    Css = $"width:16px;" +
                            $"height:16px;"
                }
            });
            SpinnerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Spinner--medium" },
                Properties = new CssString()
                {
                    Css = $"width:20px;" +
                            $"height:20px;"
                }
            });
            SpinnerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Spinner--large" },
                Properties = new CssString()
                {
                    Css = $"width:28px;" +
                            $"height:28px;"
                }
            });
            SpinnerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-Spinner-circle{border-top-color:Highlight;}"
                }
            });
            SpinnerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Spinner-label" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.ThemePrimary};" +
                            $"margin:8px 0 0 0;" +
                            $"text-align:center;"
                }
            });
            SpinnerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Spinner--top .ms-Spinner-label" },
                Properties = new CssString()
                {
                    Css = $"margin: 0 0 8px 0;"
                }
            });
            SpinnerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Spinner--right .ms-Spinner-label" },
                Properties = new CssString()
                {
                    Css = $"margin: 0 0 0 8px;"
                }
            });
            SpinnerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Spinner--left .ms-Spinner-label" },
                Properties = new CssString()
                {
                    Css = $"margin: 0 8px 0 0;"
                }
            });

            //Label enable && unchecked
            SpinnerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Spinner-screenReaderText" },
                Properties = new CssString()
                {
                    Css = $"position:absolute;" +
                            "width:1px;" +
                            "height:1px;" +
                            "margin:-1px;" +
                            "padding:0;" +
                            "border:0;" +
                            "overflow:hidden;"
                }
            });
            SpinnerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@keyframes spinAnimation" },
                Properties = new CssString()
                {
                    Css = "0%{transform: rotate(0deg);} 100%{transform:rotate(360deg);}"
                }
            });
        }
    }
}
