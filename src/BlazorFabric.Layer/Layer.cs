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
    // to send the LayerHost to anywhere in the app and render items to it.

    public class Layer : ComponentBase, IDisposable
    {
        //internal LayerBase() { }

        [Inject] private IJSRuntime JSRuntime { get; set; }
        
        [Parameter] protected RenderFragment ChildContent { get; set; }

        [CascadingParameter(Name = "HostedContent")] protected LayerHost LayerHost { get; set; }

        private string id = Guid.NewGuid().ToString();

        protected override Task OnParametersSetAsync()
        {
            LayerHost.AddOrUpdateHostedContent(id, ChildContent);
            return base.OnParametersSetAsync();
        }

        protected override bool ShouldRender()
        {
            if (LayerHost != null)
                LayerHost.AddOrUpdateHostedContent(id, ChildContent);
            return base.ShouldRender();
        }

        protected override void OnAfterRender()
        {
            //if (layerElement == null)
            base.OnAfterRender();
        }

        public void Dispose()
        {
            LayerHost.RemoveHostedContent(this.id);
        }
    }
}
