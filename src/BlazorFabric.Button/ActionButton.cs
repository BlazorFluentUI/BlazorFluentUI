using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorFabric
{
    public class ActionButton : ButtonBase
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            StartRoot(builder, "ms-Button--action ms-Button--command");

        }
    }
}
