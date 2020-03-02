using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;

namespace BlazorFabric
{
    public class MessageBarButton : ButtonBase
    {
        private ICollection<Rule> CreateGlobalCss()
        {
            var rules = CreateBaseGlobalCss();


            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--action:not(.ms-Pivot-link)" },
                Properties = new CssString()
                {
                    Css = $"height:24px;" +
                          $"border-color:{Theme.Palette.NeutralTertiaryAlt};"
                }
            });


            return rules;
        }


        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            builder.OpenComponent<GlobalCS>(0);
            builder.AddAttribute(1, "Component", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Object>(this));
            builder.AddAttribute(2, "CreateGlobalCss", new System.Func<System.Collections.Generic.ICollection<BlazorFabric.Rule>>(CreateGlobalCss));
            builder.AddAttribute(3, "FixStyle", true);
            builder.CloseComponent();

            StartRoot(builder, "ms-Button--messageBar");

        }

    }
}
