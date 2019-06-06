using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components.RenderTree;

namespace BlazorFabric.Button
{
    public class ActionButton: ButtonBase
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            StartRoot(builder, "ms-Button--action ms-Button--command");

        }
    }
}
