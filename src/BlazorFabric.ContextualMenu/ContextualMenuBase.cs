using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlazorFabric
{
    public class ContextualMenuBase : FabricComponentBase, IDisposable
    {
        internal ContextualMenuBase() { }


        [Parameter] public bool AlignTargetEdge { get; set; }
        [Parameter] public string AriaLabel { get; set; }
        [Parameter] public int BeakWidth { get; set; } = 16;
        [Parameter] public Rectangle Bounds { get; set; }
        //[Parameter] public RenderFragment ChildContent { get; set; }
        
        [Parameter] public IEnumerable<IContextualMenuItem> Items { get; set; }
        
        [Parameter] public bool CoverTarget { get; set; }
        [Parameter] public DirectionalHint DirectionalHint { get; set; } = DirectionalHint.BottomAutoEdge;
        [Parameter] public bool DirectionalHintFixed { get; set; }
        [Parameter] public FabricComponentBase FabricComponentTarget { get; set; }
        [Parameter] public int GapSpace { get; set; } = 0;
        [Parameter] public bool IsBeakVisible { get; set; }
        
        //[Parameter] public IEnumerable<TItem> ItemsSource { get; set; }
        [Parameter] public RenderFragment<IContextualMenuItem> ItemTemplate { get; set; }
        //[Parameter] public double SubMenuHoverDelay { get; set; } = 250;
        [Parameter] public string Title { get; set; }
        [Parameter] public bool UseTargetWidth { get; set; } = false;
        [Parameter] public bool UseTargetAsMinWidth { get; set; } = false;

        [Parameter] public EventCallback<bool> OnDismiss { get; set; }
        [Parameter] public EventCallback<ContextualMenuBase> OnMenuDismissed { get; set; }
        [Parameter] public EventCallback<ContextualMenuBase> OnMenuOpened { get; set; }

        [Parameter] public bool IsSubMenu { get; set; } = false;

        // for debugging only
        [CascadingParameter(Name ="PortalId")] public string PortalId { get; set; }

        protected bool isOpen = false;

        protected bool HasIcons = false; //needed to shift margins and make space for all 
        protected bool HasCheckables = false;

        public string SubmenuActiveKey { get; set; }
        public void SetSubmenuActiveKey(string key)
        {
            
            if (string.IsNullOrWhiteSpace(key) && string.IsNullOrWhiteSpace(SubmenuActiveKey))
                return;
            System.Diagnostics.Debug.WriteLine($"SetSubmenuActiveKey(\"{key}\") from {this.DirectionalHint}");
            SubmenuActiveKey = key;
            StateHasChanged();
        }

        protected Action OnNotifyCalloutDismiss => () =>
        {

        };

        protected void Dismiss(bool dismissAll = false)
        {
            this.OnDismiss.InvokeAsync(dismissAll);
        }

        protected Action OnCalloutDismiss => () =>
        {
            //if (string.IsNullOrEmpty(SubmenuActiveKey))
            //{
            //    if (this.IsSubMenu)
            //        this.OnNotifyCalloutDismiss();
            //    else
                    this.OnDismiss.InvokeAsync(true);
            //}
        };

        protected Action OnOpenSubmenu => () =>
        {
            
        };

        protected Action<string> OnSetSubmenu => (key) =>
        {
            this.SubmenuActiveKey = key;
        };

        //protected void DismissHandler(bool isDismissed)
        //{
        //    System.Diagnostics.Debug.WriteLine($"ContextualMenu {PortalId} tried dismiss from {this.DirectionalHint} with SubmenuActiveKey = {SubmenuActiveKey}");


        //    if (string.IsNullOrEmpty(SubmenuActiveKey))
        //    {
        //        System.Diagnostics.Debug.WriteLine($"ContextualMenu dismiss successful!  {this.DirectionalHint} with SubmenuActiveKey = {SubmenuActiveKey}");
        //        OnDismiss.InvokeAsync(true);
        //    }
        //}

        protected override Task OnInitializedAsync()
        {
            System.Diagnostics.Debug.WriteLine("Creating ContextualMenu");
            return base.OnInitializedAsync();
        }

        protected override Task OnParametersSetAsync()
        {
            if (this.Items!= null)
            {
                if (this.Items.Count(x => x.IconName != null) > 0)
                    HasIcons = true;
                if (this.Items.Count(x => x.CanCheck == true) > 0)
                    HasCheckables = true;
            }
            return base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await OnMenuOpenedAsync();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task OnMenuOpenedAsync()
        {

            await this.OnMenuOpened.InvokeAsync(this);
        }

        public void Dispose()
        {
            this.OnMenuDismissed.InvokeAsync(this);

        }
    }
}
