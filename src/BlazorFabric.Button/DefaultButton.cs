using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric.Button
{
    public class DefaultButton : ButtonBase
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            StartRoot(builder, "ms-Button--default");
        }

    }
}
