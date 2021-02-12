using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using System.Linq;

namespace FluentUI.ContextualMenuInternal
{
    public class ContextualMenuItem : FluentUIComponentBase, IDisposable
    {
        //[Inject] private IJSRuntime jsRuntime { get; set; }

        [Parameter] public string Href { get; set; }
        [Parameter] public string Key { get; set; }
        [Parameter] public string Text { get; set; }
        [Parameter] public string SecondaryText { get; set; }
        [Parameter] public string IconName { get; set; }
        [Parameter] public string IconSrc { get; set; }
        [Parameter] public ContextualMenuItemType ItemType { get; set; }

        //[Parameter] public RenderFragment SubmenuContent { get; set; }
        [Parameter] public IEnumerable<IContextualMenuItem> Items { get; set; }
        [Parameter] public RenderFragment<IContextualMenuItem> ItemTemplate { get; set; }

        [Parameter] public bool Disabled { get; set; }
        [Parameter] public bool CanCheck { get; set; }
        [Parameter] public bool Checked { get; set; }
        [Parameter] public bool Split { get; set; }

        [Parameter] public EventCallback<ItemClickedArgs> OnClick { get; set; }
        [Parameter] public EventCallback<KeyboardEventArgs> OnKeyDown { get; set; }

        [Parameter] public ICommand Command { get; set; }
        [Parameter] public object CommandParameter { get; set; }

        //[Parameter] public ContextualMenu ParentContextualMenu { get; set; }

        //[CascadingParameter] public ContextualMenuBase ContextualMenu { get; set; }
        [Parameter] public EventCallback<string> SetSubmenu { get; set; }

        [Parameter] public EventCallback<bool> DismissMenu { get; set; }
        //[Parameter] public EventCallback DismissSubMenu { get; set; }

        [Parameter] public string SubmenuActiveKey { get; set; }

        [Parameter] public bool HasIcons { get; set; }
        [Parameter] public bool HasCheckables { get; set; }

        [Parameter] public EventCallback NotifyCalloutDismiss { get; set; }  // we need this to chain notifications that the callout wants to be dismissed (from scrolling, resize, or focus lost)

        protected bool isSubMenuOpen = false;

        //private ElementReference linkElementReference;
        //private List<int> eventHandlerIds;
        private Timer enterTimer = new Timer();


        //[JSInvokable] public void MouseEnterHandler()
        //{
        //    //if (ParentContextualMenu.SubmenuActiveKey == this.Key)
        //    if (isSubMenuOpen)
        //        return;
        //    if (!enterTimer.Enabled)
        //        enterTimer.Start();
        //}

        public void MouseEnterHandler(EventArgs args)
        {
            //if (ParentContextualMenu.SubmenuActiveKey == this.Key)
            if (isSubMenuOpen)
                return;
            if (!enterTimer.Enabled)
                enterTimer.Start();
        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //if (eventHandlerIds == null)
            //    eventHandlerIds = await jsRuntime.InvokeAsync<List<int>>("FluentUIContextualMenu.registerHandlers", this.RootElementReference, DotNetObjectReference.Create(this));
            await base.OnAfterRenderAsync(firstRender);
        }

