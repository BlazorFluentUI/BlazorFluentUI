using BlazorFabric.BaseComponent;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric.ContextualMenu
{
    public class ContextualMenuBase<TItem> : FabricComponentBase
    {
        internal ContextualMenuBase() { }

        
        [Parameter] protected bool AlignTargetEdge { get; set; }
        [Parameter] protected string AriaLabel { get; set; }
        [Parameter] protected int BeakWidth { get; set; } = 16;
        [Parameter] protected Rectangle Bounds { get; set; }
        [Parameter] protected RenderFragment ChildContent { get; set; }
        [Parameter] protected bool CoverTarget { get; set; }
        [Parameter] protected DirectionalHint DirectionalHint { get; set; } = DirectionalHint.BottomAutoEdge;
        [Parameter] protected bool DirectionalHintFixed { get; set; }
        [Parameter] protected FabricComponentBase FabricComponentTarget { get; set; }
        [Parameter] protected int GapSpace { get; set; } = 0;
        [Parameter] protected bool IsBeakVisible { get; set; }
        [Parameter] protected bool IsOpen { get; set; }  //to get styling
        [Parameter] protected IEnumerable<TItem> ItemsSource { get; set; }
        [Parameter] protected RenderFragment<TItem> MenuItemTemplate { get; set; }
        [Parameter] protected string Title { get; set; }
        [Parameter] protected bool UseTargetWidth { get; set; } = false;
        [Parameter] protected bool UseTargetAsMinWidth { get; set; } = false;

        [Parameter] protected EventCallback<bool> OnDismiss { get; set; }


        public int HasIconCount = 0; //needed to shift margins and make space for all 

    }
}
