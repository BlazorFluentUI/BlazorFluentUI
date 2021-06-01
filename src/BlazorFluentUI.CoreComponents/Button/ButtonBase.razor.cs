using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using BlazorFluentUI.Style;

namespace BlazorFluentUI
{
    public partial class ButtonBase : ButtonParameters, IAsyncDisposable
    {
        public ButtonBase()
        {

        }

        protected bool showMenu = false;

        private ICommand? command;
        protected bool commandDisabled = false;

        protected bool isChecked = false;

        internal bool contextMenuShown = false;

        internal bool isCompoundButton = false;
        internal bool isSplitButton = false;
        private string? _registrationGuid;

        private bool _menuShouldFocusOnMount = true;
        static List<ButtonBase> radioButtons = new();

        protected override Task OnParametersSetAsync()
        {
            showMenu = MenuItems != null;

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

            if (Checked.HasValue)
            {
                isChecked = Checked.Value;
            }
            if(IsRadioButton)
            {
                radioButtons.Add(this);
            }

            return base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (baseModule == null)
                baseModule = await JSRuntime!.InvokeAsync<IJSObjectReference>("import", BasePath);
            if (firstRender)
            {
            }

            if (contextMenuShown && _registrationGuid == null)
                await RegisterListFocusAsync();

            if (!contextMenuShown && _registrationGuid != null)
                await DeregisterListFocusAsync();


            await base.OnAfterRenderAsync(firstRender);
        }

        protected RenderFragment<ButtonBase> CustomBuildRenderTree = button => builder =>
        {
            //base.BuildRenderTree(builder);
            button.StartRoot(builder, "");

        };


        private void Command_CanExecuteChanged(object? sender, EventArgs e)
        {
            commandDisabled = !Command!.CanExecute(CommandParameter);
            InvokeAsync(StateHasChanged);
        }

