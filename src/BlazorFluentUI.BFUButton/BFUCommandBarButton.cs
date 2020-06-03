using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;

namespace BlazorFluentUI
{
    public class BFUCommandBarButton : BFUButtonBase, IHasPreloadableGlobalStyle
    {
        public ICollection<IRule> CreateGlobalCss(ITheme theme)
        {
            var rules = CreateBaseGlobalCss(theme);

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--commandBar" },
                Properties = new CssString()
                {
                    Css = $"min-width:40px;" +
                          $"padding:0px 4px;" +
                          $"height:40px;" +
                          $"color:{theme.Palette.NeutralPrimary};"+
                          $"border:none;"+
                          $"border-radius:0;"+
                          $"background-color:{theme.Palette.White}"
                }
            });


            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--commandBar:hover" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.NeutralDark};"+
                          $"background-color:{theme.Palette.NeutralLighter}"
                }
            });
            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--commandBar .ms-Button-icon:hover" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.ThemeDarkAlt};"
                }
            });
            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--commandBar .ms-Button-menuIcon:hover" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.NeutralPrimary};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--commandBar:active" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.Palette.NeutralLight};"+
                          $"color:{theme.Palette.Black};"
                }
            });
            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--commandBar:active .ms-Button-icon" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.ThemeDarkAlt};"
                }
            });
            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--commandBar:active .ms-Button-menuIcon" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.NeutralPrimary};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--commandBar.is-checked:not(.is-disabled)" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.Palette.NeutralLight};" +
                          $"color:{theme.Palette.NeutralDark};"
                }
            });
            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--commandBar.is-checked:not(.is-disabled) .ms-Button-icon" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.ThemeDark};"
                }
            });
            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--commandBar.is-checked:not(.is-disabled) .ms-Button-menuIcon" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.NeutralPrimary};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--commandBar.is-checked:not(.is-disabled):hover" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.Palette.NeutralQuaternaryAlt};"
                }
            });
            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--commandBar.is-checked:not(.is-disabled):hover .ms-Button-icon" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.ThemeDark};"
                }
            });
            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--commandBar.is-checked:not(.is-disabled):hover .ms-Button-menuIcon" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.NeutralPrimary};"
                }
            });


            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--commandBar.is-expanded)" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.Palette.NeutralLight};" +
                          $"color:{theme.Palette.NeutralDark};"
                }
            });
            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--commandBar.is-expanded .ms-Button-icon" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.ThemeDark};"
                }
            });
            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--commandBar.is-expanded .ms-Button-menuIcon" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.NeutralPrimary};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--commandBar.is-expanded:hover)" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.Palette.NeutralQuaternaryAlt};" 
                }
            });


            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--commandBar.is-disabled)" },
                Properties = new CssString()
                {
                    Css = $"background-color:{theme.Palette.White};"
                }
            });
            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--commandBar.is-disabled .ms-Button-icon" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.SemanticTextColors.DisabledBodySubtext};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--commandBar .ms-Button-icon" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.ThemePrimary};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--commandBar .ms-Button-label" },
                Properties = new CssString()
                {
                    Css = $"font-weight:normal;"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--commandBar .ms-Button-menuIcon" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.NeutralSecondary};"
                }
            });

            return rules;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            builder.OpenComponent<BFUGlobalCS>(0);
            builder.AddAttribute(1, "Component", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Object>(this));
            builder.AddAttribute(2, "CreateGlobalCss", new System.Func<ICollection<IRule>>(() => CreateGlobalCss(Theme)));
            builder.AddAttribute(3, "FixStyle", true);
            builder.CloseComponent();

            StartRoot(builder, "ms-Button--commandBar");
        }

    }
}
