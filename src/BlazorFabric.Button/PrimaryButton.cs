using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public class PrimaryButton : ButtonBase
    {
        private ICollection<Rule> CreateGlobalCss()
        {
            var rules = CreateBaseGlobalCss();

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button--primary" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.White};" +
                          $"background-color:{Theme.Palette.ThemePrimary};" +
                          $"border:none;"+
                          $"border-width:0px;"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Fabric--isFocusVisible .ms-Button--primary:focus:after" },
                Properties = new CssString()
                {
                    Css = $"outline:none;" +
                          $"border-color:{Theme.Palette.White};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button--primary:hover" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.White};" +
                          $"background-color:{Theme.Palette.ThemeDarkAlt};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button--primary:active" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.White};" +
                         $"background-color:{Theme.Palette.ThemeDark};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button--primary.is-checked:not(.is-disabled)" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.White};" +
                         $"background-color:{Theme.Palette.ThemeDark};"+
                         $"border:none;"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button--primary.is-checked:hover" },
                Properties = new CssString()
                {
                    Css = $"background-color:{Theme.Palette.ThemePrimary};"+
                          $"color:{Theme.Palette.White};"+
                          $"border:none;"
                }
            });

            //rules.Add(new Rule()
            //{
            //    Selector = new CssStringSelector() { SelectorName = ".ms-Button--default.is-disabled" },
            //    Properties = new CssString()
            //    {
            //        Css = $"background-color:{Theme.Palette.NeutralLighter};" +
            //              $"boder-color:{Theme.Palette.NeutralLighter};"
            //    }
            //});

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-Button--primary{color: Window; background-color:WindowText;-ms-high-contrast-adjust:none;}"+
                          ".ms-Button--primary:hover{color: Window; background-color:Highlight;}"+
                          ".ms-Button--primary:active{color: Window; background-color:WindowText;-ms-high-contrast-adjust:none;}" +
                          ".ms-Button--primary.is-disabled{color: GrayText; background-color:Window;border-color:GrayText;}" 
                }
            });


            return rules;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenComponent<GlobalCS>(0);
            builder.AddAttribute(1, "Component", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Object>(this));
            builder.AddAttribute(2, "CreateGlobalCss", new System.Func<System.Collections.Generic.ICollection<BlazorFabric.Rule>>(CreateGlobalCss));
            builder.AddAttribute(3, "FixStyle", true);
            builder.CloseComponent();

            Primary = true;
            base.BuildRenderTree(builder);
            StartRoot(builder, "");
        }

    }
}
