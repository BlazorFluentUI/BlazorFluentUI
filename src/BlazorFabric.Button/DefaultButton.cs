using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public class DefaultButton : ButtonBase, IHasPreloadableGlobalStyle
    {
        public virtual ICollection<Rule> CreateGlobalCss(ITheme theme)
        {
            var rules = CreateBaseGlobalCss(theme);

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button--default" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.ButtonText};" +
                          $"background-color:{theme.Palette.White};" +
                          $"border:1px solid {theme.Palette.NeutralSecondaryAlt};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button--default:hover" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.ButtonTextHovered};" +
                          $"background-color:{theme.Palette.NeutralLighter};" 
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button--default:active" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.ButtonTextChecked};" +
                         $"background-color:{theme.Palette.NeutralLight};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button--default.is-checked:not(.is-disabled)" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.ButtonTextChecked};" +
                         $"background-color:{theme.Palette.NeutralLight};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button--default.is-checked:hover" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.Palette.NeutralLight};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button--default.is-disabled" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.Palette.NeutralLighter};"+
                          $"border-color:{theme.Palette.NeutralLighter};"
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
            builder.AddAttribute(2, "CreateGlobalCss", new System.Func<System.Collections.Generic.ICollection<BlazorFabric.Rule>>(()=>CreateGlobalCss(Theme)));
            builder.AddAttribute(3, "FixStyle", true);
            builder.CloseComponent();

            base.BuildRenderTree(builder);
            StartRoot(builder, "");
        }

      

    }
}
