using BlazorFluentUI.Style;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class BFUNav: BFUComponentBase, IHasPreloadableGlobalStyle
    {
        [Parameter] public RenderFragment ChildContent { get; set; }
        //[Parameter] public string AriaLabel { get; set; }
        [Parameter] public string ExpandButtonAriaLabel { get; set; }

        [Parameter] public bool IsOnTop { get; set; }

        protected override Task OnInitializedAsync()
        {
            //System.Diagnostics.Debug.WriteLine("Initializing NavBase");
            return base.OnInitializedAsync();
        }

        protected override Task OnParametersSetAsync()
        {
            return base.OnParametersSetAsync();
        }

        public void ManuallyRefresh()
        {
            StateHasChanged();
        }

        public ICollection<IRule> CreateGlobalCss(ITheme theme)
        {
            var navRules = new HashSet<IRule>();
            // ROOT
            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav" },
                Properties = new CssString()
                {
                    Css = $"overflow-y:auto;" +
                          $"user-select:none;"+
                          $"-webkit-overflow-scrolling:touch;"
                }
            });
            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav.is-on-top" },
                Properties = new CssString()
                {
                    Css = $"position:absolute;"
                }
            });

            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-linkText" },
                Properties = new CssString()
                {
                    Css = $"margin:0px 4px;"+
                          $"overflow:hidden;"+
                          $"vertical-align:middle;" +
                          $"text-align:left;" +
                          $"text-overflow:ellipsis;"
                }
            });

            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-compositeLink" },
                Properties = new CssString()
                {
                    Css = $"display:block;" +
                          $"position:relative;" +
                          $"color:{theme.SemanticTextColors.BodyText};" +
                          $"background-color:{theme.SemanticColors.BodyBackground};"
                }
            });

            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-compositeLink.is-disabled" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.DisabledText};" 
                }
            });


            var focusProps = new Style.FocusStyleProps(theme);
            var focusStyles = BlazorFluentUI.Style.FocusStyle.GetFocusStyle(focusProps, ".ms-Nav .ms-Nav-compositeLink .ms-Nav-link");
            foreach (var rule in focusStyles.AddRules)
                navRules.Add(rule);

            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav .ms-Nav-compositeLink .ms-Nav-link" },
                Properties = new CssString()
                {
                    Css = focusStyles.MergeRules + 
                          $"display:block;" +
                          $"position:relative;" +
                          $"height:44px;" +
                          $"width:100%;" +
                          $"line-height:44px;" +
                          $"text-decoration:none;" +
                          $"cursor:pointer;" +
                          $"text-overflow:ellipsis;" +
                          $"white-space:nowrap;" +
                          $"overflow:hidden;" +
                          $"padding-left:20px;" +
                          $"padding-right:20px;" +
                          $"color:{theme.SemanticTextColors.BodyText};"
                }
            });

            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-Nav-link {border-color:transparent;}"+
                          ".ms-Nav-link:focus {border-color:WindowText;}"
                }
            });

            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-compositeLink:hover:not(.is-disabled) .ms-Nav-link" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.SemanticColors.BodyBackgroundHovered};"+
                          $"color:{theme.SemanticTextColors.BodyText};"
                }
            });

            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-compositeLink.is-selected .ms-Nav-link" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.SemanticColors.BodyBackgroundChecked};" +
                          $"font-weight:{theme.FontStyle.FontWeight.SemiBold};"+
                         $"color:{theme.SemanticTextColors.BodyTextChecked};"
                }
            });

            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-compositeLink.is-selected .ms-Nav-link::after" },
                Properties = new CssString()
                {
                    Css = $"border-left:2px solid {theme.Palette.ThemePrimary};" +
                          $"content:'';" +
                          $"position:absolute;" +
                          $"top:0;" +
                          $"bottom:0;" +
                          $"left:0;" +
                          $"right:0;" +
                          $"pointer-events:none;" 
                }
            });

            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-compositeLink.is-disabled .ms-Nav-link" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.DisabledText};"
                }
            });

            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-compositeLink.is-button .ms-Nav-link" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.ThemePrimary};"
                }
            });

            //var focusProps = new Style.FocusStyleProps(theme);
            var chevronButtonFocusStyles = BlazorFluentUI.Style.FocusStyle.GetFocusStyle(focusProps, ".ms-Nav-chevronButton");
            foreach (var rule in chevronButtonFocusStyles.AddRules)
                navRules.Add(rule);

            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-chevronButton" },
                Properties = new CssString()
                {
                    Css = chevronButtonFocusStyles.MergeRules +
                          $"display:block;" +
                          $"text-align:left;" +
                          $"line-height:44px;" +
                          $"font-weight:{theme.FontStyle.FontWeight.Regular};"+
                          $"font-size:{theme.FontStyle.FontSize.Small};" +
                          $"margin:5px 0px;" +
                          $"padding:0px 20px 0px 28px;" +
                          $"border:none;" +
                          $"text-overflow:ellipsis;" +
                          $"white-space:nowrap;" +
                          $"overflow:hidden;" +
                          $"cursor:pointer;" +
                          $"color:{theme.SemanticTextColors.BodyText};"+
                          $"background-color:transparent;"
                }
            });
            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-chevronButton:visited" },
                Properties = new CssString()
                {
                    Css = "color:inherit;"
                }
            });
            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-chevronButton:hover" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.BodyText};"+
                          $"background-color:{theme.SemanticColors.BodyBackgroundHovered};"
                }
            });
            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-compositeLink:hover .ms-Nav-chevronButton" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.BodyText};" +
                          $"background-color:{theme.SemanticColors.BodyBackgroundHovered};"
                }
            });
            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-chevronButton.is-group" },
                Properties = new CssString()
                {
                    Css = $"width:100%;" +
                          $"height:44px;"+
                          $"border-bottom:1px solid {theme.SemanticColors.BodyDivider};"
                }
            });

            navRules.Add(new Rule()
            {
                /* i.e. is a link */
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-compositeLink:not(.is-button) .ms-Nav-chevronButton" },
                Properties = new CssString()
                {
                    Css = $"display:block;"+
                          $"width:26px;" +
                          $"height:42px;" +
                          $"position:absolute;" +
                          $"top:1px;" +
                          //$"left:1px;" +
                          $"z-index:{theme.ZIndex.Nav};" +
                          $"padding:0;" +
                          $"margin:0;" 
                }
            });
            //navRules.Add(new Rule()
            //{
            //    Selector = new CssStringSelector() { SelectorName = ".ms-Nav-compositeLink:not(.is-button).depth-one .ms-Nav-chevronButton" },
            //    Properties = new CssString()
            //    {
            //        Css = $"left:15px;"
            //    }
            //});
            //navRules.Add(new Rule()
            //{
            //    Selector = new CssStringSelector() { SelectorName = ".ms-Nav-compositeLink:not(.is-button).depth-two .ms-Nav-chevronButton" },
            //    Properties = new CssString()
            //    {
            //        Css = $"left:29px;"
            //    }
            //});
            //navRules.Add(new Rule()
            //{
            //    Selector = new CssStringSelector() { SelectorName = ".ms-Nav-compositeLink:not(.is-button).depth-three .ms-Nav-chevronButton" },
            //    Properties = new CssString()
            //    {
            //        Css = $"left:43px;"
            //    }
            //});
            //navRules.Add(new Rule()
            //{
            //    Selector = new CssStringSelector() { SelectorName = ".ms-Nav-compositeLink:not(.is-button).depth-four .ms-Nav-chevronButton" },
            //    Properties = new CssString()
            //    {
            //        Css = $"left:57px;"
            //    }
            //});

            //navRules.Add(new Rule()
            //{
            //    Selector = new CssStringSelector() { SelectorName = ".ms-Nav .ms-Nav-compositeLink .ms-Nav-link" },
            //    Properties = new CssString()
            //    {
            //        Css = $"padding-left:27px;"
            //    }
            //});
            //navRules.Add(new Rule()
            //{
            //    Selector = new CssStringSelector() { SelectorName = ".ms-Nav .ms-Nav-compositeLink.depth-one .ms-Nav-link" },
            //    Properties = new CssString()
            //    {
            //        Css = $"padding-left:41px;"
            //    }
            //});
            //navRules.Add(new Rule()
            //{
            //    Selector = new CssStringSelector() { SelectorName = ".ms-Nav .ms-Nav-compositeLink.depth-two .ms-Nav-link" },
            //    Properties = new CssString()
            //    {
            //        Css = $"padding-left:55px;"
            //    }
            //});
            //navRules.Add(new Rule()
            //{
            //    Selector = new CssStringSelector() { SelectorName = ".ms-Nav .ms-Nav-compositeLink.depth-three .ms-Nav-link" },
            //    Properties = new CssString()
            //    {
            //        Css = $"padding-left:79px;"
            //    }
            //});
            //navRules.Add(new Rule()
            //{
            //    Selector = new CssStringSelector() { SelectorName = ".ms-Nav .ms-Nav-compositeLink.depth-four .ms-Nav-link" },
            //    Properties = new CssString()
            //    {
            //        Css = $"padding-left:83px;"
            //    }
            //});
            //navRules.Add(new Rule()
            //{
            //    Selector = new CssStringSelector() { SelectorName = ".ms-Nav .ms-Nav-compositeLink.depth-five .ms-Nav-link" },
            //    Properties = new CssString()
            //    {
            //        Css = $"padding-left:101px;"
            //    }
            //});


            //navRules.Add(new Rule()
            //{
            //    Selector = new CssStringSelector() { SelectorName = ".ms-Nav .ms-Nav-compositeLink .ms-Nav-link.has-icon" },
            //    Properties = new CssString()
            //    {
            //        Css = $"padding-left:3px;"
            //    }
            //});
            //navRules.Add(new Rule()
            //{
            //    Selector = new CssStringSelector() { SelectorName = ".ms-Nav .ms-Nav-compositeLink.depth-one .ms-Nav-link.has-icon" },
            //    Properties = new CssString()
            //    {
            //        Css = $"padding-left:17px;"
            //    }
            //});
            //navRules.Add(new Rule()
            //{
            //    Selector = new CssStringSelector() { SelectorName = ".ms-Nav .ms-Nav-compositeLink.depth-two .ms-Nav-link.has-icon" },
            //    Properties = new CssString()
            //    {
            //        Css = $"padding-left:31px;"
            //    }
            //});
            //navRules.Add(new Rule()
            //{
            //    Selector = new CssStringSelector() { SelectorName = ".ms-Nav .ms-Nav-compositeLink.depth-three .ms-Nav-link.has-icon" },
            //    Properties = new CssString()
            //    {
            //        Css = $"padding-left:45px;"
            //    }
            //});
            //navRules.Add(new Rule()
            //{
            //    Selector = new CssStringSelector() { SelectorName = ".ms-Nav .ms-Nav-compositeLink.depth-four .ms-Nav-link.has-icon" },
            //    Properties = new CssString()
            //    {
            //        Css = $"padding-left:59px;"
            //    }
            //});
            //navRules.Add(new Rule()
            //{
            //    Selector = new CssStringSelector() { SelectorName = ".ms-Nav .ms-Nav-compositeLink.depth-five .ms-Nav-link.has-icon" },
            //    Properties = new CssString()
            //    {
            //        Css = $"padding-left:73px;"
            //    }
            //});


            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav .ms-Nav-compositeLink.is-selected .ms-Nav-chevronButton" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.ThemePrimary};"+
                          $"background-color:{theme.Palette.NeutralLighterAlt};"
                }
            });
            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav .ms-Nav-compositeLink.is-selected .ms-Nav-chevronButton::after" },
                Properties = new CssString()
                {
                    Css = $"border-left:2px solid {theme.Palette.ThemePrimary};" +
                          $"content:'';"+
                          $"position:absolute;" +
                          $"top:0;" +
                          $"right:0;" +
                          $"bottom:0;" +
                          $"left:0;" +
                          $"pointer-events:none;"

                }
            });

            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-chevron" },
                Properties = new CssString()
                {
                    Css = $"position:absolute;" +
                          $"left:8px;" +
                          $"height:36px;" +
                          $"line-height:36px;" +
                          $"font-size:{theme.FontStyle.FontSize.Small};" +
                          $"transition:transform .1s linear;"
                }
            });
            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-compositeLink.is-expanded > * > .ms-Nav-chevron" },
                Properties = new CssString()
                {
                    Css = $"transform:rotate(-180deg);"
                }
            });
            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-compositeLink:not(.is-button) > * > .ms-Nav-chevron" },
                Properties = new CssString()
                {
                    Css = $"top:0;"
                }
            });
            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-group.is-expanded > * > .ms-Nav-chevron" },
                Properties = new CssString()
                {
                    Css = $"transform:rotate(-180deg);"
                }
            });
            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-group:not(.is-button) > * > .ms-Nav-chevron" },
                Properties = new CssString()
                {
                    Css = $"top:0;"
                }
            });


            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-navItem" },
                Properties = new CssString()
                {
                    Css = $"padding:0;"
                }
            });
            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-navItems" },
                Properties = new CssString()
                {
                    Css = $"padding:0;"+
                          $"list-style-type:none;"
                }
            });

            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-group" },
                Properties = new CssString()
                {
                    Css = $"position:relative;"
                }
            });

            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-groupContent" },
                Properties = new CssString()
                {
                    Css = $"display:none;" +
                          $"margin-bottom:40px;"
                }
            });

            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-group.is-expanded .ms-Nav-groupContent" },
                Properties = new CssString()
                {
                    Css = $"display:block;"
                }
            });


            return navRules;
        }


    }
}
