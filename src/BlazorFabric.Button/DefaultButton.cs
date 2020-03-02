using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public class DefaultButton : ButtonBase
    {
        private ICollection<Rule> CreateGlobalCss()
        {
            var rules = CreateBaseGlobalCss();

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button--default" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.SemanticTextColors.ButtonText};" +
                          $"background-color:{Theme.Palette.White};" +
                          $"border:1px solid {Theme.Palette.NeutralSecondaryAlt};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button--default:hover" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.SemanticTextColors.ButtonTextHovered};" +
                          $"background-color:{Theme.Palette.NeutralLighter};" 
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button--default:active" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.SemanticTextColors.ButtonTextChecked};" +
                         $"background-color:{Theme.Palette.NeutralLight};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button--default.is-checked:not(.is-disabled)" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.SemanticTextColors.ButtonTextChecked};" +
                         $"background-color:{Theme.Palette.NeutralLight};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button--default.is-checked:hover" },
                Properties = new CssString()
                {
                    Css = $"background-color:{Theme.Palette.NeutralLight};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button--default.is-disabled" },
                Properties = new CssString()
                {
                    Css = $"background-color:{Theme.Palette.NeutralLighter};"+
                          $"border-color:{Theme.Palette.NeutralLighter};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-Button--default{color: Highlight; border-color:Highlight;}"
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

            base.BuildRenderTree(builder);
            StartRoot(builder, "");
        }

      

    }
}
