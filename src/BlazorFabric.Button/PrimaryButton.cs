using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
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
            Primary = true;
            base.BuildRenderTree(builder);
            StartRoot(builder, "");
        }

    }
}
