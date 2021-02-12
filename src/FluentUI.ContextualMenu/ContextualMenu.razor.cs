using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class ContextualMenu : ResponsiveComponentBase, IAsyncDisposable
    {
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public bool AlignTargetEdge { get; set; }
        //[Parameter] public string AriaLabel { get; set; }
        [Parameter] public int BeakWidth { get; set; } = 16;
        [Parameter] public Rectangle Bounds { get; set; }
        //[Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public IEnumerable<object> Items { get; set; }

        [Parameter] public bool CoverTarget { get; set; }
        [Parameter] public DirectionalHint DirectionalHint { get; set; } = DirectionalHint.BottomAutoEdge;
        [Parameter] public bool DirectionalHintFixed { get; set; }
        [Parameter] public FluentUIComponentBase FabricComponentTarget { get; set; }
        [Parameter] public int GapSpace { get; set; } = 0;
        [Parameter] public bool IsBeakVisible { get; set; } = false;

        //[Parameter] public IEnumerable<TItem> ItemsSource { get; set; }
        [Parameter] public RenderFragment<object> ItemTemplate { get; set; }
        //[Parameter] public double SubMenuHoverDelay { get; set; } = 250;

        /// <summary>
        /// If true, it is tried first to generate the default item
        /// if it is IContextualMenuItem. If not than the 
        /// ItemTemplate will be applied.
        /// </summary>
        [Parameter] public bool SubordinateItemTemplate { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public bool UseTargetWidth { get; set; } = false;
        [Parameter] public bool UseTargetAsMinWidth { get; set; } = false;

        [Parameter] public EventCallback<bool> OnDismiss { get; set; }
        [Parameter] public EventCallback<ContextualMenu> OnMenuDismissed { get; set; }
        [Parameter] public EventCallback<ContextualMenu> OnMenuOpened { get; set; }

        [Parameter] public bool IsSubMenu { get; set; } = false;

        [Parameter] public bool ShouldFocusOnMount { get; set; }

        // for debugging only
        [CascadingParameter(Name = "PortalId")] public string PortalId { get; set; }

        private bool isOpen = false;

        private bool HasIcons = false; //needed to shift margins and make space for all 
        private bool HasCheckables = false;

        private FocusZone _focusZoneReference;

        public string SubmenuActiveKey { get; set; }
        //public void SetSubmenuActiveKey(string key)
        //{

        //    if (string.IsNullOrWhiteSpace(key) && string.IsNullOrWhiteSpace(SubmenuActiveKey))
        //        return;
        //    System.Diagnostics.Debug.WriteLine($"SetSubmenuActiveKey(\"{key}\") from {this.DirectionalHint}");
        //    SubmenuActiveKey = key;
        //    StateHasChanged();
        //}
        private void KeyDownHandler(KeyboardEventArgs args)
        {
            if (args.Key == "ArrowLeft" && IsSubMenu)
            {
                Dismiss(false);
            }
        }


        private void OnCalloutPositioned()
        {
            _focusZoneReference.FocusFirstElement();
        }

        protected Action OnNotifyCalloutDismiss => () =>
        {

        };


        protected void Dismiss(bool dismissAll = false)
        {
            OnDismiss.InvokeAsync(dismissAll);
        }

        protected Action OnCalloutDismiss => () =>
        {
            //if (string.IsNullOrEmpty(SubmenuActiveKey))
            //{
            //    if (this.IsSubMenu)
            //        this.OnNotifyCalloutDismiss();
            //    else
            Dismiss(true);
            //}
        };

        protected Action OnOpenSubmenu => () =>
        {

        };

        protected Action<string> OnSetSubmenu => (key) =>
        {
            SubmenuActiveKey = key;
        };

        protected override Task OnInitializedAsync()
        {
            System.Diagnostics.Debug.WriteLine("Creating ContextualMenu");
            return base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (Items != null)
            {
                if (Items.Count(x =>
                x is IContextualMenuItem ? ((IContextualMenuItem)x).IconName != null | ((IContextualMenuItem)x).IconSrc != null : false) > 0)
                    HasIcons = true;
                if (Items.Count(x => x is IContextualMenuItem ? ((IContextualMenuItem)x).CanCheck == true : false) > 0)
                    HasCheckables = true;
            }
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
            await OnMenuOpened.InvokeAsync(this);
        }

        public override async ValueTask DisposeAsync()
        {
            await OnMenuDismissed.InvokeAsync(this);
            await base.DisposeAsync();
        }


    }
}