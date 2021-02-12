using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class LayerHost : FluentUIComponentBase, IDisposable
    {

        [Parameter] public RenderFragment? ChildContent { get; set; }

        [Parameter] public RenderFragment? HostedContent { get; set; }

        [Parameter] public string? Id { get; set; }

        [Parameter] public bool InsertFirst { get; set; } = false;

        [Parameter] public bool IsFixed { get; set; } = true;

        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] private LayerHostService LayerHostService { get; set; }

        protected LayerPortalGenerator? portalGeneratorReference;


        public Task AddOrUpdateHostedContentAsync(string layerId, RenderFragment? renderFragment)
        {
            return portalGeneratorReference?.AddOrUpdateHostedContentAsync(layerId, renderFragment);
        }

        public Task RemoveHostedContentAsync(string layerId)
        {
            return portalGeneratorReference?.RemoveHostedContentAsync(layerId);
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