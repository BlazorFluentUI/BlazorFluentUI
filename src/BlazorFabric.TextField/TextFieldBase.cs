using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric.TextField
{
    public class TextFieldBase : ComponentBase
    {
        [Parameter] protected bool Required { get; set; }
        [Parameter] protected bool Multiline { get; set; }
        [Parameter] protected bool Resizable { get; set; }
        [Parameter] protected bool AutoAdjustHeight { get; set; }
        [Parameter] protected bool Underlined { get; set; }
        [Parameter] protected bool Borderless { get; set; }
        [Parameter] protected string Label { get; set; }
        [Parameter] protected RenderFragment RenderLabel { get; set; }
        [Parameter] protected string Description { get; set; }
        [Parameter] protected string Prefix { get; set; }
        [Parameter] protected string Suffix { get; set; }
        [Parameter] protected string DefaultValue { get; set; }
        [Parameter] protected string Value { get; set; }
        [Parameter] protected bool Disabled { get; set; }
        [Parameter] protected bool ReadOnly { get; set; }
        [Parameter] protected string ErrorMessage { get; set; }
        [Parameter] protected string AriaLabel { get; set; }
        [Parameter] protected bool AutoComplete { get; set; }
        [Parameter] protected string Mask { get; set; }
        [Parameter] protected string MaskChar { get; set; }
        [Parameter] protected string MaskFormat { get; set; }

        protected string id = Guid.NewGuid().ToString();
        protected string descriptionId = Guid.NewGuid().ToString();

        protected string errorMessageShown = null;

        protected string currentValue;

        protected bool isFocused = false;

        protected override Task OnParametersSetAsync()
        {
            if (DefaultValue != null)
                currentValue = DefaultValue;

            if (Value != null)
                currentValue = Value;

            return base.OnParametersSetAsync();
        }

        protected Task OnInput(UIChangeEventArgs args)
        {

            return Task.CompletedTask;
        }

        protected Task OnChange(UIChangeEventArgs args)
        {

            return Task.CompletedTask;
        }

        protected Task OnFocus(UIFocusEventArgs args)
        {
            isFocused = true;
            //StateHasChanged();
            return Task.CompletedTask;
        }

        protected Task OnBlur(UIFocusEventArgs args)
        {
            isFocused = false;
            //StateHasChanged();
            return Task.CompletedTask;
        }

    }
}
