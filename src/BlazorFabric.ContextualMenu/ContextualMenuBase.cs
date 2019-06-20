using BlazorFabric.BaseComponent;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric.ContextualMenu
{
    public class ContextualMenuBase<TItem> : FabricComponentBase
    {
        internal ContextualMenuBase() { }

        
        [Parameter] public bool AlignTargetEdge { get; set; }
        [Parameter] public string AriaLabel { get; set; }
        [Parameter] public int BeakWidth { get; set; } = 16;
        [Parameter] public Rectangle Bounds { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public bool CoverTarget { get; set; }
        [Parameter] public DirectionalHint DirectionalHint { get; set; } = DirectionalHint.BottomAutoEdge;
        [Parameter] public bool DirectionalHintFixed { get; set; }
        [Parameter] public FabricComponentBase FabricComponentTarget { get; set; }
        [Parameter] public int GapSpace { get; set; } = 0;
        [Parameter] public bool IsBeakVisible { get; set; }
        [Parameter] public bool IsOpen { get; set; }  //to get styling
        [Parameter] public IEnumerable<TItem> ItemsSource { get; set; }
        [Parameter] public RenderFragment<TItem> MenuItemTemplate { get; set; }
        //[Parameter] public double SubMenuHoverDelay { get; set; } = 250;
        [Parameter] public string Title { get; set; }
        [Parameter] public bool UseTargetWidth { get; set; } = false;
        [Parameter] public bool UseTargetAsMinWidth { get; set; } = false;

        [Parameter] public EventCallback<bool> OnDismiss { get; set; }

        public string SubmenuActiveKey { get; set; }
        public void SetSubmenuActiveKey(string key)
        {
            SubmenuActiveKey = key;
            StateHasChanged();
        }

        protected override Task OnInitAsync()
        {
            System.Diagnostics.Debug.WriteLine("Creating ContextualMenu");
            return base.OnInitAsync();
        }

        public int HasIconCount = 0; //needed to shift margins and make space for all 
        public int HasCheckable = 0;
    }
}
