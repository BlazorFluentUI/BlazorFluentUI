using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;

namespace BlazorFluentUI
{
    public class BFUMessageBarButton : BFUDefaultButton, IHasPreloadableGlobalStyle
    {
        public override ICollection<IRule> CreateGlobalCss(ITheme theme)
        {

            var rules = base.CreateGlobalCss(theme);


            rules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Button.ms-Button--messageBar" },
                Properties = new CssString()
                {
                    Css = $"height:24px;" +
                          $"border-color:{theme.Palette.NeutralTertiaryAlt};"
                }
            });


            return rules;
        }


        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            //base.BuildRenderTree(builder);

            builder.OpenComponent<BFUGlobalCS>(0);
            builder.AddAttribute(1, "Component", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Object>(this));
            builder.AddAttribute(2, "CreateGlobalCss", new System.Func<ICollection<IRule>>(()=>CreateGlobalCss(Theme)));
            builder.AddAttribute(3, "FixStyle", true);
            builder.CloseComponent();

            StartRoot(builder, "ms-Button--default ms-Button--messageBar");

        }

    }
}
