using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorFabric
{
    public class CompoundButton : ButtonBase
    {
        [Parameter] public string SecondaryText { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            StartRoot(builder, "ms-Button--compound");

        }
    }
}
