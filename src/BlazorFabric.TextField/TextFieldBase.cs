using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public class TextFieldBase : FabricComponentBase
    {
        [Inject] private IJSRuntime JSRuntime { get; set; }

        [Parameter] public bool Required { get; set; }
        [Parameter] public bool Multiline { get; set; }
        [Parameter] public InputType InputType { get; set; } = InputType.Text;
        [Parameter] public bool Resizable { get; set; } = true;
        [Parameter] public bool AutoAdjustHeight { get; set; }
        [Parameter] public bool Underlined { get; set; }
        [Parameter] public bool Borderless { get; set; }
        [Parameter] public string Label { get; set; }
        [Parameter] public RenderFragment RenderLabel { get; set; }
        [Parameter] public string Description { get; set; }
        [Parameter] public string Prefix { get; set; }
        [Parameter] public string Suffix { get; set; }
        [Parameter] public string DefaultValue { get; set; }
        [Parameter] public string Value { get; set; }
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public bool ReadOnly { get; set; }
        [Parameter] public string ErrorMessage { get; set; }
        [Parameter] public bool ValidateOnFocusIn { get; set; }
        [Parameter] public bool ValidateOnFocusOut { get; set; }
        [Parameter] public bool ValidateOnLoad { get; set; } = true;
        [Parameter] public string AriaLabel { get; set; }
        [Parameter] public bool AutoComplete { get; set; }
        [Parameter] public string Mask { get; set; }
        [Parameter] public string MaskChar { get; set; }
        [Parameter] public string MaskFormat { get; set; }
        [Parameter] public string Placeholder { get; set; }

        [Parameter]
        public EventCallback<KeyboardEventArgs> OnKeyDown { get; set; }
        [Parameter]
        public EventCallback<KeyboardEventArgs> OnKeyUp { get; set; }
        [Parameter]
        public EventCallback<KeyboardEventArgs> OnKeyPress { get; set; }
        [Parameter]
        public Func<string, string> OnGetErrorMessage { get; set; }
        [Parameter]
        public Action<string, string> OnNotifyValidationResult { get; set; }

        //[Parameter]
        //protected Func<UIChangeEventArgs, Task> OnChange { get; set; }
        //[Parameter]
        //protected Func<UIChangeEventArgs, Task> OnInput { get; set; }
        [Parameter]
        public EventCallback<string> OnChange { get; set; }
        [Parameter]
        public EventCallback<string> OnInput { get; set; }

        protected string id = Guid.NewGuid().ToString();
        protected string descriptionId = Guid.NewGuid().ToString();

        private bool firstRendered = false;
        private string latestValidatedValue = "";
        private string currentValue;
        protected string CurrentValue
        {
            get => currentValue;
            set
            {
                if (value == currentValue)
                    return;
                currentValue = value;
                OnChange.InvokeAsync(value);
            }
        }

        protected ElementReference textAreaRef;
        protected string inlineTextAreaStyle = "";
        protected bool isFocused = false;

        protected override Task OnParametersSetAsync()
        {
            if (DefaultValue != null)
                CurrentValue = DefaultValue;

            if (Value != null)
                CurrentValue = Value;

            if (ValidateOnLoad && ValidateAllChanges())
            {
                Validate(CurrentValue);
            }

            return base.OnParametersSetAsync();
        }

        protected async Task InputHandler(ChangeEventArgs args)
        {
            if (ValidateAllChanges())
            {
                Validate((string)args.Value);
            }
            await AdjustInputHeightAsync();
            await OnInput.InvokeAsync((string)args.Value);
            //await InputChanged.InvokeAsync((string)args.Value);
            //if (this.OnInput != null)
            //{
            //    await this.OnInput.Invoke(args);
            //}
        }

        protected async Task ChangeHandler(ChangeEventArgs args)
        {
            await OnChange.InvokeAsync((string)args.Value);
            //await ChangeChanged.InvokeAsync((string)args.Value);
            //if (this.OnChange != null)
            //{
            //    await this.OnChange.Invoke(args);
            //}
        }

        protected Task OnFocus(FocusEventArgs args)
        {
            isFocused = true;
            if (ValidateOnFocusIn)
            {
                Validate(CurrentValue);
            }
            //StateHasChanged();
            return Task.CompletedTask;
        }

        protected Task OnBlur(FocusEventArgs args)
        {
            isFocused = false;
            if (ValidateOnFocusOut)
            {
                Validate(CurrentValue);
            }
            //StateHasChanged();
            return Task.CompletedTask;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRendered)
            {
                firstRendered = true;
                await AdjustInputHeightAsync();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task AdjustInputHeightAsync()
        {
            if (this.AutoAdjustHeight == true && this.Multiline)
            {
                var scrollHeight = await JSRuntime.InvokeAsync<double>("BlazorFabricTextField.getScrollHeight", textAreaRef);
                inlineTextAreaStyle = $"height: {scrollHeight}px";
            }
        }

        private void Validate(string value)
        {
            if (string.IsNullOrEmpty(value) || latestValidatedValue == value)
                return;

            latestValidatedValue = value;
            string errorMessage = OnGetErrorMessage?.Invoke(value);
            if (errorMessage != null)
            {
                ErrorMessage = errorMessage;
            }
            OnNotifyValidationResult?.Invoke(errorMessage, value);
        }

        private bool ValidateAllChanges()
        {
            return !ValidateOnFocusIn && !ValidateOnFocusOut;
        }
    }
}
