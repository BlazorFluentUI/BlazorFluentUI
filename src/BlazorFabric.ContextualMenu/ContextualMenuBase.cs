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

        [Parameter] protected bool Title { get; set; }

        [Parameter] protected bool IsBeakVisible { get; set; }
        [Parameter] protected string DirectionalHint { get; set; }
        [Parameter] protected int GapSpace { get; set; } = 0;
        [Parameter] protected int BeakWidth { get; set; } = 16;

        [Parameter] protected string Target { get; set; }
        [Parameter] protected bool UseTargetWidth { get; set; } = false;
        [Parameter] protected bool UseTargetAsMinWidth { get; set; } = false;

        [Parameter] protected RenderFragment ChildContent { get; set; }

        [Parameter] protected IEnumerable<TItem> ItemsSource { get; set; }
        [Parameter] protected RenderFragment<TItem> MenuItemTemplate { get; set; }
    }
}
