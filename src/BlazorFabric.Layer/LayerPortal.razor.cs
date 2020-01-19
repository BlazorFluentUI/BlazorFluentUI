using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public partial class LayerPortal : FabricComponentBase, IDisposable
    {
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public string Id { get; set; }
        [Parameter] public bool IsFixed { get; set; } = true;

        //protected Dictionary<string, (RenderFragment fragment, bool isFixed)> fragments = new Dictionary<string, (RenderFragment fragment, bool isFixed)>();


        protected bool shouldRender = false;

        protected override bool ShouldRender()
        {
            if (shouldRender)
            {
                shouldRender = false;
                return true;
            }
            return false;
        }

        public void Rerender()
        {
            shouldRender = true;
            InvokeAsync(StateHasChanged);
        }

        //public void SetChildContent(string layerId, RenderFragment renderFragment, bool isFixed)
        //{
        //    if (fragments.ContainsKey(layerId))
        //    {
        //        fragments[layerId] = (renderFragment, isFixed);
        //    }
        //    else
        //    {
        //        fragments.Add(layerId, (renderFragment, isFixed));
        //    }
        //    //this.ChildContent = renderFragment;
        //    //this.IsFixed = isFixed;
        //    Rerender();
        //}

        //public void RemoveChildContent(string layerId)
        //{
        //    fragments.Remove(layerId);
        //    //this.ChildContent = null;
        //    Rerender();
        //}

        public void Dispose()
        {
            
        }
    }
}
