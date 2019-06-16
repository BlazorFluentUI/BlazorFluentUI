using BlazorFabric.BaseComponent;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric.Layer
{
    public class LayerPortalBase : FabricComponentBase, IDisposable
    {
        //[Parameter] protected RenderFragment ChildContent { get; set; }

        //protected bool IsFixed = true;

        protected Dictionary<string, (RenderFragment fragment, bool isFixed)> fragments = new Dictionary<string, (RenderFragment fragment, bool isFixed)>();


        public void SetChildContent(string layerId, RenderFragment renderFragment, bool isFixed)
        {
            if (fragments.ContainsKey(layerId))
            {
                fragments[layerId] = (renderFragment, isFixed);
            }
            else
            {
                fragments.Add(layerId, (renderFragment, isFixed));
            }
            //this.ChildContent = renderFragment;
            //this.IsFixed = isFixed;
            StateHasChanged();
        }

        public void RemoveChildContent(string layerId)
        {
            fragments.Remove(layerId);
            //this.ChildContent = null;
            StateHasChanged();
        }

        public void Dispose()
        {
            
        }
    }
}
