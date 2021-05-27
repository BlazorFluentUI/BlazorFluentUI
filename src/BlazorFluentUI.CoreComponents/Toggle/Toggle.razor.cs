using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlazorFluentUI
{
    public partial class Toggle : FluentUIComponentBase
    {
        [Parameter] public bool? Checked { get; set; }
        [Parameter] public bool DefaultChecked { get; set; }

        [Parameter] public bool ValidateOnInit { get; set; }
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public bool InlineLabel { get; set; }
        [Parameter] public string ? Label { get; set; }
        [Parameter] public RenderFragment? CustomLabel { get; set; }
        [Parameter] public string? OffText { get; set; }
        [Parameter] public string? OnLabel { get; set; }

        [Parameter] public EventCallback<bool> CheckedChanged { get; set; }
        [Parameter] public Expression<Func<bool>>? CheckedExpression { get; set; }
        [Parameter] public string? OnText { get; set; }
        [Parameter] public string? DefaultText { get; set; }

        [Parameter] public ICommand? Command { get; set; }
        [Parameter] public object? CommandParameter { get; set; }
        [Parameter] public ToggleRole Role { get; set; } = ToggleRole.Switch;

        [CascadingParameter] EditContext CascadedEditContext { get; set; } = default!;
        private FieldIdentifier FieldIdentifier;

        private ICommand? command;
        protected bool commandDisabled = false;

        private bool IsChecked;
        private bool CheckedUncontrolled;

        private readonly string Id = $"id_{Guid.NewGuid().ToString().Replace("-", "")}";

        private string LabelId => Id + "_label";
        private string StateTextId => Id + "_stateText";
   
        private string? LabelledById;
        private string? StateText;

        private bool onOffMissing = false;

        private ICollection<Rule>? ToggleRules { get; set; }

        private void Command_CanExecuteChanged(object? sender, EventArgs e)
        {
            commandDisabled = !Command!.CanExecute(CommandParameter);
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
            if (!Checked.HasValue)
            {
                IsChecked = DefaultChecked;
                CheckedUncontrolled = true;
                
            }

            if (CascadedEditContext != null)
            {
                if (CheckedExpression == null)
                {
                    throw new InvalidOperationException($"{GetType()} requires a value for the 'CheckedExpression' " +
                        $"parameter. Normally this is provided automatically when using 'bind-Checked'.");
                }
                FieldIdentifier = FieldIdentifier.Create(CheckedExpression);

                if (ValidateOnInit)
                {
                    CascadedEditContext.NotifyFieldChanged(FieldIdentifier);
                }

                CascadedEditContext.OnValidationStateChanged += CascadedEditContext_OnValidationStateChanged;
            }

            //IsChecked = Checked ?? DefaultChecked;
            //CheckedChanged.InvokeAsync(IsChecked);

            base.OnInitialized();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        }

        protected override Task OnParametersSetAsync()
        {
            if (!CheckedUncontrolled)
            {
                IsChecked = Checked ?? IsChecked;
            }
            StateText = (IsChecked ? OnText : OffText) ?? DefaultText ?? "";

            if (string.IsNullOrWhiteSpace(AriaLabel))
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

        private void CascadedEditContext_OnValidationStateChanged(object? sender, ValidationStateChangedEventArgs e)
        {
            InvokeAsync(() => StateHasChanged());  //invokeasync required for serverside
        }

        protected Task OnClick(MouseEventArgs args)
        {
            //Debug.WriteLine($"Clicked and {(!IsChecked ? "on" : "off")} ({(!Disabled ? "not" : "")} Disabled)");

            if (Command != null)
            {
                Command.Execute(CommandParameter);
            }

            Checked = IsChecked = !IsChecked;
            StateText = (IsChecked ? OnText : OffText) ?? DefaultText ?? "";
            return CheckedChanged.InvokeAsync(IsChecked);
        }

        private string GetToggleRole()
        {
            return Role switch
            {
                ToggleRole.Checkbox => "checkbox",
                ToggleRole.MenuItemCheckbox => "menuitemcheckbox",
                ToggleRole.Switch => "switch",
                _ => "switch",
            };
        }

    }
}
