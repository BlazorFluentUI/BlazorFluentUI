using BlazorFabric.BaseComponent;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric.Layer
{
    public class LayerHostBase : FabricComponentBase
    {
        internal LayerHostBase() { }
               
        [Parameter] protected RenderFragment ChildContent { get; set; }

        [Parameter] protected RenderFragment HostedContent { get; set; }

        [Parameter] protected bool IsFixed { get; set; } = true;

        //[Parameter] public Func<UIEventArgs, Task> OnScroll { get; set; }  // CAN'T USE THESE... NEED TO USE FUNC INSTEAD
        //[Parameter] public EventCallback<UIEventArgs> OnResize { get; set; } 
        //[Parameter] public EventCallback<UIFocusEventArgs> OnFocusIn { get; set; }
        //[Parameter] public EventCallback<UIMouseEventArgs> OnClick { get; set; }

        //public bool IsSet { get; set; } = false;

        
        protected LayerPortalGenerator portalGeneratorRef;

        //protected RenderFragment RenderPortals() => builder =>
        //{
        //    foreach (var portalPair in portalFragments)
        //    {
        //        int sequenceStart = 0;
        //        if (portalSequenceStarts.ContainsKey(portalPair.Key))
        //            sequenceStart = portalSequenceStarts[portalPair.Key];
        //        else
        //        {
        //            sequenceStart = sequenceCount;
        //            portalSequenceStarts.Add(portalPair.Key, sequenceStart);
        //            sequenceCount += 5; //advance the count for the next new layerportal
        //            // this will eventually run out of numbers... need to reset everything at some point...  maybe it's not necessary
        //        }
        //        builder.OpenComponent<LayerPortal>(sequenceStart);
        //        builder.AddComponentReferenceCapture(sequenceStart + 1, (component) => portals[portalPair.Key] = (LayerPortal)component);
        //        builder.AddAttribute(sequenceStart + 2, "ChildContent", portalPair.Value);
        //        builder.CloseComponent();
        //    }
        //};


        //protected LayerPortal layerPortal;

        public void AddOrUpdateHostedContent(string layerId, RenderFragment renderFragment)
        {
            portalGeneratorRef.AddOrUpdateHostedContent(layerId, renderFragment);//.Add(layerId, renderFragment); //should render the first time and not after unless explicitly set.

            //until we can get references from a loop, looks like we can only use one portal at a time.
            //maybe with preview 6

            //layerPortal.SetChildContent(layerId, renderFragment, IsFixed);
           
        }

        public void RemoveHostedContent(string layerId)
        {
            //portalFragments.Remove(layerId);
            //if (portals.ContainsKey(layerId))
            //    portals.Remove(layerId);
            //portalSequenceStarts.Remove(layerId);
            //layerPortal.RemoveChildContent(layerId);
            portalGeneratorRef.RemoveHostedContent(layerId);
        }

        //protected Task ScrollHandler(UIEventArgs args)
        //{
        //    if (OnScroll != null)
        //    {
        //        return OnScroll.Invoke(args);
        //    }
        //    return Task.CompletedTask;
        //    //return OnScroll.InvokeAsync(args);
        //}

        //protected Task ResizeHandler(UIEventArgs args)
        //{
        //    return OnResize.InvokeAsync(args);
        //}

        //protected Task FocusInHandler(UIFocusEventArgs args)
        //{
        //    return OnFocusIn.InvokeAsync(args);
        //}

        //protected Task ClickHandler(UIMouseEventArgs args)
        //{
        //    return OnClick.InvokeAsync(args);
        //}

    }
}