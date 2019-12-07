using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class LayerHost : FabricComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public RenderFragment HostedContent { get; set; }

        [Parameter] public bool IsFixed { get; set; } = true;
        
        protected LayerPortalGenerator portalGeneratorReference;

        public void AddOrUpdateHostedContent(string layerId, RenderFragment renderFragment, string style)
        {
            portalGeneratorReference.AddOrUpdateHostedContent(layerId, renderFragment, style);//.Add(layerId, renderFragment); //should render the first time and not after unless explicitly set.
        }

        public void RemoveHostedContent(string layerId)
        {
            portalGeneratorReference.RemoveHostedContent(layerId);
        }

    }
}