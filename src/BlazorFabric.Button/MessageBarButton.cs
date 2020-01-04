using Microsoft.AspNetCore.Components.Rendering;


namespace BlazorFabric
{
    public class MessageBarButton : ButtonBase
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            StartRoot(builder, "ms-Button--messageBar");

        }

        //protected override void OnParametersSet()
        //{
        //    this.SecondaryText = null;
        //    base.OnParametersSet();
        //}
    }
}
