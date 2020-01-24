using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public class CalendarDateButton : ButtonBase
    {
        [Parameter] public bool AriaSelected { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            StartRoot(builder, "ms-Button--calendarDate");

        }

        protected override void AddContent(RenderTreeBuilder builder, string buttonClassName)
        {
            builder.OpenElement(21, "button");

            builder.AddAttribute(23, "class", $"mediumFont ms-Button {buttonClassName} {(Disabled ? "is-disabled" : "")} {(isChecked ? "is-checked" : "")} {this.ClassName}");
            builder.AddAttribute(24, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, this.ClickHandler));
            builder.AddAttribute(25, "disabled", this.Disabled && !this.AllowDisabledFocus);
            builder.AddAttribute(26, "data-is-focusable", this.Disabled || this.Split ? false : true);
            builder.AddAttribute(27, "style", this.Style);

            builder.AddAttribute(28, "aria-selected", this.AriaSelected);
            builder.AddAttribute(29, "aria-label", this.AriaLabel);

            builder.AddElementReferenceCapture(30, (elementRef) => { RootElementReference = elementRef; });

            //skipping KeytipData component
            builder.OpenElement(40, "div");
            builder.AddAttribute(41, "class", "ms-Button-flexContainer");

            if (this.Text != null)
            {
                builder.OpenElement(51, "div");
                builder.AddAttribute(52, "class", "ms-Button-textContainer");
                builder.OpenElement(53, "div");
                builder.AddAttribute(54, "class", "ms-Button-label");
                builder.AddContent(55, this.Text);
                builder.CloseElement();
                builder.CloseElement();
            }
            if (this.AriaDescripton != null)
            {
                builder.OpenElement(71, "span");
                builder.AddAttribute(72, "class", "ms-Button-screenReaderText");
                builder.AddContent(73, this.AriaDescripton);
                builder.CloseElement();
            }


            builder.CloseElement();
            builder.CloseElement();
        }

    }
}