        public void Dispose()
        {
            //jsRuntime.InvokeAsync<object>("FluentUIContextualMenu.unregisterHandlers", eventHandlerIds);
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {

            //if (IconName != null && parameters.GetValueOrDefault<string>("IconName") == null)
            //{
            //    if (ParentContextualMenu != null)
            //        ParentContextualMenu.HasIconCount--;
            //}
            //if (CanCheck && parameters.GetValueOrDefault<bool>("CanCheck") == false)
            //{
            //    if (ParentContextualMenu != null)
            //        ParentContextualMenu.HasCheckable--;
            //}
            return base.SetParametersAsync(parameters);
        }

        protected override Task OnParametersSetAsync()
        {
            if (Key == SubmenuActiveKey)
                isSubMenuOpen = true;
            else
                isSubMenuOpen = false;
            //if (IconName != null)
            //    ParentContextualMenu.HasIconCount++;
            //if (CanCheck == true)
            //    ParentContextualMenu.HasCheckable++;

            //if (!enterTimer.Enabled)
            //    enterTimer.Interval = ContextualMenu.SubMenuHoverDelay;
            //if (!enterTimer.Enabled)
            //    enterTimer.Interval = ContextualMenu.SubMenuHoverDelay;


            return base.OnParametersSetAsync();
        }

        protected override Task OnInitializedAsync()
        {
            System.Diagnostics.Debug.WriteLine("Creating MenuItem");
            enterTimer.AutoReset = false;
            enterTimer.Interval = 250; // can't set this in ParametersSetAsync because it resets the countdown after each refresh, which causes another refresh, infinite loop
            enterTimer.Elapsed += EnterTimer_Elapsed;
            //leaveTimer.Elapsed += LeaveTimer_Elapsed;
            return base.OnInitializedAsync();
        }

        private async void DismissSubMenu(bool dismissAll)
        {
            await SetSubmenu.InvokeAsync(null);
            if (dismissAll)
            {
                await DismissMenu.InvokeAsync(true);
            }
        }

        private async void KeyDownHandler(KeyboardEventArgs args)
        {
            // check for a right key press so we can open the submenu if it exists
            if (args.Key == "ArrowRight" && !isSubMenuOpen)
            {
                await SetSubmenu.InvokeAsync(Key);
            }

            // send it to the parent ContextualMenu so we can use a left key press to close the menu.
            await OnKeyDown.InvokeAsync(args);
        }

        //[JSInvokable] 
        //public async void ClickHandler()
        //{
        //    System.Diagnostics.Debug.WriteLine($"ContextualMenuItem called click: {this.Key}");

        //    await this.OnClick.InvokeAsync(new ItemClickedArgs() { MouseEventArgs = new MouseEventArgs(), Key = this.Key });
        //    this.Command?.Execute(CommandParameter);

        //    if (!CanCheck)
        //        await this.DismissMenu.InvokeAsync(true);
        //    //await ParentContextualMenu.OnDismiss.InvokeAsync(true);
        //}

        public async Task ClickHandler(MouseEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine($"ContextualMenuItem called click: {Key}");

            await OnClick.InvokeAsync(new ItemClickedArgs() { MouseEventArgs = new MouseEventArgs(), Key = Key });
            Command?.Execute(CommandParameter);

            if (!CanCheck)
                await DismissMenu.InvokeAsync(true);
            //await ParentContextualMenu.OnDismiss.InvokeAsync(true);
        }

        //private void LeaveTimer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    //do nothing for now... eventually
        //}

        private async void EnterTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await InvokeAsync(() =>
            {
                enterTimer.Stop();

                if (Items != null)
                    SetSubmenu.InvokeAsync(Key);
                else
                    SetSubmenu.InvokeAsync(null);
            });
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            if (ItemType == ContextualMenuItemType.Divider)
            {
                RenderSeparator(builder);
            }
            else if (ItemType == ContextualMenuItemType.Header)
            {
                RenderSeparator(builder);
                RenderHeader(builder);
            }
            else if (ItemType == ContextualMenuItemType.Section)
            {

            }
            else
            {
                RenderNormalItem(builder);
            }
        }

        private void RenderSeparator(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "li");
            builder.AddAttribute(1, "role", "separator");
            builder.AddAttribute(2, "class", "ms-ContextualMenu-divider");
            builder.AddAttribute(3, "aria-hidden", true);
            builder.AddElementReferenceCapture(4, (element) => RootElementReference = element);
            builder.CloseElement();
        }

        private void RenderHeader(RenderTreeBuilder builder)
        {
            builder.OpenElement(11, "li");
            {
                builder.AddAttribute(12, "class", "ms-ContextualMenu-item");
                builder.OpenElement(13, "div");
                {
                    builder.AddAttribute(14, "class", "ms-ContextualMenu-header");
                    RenderMenuItemContent(builder);
                }
                builder.CloseElement();
            }
            //builder.AddElementReferenceCapture(15, (element) => RootElementReference = element);
            builder.CloseElement();
        }

        private void RenderNormalItem(RenderTreeBuilder builder)
        {
            builder.OpenElement(11, "li");
            builder.AddAttribute(12, "class", $"ms-ContextualMenu-item mediumFont {(Disabled ? "is-disabled" : "")} {(Checked ? "is-checked" : "")} {(isSubMenuOpen ? "is-expanded" : "")}");
            builder.AddElementReferenceCapture(13, (element) => RootElementReference = element);
            RenderItemKind(builder);
            builder.CloseElement();
        }

        private void RenderItemKind(RenderTreeBuilder builder)
        {
            if (Href != null)
            {
                RenderMenuAnchor(builder);
                return;
            }
            if (Split && Items != null)
            {
                //RenderSplitButton
                return;
            }
            RenderButtonItem(builder);
        }

        private void RenderMenuAnchor(RenderTreeBuilder builder)
        {
            builder.OpenElement(20, "div");
            //skip KeytipData
            builder.OpenElement(21, "a");
            builder.AddAttribute(22, "href", Href);
            builder.AddAttribute(23, "onkeydown", EventCallback.Factory.Create(this, KeyDownHandler));
            builder.AddAttribute(23, "onclick", EventCallback.Factory.Create(this, () => DismissMenu.InvokeAsync(true)));
            //builder.AddAttribute(23, "onclick:preventDefault", true);
            builder.AddAttribute(24, "role", "menuitem");
            builder.AddAttribute(25, "class", $"ms-ContextualMenu-link ms-ContextualMenu-anchorLink mediumFont {(Items?.Concat(Items.Where(x => x.Items != null).SelectMany(x => GetChild(x.Items))).FirstOrDefault(x => x.Checked == true) != null ? $" subgroup-is-checked" : "")}");  //the subgroup check is only for NavBar
            builder.AddAttribute(27, "onmouseenter", EventCallback.Factory.Create(this, MouseEnterHandler));
            builder.AddAttribute(28, "onmouseenter:preventDefault", true);

            RenderMenuItemContent(builder);

            builder.CloseElement();
            builder.CloseElement();
        }

