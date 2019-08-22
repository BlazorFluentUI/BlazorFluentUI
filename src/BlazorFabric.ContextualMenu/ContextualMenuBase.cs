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
        
        [CascadingParameter] public ContextualMenuBase<object> ContextualMenu { get; set; }  //maybe this contextualmenu is a child of another
        [CascadingParameter(Name ="PortalId")] public string PortalId { get; set; }

        public string SubmenuActiveKey { get; set; }
        public void SetSubmenuActiveKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key) && string.IsNullOrWhiteSpace(SubmenuActiveKey))
                return;
            System.Diagnostics.Debug.WriteLine($"SetSubmenuActiveKey(\"{key}\") from {this.DirectionalHint}");
            SubmenuActiveKey = key;
            StateHasChanged();
        }

        protected void DismissHandler(bool isDismissed)
        {
            System.Diagnostics.Debug.WriteLine($"ContextualMenu {PortalId} tried dismiss from {this.DirectionalHint} with SubmenuActiveKey = {SubmenuActiveKey}");


            if (string.IsNullOrEmpty(SubmenuActiveKey))
            {
                System.Diagnostics.Debug.WriteLine($"ContextualMenu dismiss successful!  {this.DirectionalHint} with SubmenuActiveKey = {SubmenuActiveKey}");
                OnDismiss.InvokeAsync(true);
            }
        }

        protected override Task OnInitializedAsync()
        {
            System.Diagnostics.Debug.WriteLine("Creating ContextualMenu");
            return base.OnInitializedAsync();
        }

        public int HasIconCount = 0; //needed to shift margins and make space for all 
        public int HasCheckable = 0;
    }
}
