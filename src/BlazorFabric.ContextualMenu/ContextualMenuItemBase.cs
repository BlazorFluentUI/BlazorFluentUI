using BlazorFabric.BaseComponent;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric.ContextualMenu
{
    public class ContextualMenuItemBase : FabricComponentBase
    {
        internal ContextualMenuItemBase() { }

        [Parameter] protected string Key { get; set; }
        [Parameter] protected string Text { get; set; }
        [Parameter] protected string SecondaryText { get; set; }
        [Parameter] protected ContextualMenuItemType ItemType { get; set; }

        [Parameter] protected bool Disabled { get; set; }
        [Parameter] protected bool CanCheck { get; set; }
        [Parameter] protected bool Checked { get; set; }
        [Parameter] protected bool Split { get; set; }
    }
}
