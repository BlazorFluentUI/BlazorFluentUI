using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    // Blazor doesn't have a React equivalent to "createPortal" so to use a Layer, you need to place 
    // a LayerHost manually near the root of the app.  This will allow you to use a CascadeParameter 
    // to send the LayerHost to anywhere in the app and render items to it.

    public class BFULayer : BFUComponentBase, IDisposable
    {        
        [Inject] private IJSRuntime? JSRuntime { get; set; }
        
        [Parameter] public RenderFragment? ChildContent { get; set; }

        [CascadingParameter(Name = "HostedContent")] protected BFULayerHost? LayerHost { get; set; }

        private bool addedToHost = false;

        public string id = Guid.NewGuid().ToString();
        private ElementReference _element;

        protected override Task OnParametersSetAsync()
        {
            if (!addedToHost)
            {
                if (LayerHost == null)
                {
                    throw new Exception("LayerHost is not present.  You need to add a LayerHost near the root of the app.");
                }
                LayerHost.AddOrUpdateHostedContent(id, ChildContent);
                addedToHost = true;
            }
            return base.OnParametersSetAsync();
        }

        protected override bool ShouldRender()
        {
            if (LayerHost != null) 
            {
                LayerHost.AddOrUpdateHostedContent(id, ChildContent);
                addedToHost = true;
            }
            return true;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "span");
            builder.AddAttribute(1, "class", "ms-layer");
            builder.AddAttribute(2, "style", this.Style);
            builder.AddAttribute(3, "data-layer-id", this.id);
            builder.AddElementReferenceCapture(4, element => _element=element);
            builder.CloseElement();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntime.InvokeVoidAsync("BlazorFluentUiBaseComponent.addOrUpdateVirtualParent", _element);
            await base.OnAfterRenderAsync(firstRender);
        }

        public void Dispose()
        {
            Debug.WriteLine($"Layer disposed: {this.id}");
            LayerHost?.RemoveHostedContent(this.id);
            addedToHost = false;
        }
    }
}
