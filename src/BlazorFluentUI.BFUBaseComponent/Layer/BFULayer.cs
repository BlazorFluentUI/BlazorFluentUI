using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    // Blazor doesn't have a React equivalent to "createPortal" so to use a Layer, you need to place 
    // a LayerHost manually near the root of the app.  This will allow you to use a CascadeParameter 
    // to send the LayerHost to anywhere in the app and render items to it.

    public class BFULayer : BFUComponentBase, IAsyncDisposable
    {        
        [Inject] private IJSRuntime? JSRuntime { get; set; }
        [Inject] private LayerHostService LayerHostService { get; set; }
        
        [Parameter] public RenderFragment? ChildContent { get; set; }
        [Parameter] public string? HostId { get; set; }

        //[CascadingParameter(Name = "HostedContent")] protected BFULayerHost? LayerHost { get; set; }
        private BFULayerHost LayerHost { get; set; }

        private bool addedToHost = false;

        public string id = Guid.NewGuid().ToString();
        private ElementReference _element;

        private bool isFirstRendered = false;

        protected override async Task OnParametersSetAsync()
        {
            if (!addedToHost)
            {
                if (HostId == null)
                {
                    LayerHost = LayerHostService.GetDefaultHost();
                }
                else
                {
                    LayerHost = LayerHostService.GetHost(HostId);
                }

                if (LayerHost != null)
                {
                    LayerHost.AddOrUpdateHostedContent(id, ChildContent);
                    addedToHost = true;
                }
            }
            await base.OnParametersSetAsync();
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
            if (firstRender)
            {
                isFirstRendered = true;
                if (!addedToHost)
                {
                    if (HostId == null)
                    {
                        LayerHost = LayerHostService.GetDefaultHost();
                    }
                    else
                    {
                        LayerHost = LayerHostService.GetHost(HostId);
                    }

                    if (LayerHost != null)
                    {
                        LayerHost.AddOrUpdateHostedContent(id, ChildContent);
                        addedToHost = true;
                        StateHasChanged();
                    }
                }

                await JSRuntime.InvokeVoidAsync("BlazorFluentUiBaseComponent.addOrUpdateVirtualParent", _element);
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        public ValueTask DisposeAsync()
        {
            LayerHost?.RemoveHostedContent(this.id);
            addedToHost = false;
            return ValueTask.CompletedTask;
        }
    }
}
