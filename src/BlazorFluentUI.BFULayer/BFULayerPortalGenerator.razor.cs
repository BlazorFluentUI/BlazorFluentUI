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


        private void Portals_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:

                    break;
            }

        }


        public void AddOrUpdateHostedContent(string layerId, RenderFragment? renderFragment)
        {
            var foundPortalFragment = portalFragments.FirstOrDefault(x => x.Id == layerId);
            if (foundPortalFragment != null)
            {
                foundPortalFragment.Fragment = renderFragment;
                portals[layerId].Rerender();
            }
            else
            {
                if (layerId == null)
                    throw new Exception("The Layer Id should not be null.");
                portalFragments.Add(new PortalDetails { Id = layerId, Fragment = renderFragment }); //should render the first time and not after unless explicitly set.
                InvokeAsync(StateHasChanged);
            }
           
        }

        public void RemoveHostedContent(string layerId)
        {
            portalFragments.Remove(portalFragments.First(x => x.Id == layerId));
            if (portals.ContainsKey(layerId))
                portals.Remove(layerId);
            portalSequenceStarts.Remove(layerId);
            InvokeAsync(StateHasChanged);
        }



    }
}
