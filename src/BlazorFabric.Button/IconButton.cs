using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public class IconButton : ButtonBase
    {
        //protected override Task OnParametersSetAsync()
        //{
        //    this.ClassName = this.ClassName + " ms-Button--primary";
        //    return base.OnParametersSetAsync();
        //}

        protected override void OnParametersSet()
        {
            this.Text = null;
            base.OnParametersSet();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            StartRoot(builder, "ms-Button--icon");
        }

    }
}
