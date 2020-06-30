using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class BFULayerPortalGenerator : ComponentBase
    {
        [Parameter] public RenderFragment? ChildContent { get; set; }

        //private int sequenceCount = 0;
        private Dictionary<string, int> portalSequenceStarts = new Dictionary<string, int>();
        private List<PortalDetails> portalFragments = new List<PortalDetails>();
        private Dictionary<string, BFULayerPortal> portals = new Dictionary<string, BFULayerPortal>();

        protected override Task OnInitializedAsync()
        {
            //portals.CollectionChanged += Portals_CollectionChanged;
            return base.OnInitializedAsync();
        }

        private void Portals_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:

                    break;
            }

        }

        //public void SetVirtualParent(ElementReference parent, string childPortalId)
        //{
        //    if (portals.ContainsKey(childPortalId))
        //    {
        //        portals[childPortalId].SetVirtualParentAsync(parent);
        //    }
        //}

        //protected override void BuildRenderTree(RenderTreeBuilder builder)
        //{
        //    base.BuildRenderTree(builder);
        //    foreach (var portalPair in portalFragments)
        //    {
        //        int sequenceStart = 0;
        //        if (portalSequenceStarts.ContainsKey(portalPair.id))
        //            sequenceStart = portalSequenceStarts[portalPair.id];
        //        else
        //        {
        //            sequenceStart = sequenceCount;
        //            portalSequenceStarts.Add(portalPair.id, sequenceStart);
        //            sequenceCount += 5; //advance the count for the next new layerportal
        //            // this will eventually run out of numbers... need to reset everything at some point...  maybe it's not necessary
        //        }
        //        builder.OpenComponent<BFULayerPortal>(sequenceStart);
        //        builder.AddAttribute(sequenceStart + 1, "ChildContent", portalPair.fragment);
        //        builder.AddAttribute(sequenceStart + 2, "Id", portalPair.id);
        //        builder.AddAttribute(sequenceStart + 3, "Style", portalPair.style);
        //        builder.AddComponentReferenceCapture(sequenceStart + 4, (component) => portals[portalPair.id] = (BFULayerPortal)component);
        //        builder.CloseComponent();
        //    }
        //}

        public void AddOrUpdateHostedContent(string layerId, RenderFragment? renderFragment)
        {
            var foundPortalFragment = portalFragments.FirstOrDefault(x => x.Id == layerId);
            if (foundPortalFragment != null)
            {
                foundPortalFragment.Fragment = renderFragment;
//                foundPortalFragment.Parent = parent;
                //System.Diagnostics.Debug.WriteLine($"Rerendering layer: {layerId}");
                portals[layerId].Rerender();
            }
            else
            {
                if (layerId == null)
                    throw new Exception("The Layer Id should not be null.");
                //System.Diagnostics.Debug.WriteLine($"Adding new layer: {layerId}, {portalFragments.Count} layer(s) in host currently.");
                portalFragments.Add(new PortalDetails { Id = layerId, Fragment = renderFragment }); //should render the first time and not after unless explicitly set.
                InvokeAsync(StateHasChanged);
            }
           
        }

        public void RemoveHostedContent(string layerId)
        {
            //System.Diagnostics.Debug.WriteLine($"Disposing layer contents: {layerId}");
            portalFragments.Remove(portalFragments.First(x => x.Id == layerId));
            if (portals.ContainsKey(layerId))
                portals.Remove(layerId);
            portalSequenceStarts.Remove(layerId);
            InvokeAsync(StateHasChanged);
        }



    }
}
