using FluentUI.Style;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class TextField : FluentUIComponentBase
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
        [Parameter] public RenderFragment PrefixContent { get; set; }
        [Parameter] public string Suffix { get; set; }
        [Parameter] public RenderFragment SuffixContent { get; set; }
        [Parameter] public string DefaultValue { get; set; }
        [Parameter] public string Value { get; set; }
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public bool ReadOnly { get; set; }
        [Parameter] public string ErrorMessage { get; set; }
        [Parameter] public bool ValidateOnFocusIn { get; set; }
        [Parameter] public bool ValidateOnFocusOut { get; set; }
        [Parameter] public bool ValidateOnLoad { get; set; } = true;
        [Parameter] public int DeferredValidationTime { get; set; } = 0;
        [Parameter] public AutoComplete AutoComplete { get; set; } = AutoComplete.On;
        [Parameter] public string Placeholder { get; set; }
        [Parameter] public string IconName { get; set; }
        [Parameter] public string IconSrc { get; set; }

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

        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }  // expose click event for Combobox and pickers
        [Parameter]
        public EventCallback<FocusEventArgs> OnBlur { get; set; }
        [Parameter]
        public EventCallback<FocusEventArgs> OnFocus { get; set; }

        //[Parameter]
        //protected Func<UIChangeEventArgs, Task> OnChange { get; set; }
        //[Parameter]
        //protected Func<UIChangeEventArgs, Task> OnInput { get; set; }
        [Parameter]
        public EventCallback<string> OnChange { get; set; }
        [Parameter]
        public EventCallback<string> OnInput { get; set; }

        /// <summary>
        /// Gets or sets an expression that identifies the bound value.
        /// </summary>
        [Parameter] public Expression<Func<string>>? ValueExpression { get; set; }
        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }

        [CascadingParameter] EditContext CascadedEditContext { get; set; } = default!;


        private bool shouldRender = true;

        protected string id = Guid.NewGuid().ToString();
        protected string descriptionId = Guid.NewGuid().ToString();

        private bool firstRendered = false;
        private int deferredValidationTime;
        private bool defaultErrorMessageIsSet;
        private string latestValidatedValue = "";
        private string currentValue;
        private bool hasIcon;
        private bool hasLabel;
        private Rule TextField_Field_HasIcon = new Rule();

        private ICollection<IRule> TextFieldLocalRules { get; set; } = new List<IRule>();
        private ICollection<Task> DeferredValidationTasks = new List<Task>();

        private FieldIdentifier FieldIdentifier;

        protected string CurrentValue
        {
            get => currentValue;
            set
            {
                if (value == currentValue)
                    return;
                currentValue = value;

                InputHandler(new ChangeEventArgs { Value = currentValue });
                //ChangeHandler(new ChangeEventArgs() { Value = value }).ConfigureAwait(true);
            }
        }

        protected ElementReference textAreaRef;
        protected double autoAdjustedHeight = -1;
        protected bool isFocused = false;


        protected override Task OnInitializedAsync()
        {
            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                defaultErrorMessageIsSet = true;
            }

            // to prevent changes after initialisation
            deferredValidationTime = DeferredValidationTime;
            hasIcon = !string.IsNullOrWhiteSpace(IconName) || !string.IsNullOrWhiteSpace(IconSrc);
            hasLabel = !string.IsNullOrWhiteSpace(Label);
            if (hasIcon)
            {
                CreateLocalCss();
            }

            return base.OnInitializedAsync();
        }

        private IReadOnlyDictionary<string, object> lastParameters;

        public override Task SetParametersAsync(ParameterView parameters)
        {
            if (lastParameters != null)
            {
                var currentParameters = parameters.ToDictionary();
                foreach (var curr in currentParameters)
                {
                    var lastValue = lastParameters[curr.Key];
                    if (curr.Value == null)
                    {
                        if (lastValue != null)
                        {
                            shouldRender = true;
                            break;
                        }
                        continue;
                    }
                    Type t = curr.Value.GetType();
                    bool isComparableType = t.IsPrimitive || t == typeof(string) || t == typeof(RenderFragment);
                    if (isComparableType)
                    {
                        if (curr.Value != lastValue)
                        {
                            shouldRender = true;
                            break;
                        }
                    }
                    else if (t.IsEnum)
                    {
                        if ((int)curr.Value != (int)lastValue)
                        {
                            shouldRender = true;
                            break;
                        }
                    }
                    else if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(EventCallback<>))
                    {
                        //is this enough?
                        var prop = t.GetProperty("HasDelegate");
                        bool aHasDelegate = (bool)prop.GetValue(curr.Value);
                        bool bHasDelegate = (bool)prop.GetValue(lastValue);

                        if ((aHasDelegate && !bHasDelegate) || (!aHasDelegate && bHasDelegate))
                        {
                            shouldRender = true;
                            break;
                        }

                    }
                    else
                    {
                        //custom check for changes for non-primitive types.  Must manually check each type
                        switch (curr.Key)
                        {
                            case "ValueExpression":
                                if (curr.Value != lastValue)
                                {
                                    if (curr.Value != null && lastValue != null && curr.Value.ToString() == lastValue.ToString())
                                    {
                                    }
                                    else
                                    {
                                        shouldRender = true;
                                        break;
                                    }
                                }
                                break;
                            case "OnGetErrorMessage":
                            case "OnNotifyValidationResult":
                                if ((curr.Value == null && lastValue != null) || (curr.Value != null && lastValue == null))
                                {
                                    shouldRender = true;
                                    break;
                                }
                                var a = (curr.Value as System.Delegate).Method.GetMethodBody().GetILAsByteArray();
                                var b = (lastValue as System.Delegate).Method.GetMethodBody().GetILAsByteArray();
                                if (a.Length != b.Length)
                                {
                                    shouldRender = true;
                                    break;
                                }
                                for (int i = 0; i < a.Length; i++)
                                {
                                    if (a[i] != b[i])
                                    {
                                        shouldRender = true;
                                        break;
                                    }
                                }
                                break;
                            case "Theme":
                                // ignore Theme changes as they do not affect TextField internally
                                break;
                            default:

                                break;

                        }
                    }

                }

                lastParameters = currentParameters;
            }
            else
            {
                lastParameters = parameters.ToDictionary();
                shouldRender = true;
            }
            parameters.SetParameterProperties(this);

            if (CascadedEditContext != null && ValueExpression != null)
            {
                CascadedEditContext.OnValidationStateChanged += CascadedEditContext_OnValidationStateChanged;
                FieldIdentifier = FieldIdentifier.Create<string>(ValueExpression);
            }


            return base.SetParametersAsync(ParameterView.Empty);
        }

        private void CascadedEditContext_OnValidationStateChanged(object? sender, ValidationStateChangedEventArgs e)
        {
            //SetAdditionalAttributesIfValidationFailed();

            InvokeAsync(() =>
            {
                shouldRender = true;
                StateHasChanged();
            });
            //invokeasync required for serverside
        }

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

        protected override bool ShouldRender()
        {
            if (shouldRender)
            {
                shouldRender = false;
                return true;
            }
            return false;
        }

        protected override void OnThemeChanged()
        {
            if (hasIcon)
            {
                SetStyle();
            }
        }

        private void CreateLocalCss()
        {
            if (hasIcon)
            {
                TextField_Field_HasIcon.Selector = new ClassSelector() { SelectorName = "ms-TextField-field" };

                TextFieldLocalRules.Add(TextField_Field_HasIcon);
            }
        }


        protected void InputHandler(ChangeEventArgs args)
        {
            if (!defaultErrorMessageIsSet && OnGetErrorMessage != null && !string.IsNullOrWhiteSpace(ErrorMessage))
            {
                ErrorMessage = "";
                //StateHasChanged();
            }

            if (ValidateAllChanges())
            {
                _ = DeferredValidation((string)args.Value).ConfigureAwait(false);
            }

            _ = AdjustInputHeightAsync();

            _ = OnInput.InvokeAsync((string)args.Value);
            shouldRender = true;
        }

        protected async Task ChangeHandler(ChangeEventArgs args)
        {
            await OnChange.InvokeAsync((string)args.Value);
            await ValueChanged.InvokeAsync((string)args.Value);
            shouldRender = true;
        }

        protected async Task OnFocusInternal(FocusEventArgs args)
        {
            if (OnFocus.HasDelegate)
            {
                await OnFocus.InvokeAsync(args);
            }
            isFocused = true;
            if (ValidateOnFocusIn && !defaultErrorMessageIsSet)
            {
                Validate(CurrentValue);
            }
            //return Task.CompletedTask;
            shouldRender = true;
        }

        protected async Task OnBlurInternal(FocusEventArgs args)
        {
            if (OnBlur.HasDelegate)
            {
                await OnBlur.InvokeAsync(args);
            }
            isFocused = false;
            if (ValidateOnFocusOut && !defaultErrorMessageIsSet)
            {
                Validate(CurrentValue);
            }
            shouldRender = true;
            //return Task.CompletedTask;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRendered)
            {
                firstRendered = true;
                _ = AdjustInputHeightAsync();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task AdjustInputHeightAsync()
        {
            if (AutoAdjustHeight == true && Multiline)
            {
                var scrollHeight = await JSRuntime.InvokeAsync<double>("FluentUIBaseComponent.getScrollHeight", textAreaRef);
                //inlineTextAreaStyle = $"height: {scrollHeight}px";
                if (autoAdjustedHeight != scrollHeight)
                {
                    autoAdjustedHeight = scrollHeight;
                    await InvokeAsync(StateHasChanged);
                }
            }
            else
            {
                if (autoAdjustedHeight != -1)
                {
                    autoAdjustedHeight = -1;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        protected string GetAutoCompleteString()
        {
            var value = AutoComplete.ToString();
            value = Char.ToLowerInvariant(value[0]) + value.Substring(1);
            string result = "";
            foreach (var c in value.ToCharArray())
            {
                if (Char.IsUpper(c))
                {
                    result += "-";
                    result += Char.ToLowerInvariant(c);
                }
                else
                    result += c;
            }
            return result;
        }

        private void Validate(string value)
        {
            if (CascadedEditContext != null && ValueExpression != null)
            {
                CascadedEditContext.NotifyFieldChanged(FieldIdentifier);
                if (CascadedEditContext.GetValidationMessages(FieldIdentifier).Any())
                {
                    ErrorMessage = string.Join(',', CascadedEditContext.GetValidationMessages(FieldIdentifier));
                }
                else
                {
                    ErrorMessage = "";
                }
            }
            else
            {
                if (value == null || latestValidatedValue == value)
                    return;

                latestValidatedValue = value;
                string errorMessage = OnGetErrorMessage?.Invoke(value);
                if (errorMessage != null)
                {
                    ErrorMessage = errorMessage;
                }
                OnNotifyValidationResult?.Invoke(errorMessage, value);

                OnNotifyValidationResult?.Invoke(ErrorMessage, value);
            }

        }

        private bool ValidateAllChanges()
        {
            return (OnGetErrorMessage != null && !defaultErrorMessageIsSet && !ValidateOnFocusIn && !ValidateOnFocusOut && (firstRendered && !ValidateOnLoad)) || CascadedEditContext != null;
        }

        private async Task DeferredValidation(string value)
        {
            if (deferredValidationTime == 0)
            {
                Validate(value);
            }
            else
            {
                DeferredValidationTasks.Add(Task.Run(async () =>
                {
                    await Task.Delay(deferredValidationTime);
                }));
                var TaskCount = DeferredValidationTasks.Count();
                await Task.WhenAll(DeferredValidationTasks.ToArray());
                if (TaskCount == DeferredValidationTasks.Count())
                {
                    _ = Task.Run(() =>
                    {
                        InvokeAsync(() =>
                        {
                            Validate(value);
                            shouldRender = true;
                            StateHasChanged();
                        });
                        //invokeasync required for serverside
                    }).ConfigureAwait(false);
                }
            }
        }

        private void SetStyle()
        {
            if (hasIcon)
            {
                TextField_Field_HasIcon.Properties = new CssString()
                {
                    Css = $"padding-right:{(Multiline ? "40px" : "24px")};"
                };
            }
        }


    }
}