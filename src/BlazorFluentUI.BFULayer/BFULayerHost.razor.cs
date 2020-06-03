﻿using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BlazorFluentUI
{
    public partial class BFULayerHost : BFUComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public RenderFragment HostedContent { get; set; }

        [Parameter] public bool IsFixed { get; set; } = true;

        protected BFULayerPortalGenerator portalGeneratorReference;

        public void AddOrUpdateHostedContent(string layerId, RenderFragment renderFragment, string style)
        {
            portalGeneratorReference.AddOrUpdateHostedContent(layerId, renderFragment, style);//.Add(layerId, renderFragment); //should render the first time and not after unless explicitly set.
        }

        public void RemoveHostedContent(string layerId)
        {
            portalGeneratorReference.RemoveHostedContent(layerId);
        }

        private ICollection<IRule> CreateGlobalCss()
        {
            var layerRules = new HashSet<IRule>();
            // ROOT
            layerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Layer--fixed" },
                Properties = new CssString()
                {
                    Css = $"position:fixed;"+
                          $"z-index:{Theme.ZIndex.Layer};"+
                          $"top:0;"+
                          $"left:0;" +
                          $"width:100vw;" +
                          $"height:100vh;" +
                          $"visibility:hidden;"
                }
            });

            layerRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-Layer-content" },
                Properties = new CssString()
                {
                    Css = $"visibility:visible;"
                }
            });



            return layerRules;
        }

    }
}