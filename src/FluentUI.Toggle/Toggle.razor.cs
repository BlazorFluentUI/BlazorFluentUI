using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FluentUI
{
    public partial class Toggle : FluentUIComponentBase
    {
        [Parameter] public bool? Checked { get; set; }
        [Parameter] public bool DefaultChecked { get; set; }
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public bool InlineLabel { get; set; }
        [Parameter] public string Label { get; set; }
        [Parameter] public string OffText { get; set; }
        [Parameter] public string OnLabel { get; set; }

        [Parameter] public EventCallback<bool?> CheckedChanged { get; set; }
        [Parameter] public string OnText { get; set; }
        [Parameter] public string DefaultText { get; set; }

        [Parameter] public ICommand Command { get; set; }
        [Parameter] public object CommandParameter { get; set; }

        private ICommand command;
        protected bool commandDisabled = false;

        protected bool IsChecked;
        protected string Id = Guid.NewGuid().ToString();
        protected string LabelId => Id + "-label";
        protected string StateTextId => Id + "-stateText";
        protected string BadAriaLabel;
        protected string LabelledById;
        protected string StateText => Checked.HasValue ? (Checked.GetValueOrDefault() ? OnText : OffText) : DefaultText ?? "";

        private bool onOffMissing = false;

        private ICollection<Rule> ToggleRules { get; set; }

        private void Command_CanExecuteChanged(object sender, EventArgs e)
        {
            commandDisabled = !Command.CanExecute(CommandParameter);
            InvokeAsync(StateHasChanged);
        }

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

        protected override void OnInitialized()
        {
            IsChecked = Checked ?? DefaultChecked;
            base.OnInitialized();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        }

        protected override Task OnParametersSetAsync()
        {
            IsChecked = Checked ?? IsChecked;

            if (string.IsNullOrWhiteSpace(AriaLabel) && string.IsNullOrWhiteSpace(BadAriaLabel))
                LabelledById = LabelId;
            else
                LabelledById = StateTextId;

            if (string.IsNullOrWhiteSpace(OffText) && string.IsNullOrWhiteSpace(OnText))
                onOffMissing = true;

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

            if (Command != null)
            {
                Command.Execute(CommandParameter);
            }

            return CheckedChanged.InvokeAsync(!IsChecked);
        }

    }
}
