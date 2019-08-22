using BlazorFabric.BaseComponent;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlazorFabric.Button
{
    public class ButtonBase : FabricComponentBase
    {
        internal ButtonBase()
        {

        }

        public ElementReference ButtonRef { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] protected string Href { get; set; }
        [Parameter] public bool Primary { get; set; }
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public bool AllowDisabledFocus { get; set; }
        [Parameter] public bool PrimaryDisabled { get; set; }
        [Parameter] public bool? Checked { get; set; }
        [Parameter] public string AriaLabel { get; set; }
        [Parameter] public string AriaDescripton { get; set; }
        [Parameter] public bool AriaHidden { get; set; }
        [Parameter] public string Text { get; set; }
        [Parameter] public string SecondaryText { get; set; }
        [Parameter] public bool Toggle { get; set; }
        [Parameter] public bool Split { get; set; }
        [Parameter] public string IconName { get; set; }

        [Parameter] public RenderFragment ContextualMenuContent { get; set; }
        [Parameter] public RenderFragment ContextualMenuItemsSource { get; set; }
        [Parameter] public RenderFragment ContextualMenuItemTemplate { get; set; }

        [Parameter] public EventCallback<bool> CheckedChanged { get; set; }
        [Parameter] public EventCallback<UIMouseEventArgs> OnClick { get; set; }
        [Parameter] public ICommand Command { get; set; }
        [Parameter] public object CommandParameter { get; set; }

        protected bool showMenu = false;

        private ICommand command;
        protected bool commandDisabled = false;

        protected bool isChecked = false;

        private bool contextMenuShown = false;

        protected override Task OnParametersSetAsync()
        {
            showMenu = this.ContextualMenuContent != null || this.ContextualMenuItemsSource != null;
            //if (MenuContent == null)
            //{
            //    Debug.WriteLine("MenuContent is null");
            //}
            //else
            //{
            //    Debug.WriteLine("MenuContent is NOT null");

            //}

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
                Command.CanExecuteChanged += Command_CanExecuteChanged;
            }

            if (this.Checked.HasValue)
            {
                isChecked = this.Checked.Value;
            }

            return base.OnParametersSetAsync();
        }

        private void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            commandDisabled = Command.CanExecute(CommandParameter);
        }

        protected async void ClickHandler(UIMouseEventArgs args)
        {
            if (Toggle)
            {
                this.isChecked = !this.isChecked;
                await this.CheckedChanged.InvokeAsync(this.isChecked);
            }
            contextMenuShown = !contextMenuShown;

            await OnClick.InvokeAsync(args);
            //if (Clicked != null)
            //{
            //    await Clicked.Invoke(this, args);
            //}
            if (Command != null)
            {
                Command.Execute(CommandParameter);
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
           //could add a DoNotLayer option here later
            //{
                AddContent(builder, buttonClassName);
            //}


        }

        protected void AddContent(RenderTreeBuilder builder, string buttonClassName)
        {
            if (this.Href == null)
            {
                builder.OpenElement(2, "button");
            }
            else
            {
                builder.OpenElement(2, "a");
                //builder.AddElementReferenceCapture(3, (elementRef) => { RootElementReference = elementRef; });
                builder.AddAttribute(3, "href", this.Href);

            }

            builder.AddAttribute(4, "class", $"ms-Button {buttonClassName} {this.ClassName} mediumFont {(Disabled ? "is-disabled" : "")} {(isChecked ? "is-checked" : "")}");
            builder.AddAttribute(5, "onclick", EventCallback.Factory.Create<UIMouseEventArgs>(this, this.ClickHandler));
            builder.AddAttribute(6, "disabled", this.Disabled && !this.AllowDisabledFocus);
            builder.AddAttribute(7, "data-is-focusable", this.Disabled || this.Split ? false : true);

            builder.AddElementReferenceCapture(8, (elementRef) => { RootElementReference = elementRef; });

            //if (MenuContent != null) // menu!
            //{
            //    builder.OpenElement(7, "div");
            //    builder.AddAttribute(8, "style", "display: inline-block;");

            //    builder.CloseElement();
            //}
            //skipping KeytipData component
            builder.OpenElement(9, "div");
            builder.AddAttribute(10, "class", "ms-Button-flexContainer");

            if (this.IconName != null)
            {
                builder.OpenComponent<BlazorFabric.Icon.Icon>(11);
                builder.AddAttribute(12, "ClassName", "ms-Button-icon");
                builder.AddAttribute(13, "IconName", this.IconName);
                builder.CloseComponent();
            }
            if (this.Text != null)
            {
                builder.OpenElement(14, "div");
                builder.AddAttribute(15, "class", "ms-Button-textContainer");
                builder.OpenElement(16, "div");
                builder.AddAttribute(17, "class", "ms-Button-label");
                builder.AddContent(18, this.Text);
                builder.CloseElement();
                if (this.SecondaryText != null)
                {
                    builder.OpenElement(19, "div");
                    builder.AddAttribute(20, "class", "ms-Button-description");
                    builder.AddContent(21, this.SecondaryText);
                    builder.CloseElement();
                }
                builder.CloseElement();
            }
            if (this.AriaDescripton != null)
            {
                builder.OpenElement(22, "span");
                builder.AddAttribute(23, "class", "ms-Button-screenReaderText");
                builder.AddContent(24, this.AriaDescripton);
                builder.CloseElement();
            }
            if (this.Text == null && this.ChildContent != null)
            {
                builder.AddContent(25, this.ChildContent);
            }
            if (!this.Split && false)
            {
                builder.OpenComponent<BlazorFabric.Icon.Icon>(26);
                builder.AddAttribute(27, "IconName", "ChevronDown");
                builder.AddAttribute(28, "ClassName", "ms-Button-menuIcon");
                builder.CloseComponent();
            }
            //menu here!
            if (ContextualMenuItemsSource != null && contextMenuShown)
            {
                //builder.OpenElement(0, "div");
                //builder.AddAttribute(1, "style", "display:inline-block;");
                //AddContent(builder, buttonClassName);
                ////builder.AddContent(50, ;
                //builder.AddAttribute(51, "ChildContent", MenuContent);
                //builder.CloseComponent();
            }
            else if (ContextualMenuContent != null && contextMenuShown)
            {                
                builder.OpenComponent<ContextualMenu.ContextualMenu<object>>(29);
                builder.AddAttribute(30, "FabricComponentTarget", this);
                builder.AddAttribute(31, "OnDismiss", EventCallback.Factory.Create<bool>(this, (isDismissed) => { contextMenuShown = false; }));
                builder.AddAttribute(32, "IsOpen", contextMenuShown);
                builder.AddAttribute(33, "ChildContent", ContextualMenuContent);
                builder.CloseComponent();
            }



            builder.CloseElement();

            //if (false)
            //{
            //    //render Menu, donotlayer,  not yet made
            //}
            //if (false) // menu causes inline-block div
            //{
            //    builder.CloseElement();
            //}

            builder.CloseElement();
        }

        
        
       


    }
}
