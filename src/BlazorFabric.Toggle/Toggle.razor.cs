using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class Toggle : FabricComponentBase
    {
        //[Parameter] public string AriaLabel { get; set; }
        [Parameter] public bool? Checked { get; set; }
        [Parameter] public bool DefaultChecked { get; set; }
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public bool InlineLabel { get; set; }
        [Parameter] public string Label { get; set; }
        [Parameter] public string OffText { get; set; }
        [Parameter] public string OnLabel { get; set; }

        [Parameter] public EventCallback<bool> CheckedChanged { get; set; }
        [Parameter] public string OnText { get; set; }

        protected bool IsChecked;
        protected string Id = Guid.NewGuid().ToString();
        protected string LabelId => Id + "-label";
        protected string StateTextId => Id + "-stateText";
        protected string BadAriaLabel;
        protected string LabelledById;
        protected string StateText => IsChecked ? OnText : OffText;

        private bool onOffMissing = false;

        private bool firstLoad = true;

        protected string GetClassExtras()
        {
            string classNames = "";

            if (InlineLabel)
                classNames += " ms-Toggle--inlineLabel";
            
            if (Disabled)
                classNames += " ms-Toggle--disabled";
            else
                classNames += " ms-Toggle--enabled";

            if (IsChecked)
                classNames += " ms-Toggle--checked";
            else
                classNames += " ms-Toggle--unchecked";

            if (onOffMissing)
                classNames += " ms-Toggle--onOffMissing";

            classNames += $" {ClassName}";

            return classNames;
        }


        protected override Task OnParametersSetAsync()
        {
            if (firstLoad)
            {
                firstLoad = false;
                IsChecked = this.Checked ?? this.DefaultChecked;
            }
            else
            {
                IsChecked = this.Checked ?? IsChecked;
            }

            //if (IsChecked)
            //{
            //    StateText = OnText;
            //}
            //else
            //{
            //    StateText = OffText;
            //}
            if (string.IsNullOrWhiteSpace(AriaLabel) && string.IsNullOrWhiteSpace(BadAriaLabel))
                LabelledById = LabelId;
            else
                LabelledById = StateTextId;

            if (string.IsNullOrWhiteSpace(OffText) && string.IsNullOrWhiteSpace(OnText))
                onOffMissing = true;

            return base.OnParametersSetAsync();
        }

        protected Task OnClick(MouseEventArgs args)
        {
            Debug.WriteLine("Clicked");
            if (!Disabled)
            {
                Debug.WriteLine("Not Disabled");
                if (Checked == null)  // only update internally if Checked is not set
                {
                    Debug.WriteLine($"Checked not set so switch to: {!IsChecked}");
                    IsChecked = !IsChecked;
                }
            }

            return this.CheckedChanged.InvokeAsync(!IsChecked);

            //return Task.CompletedTask;
        }
    }
}
