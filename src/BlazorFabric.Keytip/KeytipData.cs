using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public class KeytipData : FabricComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }




        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.AddContent(0, ChildContent);
            //base.BuildRenderTree(builder);
        }
    }
}
