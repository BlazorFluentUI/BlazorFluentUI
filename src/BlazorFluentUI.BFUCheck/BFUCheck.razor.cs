﻿using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BlazorFluentUI
{
    public partial class BFUCheck : BFUComponentBase, IHasPreloadableGlobalStyle
    {
        [Parameter]
        public bool Checked { get; set; }

        [Parameter]
        public bool UseFastIcons { get; set; }

        public ICollection<IRule> CreateGlobalCss(ITheme theme)
        {
            var checkRules = new HashSet<IRule>();

            checkRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Check" },
                Properties = new CssString()
                {
                    Css = $"line-height:1;" +
                          $"width:18px;" +
                          $"height:18px;" +
                          $"vertical-align:top;" +
                          $"position:relative;" +
                          $"user-select:none;"

                }
            });

            checkRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Check:before" },
                Properties = new CssString()
                {
                    Css = $"content:'';" +
                          $"position:absolute;" +
                          $"top:1px;" +
                          $"right:1px;" +
                          $"bottom:1px;" +
                          $"left:1px;" +
                          $"border-radius:50%;" +
                          $"opacity:1;" +
                          $"background:{theme.SemanticColors.BodyBackground};" 
                }
            });

            checkRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Check-checkHost:hover .ms-Check, .ms-Check-checkHost:focus .ms-Check, .ms-Check:focus" },
                Properties = new CssString()
                {
                    Css = $"opacity:1;" 
                }
            });

            checkRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Check.is-checked:before" },
                Properties = new CssString()
                {
                    Css = $"content:'';" +
                          $"opacity:1;" +
                          $"background:{theme.Palette.ThemePrimary};"
                }
            });

            checkRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-Check.is-checked:before {" +
                         $"background:Window;" +
                         "}"
                }
            });

            //Circle            
            checkRules.Add(
              new Rule()
              {
                  Selector = new CssStringSelector() { SelectorName = ".ms-Check-circle" },
                  Properties = new CssString()
                  {
                      Css = $"color:{theme.Palette.NeutralSecondary};" +
                            $"font-size:18px;" +
                            $"position:absolute;" +
                            $"left:0px;" +
                            $"top:0px;" +
                            $"width:18px;" +
                            $"height:18px;" +
                            $"text-align:center;" +
                            $"vertical-align:middle;"
                  }
              });
            checkRules.Add(
              new Rule()
              {
                  Selector = new CssStringSelector() { SelectorName = ".ms-Check.is-checked .ms-Check-circle" },
                  Properties = new CssString()
                  {
                      Css = $"color:{theme.Palette.White};" 
                  }
              });
            checkRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-Check-circle.is-checked:before {" +
                         $"color:WindowText;" +
                         "}"
                }
            });

            //Check
            checkRules.Add(
            new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Check-check" },
                Properties = new CssString()
                {
                    Css = $"font-size:18px;" +
                          $"position:absolute;" +
                          $"left:0px;" +
                          $"top:0px;" +
                          $"width:18px;" +
                          $"height:18px;" +
                          $"text-align:center;" +
                          $"vertical-align:middle;" +
                          $"opacity:0;" +
                          $"color:{theme.Palette.NeutralSecondary};" +
                          $"font-size:{theme.FontStyle.FontSize.MediumPlus};" +  //This is technically IconFontSize.Medium which is equivalent to FontSize.MediumPlus (16px)
                          $"left:0.5px;" //isRTL ignored
                }
            });
            checkRules.Add(
            new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Check-check:hover" },
                Properties = new CssString()
                {
                    Css = $"opacity:1;"
                }
            });
            checkRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-Check-check {" +
                         $"-ms-high-contrast-adjust:none;" +
                         "}"
                }
            });
            checkRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Check.is-checked .ms-Check-check" },
                Properties = new CssString()
                {
                    Css = $"opacity:1;"+
                            $"color:{theme.Palette.White};"+
                            $"font-weight:900;"
                }
            });
            checkRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-Check.is-checked .ms-Check-check {" +
                         $"border:none;" +
                         $"color:WindowText;"+
                         "}"
                }
            });

            return checkRules;
        }
    }
}
