using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric.TextField
{
    public class TextFieldBase : ComponentBase
    {
        [Inject] private IJSRuntime JSRuntime { get; set; }

        [Parameter] protected bool Required { get; set; }
        [Parameter] protected bool Multiline { get; set; }
        [Parameter] protected bool Resizable { get; set; } = true;
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
        [Parameter] protected string Placeholder { get; set; }

        //[Parameter]
        //protected Func<UIChangeEventArgs, Task> OnChange { get; set; }
        //[Parameter]
        //protected Func<UIChangeEventArgs, Task> OnInput { get; set; }
        [Parameter]
        protected EventCallback<string> OnChange { get; set; }
        [Parameter]
        protected EventCallback<string> OnInput { get; set; }

        protected string id = Guid.NewGuid().ToString();
        protected string descriptionId = Guid.NewGuid().ToString();

        private bool firstRendered = false;
        protected string currentValue;
        protected ElementRef textAreaRef;
        protected string inlineTextAreaStyle = "";
        protected bool isFocused = false;

        protected override Task OnParametersSetAsync()
        {
            if (DefaultValue != null)
                currentValue = DefaultValue;

            if (Value != null)
                currentValue = Value;

            return base.OnParametersSetAsync();
        }

        protected async Task InputHandler(UIChangeEventArgs args)
        {
            await AdjustInputHeightAsync();
            await OnInput.InvokeAsync((string)args.Value);
            //await InputChanged.InvokeAsync((string)args.Value);
            //if (this.OnInput != null)
            //{
            //    await this.OnInput.Invoke(args);
            //}
        }

        protected async Task ChangeHandler(UIChangeEventArgs args)
        {
            await OnChange.InvokeAsync((string)args.Value);
            //await ChangeChanged.InvokeAsync((string)args.Value);
            //if (this.OnChange != null)
            //{
            //    await this.OnChange.Invoke(args);
            //}
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

        protected override async Task OnAfterRenderAsync()
        {
            if (!firstRendered)
            {
                firstRendered = true;
                await AdjustInputHeightAsync();
            }
            await base.OnAfterRenderAsync();
        }

        private async Task AdjustInputHeightAsync()
        {
            if (this.AutoAdjustHeight == true && this.Multiline==true)
            {
                var scrollHeight = await JSRuntime.InvokeAsync<int>("BlazorFabricTextField.getScrollHeight", textAreaRef);
                inlineTextAreaStyle = $"height: {scrollHeight}px"; 
            }
        }

    }
}
