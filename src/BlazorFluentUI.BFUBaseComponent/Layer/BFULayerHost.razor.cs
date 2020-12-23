using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class BFULayerHost : BFUComponentBase, IDisposable
    {

        [Parameter] public RenderFragment? ChildContent { get; set; }

        [Parameter] public RenderFragment? HostedContent { get; set; }

        [Parameter] public string? Id { get; set; }

        [Parameter] public bool InsertFirst { get; set; } = false;

        [Parameter] public bool IsFixed { get; set; } = true;

        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] private LayerHostService LayerHostService { get; set; }

        protected BFULayerPortalGenerator? portalGeneratorReference;


        public void AddOrUpdateHostedContent(string layerId, RenderFragment? renderFragment)
        {
            portalGeneratorReference?.AddOrUpdateHostedContent(layerId, renderFragment);
        }

        public void RemoveHostedContent(string layerId)
        {
            portalGeneratorReference?.RemoveHostedContent(layerId);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                LayerHostService.RegisterHost(this);
                //JSRuntime.InvokeAsync<string>("registerLayerHost")
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        public void Dispose()
        {
            LayerHostService.RemoveHost(this);
        }
    }
}