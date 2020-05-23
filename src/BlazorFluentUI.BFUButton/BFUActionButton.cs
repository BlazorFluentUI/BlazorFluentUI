using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;

namespace BlazorFluentUI
{
    public class BFUActionButton : BFUButtonBase, IHasPreloadableGlobalStyle
    {
        public ICollection<IRule> CreateGlobalCss(ITheme theme)
        {
            var rules = CreateBaseGlobalCss(theme);

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button--action" },
                Properties = new CssString()
                {
                    Css = $"min-width:unset;" 
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--action" }, //:not(.ms-Pivot-link)" },
                Properties = new CssString()
                {
                    Css = $"padding:0px 4px;" +
                          $"height:40px;" +
                          $"background-color:transparent;" +
                          $"color:{theme.Palette.NeutralPrimary};"+
                          $"border:1px solid transparent;"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--action:not(.ms-Pivot-link):hover" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.ThemePrimary};"
                }
            });
            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--action:not(.ms-Pivot-link) .ms-Button-icon:hover" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.ThemePrimary};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--action:not(.ms-Pivot-link):active" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.Black};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--action:not(.ms-Pivot-link).is-expanded" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.ThemePrimary};"
                }
            });
            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--action:not(.ms-Pivot-link) .ms-Button-icon:active" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.ThemeDarker};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--action:not(.ms-Pivot-link).is-disabled" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.NeutralTertiary};"+
                          $"background-color:transparent;"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--action:not(.ms-Pivot-link).is-checked:not(.is-disabled)" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.Black};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--action:not(.ms-Pivot-link).is-checked .ms-Button-icon" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.ThemeDarker};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--action:not(.ms-Pivot-link) .ms-Button-flexContainer" },
                Properties = new CssString()
                {
                    Css = $"justify-content:flex-start;"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--action:not(.ms-Pivot-link) .ms-Button-icon" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.ThemeDarkAlt};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--action:not(.ms-Pivot-link).is-disabled .ms-Button-icon" },
                Properties = new CssString()
                {
                    Css = $"color:inherit;"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--action:not(.ms-Pivot-link) .ms-Button-menuIcon" },
                Properties = new CssString()
                {
                    Css = $"color:{theme.Palette.NeutralSecondary};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--action:not(.ms-Pivot-link) .ms-Button-textContainer" },
                Properties = new CssString()
                {
                    Css = $"flex-grow:0;"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--action:not(.ms-Pivot-link) .ms-Button-label" },
                Properties = new CssString()
                {
                    Css = $"font-weight:{theme.FontStyle.FontWeight.Regular};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = "@media screen and (-ms-high-contrast: active)" },
                Properties = new CssString()
                {
                    Css = ".ms-Button.ms-Button--action:not(.ms-Pivot-link):hover{color: Highlight; background-color:Highlight;}"
                }
            });


            return rules;
        }


        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenComponent<BFUGlobalCS>(0);
            builder.AddAttribute(1, "Component", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Object>(this));
            builder.AddAttribute(2, "CreateGlobalCss", new System.Func<ICollection<IRule>>(()=>CreateGlobalCss(Theme)));
            builder.AddAttribute(3, "FixStyle", true);
            builder.CloseComponent();


            base.BuildRenderTree(builder);

            StartRoot(builder, "ms-Button--action ms-Button--command");

        }
    }
}
