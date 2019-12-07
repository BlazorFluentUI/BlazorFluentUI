using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class Checkbox : FabricComponentBase
    {
        [Parameter]
        public bool? Checked { get; set; }

        /// <summary>
        /// Default checked state. Mutually exclusive to "checked". Use this if you want an uncontrolled component, and
        /// want the Checkbox instance to maintain its own state.
        /// </summary>
        [Parameter]
        public bool DefaultChecked { get; set; }

        /// <summary>
        /// Label to display next to the checkbox.
        /// </summary>
        [Parameter]
        public string Label { get; set; }

        /// <summary>
        /// Disabled state of the checkbox.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Accessible label for the checkbox.
        /// </summary>
        //[Parameter]
        //public string AriaLabel { get; set; }

        //[Parameter]
        //public string AriaLabelledBy { get; set; }

        //[Parameter]
        //public string AriaDescribedBy { get; set; }

        [Parameter]
        public int? AriaPositionInSet { get; set; }

        [Parameter]
        public int? AriaSetSize { get; set; }

        [Parameter]
        public bool Reversed { get; set; }

        
        [Parameter]
        public Func<ChangeEventArgs, Task> Changed { get; set; }

        [Parameter]
        public EventCallback<bool> CheckedChanged { get; set; }

        protected bool isChecked;
        protected string Id = Guid.NewGuid().ToString();

        protected override Task OnParametersSetAsync()
        {
            this.isChecked = this.Checked ?? this.DefaultChecked;
            return base.OnParametersSetAsync();
        }

        protected async Task OnChange(ChangeEventArgs args)
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

        protected Task OnClick(MouseEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine($"Clicked");
            return Task.CompletedTask;
        }

    }
}