        private IEnumerable<IContextualMenuItem> GetChild(IEnumerable<IContextualMenuItem> list)
        {
            return list.Concat(list.Where(x => x.Items != null).SelectMany(x => GetChild(x.Items)));
        }

        private void RenderButtonItem(RenderTreeBuilder builder)
        {
            builder.OpenElement(20, "div");
            //skip KeytipData
            builder.OpenElement(21, "button");
            builder.AddAttribute(22, "onkeydown", EventCallback.Factory.Create(this, KeyDownHandler));
            builder.AddAttribute(23, "onclick", EventCallback.Factory.Create(this, ClickHandler));
            builder.AddAttribute(24, "onclick:preventDefault", true);
            builder.AddAttribute(25, "role", "menuitem");
            builder.AddAttribute(26, "class", $"ms-ContextualMenu-link mediumFont {(Items?.Concat(Items.Where(x => x.Items != null).SelectMany(x => GetChild(x.Items))).FirstOrDefault(x => x.Checked == true) != null ? $" subgroup-is-checked" : "")}");  //the subgroup check is only for NavBar
            builder.AddAttribute(27, "onmouseenter", EventCallback.Factory.Create(this, MouseEnterHandler));
            builder.AddAttribute(28, "onmouseenter:preventDefault", true);
            //builder.AddElementReferenceCapture(29, (linkElement) => linkElementReference = linkElement);  //need this to register mouse events in javascript (not available in Blazor)

            RenderMenuItemContent(builder);

            builder.CloseElement();
            builder.CloseElement();
        }


        private void RenderMenuItemContent(RenderTreeBuilder builder)
        {
            builder.OpenElement(40, "div");
            builder.AddAttribute(41, "class", Split ? "ms-ContextualMenu-linkContentMenu" : "ms-ContextualMenu-linkContent");

            if (HasCheckables)
                RenderCheckMarkIcon(builder);
            if (HasIcons)
                RenderItemIcon(builder);
            RenderItemName(builder);
            if (SecondaryText != null)
                RenderSecondaryText(builder);
            if (Items != null)
            {
                RenderSubMenuIcon(builder);
                if (isSubMenuOpen)
                    //if (ParentContextualMenu.SubmenuActiveKey == Key)
                    RenderSubContextualMenu(builder);
            }
            builder.CloseElement();
        }

        private void RenderCheckMarkIcon(RenderTreeBuilder builder)
        {
            builder.OpenComponent<Icon>(45);
            builder.AddAttribute(46, "IconName", Checked ? "CheckMark" : "");
            builder.AddAttribute(47, "ClassName", "ms-ContextualMenu-checkmarkIcon");
            builder.CloseComponent();
        }


        private void RenderItemIcon(RenderTreeBuilder builder)
        {
            builder.OpenComponent<Icon>(51);
            builder.AddAttribute(52, "IconName", IconName);
            builder.AddAttribute(53, "IconSrc", IconSrc);
            builder.AddAttribute(54, "ClassName", "ms-ContextualMenu-icon");
            builder.CloseComponent();
        }

        private void RenderItemName(RenderTreeBuilder builder)
        {
            builder.OpenElement(55, "span");
            builder.AddAttribute(56, "class", "ms-ContextualMenu-itemText");
            builder.AddContent(57, Text);
            builder.CloseElement();
        }

        private void RenderSecondaryText(RenderTreeBuilder builder)
        {
            builder.OpenElement(61, "span");
            builder.AddAttribute(62, "class", "ms-ContextualMenu-secondaryText");
            builder.AddContent(63, SecondaryText);
            builder.CloseElement();
        }

        private void RenderSubMenuIcon(RenderTreeBuilder builder)
        {
            builder.OpenComponent<Icon>(65);
            builder.AddAttribute(66, "ClassName", "ms-ContextualMenu-submenuIcon");
            builder.AddAttribute(67, "IconName", "ChevronRight");  //ignore RTL for now.
            builder.CloseComponent();
        }

        private void RenderSubContextualMenu(RenderTreeBuilder builder)
        {
            builder.OpenComponent<ContextualMenu>(70);
            builder.AddAttribute(71, "FabricComponentTarget", this);
            builder.AddAttribute(72, "OnDismiss", EventCallback.Factory.Create<bool>(this, DismissSubMenu));//EventCallback.Factory.Create<bool>(this, (isDismissed) => { ParentContextualMenu.SetSubmenuActiveKey(""); ParentContextualMenu.OnDismiss.InvokeAsync(true); }));
            //builder.AddAttribute(73, "IsOpen", ParentContextualMenu.SubmenuActiveKey == Key);
            builder.AddAttribute(74, "DirectionalHint", DirectionalHint.RightTopEdge);
            builder.AddAttribute(75, "Items", Items);
            builder.AddAttribute(76, "IsSubMenu", true);

            //builder.AddAttribute(76, "ParentContextualMenu", this.ParentContextualMenu);
            //builder.AddAttribute(75, "ChildContent", SubmenuContent);
            builder.CloseComponent();
        }








    }
}
