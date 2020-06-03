using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace BlazorFluentUI
{
    public partial class BFUDialogContent : BFUComponentBase, IHasPreloadableGlobalStyle
    {
        [Parameter] public string CloseButtonAriaLabel { get; set; } = "Close";  //need to localize
        [Parameter] public RenderFragment ContentTemplate { get; set; }
        [Parameter] public DialogType DialogType { get; set; } = DialogType.Normal;
        [Parameter] public string DraggableHeaderClassName { get; set; }
        [Parameter] public RenderFragment FooterTemplate { get; set; }
        [Parameter] public bool IsMultiline { get; set; }
        [Parameter] public EventCallback<EventArgs> OnDismiss { get; set; }
        [Parameter] public bool ShowCloseButton { get; set; }
        [Parameter] public string SubText { get; set; }
        [Parameter] public string SubTextId { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public string TitleId { get; set; }


        public ICollection<IRule> CreateGlobalCss(ITheme theme)
        {
            // ToDo Button Selector for Icon when hidden isn't implement so far
            // ToDo Headeer DraggableHeader isn't implement so far

            var GlobalCssRules = new HashSet<IRule>();
            #region ms-Dialog-content
            GlobalCssRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dialog-content.ms-Dialog--lgHeader" },
                Properties = new CssString()
                {
                    Css = $"border-top:4px solid {theme.Palette.ThemePrimary};"
                }
            });
            GlobalCssRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dialog-content.ms-Dialog--close" },
                Properties = new CssString()
                {
                    Css = $"flex-grow:1;" +
                        $"overflow-y:hidden;"
                }
            });
            #endregion
            #region ms-Dialog-subText
            GlobalCssRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dialog-subText" },
                Properties = new CssString()
                {
                    Css = $"margin:0 0 24px 0;" +
                        $"color:{theme.SemanticTextColors.ButtonText};" +  //Was BodySubText, but this defaults to a very dark color in darkmode... bug?  Same bug found in React components.
                        $"line-height:1.5;" +
                        $"word-break:break-word;" +
                        $"font-weight:{theme.FontStyle.FontWeight.Regular};"
                }
            });
            #endregion
            #region ms-Dialog-header
            GlobalCssRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dialog-header" },
                Properties = new CssString()
                {
                    Css = $"position:relative;" +
                        $"width:100%;" +
                        $"box-sizing:border-box;"
                }
            });
            #endregion
            #region ms-Dialog-inner
            GlobalCssRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dialog-inner" },
                Properties = new CssString()
                {
                    Css = $"padding:0 24px 24px;"
                }
            });
            GlobalCssRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $"@media (min-width:{theme.CommonStyle.ScreenWidthMinSmall}px) and (max-width:{theme.CommonStyle.ScreenWidthMaxSmall}px)" },
                Properties = new CssString()
                {
                    Css = ".ms-Dialog-inner{padding:0 16px 16px;}"
                }
            });
            #endregion 
            #region ms-Dialog-innerContent
            GlobalCssRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dialog-innerContent" },
                Properties = new CssString()
                {
                    Css = $"position:relative;" +
                        $"width:100%;"
                }
            });
            #endregion
            #region ms-Dialog-title
            GlobalCssRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dialog-title" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.BodyText};" +
                        $"margin:0;" +
                        $"padding:16px 46px 20px 24px;" +
                        $"line-height:normal;"
                }
            });
            GlobalCssRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dialog--lgHeader .ms-Dialog-title" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticColors.MenuHeader};"
                }
            });
            GlobalCssRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dialog--multiline .ms-Dialog-title" },
                Properties = new CssString()
                {
                    Css = $"font-size:{theme.FontStyle.FontSize.XxLarge};"
                }
            });
            GlobalCssRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $"@media (min-width:{theme.CommonStyle.ScreenWidthMinSmall}px) and (max-width:{theme.CommonStyle.ScreenWidthMaxSmall}px)" },
                Properties = new CssString()
                {
                    Css = ".ms-Dialog-title{padding:16px 46px 16px 16px;}"
                }
            });
            #endregion 
            #region ms-Dialog-topButton
            GlobalCssRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dialog-topButton" },
                Properties = new CssString()
                {
                    Css = $"display:flex;" +
                        $"flex-direction:row;" +
                        $"flex-wrap:nowrap;" +
                        $"position:absolute;" +
                        $"top:0;" +
                        $"right:0;" +
                        $"padding:15px 15px 0 0;"
                }
            });
            GlobalCssRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dialog-topButton > *" },
                Properties = new CssString()
                {
                    Css = $"flex:0 0 auto;"
                }
            });
            GlobalCssRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dialog-topButton.ms-Dialog-button" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.ButtonText};"
                }
            });
            GlobalCssRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dialog-topButton.ms-Dialog-button:hover" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.ButtonTextHovered};" +
                        $"border-radius:{theme.Effects.RoundedCorner2};"
                }
            });
            GlobalCssRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $"@media (min-width:{theme.CommonStyle.ScreenWidthMinSmall}px) and (max-width:{theme.CommonStyle.ScreenWidthMaxSmall}px)" },
                Properties = new CssString()
                {
                    Css = ".ms-Dialog-topButton{padding:15px 8px 0 0;}"
                }
            });
            #endregion
            // Footer
            #region ms-Dialog-actions
            GlobalCssRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dialog-actions" },
                Properties = new CssString()
                {
                    Css = $"position:relative;" +
                        $"width:100%;" +
                        $"min-height:24px;" +
                        $"line-height:24px;" +
                        $"margin:16px 0 0;" +
                        $"font-size:0;"
                }
            });
            GlobalCssRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dialog-actions .ms-Button" },
                Properties = new CssString()
                {
                    Css = $"line-height:normal;"
                }
            });
            #endregion
            #region ms-Dialog-actionsRight
            GlobalCssRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dialog-actionsRight" },
                Properties = new CssString()
                {
                    Css = $"display: flex;" +
                        $"flex-direction: row;" +
                        $"justify-content: flex-end;" +
                        $"margin-right:-4px;" +
                        $"font-size:0;"
                }
            });
            GlobalCssRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Dialog-actionsRight > * " },
                Properties = new CssString()
                {
                    Css = $"margin:0 4px;"
                }
            });
            #endregion
            return GlobalCssRules;
        }
    }
}
