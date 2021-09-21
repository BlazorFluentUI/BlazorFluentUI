using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class Checkbox : FluentUIComponentBase
    {
        [Parameter] public int? AriaPositionInSet { get; set; }

        [Parameter] public int? AriaSetSize { get; set; }

        /// <summary>
        /// Allows you to set the checkbox to be at the before (start) or after (end) the label.
        /// </summary>
        [Parameter] public BoxSide BoxSide { get; set; }
        /// <summary>
        /// Checked state. Mutually exclusive to "defaultChecked". Use this if you control the checked state at a higher
        /// level and plan to pass in the correct value based on handling onChange events and re-rendering.
        /// </summary>
        [Parameter] public bool? Checked { get; set; }

        /// <summary>
        /// Default checked state. Mutually exclusive to "checked". Use this if you want an uncontrolled component, and
        /// want the Checkbox instance to maintain its own state.
        /// </summary>
        [Parameter] public bool DefaultChecked { get; set; }

        /// <summary>
        /// Optional uncontrolled indeterminate visual state for checkbox. Setting indeterminate state takes visual
        /// precedence over checked or defaultChecked props given but does not affect checked state.
        /// This is not a toggleable state. On load the checkbox will receive indeterminate visual state and after
        /// the user's first click it will be removed exposing the true state of the checkbox.
        /// </summary>
        [Parameter] public bool DefaultIndeterminate { get; set; }

        /// <summary>
        /// Disabled state of the checkbox.
        /// </summary>
        [Parameter] public bool Disabled { get; set; }

        [Parameter] public bool ValidateOnInit { get; set; }

        /// <summary>
        /// Optional controlled indeterminate visual state for checkbox. Setting indeterminate state takes visual
        /// precedence over checked or defaultChecked props given but does not affect checked state.
        /// This should not be a toggleable state. On load the checkbox will receive indeterminate visual state and after
        /// the first user click it should be removed by your supplied
        /// onChange callback function exposing the true state of the checkbox.
        /// </summary>
        [Parameter] public bool? Indeterminate { get; set; }

        /// <summary>
        /// Label to display next to the checkbox.
        /// </summary>
        [Parameter] public string? Label { get; set; }


        [Parameter] public EventCallback<bool> CheckedChanged { get; set; }

        [Parameter] public RenderFragment? OnRenderLabel { get; set; }

        [Parameter] public Expression<Func<bool>>? CheckedExpression { get; set; }

        [Parameter] public EventCallback<bool> IndeterminateChanged { get; set; }

        [CascadingParameter] EditContext CascadedEditContext { get; set; } = default!;

        private FieldIdentifier FieldIdentifier;

        private readonly string Id = $"id_{Guid.NewGuid().ToString().Replace("-", "")}";
        private bool _isChecked;
        private bool _reversed;
        private bool _indeterminate;
        private bool _checkedUncontrolled;
        private bool _indeterminateUncontrolled;
        private bool _indeterminateChanged;

        protected override void OnInitialized()
        {
            if (!Checked.HasValue)
            {
                _isChecked = DefaultChecked;
                _checkedUncontrolled = true;
            }
            if (!Indeterminate.HasValue)
            {
                _indeterminate = DefaultIndeterminate;
                _indeterminateUncontrolled = true;
            }
            else
            {
                _indeterminate = Indeterminate.Value;
            }
            _reversed = BoxSide == BoxSide.End;

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

            base.OnInitialized();
        }

        protected override Task OnParametersSetAsync()
        {
            if (!_checkedUncontrolled)
            {
                _isChecked = Checked.GetValueOrDefault();
            }
            return base.OnParametersSetAsync();
        }

        private void CascadedEditContext_OnValidationStateChanged(object? sender, ValidationStateChangedEventArgs e)
        {
            InvokeAsync(() => StateHasChanged());  //invokeasync required for serverside
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (_indeterminateChanged)
            {
                if (!_checkedUncontrolled)
                {
                    Checked = !Checked;
                }
                else
                {
                    _isChecked = !_isChecked;
                }
                _indeterminateChanged = false;
                StateHasChanged();
            }
            base.OnAfterRender(firstRender);
        }

        protected async Task InternalOnChange(ChangeEventArgs args)
        {
            if (_indeterminate)
            {
                _indeterminate = false;
                _indeterminateChanged = true;

                if (!_indeterminateUncontrolled)
                {
                    Indeterminate = false;
                    await IndeterminateChanged.InvokeAsync(false);
                }
            }

            bool? value = (bool?)args.Value;
            if (!_checkedUncontrolled)
            {
                Checked = value;
            }
            else
            {
                _isChecked = value.GetValueOrDefault();
            }

            await CheckedChanged.InvokeAsync(value.GetValueOrDefault());

            CascadedEditContext?.NotifyFieldChanged(FieldIdentifier);
        }

        
    }
}
