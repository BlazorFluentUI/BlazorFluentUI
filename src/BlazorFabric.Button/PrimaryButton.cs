using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric.Button
{
    public class PrimaryButton : ButtonBase
    {
        //protected override Task OnParametersSetAsync()
        //{
        //    this.ClassName = this.ClassName + " ms-Button--primary";
        //    return base.OnParametersSetAsync();
        //}

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            StartRoot(builder, "ms-Button--primary");
        }

    }
}