        protected async void ClickHandler(MouseEventArgs args)
        {
            if (Toggle)
            {
                isChecked = !isChecked;
                await CheckedChanged.InvokeAsync(isChecked);
            }
            if(IsRadioButton)
            {
                isChecked = true;
                await CheckedChanged.InvokeAsync(isChecked);
                foreach(ButtonBase bFUButtonBase in radioButtons)
                {
                    if(bFUButtonBase != this && bFUButtonBase.GroupName == GroupName)
                    {
                        if (bFUButtonBase.isChecked == true)
                        {
                            bFUButtonBase.isChecked = false;
                            await bFUButtonBase.CheckedChanged.InvokeAsync(false);
                            bFUButtonBase.StateHasChanged();
                        }
                    }
                }
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
            if (_registrationGuid != null)
            {
                await DeregisterListFocusAsync();
            }
            _registrationGuid = $"id_{Guid.NewGuid().ToString().Replace("-", "")}";
            await baseModule!.InvokeVoidAsync("registerKeyEventsForList", RootElementReference, _registrationGuid);
        }

        private async Task DeregisterListFocusAsync()
        {
                await baseModule!.InvokeVoidAsync("deregisterKeyEventsForList", _registrationGuid);
       }

        //public async Task Focus()
        //{
        //    await ButtonRef.FocusAsync();
        //}

        public static void DismissMenu(bool isDismissed)
        {

        }

        public static void OpenMenu(bool shouldFocusOnContainer, bool shouldFocusOnMount)
        {

        }

        protected void StartRoot(RenderTreeBuilder builder, string buttonClassName)
        {

            isSplitButton = (Split && OnClick.HasDelegate && MenuItems != null);
            isCompoundButton = SecondaryText != null;
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
            builder.AddElementReferenceCapture(16, element => RootElementReference = element);
            
            AddContent(builder, buttonClassName);
            AddSplitButtonMenu(builder, buttonClassName);
            AddSplitButtonDivider(builder, buttonClassName);

            builder.CloseElement(); // closes span 14
            builder.CloseElement(); //closes div 11
        }


        protected virtual void AddContent(RenderTreeBuilder builder, string buttonClassName)
        {

            if (Href == null)
            {
                builder.OpenElement(25, "button");
            }
            else
            {
                builder.OpenElement(25, "a");
                builder.AddAttribute(26, "href", Href);

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
                builder.AddAttribute(27, "class", $"ms-Button {buttonClassName} {ClassName} {(Disabled || PrimaryDisabled || commandDisabled ? "is-disabled" : "")} {(isChecked ? "is-checked" : "")}");
                builder.AddAttribute(28, "disabled", (Disabled || PrimaryDisabled || commandDisabled) && !AllowDisabledFocus);
            }
            else
            {
                builder.AddAttribute(27, "class", $"ms-Button {buttonClassName} {ClassName} {(Disabled || commandDisabled ? " is-disabled" : "")}{(isChecked ? " is-checked" : "")}{(contextMenuShown ? " is-expanded" : "")}");
                builder.AddAttribute(28, "disabled", (Disabled || commandDisabled) && !AllowDisabledFocus);
            }
            builder.AddAttribute(29, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, ClickHandler));
            builder.AddAttribute(30, "onkeydown", EventCallback.Factory.Create<KeyboardEventArgs>(this, KeyDownHandler));

            builder.AddAttribute(31, "data-is-focusable", !Disabled && !PrimaryDisabled && !commandDisabled && !isSplitButton);
            builder.AddAttribute(32, "style", Style);
            builder.AddMultipleAttributes(33, UnknownProperties);

            if (!isSplitButton)
                builder.AddElementReferenceCapture(34, (element) => RootElementReference = element );

            builder.OpenElement(35, "span");
            builder.AddAttribute(36, "class", "ms-Button-flexContainer");

            if (IconName != null | IconSrc != null)
            {
                builder.OpenComponent<Icon>(40);
                builder.AddAttribute(41, "ClassName", "ms-Button-icon");
                builder.AddAttribute(42, "IconName", IconName);
                builder.AddAttribute(42, "IconSrc", IconSrc);
                builder.CloseComponent(); //closes Icon 40
            }
            if (Text != null || (isCompoundButton && SecondaryText != null))
            {
                builder.OpenElement(51, "span");
                builder.AddAttribute(52, "class", "ms-Button-textContainer");

                builder.OpenElement(53, "span");
                builder.AddAttribute(54, "class", "ms-Button-label");
                builder.AddContent(55, Text ?? "");
                builder.CloseElement();  //closes span (53)

                if (isCompoundButton && SecondaryText != null)
                {
                    builder.OpenElement(61, "span");
                    builder.AddAttribute(62, "class", "ms-Button-description");
                    builder.AddContent(63, SecondaryText);
                    builder.CloseElement(); //closes div 61
                }
                builder.CloseElement();//closes div (51)
            }
            if (AriaDescripton != null)
            {
                builder.OpenElement(71, "span");
                builder.AddAttribute(72, "class", "ms-Button-screenReaderText");
                builder.AddContent(73, AriaDescripton);
                builder.CloseElement(); //closes span 71
            }
            if (Text == null && ContentTemplate != null)
            {
                builder.AddContent(81, ContentTemplate);
            }
            if (!isSplitButton && MenuItems != null && !HideChevron)
            {
                builder.OpenComponent<Icon>(90);
                builder.AddAttribute(91, "IconName", "ChevronDown");
                builder.AddAttribute(92, "IconSrc", "IconSrc");
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
                builder.AddAttribute(106, "ItemTemplate", MenuItemTemplate);
                builder.AddAttribute(107, "SubordinateItemTemplate", SubordinateItemTemplate);

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
                builder.OpenComponent<PrimaryButton>(105);
                builder.AddAttribute(106, "IconName", "ChevronDown");
                builder.AddAttribute(107, "ClassName", $"ms-Button-menuIcon{(Disabled || commandDisabled ? " is-disabled" : "")} {(isChecked ? " is-checked" : "")}{(contextMenuShown ? " is-expanded" : "")}");
                builder.AddAttribute(108, "OnClick", EventCallback.Factory.Create(this, MenuClickHandler));
                builder.AddAttribute(109, "Disabled", Disabled);
                builder.CloseComponent();
            }
            else
            {
                builder.OpenComponent<DefaultButton>(105);
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
                builder.AddAttribute(111, "class", $"ms-Button-divider ms-Button--primary{(Disabled ? " is-disabled" : "")}");
            }
            else
            {
                builder.AddAttribute(111, "class", $"ms-Button-divider ms-Button--default{(Disabled ? " is-disabled" : "")}");
            }
            builder.AddAttribute(112, "aria-hidden", true);
            builder.CloseElement();

        }

        public override async ValueTask DisposeAsync()
        {
            if (_registrationGuid != null)
                await DeregisterListFocusAsync();

            if (IsRadioButton && radioButtons.Contains(this))
            {
                radioButtons.Remove(this);
            }
        }
    }
}