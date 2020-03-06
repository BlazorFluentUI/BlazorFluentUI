using BlazorFabric.Style;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class Nav: FabricComponentBase
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

        private ICollection<Rule> CreateGlobalCss()
        {
            var navRules = new HashSet<Rule>();
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
                          $"color:{Theme.SemanticTextColors.BodyText};" +
                          $"background-color:{Theme.SemanticColors.BodyBackground};"
                }
            });

            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-compositeLink.is-disabled" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.SemanticTextColors.DisabledText};" 
                }
            });


            var focusProps = new Style.FocusStyleProps(Theme);
            var focusStyles = BlazorFabric.Style.FocusStyle.GetFocusStyle(focusProps, ".ms-Nav-link");
            foreach (var rule in focusStyles.AddRules)
                navRules.Add(rule);

            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-link" },
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
                          $"color:{Theme.SemanticTextColors.BodyText};"
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
                    Css = $"background-color:{Theme.Palette.NeutralLighterAlt};"+
                          $"color:{Theme.SemanticTextColors.BodyText};"
                }
            });

            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-compositeLink.is-selected .ms-Nav-link" },
                Properties = new CssString()
                {
                    Css = $"background-color:{Theme.Palette.NeutralLighter};" +
                         $"color:{Theme.Palette.ThemePrimary};"
                }
            });

            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-compositeLink.is-selected .ms-Nav-link:after" },
                Properties = new CssString()
                {
                    Css = $"border-left:2px solid {Theme.Palette.NeutralLighter};" +
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
                    Css = $"color:{Theme.SemanticTextColors.DisabledText};"
                }
            });

            navRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Nav-compositeLink.is-button .ms-Nav-link" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.ThemePrimary};"
                }
            });

            return navRules;
        }


    }
}
