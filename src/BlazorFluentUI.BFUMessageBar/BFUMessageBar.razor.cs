using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class BFUMessageBar : BFUComponentBase
    {
        [Parameter]
        public bool IsMultiline { get; set; } = true;

        //[Parameter]
        //public MessageBarBase ComponentRef { get; set; }

        [Parameter]
        public MessageBarType MessageBarType { get; set; } = MessageBarType.Info;

        [Parameter]
        public bool Truncated { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public RenderFragment Actions { get; set; }

        [Parameter]
        public string DismissButtonAriaLabel { get; set; }

        [Parameter]
        public string OverflowButtonAriaLabel { get; set; }

        [Parameter]
        public EventCallback OnDismiss { get; set; }

        [Parameter]
        public BFUMessageBar ComponentRef
        {
            get => componentRef;
            set
            {
                if (value == componentRef)
                    return;
                componentRef = value;
                MessageBarType = value.MessageBarType;
                ComponentRefChanged.InvokeAsync(value);
            }
        }

        [Parameter]
        public EventCallback<BFUMessageBar> ComponentRefChanged { get; set; }

        private BFUMessageBar componentRef;

        protected bool HasDismiss { get => (OnDismiss.HasDelegate); }

        protected bool HasExpand { get => (Truncated && Actions == null); }

        protected bool ExpandSingelLine { get; set; }

        protected void Truncate()
        {
            ExpandSingelLine = !ExpandSingelLine;
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {

            if (firstRender)
                ComponentRef = this as BFUMessageBar;
            return base.OnAfterRenderAsync(firstRender);
        }

        protected string GetTypeCss()
        {
            switch (MessageBarType)
            {
                case MessageBarType.Warning:
                    return " ms-MessageBar--warning ";
                case MessageBarType.Error:
                    return " ms-MessageBar--error ";
                case MessageBarType.Blocked:
                    return " ms-MessageBar--blocked ";
                case MessageBarType.SevereWarning:
                    return " ms-MessageBar--severeWarning ";
                case MessageBarType.Success:
                    return " ms-MessageBar--success ";
                default:
                    return "";
            }
        }

        private ICollection<IRule> CreateGlobalCss()
        {
            // ToDo SmallScreenSelector for innerText isn't implement so far

            var messageBarGlobalRules = new HashSet<IRule>();
            #region ms-MessageBar
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar" },
                Properties = new CssString()
                {
                    Css = $"background:{Theme.Palette.NeutralLighter};" +
                        $"color:{Theme.Palette.NeutralPrimary};" +
                        $"min-height:32px;" +
                        $"width:100%;" +
                        $"display:flex;" + 
                        $"word-break:break-word;"
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar.ms-MessageBar--error, .ms-MessageBar.ms-MessageBar--blocked" },
                Properties = new CssString()
                {
                    Css = $"background:{Theme.SemanticColors.ErrorBackground};"
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar.ms-MessageBar--severeWarning" },
                Properties = new CssString()
                {
                    Css = $"background:{Theme.SemanticColors.BlockingBackground};"
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar.ms-MessageBar--success" },
                Properties = new CssString()
                {
                    Css = $"background:{Theme.SemanticColors.SuccessBackground};"
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar.ms-MessageBar--warning" },
                Properties = new CssString()
                {
                    Css = $"background:{Theme.SemanticColors.WarningBackground};"
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar.ms-MessageBar-multiline" },
                Properties = new CssString()
                {
                    Css = $" flex-direction:column;"
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar .ms-Link" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.ThemeDark};" +
                        $"font-size:{Theme.FontStyle.FontSize.Small};" +
                        $"font-weight:{Theme.FontStyle.FontWeight.Regular};"
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-MessageBar .ms-Link {-ms-high-contrast-adjust:auto;}" +
                        ".ms-MessageBar.ms-MessageBar--error, .ms-MessageBar.ms-MessageBar--blocked, .ms-MessageBar.ms-MessageBar--severeWarning {background:rgba(255, 0, 0, 0.3); border:1px solid WindowText; color:WindowText}" +
                        ".ms-MessageBar.ms-MessageBar--success {background:rgba(48, 241, 73, 0.3); border:1px solid WindowText; color:WindowText}" +
                        ".ms-MessageBar.ms-MessageBar--warning {background:rgba(255, 254, 57, 0.3); border:1px solid WindowText; color:WindowText}" +
                        ".ms-MessageBar {background:Window; border:1px solid WindowText; color:WindowText}" 
                }
            });
            #endregion
            #region ms-MessageBar-content
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar-content" },
                Properties = new CssString()
                {
                    Css = $"display: flex;" +
                            $"width: 100%;" +
                            $"line-height: normal;"
                }
            });
            #endregion
            #region ms-MessageBar-icon
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar-icon" },
                Properties = new CssString()
                {
                    Css = $"font-size:{Theme.FontStyle.IconFontSize.Medium};" +
                            $"min-width:16px;" +
                            $"min-height:16px;" +
                            $"display:flex;" +
                            $"flex-shrink:0;" +
                            $"margin:8px 0 8px 12px"
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar-icon .ms-Icon" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.NeutralSecondary};"
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar-icon.ms-MessageBar--error .ms-Icon, .ms-MessageBar-icon.ms-MessageBar--blocked .ms-Icon, .ms-MessageBar-icon.ms-MessageBar--severeWarning .ms-Icon" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.SemanticTextColors.ErrorText};"
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar-icon.ms-MessageBar--success .ms-Icon" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.Green};"
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar-icon.ms-MessageBar--warning .ms-Icon" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.SemanticTextColors.WarningText};"
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-MessageBar-icon {-ms-high-contrast-adjust:none;color:WindowText}"
                }
            });
            #endregion
            #region ms-MessageBar-text
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar-text" },
                Properties = new CssString()
                {
                    Css = $"min-width:0;" +
                            $"display:flex;" +
                            $"flex-grow:1;" +
                            $"margin:8px;" +
                            $"font-size:{Theme.FontStyle.FontSize.Small};" +
                            $"font-weight:{Theme.FontStyle.FontWeight.Regular};"
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar-text:not(.ms-MessageBar-dismissSingleLine)" },
                Properties = new CssString()
                {
                    Css = $"margin-right:12px;"
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-MessageBar-text {-ms-high-contrast-adjust:none;}"
                }
            });
            #endregion
            #region ms-MessageBar-innerText
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar-innerText" },
                Properties = new CssString()
                {
                    Css = $"line-height:16px;"
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar-innerText span, .ms-MessageBar-innerText a" },
                Properties = new CssString()
                {
                    Css = $"padding-left:4px;"
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar-innerText.ms-MessageBar-expandSingleLine" },
                Properties = new CssString()
                {
                    Css = $"overflow:visible;" +
                            $"white-space:normal;"
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar-innerText:not(.ms-MessageBar-multiline):not(.ms-MessageBar-expandSingleLine)" },
                Properties = new CssString()
                {
                    Css = $"overflow:hidden;" +
                            $"text-overflow:ellipsis;" +
                            $"white-space:nowrap;"
                }
            });
            #endregion
            #region ms-MessageBar-dismissal
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar-dismissal" },
                Properties = new CssString()
                {
                    Css = $"flex-shrink:0;" +
                            $"width:32px;" +
                            $"height:32px;" +
                            $"padding:8px 12px;"
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar-dismissal .ms-Button-icon" },
                Properties = new CssString()
                {
                    Css = $"font-size:{Theme.FontStyle.FontSize.XSmall};" +
                            $"height:10px;" +
                            $"line-height:10px;" +
                            $"color:{Theme.Palette.NeutralPrimary});"
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar-dismissal.ms-Button--icon:hover" },
                Properties = new CssString()
                {
                    Css = $"background-color:transparent;"
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar-dismissal.ms-Button--icon:active" },
                Properties = new CssString()
                {
                    Css = $"background-color:transparent;"
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-MessageBar-dismissal .ms-Button-icon {-ms-high-contrast-adjust:none;color:WindowText}"
                }
            });
            #endregion
            #region ms-MessageBar-expand
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar-expand" },
                Properties = new CssString()
                {
                    Css = $"flex-shrink:0;" +
                            $"width:32px;" +
                            $"height:32px;" +
                            $"padding:8px 12px;"
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar-expand .ms-Button-icon" },
                Properties = new CssString()
                {
                    Css = $"font-size:{Theme.FontStyle.FontSize.XSmall};" +
                            $"height:10px;" +
                            $"line-height:10px;" +
                            $"color:{Theme.Palette.NeutralPrimary});"
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar-expand.ms-Button--icon:hover" },
                Properties = new CssString()
                {
                    Css = $"background-color:transparent;"
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar-expand.ms-Button--icon:active" },
                Properties = new CssString()
                {
                    Css = $"background-color:transparent;"
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-MessageBar-expand .ms-Button-icon {-ms-high-contrast-adjust:none;color:WindowText}"
                }
            });
            #endregion
            #region ms-MessageBar-actions
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar-actions" },
                Properties = new CssString()
                {
                    Css = $"display: flex;" +
                            $"flex-grow:0;" +
                            $"flex-shrink:0;" +
                            $"flex-basis:auto;" +
                            $"flex-Direction:row-reverse;" +
                            $"align-items:center;" + 
                            $"margin:0 12px 8px 8px; "
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar-actions button:nth-child(n+2)" },
                Properties = new CssString()
                {
                    Css = $"margin-left:8px;"
                }
            });
            #endregion
            #region ms-MessageBar-actionsSingleLine
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar-actionsSingleLine" },
                Properties = new CssString()
                {
                    Css = $"display: flex;" +
                            $"flex-grow:0;" +
                            $"flex-shrink:0;" +
                            $"flex-basis:auto;" +
                            $"flex-Direction:row-reverse;" +
                            $"align-items:center;" +
                            $"margin:0 12px 0px 8px; "
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar-actionsSingleLine button:nth-child(n+2)" },
                Properties = new CssString()
                {
                    Css = $"margin-left:8px;"
                }
            });
            messageBarGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-MessageBar-actionsSingleLine .ms-MessageBar-dismissSingleLine" },
                Properties = new CssString()
                {
                    Css = $"margin-right:0;"
                }
            });
            #endregion
            return messageBarGlobalRules;
        }
    }
}
