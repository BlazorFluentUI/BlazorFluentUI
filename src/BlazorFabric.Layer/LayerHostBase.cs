using BlazorFabric.BaseComponent;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric.Layer
{
    public class LayerHostBase : FabricComponentBase
    {
        internal LayerHostBase() { }
               
        [Parameter] protected RenderFragment ChildContent { get; set; }

        [Parameter] protected RenderFragment HostedContent { get; set; }

        [Parameter] protected bool IsFixed { get; set; } = true;

        public bool IsSet { get; set; } = false;

        public void SetHostedContent(RenderFragment renderFragment)
        {
            if (!IsSet)
            {
                this.IsSet = true;
                HostedContent = renderFragment;
                StateHasChanged();
            }
        }

    }
}