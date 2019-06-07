using BlazorFabric.BaseComponent;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric.Layer
{
    public class LayerHostBase : FabricComponentBase
    {
        internal LayerHostBase() { }
               
        [Parameter] protected RenderFragment ChildContent { get; set; }

        [Parameter] protected RenderFragment HostedContent { get; set; }

        [Parameter] protected bool IsFixed { get; set; } = true;

        //public bool IsSet { get; set; } = false;

        protected Dictionary<string, (RenderFragment fragment, LayerPortal portal)> portals = new Dictionary<string, (RenderFragment fragment, LayerPortal portal)>();

        //protected override void BuildRenderTree(RenderTreeBuilder builder)
        //{
        //    base.BuildRenderTree(builder);

        //    builder.OpenComponent<CascadingValue<LayerHost>>(0);


        //    builder.CloseComponent();

        //}
        protected LayerPortal layerPortal;

        public void AddOrUpdateHostedContent(string layerId, RenderFragment renderFragment)
        {
            //until we can get references from a loop, looks like we can only use one portal at a time.
            //maybe with preview 6
            layerPortal.SetChildContent(renderFragment, IsFixed);
        }

    }
}