using BlazorFluentUI.Style;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public class TextField : TextFieldBase<string> { }

    public partial class TextFieldBase<TValue> : FluentUIComponentBase
    {
        [Inject] private IJSRuntime? JSRuntime { get; set; }
        [Parameter] public bool Required { get; set; }
        [Parameter] public bool Multiline { get; set; }
        [Parameter] public InputType InputType { get; set; } = InputType.Text;
        [Parameter] public bool Resizable { get; set; } = true;
        [Parameter] public bool AutoAdjustHeight { get; set; }
        [Parameter] public bool Underlined { get; set; }
        [Parameter] public bool Borderless { get; set; }
        [Parameter] public string? Label { get; set; }
        [Parameter] public RenderFragment? RenderLabel { get; set; }
        [Parameter] public string? Description { get; set; }
        [Parameter] public string? Prefix { get; set; }
        [Parameter] public RenderFragment? PrefixContent { get; set; }
        [Parameter] public string? Suffix { get; set; }
        [Parameter] public RenderFragment? SuffixContent { get; set; }
        [Parameter] public TValue? DefaultValue { get; set; }
        [Parameter] public TValue? Value { get; set; }
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public bool ReadOnly { get; set; }
        [Parameter] public string? ErrorMessage { get; set; }
        [Parameter] public bool ValidateOnFocusIn { get; set; }
        [Parameter] public bool ValidateOnFocusOut { get; set; }
        [Parameter] public bool ValidateOnLoad { get; set; } = true;
        [Parameter] public int DeferredValidationTime { get; set; } = 0;
        [Parameter] public AutoComplete AutoComplete { get; set; } = AutoComplete.On;
        [Parameter] public string? Placeholder { get; set; }
        [Parameter] public string? IconName { get; set; }
        [Parameter] public string? IconSrc { get; set; }

        [Parameter]
        public EventCallback<KeyboardEventArgs> OnKeyDown { get; set; }
        [Parameter]
        public EventCallback<KeyboardEventArgs> OnKeyUp { get; set; }
        [Parameter]
        public EventCallback<KeyboardEventArgs> OnKeyPress { get; set; }
        [Parameter]
        public Func<TValue, string>? OnGetErrorMessage { get; set; }
        [Parameter]
        public Action<TValue, string>? OnNotifyValidationResult { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }  // expose click event for Combobox and pickers
        [Parameter]
        public EventCallback<FocusEventArgs> OnBlur { get; set; }
        [Parameter]
        public EventCallback<FocusEventArgs> OnFocus { get; set; }

        [Parameter] public EventCallback<TValue> OnChange { get; set; }
        [Parameter] public EventCallback<TValue> OnInput { get; set; }

        /// <summary>
        /// Gets or sets an expression that identifies the bound value.
        /// </summary>
        [Parameter] public Expression<Func<TValue>>? ValueExpression { get; set; }
        [Parameter] public EventCallback<TValue> ValueChanged { get; set; }

        [CascadingParameter] EditContext CascadedEditContext { get; set; } = default!;

        protected string id = Guid.NewGuid().ToString();
        protected string descriptionId = Guid.NewGuid().ToString();

        private bool shouldRender = true;
        private bool firstRendered = false;
        private int deferredValidationTime;
        private bool defaultErrorMessageIsSet;
        private TValue? latestValidatedValue = default;
        private TValue? currentValue;
        private bool hasIcon;
        private bool hasLabel;

        private readonly Rule TextField_Field_HasIcon = new();

        private ICollection<IRule> TextFieldLocalRules { get; set; } = new List<IRule>();
        private readonly ICollection<Task> DeferredValidationTasks = new List<Task>();

        private FieldIdentifier FieldIdentifier;


        private ValidationMessageStore? _parsingValidationMessages;
        private Type? _nullableUnderlyingType;
        private bool _previousParsingAttemptFailed;

        protected TValue? CurrentValue
        {
            get => currentValue;
            set
            {
                bool hasChanged = !EqualityComparer<TValue>.Default.Equals(value, currentValue);
                if (hasChanged)
                {

                    currentValue = value;

                    InputHandler(new ChangeEventArgs { Value = currentValue }).ConfigureAwait(true);
                    //ChangeHandler(new ChangeEventArgs() { Value = value }).ConfigureAwait(true);
                }
            }
        }


        protected ElementReference textAreaRef;
        protected ElementReference inputRef;
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

        private IReadOnlyDictionary<string, object>? lastParameters;

        public override Task SetParametersAsync(ParameterView parameters)
        {
            if (lastParameters != null)
            {
                IReadOnlyDictionary<string, object>? currentParameters = parameters.ToDictionary();
                foreach (KeyValuePair<string, object> curr in currentParameters)
                {
                    object? lastValue = lastParameters[curr.Key];
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
                        System.Reflection.PropertyInfo? prop = t.GetProperty("HasDelegate");
                        bool aHasDelegate = (bool?)prop!.GetValue(curr.Value) ?? false;
                        bool bHasDelegate = (bool?)prop!.GetValue(lastValue) ?? false;

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
                                byte[]? a = ((Delegate)curr.Value!).Method.GetMethodBody()!.GetILAsByteArray();
                                byte[]? b = ((Delegate)lastValue!).Method.GetMethodBody()!.GetILAsByteArray();
                                if (a!.Length != b!.Length)
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
                FieldIdentifier = FieldIdentifier.Create<TValue>(ValueExpression);

                _nullableUnderlyingType = Nullable.GetUnderlyingType(typeof(TValue));
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
            string? localValue = FormatValueAsString(DefaultValue);
            if (localValue != null && localValue != "0")
                CurrentValue = DefaultValue;

            localValue = FormatValueAsString(Value);
            if (localValue != null && localValue != "0")
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


        protected async Task InputHandler(ChangeEventArgs args)
        {
            if (!defaultErrorMessageIsSet && OnGetErrorMessage != null && !string.IsNullOrWhiteSpace(ErrorMessage))
            {
                ErrorMessage = "";
                //StateHasChanged();
            }
            if (TryParseValueFromString((string?)args.Value!.ToString(), out TValue? result, out _))
            {

                if (ValidateAllChanges())
                {
                    await DeferredValidation(result).ConfigureAwait(false);
                }

                await AdjustInputHeightAsync();

                await OnInput.InvokeAsync(result);
                shouldRender = true;
            }
        }

        protected async Task ChangeHandler(ChangeEventArgs args)
        {
            if (TryParseValueFromString((string?)args.Value, out TValue? result, out _))
            {
                await OnChange.InvokeAsync(result);
                await ValueChanged.InvokeAsync(result);
                shouldRender = true;
            }
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
                double scrollHeight = await JSRuntime!.InvokeAsync<double>("FluentUIBaseComponent.getScrollHeight", textAreaRef);
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
            string? value = AutoComplete.ToString();
            value = char.ToLowerInvariant(value[0]) + value[1..];
            string result = "";
            foreach (char c in value.ToCharArray())
            {
                if (char.IsUpper(c))
                {
                    result += "-";
                    result += Char.ToLowerInvariant(c);
                }
                else
                    result += c;
            }
            return result;
        }

        private void Validate(TValue? value)
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
                if (value == null || (latestValidatedValue != null && latestValidatedValue.Equals(value)))
                    return;

                latestValidatedValue = value;
                string? errorMessage = OnGetErrorMessage?.Invoke(value);
                if (errorMessage != null)
                {
                    ErrorMessage = errorMessage;
                }
                //ToDo Why is this sent twice?
                OnNotifyValidationResult?.Invoke(value, errorMessage!);

               // OnNotifyValidationResult?.Invoke(ErrorMessage!, value);
            }

        }

        private bool ValidateAllChanges()
        {
            return (OnGetErrorMessage != null && !defaultErrorMessageIsSet && !ValidateOnFocusIn && !ValidateOnFocusOut && firstRendered && !ValidateOnLoad) || CascadedEditContext != null;
        }

        private async Task DeferredValidation(TValue? value)
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
                int TaskCount = DeferredValidationTasks.Count;
                await Task.WhenAll(DeferredValidationTasks.ToArray());
                if (TaskCount == DeferredValidationTasks.Count)
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

        public async Task Focus()
        {
            if (textAreaRef.Id != null)
            {
                await JSRuntime!.InvokeVoidAsync("FluentUIBaseComponent.focusElement", textAreaRef).ConfigureAwait(false);
            }
            if (inputRef.Id != null)
            {
                await JSRuntime!.InvokeVoidAsync("FluentUIBaseComponent.focusElement", inputRef).ConfigureAwait(false);
            }

        }

        /// <summary>
        /// Gets or sets the current value of the input, represented as a string.
        /// </summary>
        protected string? CurrentValueAsString
        {
            get => FormatValueAsString(CurrentValue);
            set
            {
                _parsingValidationMessages?.Clear();

                bool parsingFailed;

                if (_nullableUnderlyingType != null && string.IsNullOrEmpty(value))
                {
                    // Assume if it's a nullable type, null/empty inputs should correspond to default(T)
                    // Then all subclasses get nullable support almost automatically (they just have to
                    // not reject Nullable<T> based on the type itself).
                    parsingFailed = false;
                    CurrentValue = default!;
                }
                else if (TryParseValueFromString(value, out TValue? parsedValue, out string? validationErrorMessage))
                {
                    parsingFailed = false;
                    CurrentValue = parsedValue!;
                }
                else
                {
                    parsingFailed = true;

                    if (_parsingValidationMessages == null)
                    {
                        _parsingValidationMessages = new ValidationMessageStore(CascadedEditContext);
                    }

                    _parsingValidationMessages.Add(FieldIdentifier, validationErrorMessage);

                    // Since we're not writing to CurrentValue, we'll need to notify about modification from here
                    CascadedEditContext.NotifyFieldChanged(FieldIdentifier);
                }

                // We can skip the validation notification if we were previously valid and still are
                if (parsingFailed || _previousParsingAttemptFailed)
                {
                    CascadedEditContext.NotifyValidationStateChanged();
                    _previousParsingAttemptFailed = parsingFailed;
                }
            }
        }

        protected bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
        {
            if (BindConverter.TryConvertTo<TValue>(value, CultureInfo.InvariantCulture, out result))
            {
                validationErrorMessage = null;
                return true;
            }
            else
            {
                validationErrorMessage = string.Format(CultureInfo.InvariantCulture, ParsingErrorMessage, /*DisplayName ??*/ FieldIdentifier.FieldName);
                return false;
            }
        }

        /// <summary>
        /// Formats the value as a string. Derived classes can override this to determine the formatting used for <c>CurrentValueAsString</c>.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>A string representation of the value.</returns>
        protected string? FormatValueAsString(TValue? value)
        {
            // Avoiding a cast to IFormattable to avoid boxing.
            return value switch
            {
                null => null,
                int @int => BindConverter.FormatValue(@int, CultureInfo.InvariantCulture),
                long @long => BindConverter.FormatValue(@long, CultureInfo.InvariantCulture),
                short @short => BindConverter.FormatValue(@short, CultureInfo.InvariantCulture),
                float @float => BindConverter.FormatValue(@float, CultureInfo.InvariantCulture),
                double @double => BindConverter.FormatValue(@double, CultureInfo.InvariantCulture),
                decimal @decimal => BindConverter.FormatValue(@decimal, CultureInfo.InvariantCulture),
                string @string => @string,
                _ => throw new InvalidOperationException($"Unsupported type {value.GetType()}"),
            };
        }
        /// <summary>
        /// Gets or sets the error message used when displaying an a parsing error.
        /// </summary>
        [Parameter] public string ParsingErrorMessage { get; set; } = "The {0} field must be a number.";


    }
}