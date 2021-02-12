using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class LayerPortalGenerator : ComponentBase
    {
        [Parameter] public RenderFragment? ChildContent { get; set; }

        //private int sequenceCount = 0;
        private Dictionary<string, int> portalSequenceStarts = new Dictionary<string, int>();
        private List<PortalDetails> portalFragments = new List<PortalDetails>();
        private Dictionary<string, LayerPortal> portals = new Dictionary<string, LayerPortal>();


        private void Portals_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:

                    break;
            }

        }


        public async Task AddOrUpdateHostedContentAsync(string layerId, RenderFragment? renderFragment)
        {
            var foundPortalFragment = portalFragments.FirstOrDefault(x => x.Id == layerId);
            if (foundPortalFragment != null)
            {
                foundPortalFragment.Fragment = renderFragment;
                if (portals.ContainsKey(layerId))
                    portals[layerId].Rerender();
            }
            else
            {
                if (layerId == null)
                    throw new Exception("The Layer Id should not be null.");
                portalFragments.Add(new PortalDetails { Id = layerId, Fragment = renderFragment }); //should render the first time and not after unless explicitly set.
                await InvokeAsync(StateHasChanged);
            }
           
        }

        public async Task RemoveHostedContentAsync(string layerId)
        {
            portalFragments.Remove(portalFragments.First(x => x.Id == layerId));
            if (portals.ContainsKey(layerId))
                portals.Remove(layerId);
            portalSequenceStarts.Remove(layerId);
            await InvokeAsync(StateHasChanged);
        }



    }
}
