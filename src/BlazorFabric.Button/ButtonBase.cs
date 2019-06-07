using BlazorFabric.BaseComponent;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
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

        public ElementRef ButtonRef { get; set; }

        [Parameter] protected RenderFragment ChildContent { get; set; }

        /// <summary>
        ///  If provided, this component will be rendered as an anchor.
        /// </summary>
        [Parameter]
        protected string Href { get; set; }

        /// <summary>
        ///  Changes the visual presentation of the button to be emphasized (if defined)
        /// </summary>
        /// <value>
        /// false
        /// </value>
        [Parameter]
        public bool Primary { get; set; }

        /// <summary>
        /// Whether the button is disabled
        /// </summary>
        ///
        [Parameter]
        protected bool Disabled { get; set; }


        /// <summary>
        ///  Whether the button can have focus in disabled mode
        /// </summary>
        ///
        [Parameter]
        protected bool AllowDisabledFocus { get; set; }


        /// <summary>
        /// If set to true and if this is a splitButton (split == true) then the primary action of a split button is disabled.
        /// </summary>
        [Parameter]
        protected bool PrimaryDisabled { get; set; }


        ///// <summary>
        ///// Theme provided by HOC.
        ///// </summary>
        //[Parameter]
        //protected Theme Theme { get; set; }

        /// <summary>
        ///  Whether the button is checked
        /// </summary>
        [Parameter]
        protected bool? Checked { get; set; }

        [Parameter]
        protected EventCallback<bool> CheckedChanged { get; set; }

        /// <summary>
        /// The aria label of the button for the benefit of screen readers.
        /// </summary>
        ///
        [Parameter]
        protected string AriaLabel { get; set; }

        /// <summary>
        ///  Detailed description of the button for the benefit of screen readers.
        ///
        ///  Besides the compound button, other button types will need more information provided to screen reader.
        /// </summary>
        [Parameter]
        protected string AriaDescripton { get; set; }

        /// <summary>
        /// If provided and is true it adds an 'aria-hidden' attribute instructing screen readers to ignore the element.
        /// </summary>
        [Parameter]
        protected bool AriaHidden { get; set; }

        /// <summary>
        /// Text to render button label. If text is supplied, it will override any string in button children. Other children components will be passed through after the text.
        /// </summary>
        [Parameter]
        protected string Text { get; set; }

        [Parameter]
        protected string SecondaryText { get; set; }

        [Parameter]
        protected bool Toggle { get; set; }

        [Parameter]
        protected bool Split { get; set; }

        [Parameter]
        protected string IconName { get; set; }

        [Parameter]
        //protected Func<ButtonBase, UIMouseEventArgs, Task> Clicked { get; set; }
        protected EventCallback<UIMouseEventArgs> OnClick { get; set; }

        [Parameter]
        protected ICommand Command { get; set; }

        [Parameter]
        protected object CommandParameter { get; set; }


        private ICommand command;
        protected bool commandDisabled = false;

        protected bool isChecked = false;

        protected override Task OnParametersSetAsync()
        {
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

        protected async Task ClickHandler(UIMouseEventArgs args)
        {
            if (Toggle)
            {
                this.isChecked = !this.isChecked;
                await this.CheckedChanged.InvokeAsync(this.isChecked);
            }

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

        public void DismissMenu()
        {

        }

        public void OpenMenu(bool shouldFocusOnContainer, bool shouldFocusOnMount)
        {

        }


        protected void StartRoot(RenderTreeBuilder builder, string buttonClassName)
        {
            if (this.Href == null)
            {
                builder.OpenElement(0, "button");
                
            }
            else
            {
                builder.OpenElement(0, "a");
                //builder.AddElementReferenceCapture(1, (elementRef) => { RootElementRef = elementRef; });
                builder.AddAttribute(2, "href", this.Href);
                
            }

            builder.AddAttribute(3, "class", $"ms-Button {buttonClassName} {this.ClassName} mediumFont {(Disabled ? "is-disabled" : "")} {(isChecked ? "is-checked" : "")}");
            builder.AddAttribute(4, "onclick", this.ClickHandler);
            builder.AddAttribute(5, "disabled", this.Disabled && !this.AllowDisabledFocus);
            builder.AddElementReferenceCapture(6, (elementRef) => { RootElementRef = elementRef; });

            if (false) // menu!
            {
                builder.OpenElement(5, "div");
                builder.AddAttribute(6, "style", "display: inline-block;");
            }
            builder.OpenElement(7, "div");
            builder.AddAttribute(8, "class", "ms-Button-flexContainer");

            if (this.IconName != null)
            {
                builder.OpenComponent<BlazorFabric.Icon.Icon>(9);
                builder.AddAttribute(10, "ClassName", "ms-Button-icon");
                builder.AddAttribute(11, "IconName", this.IconName);
                builder.CloseComponent();
            }
            if (this.Text != null)
            {
                builder.OpenElement(12, "div");
                builder.AddAttribute(13, "class", "ms-Button-textContainer");
                builder.OpenElement(14, "div");
                builder.AddAttribute(15, "class", "ms-Button-label");
                builder.AddContent(16, this.Text);
                builder.CloseElement();
                if (this.SecondaryText != null)
                {
                    builder.OpenElement(17, "div");
                    builder.AddAttribute(18, "class", "ms-Button-description");
                    builder.AddContent(19, this.SecondaryText);
                    builder.CloseElement();
                }
                builder.CloseElement();
            }
            if (this.AriaDescripton != null)
            {
                builder.OpenElement(20, "span");
                builder.AddAttribute(21, "class", "ms-Button-screenReaderText");
                builder.AddContent(22, this.AriaDescripton);
                builder.CloseElement();
            }
            if (this.Text == null && this.ChildContent != null)
            {
                builder.AddContent(23, this.ChildContent);
            }
            if (!this.Split && false)
            {
                builder.OpenComponent<BlazorFabric.Icon.Icon>(24);
                builder.AddAttribute(25, "IconName", "ChevronDown");
                builder.AddAttribute(26, "ClassName", "ms-Button-menuIcon");
                builder.CloseComponent();
            }
            if (false)
            {
               //render Menu, !donotlayer,  not yet made
            }



            builder.CloseElement();

            if (false)
            {
                //render Menu, donotlayer,  not yet made
            }
            if (false) // menu causes inline-block div
            {
                builder.CloseElement();
            }
            
            builder.CloseElement();
            
        }
        
       


    }
}
