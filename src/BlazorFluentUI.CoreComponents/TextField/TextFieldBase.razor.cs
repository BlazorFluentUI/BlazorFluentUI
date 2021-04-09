using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BlazorFluentUI
{
    public class TextField : TextFieldBase<string?> { }
    public class TextFieldNumber<TValue> : TextFieldBase<TValue>
    {
        public TextFieldNumber()
        {
            InputType = InputType.Number;
            AutoComplete = AutoComplete.Off;
        }
    }

    public partial class TextFieldBase<TValue> : FluentUIComponentBase, IDisposable
    {
        private const string BasePath = "./_content/BlazorFluentUI.CoreComponents/baseComponent.js";
        private IJSObjectReference? baseModule;

        protected string id = Guid.NewGuid().ToString();
        protected string descriptionId = Guid.NewGuid().ToString();
        private bool hasIcon;
        private bool hasLabel;

        private ICollection<IRule> TextFieldLocalRules { get; set; } = new List<IRule>();
        private readonly ICollection<Task> DeferredValidationTasks = new List<Task>();
        private readonly Rule TextField_Field_HasIcon = new();

        protected double autoAdjustedHeight = -1;
        protected bool isFocused = false;
        private bool defaultErrorMessageIsSet;
        private bool firstRendered = false;
        private TValue? latestValidatedValue = default;

        private readonly EventHandler<ValidationStateChangedEventArgs> _validationStateChangedHandler;
        private bool _previousParsingAttemptFailed;
        private ValidationMessageStore? _parsingValidationMessages;
        private Type? _nullableUnderlyingType;

        private readonly static string _stepAttributeValue; // Null by default, so only allows whole numbers as per HTML spec

        [Inject] private IJSRuntime? JSRuntime { get; set; }
        [CascadingParameter] EditContext CascadedEditContext { get; set; } = default!;

        /// <summary>
        /// Gets or sets the associated <see cref="ElementReference"/>.
        /// <para>
        /// May be <see langword="null"/> if accessed before the component is rendered.
        /// </para>
        /// </summary>
        [DisallowNull] public ElementReference? Element { get; protected set; }
        /// <summary>
        /// Gets or sets the error message used when displaying an a parsing error.
        /// </summary
        [Parameter] public string ParsingErrorMessage { get; set; } = "The {0} field must be a number.";
        [Parameter] public bool Multiline { get; set; }
        [Parameter] public InputType InputType { get; set; } = InputType.Text;

        /// <summary>
        /// Gets or sets a collection of additional attributes that will be applied to the created element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

        /// <summary>
        /// Gets or sets the value of the input. This should be used with two-way binding.
        /// </summary>
        /// <example>
        /// @bind-Value="model.PropertyName"
        /// </example>
        [Parameter] public TValue? Value { get; set; }

        [Parameter] public TValue? DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets a callback that updates the bound value.
        /// </summary>
        [Parameter] public EventCallback<TValue> ValueChanged { get; set; }

        /// <summary>
        /// Gets or sets an expression that identifies the bound value.
        /// </summary>
        [Parameter] public Expression<Func<TValue>>? ValueExpression { get; set; }

        /// <summary>
        /// Gets or sets the display name for this field.
        /// <para>This value is used when generating error messages when the input value fails to parse correctly.</para>
        /// </summary>
        [Parameter] public string? DisplayName { get; set; }

        [Parameter] public bool Required { get; set; }


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
        [Parameter] public EventCallback<KeyboardEventArgs> OnKeyDown { get; set; }
        [Parameter] public EventCallback<KeyboardEventArgs> OnKeyUp { get; set; }
        [Parameter] public EventCallback<KeyboardEventArgs> OnKeyPress { get; set; }
        [Parameter] public Func<TValue, string>? OnGetErrorMessage { get; set; }
        [Parameter] public Action<TValue, string>? OnNotifyValidationResult { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }  // expose click event for Combobox and pickers
        [Parameter] public EventCallback<FocusEventArgs> OnBlur { get; set; }
        [Parameter] public EventCallback<FocusEventArgs> OnFocus { get; set; }

        [Parameter] public EventCallback<TValue> OnChange { get; set; }
        [Parameter] public EventCallback<TValue> OnInput { get; set; }

        /// <summary>
        /// Gets the associated <see cref="Forms.EditContext"/>.
        /// </summary>
        //protected EditContext EditContext { get; set; } = default!;

        /// <summary>
        /// Gets the <see cref="FieldIdentifier"/> for the bound value.
        /// </summary>
        protected internal FieldIdentifier FieldIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the current value of the input.
        /// </summary>
        protected TValue? CurrentValue
        {
            get => Value;
            set
            {
                bool hasChanged = !EqualityComparer<TValue>.Default.Equals(value, Value);
                if (hasChanged)
                {
                    Value = value;
                    InputHandler(new ChangeEventArgs { Value = Value }).ConfigureAwait(true);
                }
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

                    if (_parsingValidationMessages == null && CascadedEditContext != null)
                    {
                        _parsingValidationMessages = new ValidationMessageStore(CascadedEditContext);
                    }
                    if (FieldIdentifier.FieldName != null)
                    {
                        _parsingValidationMessages?.Add(FieldIdentifier, validationErrorMessage);
                        // Since we're not writing to CurrentValue, we'll need to notify about modification from here
                        CascadedEditContext?.NotifyFieldChanged(FieldIdentifier);
                    }
                    // Set ErrorMessage
                    Validate(CurrentValue);
                }

                // We can skip the validation notification if we were previously valid and still are
                if (parsingFailed || _previousParsingAttemptFailed)
                {
                    CascadedEditContext?.NotifyValidationStateChanged();
                    _previousParsingAttemptFailed = parsingFailed;
                }
            }
        }

        /// <summary>
        /// Constructs an instance of <see cref="TextField{TValue}"/>.
        /// </summary>
        public TextFieldBase()
        {
            _validationStateChangedHandler = OnValidateStateChanged;
        }

        static TextFieldBase()
        {
            // Unwrap Nullable<T>, because InputBase already deals with the Nullable aspect
            // of it for us. We will only get asked to parse the T for nonempty inputs.
            Type? targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
            if (targetType == typeof(int) ||
                targetType == typeof(long) ||
                targetType == typeof(short) ||
                targetType == typeof(float) ||
                targetType == typeof(double) ||
                targetType == typeof(decimal))
            {
                _stepAttributeValue = "any";
            }
            else
            {
                _stepAttributeValue = "";
            }
        }

        protected override Task OnInitializedAsync()
        {
            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                defaultErrorMessageIsSet = true;
            }

            // to prevent changes after initialisation
            hasIcon = !string.IsNullOrWhiteSpace(IconName) || !string.IsNullOrWhiteSpace(IconSrc);
            hasLabel = !string.IsNullOrWhiteSpace(Label);
            if (hasIcon)
            {
                CreateLocalCss();
            }

            return base.OnInitializedAsync();
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            if (CascadedEditContext != null && ValueExpression != null)
            {
                CascadedEditContext.OnValidationStateChanged += _validationStateChangedHandler;
                FieldIdentifier = FieldIdentifier.Create(ValueExpression);

                _nullableUnderlyingType = Nullable.GetUnderlyingType(typeof(TValue));

                UpdateAdditionalValidationAttributes();
            }
            // For derived components, retain the usual lifecycle with OnInit/OnParametersSet/etc.
            return base.SetParametersAsync(ParameterView.Empty);
        }

        private void CascadedEditContext_OnValidationStateChanged(object? sender, ValidationStateChangedEventArgs e)
        {
            InvokeAsync(() =>
            {
                StateHasChanged();
            });
        }

        protected override Task OnParametersSetAsync()
        {

            if (!EqualityComparer<TValue>.Default.Equals(DefaultValue, default))
            {
                Value = DefaultValue;
                DefaultValue = default;
            }

            if (ValidateAllChanges())
            {
                Validate(CurrentValue);
            }

            return base.OnParametersSetAsync();
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
            }
            if (TryParseValueFromString(Convert.ToString(args.Value, CultureInfo.InvariantCulture), out TValue? result, out string? validationErrorMessage))
            {

                if (ValidateAllChanges())
                {
                    await DeferredValidation(result).ConfigureAwait(false);
                }

                await AdjustInputHeightAsync();

                await OnInput.InvokeAsync(result);
                await ValueChanged.InvokeAsync(result);
            }
            else
            {
                //parsingFailed = true;

                if (_parsingValidationMessages == null)
                {
                    _parsingValidationMessages = new ValidationMessageStore(CascadedEditContext);
                }
                if (CascadedEditContext != null)
                {
                    _parsingValidationMessages.Add(FieldIdentifier, validationErrorMessage);
                }
            }
        }

        protected async Task ChangeHandler(ChangeEventArgs args)
        {
            if (TryParseValueFromString((string?)args.Value, out TValue? result, out _))
            {
                await OnChange.InvokeAsync(result);
                await ValueChanged.InvokeAsync(result);
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
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            baseModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", BasePath);

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
                double scrollHeight = await baseModule!.InvokeAsync<double>("getScrollHeight", Element);
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
        /// <summary>
        /// Gets a string that indicates the status of the field being edited. This will include
        /// some combination of "modified", "valid", or "invalid", depending on the status of the field.
        /// </summary>
        private string FieldClass => CascadedEditContext?.FieldCssClass(FieldIdentifier) ?? "";

        /// <summary>
        /// Gets a CSS class string that combines the <c>class</c> attribute and <see cref="FieldClass"/>
        /// properties. Derived components should typically use this value for the primary HTML element's
        /// 'class' attribute.
        /// </summary>
        protected string CssClass
        {
            get
            {
                if (AdditionalAttributes != null &&
                    AdditionalAttributes.TryGetValue("class", out var @class) &&
                    !string.IsNullOrEmpty(Convert.ToString(@class, CultureInfo.InvariantCulture)))
                {
                    return $"{@class} {FieldClass}";
                }

                return FieldClass; // Never null or empty
            }
        }

        private void OnValidateStateChanged(object? sender, ValidationStateChangedEventArgs eventArgs)
        {
            UpdateAdditionalValidationAttributes();

            StateHasChanged();
        }

        private async Task DeferredValidation(TValue? value)
        {
            if (DeferredValidationTime == 0)
            {
                Validate(value);
            }
            else
            {
                DeferredValidationTasks.Add(Task.Run(async () =>
                {
                    await Task.Delay(DeferredValidationTime);
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
                            StateHasChanged();
                        });
                        //invokeasync required for serverside
                    }).ConfigureAwait(false);
                }
            }
        }

        public async Task Focus()
        {
            ElementReference reference = Element!.Value;
            if (reference.Id != null)
                await reference.FocusAsync();
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
                OnNotifyValidationResult?.Invoke(value, errorMessage!);

                // OnNotifyValidationResult?.Invoke(ErrorMessage!, value);
            }
        }

        private bool ValidateAllChanges()
        {
            return (OnGetErrorMessage != null && !defaultErrorMessageIsSet && !ValidateOnFocusIn && !ValidateOnFocusOut && (firstRendered || ValidateOnLoad)) || CascadedEditContext != null;
        }

        protected string GetInputMode()
        {
            return CurrentValue switch
            {
                null => "text",
                int => "numeric",
                long => "numeric",
                short => "numeric",
                float => "decimal",
                double => "decimal",
                decimal => "decimal",
                string => "text",
                _ => throw new InvalidOperationException($"Unsupported type {CurrentValue.GetType()}"),
            };
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
        private void UpdateAdditionalValidationAttributes()
        {
            bool hasAriaInvalidAttribute = AdditionalAttributes != null && AdditionalAttributes.ContainsKey("aria-invalid");
            if (CascadedEditContext.GetValidationMessages(FieldIdentifier).Any())
            {
                if (hasAriaInvalidAttribute)
                {
                    // Do not overwrite the attribute value
                    return;
                }

                if (ConvertToDictionary(AdditionalAttributes, out var additionalAttributes))
                {
                    AdditionalAttributes = additionalAttributes;
                }

                // To make the `Input` components accessible by default
                // we will automatically render the `aria-invalid` attribute when the validation fails
                additionalAttributes["aria-invalid"] = true;
            }
            else if (hasAriaInvalidAttribute)
            {
                // No validation errors. Need to remove `aria-invalid` if it was rendered already
                if (AdditionalAttributes!.Count == 1)
                {
                    // Only aria-invalid argument is present which we don't need any more
                    AdditionalAttributes = null;
                }
                else
                {
                    if (ConvertToDictionary(AdditionalAttributes, out var additionalAttributes))
                    {
                        AdditionalAttributes = additionalAttributes;
                    }

                    additionalAttributes.Remove("aria-invalid");
                }
            }
        }

        /// <summary>
        /// Returns a dictionary with the same values as the specified <paramref name="source"/>.
        /// </summary>
        /// <returns>true, if a new dictrionary with copied values was created. false - otherwise.</returns>
        private static bool ConvertToDictionary(IReadOnlyDictionary<string, object>? source, out Dictionary<string, object> result)
        {
            bool newDictionaryCreated = true;
            if (source == null)
            {
                result = new Dictionary<string, object>();
            }
            else if (source is Dictionary<string, object> currentDictionary)
            {
                result = currentDictionary;
                newDictionaryCreated = false;
            }
            else
            {
                result = new Dictionary<string, object>();
                foreach (KeyValuePair<string, object> item in source)
                {
                    result.Add(item.Key, item.Value);
                }
            }

            return newDictionaryCreated;
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        void IDisposable.Dispose()
        {
            Dispose(disposing: true);
        }
    }
}