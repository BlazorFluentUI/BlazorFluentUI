using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric.Layer
{
    public class LayerPortalGenerator : ComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }

        private int sequenceCount = 0;
        private Dictionary<string, int> portalSequenceStarts = new Dictionary<string, int>();
        private Dictionary<string, RenderFragment> portalFragments = new Dictionary<string, RenderFragment>();
        private Dictionary<string, LayerPortal> portals = new Dictionary<string, LayerPortal>();

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            foreach (var portalPair in portalFragments)
            {
                int sequenceStart = 0;
                if (portalSequenceStarts.ContainsKey(portalPair.Key))
                    sequenceStart = portalSequenceStarts[portalPair.Key];
                else
                {
                    sequenceStart = sequenceCount;
                    portalSequenceStarts.Add(portalPair.Key, sequenceStart);
                    sequenceCount += 5; //advance the count for the next new layerportal
                    // this will eventually run out of numbers... need to reset everything at some point...  maybe it's not necessary
                }
                builder.OpenComponent<LayerPortal>(sequenceStart);
                builder.AddAttribute(sequenceStart + 1, "ChildContent", portalPair.Value);
                builder.AddComponentReferenceCapture(sequenceStart + 2, (component) => portals[portalPair.Key] = (LayerPortal)component);
                builder.CloseComponent();
            }
        }

        public void AddOrUpdateHostedContent(string layerId, RenderFragment renderFragment)
        {
            if (portalFragments.ContainsKey(layerId))
            {
                portalFragments[layerId] = renderFragment;
                portals[layerId].Rerender();
            }
            else
            {
                portalFragments.Add(layerId, renderFragment); //should render the first time and not after unless explicitly set.
                StateHasChanged();
            }
           
        }

        public void RemoveHostedContent(string layerId)
        {
            portalFragments.Remove(layerId);
            if (portals.ContainsKey(layerId))
                portals.Remove(layerId);
            portalSequenceStarts.Remove(layerId);
            StateHasChanged();
        }



    }
}
