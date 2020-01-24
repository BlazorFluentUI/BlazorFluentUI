using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlazorFabric
{
    public class ButtonBase : FabricComponentBase, IDisposable
    {
        internal ButtonBase()
        {

        }

        public ElementReference ButtonRef { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public string Href { get; set; }
        [Parameter] public bool Primary { get; set; }
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public bool AllowDisabledFocus { get; set; }
        [Parameter] public bool PrimaryDisabled { get; set; }
        [Parameter] public bool? Checked { get; set; }
        //[Parameter] public string AriaLabel { get; set; }
        [Parameter] public string AriaDescripton { get; set; }
        //[Parameter] public bool AriaHidden { get; set; }
        [Parameter] public string Text { get; set; }
        [Parameter] public bool Toggle { get; set; }
        [Parameter] public bool Split { get; set; }
        [Parameter] public string IconName { get; set; }
        [Parameter] public bool HideChevron { get; set; }

        [Parameter] public IEnumerable<IContextualMenuItem> MenuItems { get; set; }
        //[Parameter] public RenderFragment ContextualMenuContent { get; set; }
        //[Parameter] public RenderFragment ContextualMenuItemsSource { get; set; }
        //[Parameter] public RenderFragment ContextualMenuItemTemplate { get; set; }

        [Parameter] public EventCallback<bool> CheckedChanged { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
        [Parameter] public EventCallback<KeyboardEventArgs> OnKeyDown { get; set; }
        [Parameter] public ICommand Command { get; set; }
        [Parameter] public object CommandParameter { get; set; }
        [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> UnknownProperties { get; set; }

        [Inject] private IJSRuntime jSRuntime { get; set; }

        protected bool showMenu = false;

        private ICommand command;
        protected bool commandDisabled = false;

        protected bool isChecked = false;

        private bool contextMenuShown = false;

        private bool isCompoundButton = false;
        private bool isSplitButton = false;
        private object _registrationToken;

        private bool _menuShouldFocusOnMount = true;

        protected override Task OnParametersSetAsync()
        {
            showMenu = this.MenuItems != null;

            if (Command == null && command != null)
            {
                command.CanExecuteChanged -= Command_CanExecuteChanged;
                command = null;
            }
            if (Command != null && Command != command)
            {
                if (command != null)
                {
                    command.CanExecuteChanged -= Command_CanExecuteChanged;
                }
                command = Command;
                commandDisabled = !command.CanExecute(CommandParameter);
                Command.CanExecuteChanged += Command_CanExecuteChanged;
            }

            if (this.Checked.HasValue)
            {
                isChecked = this.Checked.Value;
            }

            return base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
            }

            if (contextMenuShown && _registrationToken == null)
                await RegisterListFocusAsync();

            if (!contextMenuShown && _registrationToken != null)
                await DeregisterListFocusAsync();


            await base.OnAfterRenderAsync(firstRender);
        }


        private void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            commandDisabled = !Command.CanExecute(CommandParameter);
            InvokeAsync(StateHasChanged);
        }

        protected async void ClickHandler(MouseEventArgs args)
        {
            if (Toggle)
            {
                this.isChecked = !this.isChecked;
                await this.CheckedChanged.InvokeAsync(this.isChecked);
            }
            if (!isSplitButton && MenuItems != null)
            {
                contextMenuShown = !contextMenuShown;
            }

            await OnClick.InvokeAsync(args);

            if (Command != null)
            {
                Command.Execute(CommandParameter);
            }
        }

        private void KeyDownHandler(KeyboardEventArgs keyboardEventArgs)
        {
            OnKeyDown.InvokeAsync(keyboardEventArgs);
        }

        private void MenuClickHandler(MouseEventArgs args)
        {
            contextMenuShown = !contextMenuShown;
        }

        private async Task RegisterListFocusAsync()
        {
            if (_registrationToken != null)
            {
                await DeregisterListFocusAsync();
            }
            _registrationToken = await jSRuntime.InvokeAsync<string>("BlazorFabricBaseComponent.registerKeyEventsForList", RootElementReference);
        }

        private async Task DeregisterListFocusAsync()
        {
            if (_registrationToken != null)
            {
                await jSRuntime.InvokeVoidAsync("BlazorFabricBaseComponent.deregisterKeyEventsForList", _registrationToken);
                _registrationToken = null;
            }
        }

        public void Focus()
        {

        }

        public void DismissMenu(bool isDismissed)
        {

        }

        public void OpenMenu(bool shouldFocusOnContainer, bool shouldFocusOnMount)
        {

        }

        protected void StartRoot(RenderTreeBuilder builder, string buttonClassName)
        {
            isSplitButton = (Split && OnClick.HasDelegate && MenuItems != null);
            isCompoundButton = this.GetType() == typeof(CompoundButton);
            if (isSplitButton)
            {
                AddSplit(builder, buttonClassName);
            }
            else
            {
                builder.OpenComponent<KeytipData>(21);
                //save attribute space 22, 23,24
                builder.AddAttribute(25, "ChildContent", (RenderFragment)(builder2 =>
                {
                    AddContent(builder2, buttonClassName);
                }));
                builder.CloseComponent();
            }

        }

        protected void AddSplit(RenderTreeBuilder builder, string buttonClassName)
        {
            builder.OpenElement(11, "div");
            builder.AddAttribute(12, "class", $"ms-Button-splitContainer");
            if (!Disabled && !PrimaryDisabled && !commandDisabled)
            {
                builder.AddAttribute(13, "tabindex", 0);
            }

            builder.OpenElement(14, "span");
            builder.AddAttribute(15, "style", "display: flex;");

            AddContent(builder, buttonClassName);
            AddSplitButtonMenu(builder, buttonClassName);
            AddSplitButtonDivider(builder, buttonClassName);

            builder.CloseElement(); // closes span 14
            builder.CloseElement(); //closes div 11
        }


        protected virtual void AddContent(RenderTreeBuilder builder, string buttonClassName)
        {
            
            if (this.Href == null)
            {
                builder.OpenElement(25, "button");
            }
            else
            {
                builder.OpenElement(25, "a");
                builder.AddAttribute(26, "href", this.Href);

            }

            if (Primary)
            {
                buttonClassName += " ms-Button--primary";
            }
            else
            {
                buttonClassName += " ms-Button--default";
            }
            if (isSplitButton)
            {
                builder.AddAttribute(27, "class", $"ms-Button {buttonClassName} {this.ClassName} mediumFont {(Disabled || PrimaryDisabled || commandDisabled ? "is-disabled" : "")} {(isChecked ? "is-checked" : "")}");
                builder.AddAttribute(28, "disabled", (Disabled || PrimaryDisabled || commandDisabled) && !this.AllowDisabledFocus);
            }
            else
            {
                builder.AddAttribute(27, "class", $"ms-Button {buttonClassName} {this.ClassName} mediumFont{(Disabled || commandDisabled ? " is-disabled" : "")}{(isChecked ? " is-checked" : "")}{(contextMenuShown ? " is-expanded" : "")}");
                builder.AddAttribute(28, "disabled", (this.Disabled || commandDisabled) && !this.AllowDisabledFocus);
            }
            builder.AddAttribute(29, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, this.ClickHandler));
            builder.AddAttribute(30, "onkeydown", EventCallback.Factory.Create<KeyboardEventArgs>(this, this.KeyDownHandler));

            builder.AddAttribute(31, "data-is-focusable", this.Disabled || PrimaryDisabled || commandDisabled || isSplitButton ? false : true);
            builder.AddAttribute(32, "style", this.Style);
            builder.AddMultipleAttributes(33, UnknownProperties);

            builder.AddElementReferenceCapture(34, (elementRef) => { RootElementReference = elementRef; });

            builder.OpenElement(35, "span");
            builder.AddAttribute(36, "class", "ms-Button-flexContainer");

            if (this.IconName != null)
            {
                builder.OpenComponent<BlazorFabric.Icon>(40);
                builder.AddAttribute(41, "ClassName", "ms-Button-icon");
                builder.AddAttribute(42, "IconName", this.IconName);
                builder.CloseComponent(); //closes Icon 40
            }
            if (this.Text != null || (isCompoundButton && (this as CompoundButton).SecondaryText != null))
            {
                builder.OpenElement(51, "span");
                builder.AddAttribute(52, "class", "ms-Button-textContainer");
                
                builder.OpenElement(53, "span");
                builder.AddAttribute(54, "class", "ms-Button-label");
                builder.AddContent(55, this.Text ?? "");
                builder.CloseElement();  //closes span (53)

                if (isCompoundButton && (this as CompoundButton).SecondaryText != null)
                {
                    builder.OpenElement(61, "span");
                    builder.AddAttribute(62, "class", "ms-Button-description smallFont");
                    builder.AddContent(63, (this as CompoundButton).SecondaryText);
                    builder.CloseElement(); //closes div 61
                }
                builder.CloseElement();//closes div (51)
            }
            if (this.AriaDescripton != null)
            {
                builder.OpenElement(71, "span");
                builder.AddAttribute(72, "class", "ms-Button-screenReaderText");
                builder.AddContent(73, this.AriaDescripton);
                builder.CloseElement(); //closes span 71
            }
            if (this.Text == null && this.ChildContent != null)
            {
                builder.AddContent(81, this.ChildContent);
            }
            if (!isSplitButton && this.MenuItems != null && !this.HideChevron)
            {
                builder.OpenComponent<BlazorFabric.Icon>(90);
                builder.AddAttribute(91, "IconName", "ChevronDown");
                builder.AddAttribute(92, "ClassName", "ms-Button-menuIcon");
                builder.CloseComponent(); //closes Icon 90
            }
            if (MenuItems != null && contextMenuShown)
            {
                builder.OpenComponent<ContextualMenu>(100);
                builder.AddAttribute(101, "FabricComponentTarget", this);
                builder.AddAttribute(102, "ShouldFocusOnMount", _menuShouldFocusOnMount);
                builder.AddAttribute(103, "OnDismiss", EventCallback.Factory.Create<bool>(this, (isDismissed) =>
                {
                    contextMenuShown = false;
                }));
                builder.AddAttribute(104, "Items", MenuItems);
                builder.AddAttribute(105, "DirectionalHint", DirectionalHint.BottomLeftEdge);
                builder.CloseComponent();  //closes ContextualMenu 100
            }

            builder.CloseElement(); //closes span 35

            //if (false)
            //{
            //    //render Menu, donotlayer,  not yet made
            //}
            //if (false) // menu causes inline-block div
            //{
            //    builder.CloseElement();
            //}

            builder.CloseElement();  // closing button or a
        }


        protected void AddSplitButtonMenu(RenderTreeBuilder builder, string buttonClassName)
        {
            if (Primary)
            {
                builder.OpenComponent<BlazorFabric.PrimaryButton>(105);
                builder.AddAttribute(106, "IconName", "ChevronDown");
                builder.AddAttribute(107, "ClassName", $"ms-Button-menuIcon{(Disabled || commandDisabled ? " is-disabled" : "")} {(isChecked ? " is-checked" : "")}{(contextMenuShown ? " is-expanded" : "")}");
                builder.AddAttribute(108, "OnClick", EventCallback.Factory.Create(this, MenuClickHandler));
                builder.AddAttribute(109, "Disabled", Disabled);
                builder.CloseComponent();
            }
            else
            {
                builder.OpenComponent<BlazorFabric.DefaultButton>(105);
                builder.AddAttribute(106, "IconName", "ChevronDown");
                builder.AddAttribute(107, "ClassName", $"ms-Button-menuIcon{(Disabled || commandDisabled ? " is-disabled" : "")} {(isChecked ? " is-checked" : "")}{(contextMenuShown ? " is-expanded" : "")}");
                builder.AddAttribute(108, "OnClick", EventCallback.Factory.Create(this, MenuClickHandler));
                builder.AddAttribute(109, "Disabled", Disabled);
                builder.CloseComponent();
            }
        }

        protected void AddSplitButtonDivider(RenderTreeBuilder builder, string buttonClassName)
        {
            builder.OpenElement(110, "span");
            if (Primary)
            {
                builder.AddAttribute(111, "class", $"ms-Button-divider ms-Button--primary{(Disabled ? " disabled" :"")}");
            }
            else
            {
                builder.AddAttribute(111, "class", $"ms-Button-divider ms-Button--default{(Disabled ? " disabled" :"")}");
            }
            builder.AddAttribute(112, "aria-hidden", true);
            builder.CloseElement();

        }

        public async void Dispose()
        {
            if (_registrationToken != null)
                await DeregisterListFocusAsync();
        }
    }
}
