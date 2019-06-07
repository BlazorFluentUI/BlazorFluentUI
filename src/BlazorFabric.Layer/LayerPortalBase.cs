using BlazorFabric.BaseComponent;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric.Layer
{
    public class LayerPortalBase : FabricComponentBase
    {
        [Parameter] protected RenderFragment ChildContent { get; set; }

        protected bool IsFixed = true;

        public void SetChildContent(RenderFragment renderFragment, bool isFixed)
        {
            this.ChildContent = renderFragment;
            this.IsFixed = isFixed;
            StateHasChanged();
        }
    }
}
