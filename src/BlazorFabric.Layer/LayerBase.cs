using BlazorFabric.BaseComponent;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric.Layer
{
    // Blazor doesn't have a React equivalent to "createPortal" so to use a Layer, you need to place 
    // a LayerHost manually near the root of the app.  This will allow you to use a CascadeParameter 
    // to send a RenderTreeBuilder from the host to anywhere in the app and render items to it.

    public class Layer : FabricComponentBase
    {
        //internal LayerBase() { }

        [Inject] private IJSRuntime JSRuntime { get; set; }
        
        [Parameter] protected RenderFragment ChildContent { get; set; }

        [CascadingParameter(Name = "HostedContent")] protected LayerHost LayerHost { get; set; }

        protected override Task OnParametersSetAsync()
        {
            LayerHost.SetHostedContent(ChildContent);
            return base.OnParametersSetAsync();
        }

        protected override void OnAfterRender()
        {
            //if (layerElement == null)
            base.OnAfterRender();
        }
    }
}
