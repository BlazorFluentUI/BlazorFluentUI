using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlazorFluentUI
{
    public class BFULayerPortalGenerator : ComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }

        private int sequenceCount = 0;
        private Dictionary<string, int> portalSequenceStarts = new Dictionary<string, int>();
        private List<(string id, RenderFragment fragment, string style)> portalFragments = new List<(string id, RenderFragment fragment, string style)>();
        private Dictionary<string, BFULayerPortal> portals = new Dictionary<string, BFULayerPortal>();

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            foreach (var portalPair in portalFragments)
            {
                int sequenceStart = 0;
                if (portalSequenceStarts.ContainsKey(portalPair.id))
                    sequenceStart = portalSequenceStarts[portalPair.id];
                else
                {
                    sequenceStart = sequenceCount;
                    portalSequenceStarts.Add(portalPair.id, sequenceStart);
                    sequenceCount += 5; //advance the count for the next new layerportal
                    // this will eventually run out of numbers... need to reset everything at some point...  maybe it's not necessary
                }
                builder.OpenComponent<BFULayerPortal>(sequenceStart);
                builder.AddAttribute(sequenceStart + 1, "ChildContent", portalPair.fragment);
                builder.AddAttribute(sequenceStart + 2, "Id", portalPair.id);
                builder.AddAttribute(sequenceStart + 3, "Style", portalPair.style);
                builder.AddComponentReferenceCapture(sequenceStart + 4, (component) => portals[portalPair.id] = (BFULayerPortal)component);
                builder.CloseComponent();
            }
        }

        public void AddOrUpdateHostedContent(string layerId, RenderFragment renderFragment, string style)
        {
            var foundPortalFragment = portalFragments.FirstOrDefault(x => x.id == layerId);
            if (foundPortalFragment != default((string,RenderFragment,string)))
            {
                foundPortalFragment.fragment = renderFragment;
                foundPortalFragment.style = style;
                System.Diagnostics.Debug.WriteLine($"Rerendering layer: {layerId}");
                portals[layerId].Rerender();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Adding new layer: {layerId}, {portalFragments.Count} layer(s) in host currently.");
                portalFragments.Add((layerId, renderFragment, style)); //should render the first time and not after unless explicitly set.
                InvokeAsync(StateHasChanged);
            }
           
        }

        public void RemoveHostedContent(string layerId)
        {
            System.Diagnostics.Debug.WriteLine($"Disposing layer contents: {layerId}");
            portalFragments.Remove(portalFragments.First(x => x.id == layerId));
            if (portals.ContainsKey(layerId))
                portals.Remove(layerId);
            portalSequenceStarts.Remove(layerId);
            InvokeAsync(StateHasChanged);
        }



    }
}
