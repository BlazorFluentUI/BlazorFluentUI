using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class Check : FabricComponentBase
    {
        [Parameter]
        public bool Checked { get; set; }

        [Parameter]
        public bool UseFastIcons { get; set; }

        private ICollection<Rule> CheckGlobalRules { get; set; } = new List<Rule>();

        protected override Task OnInitializedAsync()
        {
            if (!CStyle.ComponentStyleExist(this))
            {
                CreateCss();
            }
            return base.OnInitializedAsync();
        }
        protected override void OnThemeChanged()
        {
            CreateCss();
            base.OnThemeChanged();
        }

        protected void CreateCss()
        {
            CheckGlobalRules.Clear();

            CheckGlobalRules.Add(new Rule()
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

            CheckGlobalRules.Add(new Rule()
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
                          $"background:{Theme.SemanticColors.BodyBackground};" 
                }
            });

            CheckGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Check-checkHost:hover .ms-Check, .ms-Check-checkHost:focus .ms-Check, .ms-Check:focus" },
                Properties = new CssString()
                {
                    Css = $"opacity:1;" 
                }
            });

            CheckGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Check.is-checked:before" },
                Properties = new CssString()
                {
                    Css = $"content:'';" +
                          $"opacity:1;" +
                          $"background:{Theme.Palette.ThemePrimary};"
                }
            });

            CheckGlobalRules.Add(new Rule()
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
            CheckGlobalRules.Add(
              new Rule()
              {
                  Selector = new CssStringSelector() { SelectorName = ".ms-Check-circle" },
                  Properties = new CssString()
                  {
                      Css = $"color:{Theme.Palette.NeutralSecondary};" +
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
            CheckGlobalRules.Add(
              new Rule()
              {
                  Selector = new CssStringSelector() { SelectorName = ".ms-Check.is-checked .ms-Check-circle" },
                  Properties = new CssString()
                  {
                      Css = $"color:{Theme.Palette.White};" 
                  }
              });
            CheckGlobalRules.Add(new Rule()
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
            CheckGlobalRules.Add(
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
                          $"color:{Theme.Palette.NeutralSecondary};" +
                          $"font-size:{Theme.FontStyle.FontSize.Medium};" +
                          $"left:0.5px;" //isRTL ignored
                }
            });
            CheckGlobalRules.Add(
            new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Check-check:hover" },
                Properties = new CssString()
                {
                    Css = $"opacity:1;"
                }
            });
            CheckGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-Check-check {" +
                         $"-ms-high-contrast-adjust:none;" +
                         "}"
                }
            });
            CheckGlobalRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Check.is-checked .ms-Check-check" },
                Properties = new CssString()
                {
                    Css = $"opacity:1;"+
                            $"color:{Theme.Palette.White};"+
                            $"font-weight:900;"
                }
            });
            CheckGlobalRules.Add(new Rule()
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
        }
    }
}
