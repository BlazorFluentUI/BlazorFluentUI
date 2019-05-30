using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric.Checkbox
{
    public class CheckboxBase : ComponentBase
    {
        [Parameter]
        protected bool? Checked { get; set; }

        /// <summary>
        /// Default checked state. Mutually exclusive to "checked". Use this if you want an uncontrolled component, and
        /// want the Checkbox instance to maintain its own state.
        /// </summary>
        [Parameter]
        protected bool DefaultChecked { get; set; }

        /// <summary>
        /// Label to display next to the checkbox.
        /// </summary>
        [Parameter]
        protected string Label { get; set; }

        /// <summary>
        /// Disabled state of the checkbox.
        /// </summary>
        [Parameter]
        protected bool Disabled { get; set; }

        /// <summary>
        /// Accessible label for the checkbox.
        /// </summary>
        [Parameter]
        protected string AriaLabel { get; set; }

        [Parameter]
        protected string AriaLabelledBy { get; set; }

        [Parameter]
        protected string AriaDescribedBy { get; set; }

        [Parameter]
        protected int? AriaPositionInSet { get; set; }

        [Parameter]
        protected int? AriaSetSize { get; set; }

        [Parameter]
        protected bool Reversed { get; set; }

        
        [Parameter]
        protected Func<UIChangeEventArgs, Task> Changed { get; set; }

        [Parameter]
        protected EventCallback<bool> CheckedChanged { get; set; }

        protected bool isChecked;
        protected string Id = Guid.NewGuid().ToString();

        protected override Task OnParametersSetAsync()
        {
            this.isChecked = this.Checked ?? this.DefaultChecked;
            return base.OnParametersSetAsync();
        }

        protected async Task OnChange(UIChangeEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine($"Changed to {args.Value}");
            if (!this.Checked.HasValue)
            {
                this.isChecked = (bool)args.Value;
            }
            

            if (this.Changed != null)
                await this.Changed.Invoke(args);

            await this.CheckedChanged.InvokeAsync((bool)args.Value);

        }

        protected Task OnClick(UIMouseEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine($"Clicked");
            return Task.CompletedTask;
        }

    }
}
