using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;

namespace BlazorFluentUI
{
    public class BFUCompoundButton : BFUButtonBase
    {
        [Parameter] public string SecondaryText { get; set; }

        private ICollection<IRule> CreateGlobalCss()
        {
            var rules = CreateBaseGlobalCss(Theme);

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--compound" },
                Properties = new CssString()
                {
                    Css = $"max-width:280px;"+
                          $"min-height:72px;"+
                          $"padding:16px 12px;"+
                          $"height:auto;"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--compound .ms-Button-flexContainer" },
                Properties = new CssString()
                {
                    Css = $"flex-direction:row;" +
                          $"align-items:flex-start;" +
                          $"min-width:100%;" +
                          $"margin:unset;"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--compound .ms-Button-textContainer" },
                Properties = new CssString()
                {
                    Css = $"text-align:left;" 
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--compound .ms-Button-label" },
                Properties = new CssString()
                {
                    Css = $"margin:0 0 5px 0;" +
                          $"line-height:100%;" +
                          $"font-weight:{Theme.FontStyle.FontWeight.SemiBold};"
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--compound .ms-Button-description" },
                Properties = new CssString()
                {
                    Css = $"line-height:100%;" 
                }
            });

            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button--default.ms-Button--compound .ms-Button-description" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.NeutralSecondary};"
                }
            });
            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button--default.ms-Button--compound .ms-Button-description:hover" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.NeutralDark};"
                }
            });
            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button--default.ms-Button--compound.is-checked .ms-Button-description" },
                Properties = new CssString()
                {
                    Css = $"color:inherit;"
                }
            });
            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button--default.ms-Button--compound.is-disabled .ms-Button-description" },
                Properties = new CssString()
                {
                    Css = $"color:inherit;"
                }
            });


            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button--primary.ms-Button--compound .ms-Button-description" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.White};"
                }
            });
            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button--primary.ms-Button--compound .ms-Button-description:hover" },
                Properties = new CssString()
                {
                    Css = $"color:{Theme.Palette.White};"
                }
            });
            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button--primary.ms-Button--compound.is-checked .ms-Button-description" },
                Properties = new CssString()
                {
                    Css = $"color:inherit;"
                }
            });
            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button--primary.ms-Button--compound.is-disabled .ms-Button-description" },
                Properties = new CssString()
                {
                    Css = $"color:inherit;"
                }
            });


            return rules;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            
            builder.OpenComponent<BFUGlobalCS>(0);
            builder.AddAttribute(1, "Component", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Object>(this));
            builder.AddAttribute(2, "CreateGlobalCss", new System.Func<ICollection<IRule>>(CreateGlobalCss));
            builder.AddAttribute(3, "FixStyle", true);
            builder.CloseComponent();

            base.BuildRenderTree(builder);

            StartRoot(builder, "ms-Button--compound");


        }
    }
}
