using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BlazorFluentUI
{
    public enum ChangeType
    {
        Empty = -1,
        Default,
        Backspace,
        Delete,
        TextPasted
    }

    public class MaskValue
    {
        public char? Value { get; set; }

        /**
       ///This index refers to the index in the displayMask rather than the inputMask.
       ///This means that any escaped characters do not count toward this index.
        */
        public int DisplayIndex { get; set; }
        public Regex? Format { get; set; }

    }

    public partial class MaskedTextField : TextFieldParameters, IAsyncDisposable
    {
        private List<MaskValue> MaskCharData { get; set; } = new();
        private bool IsFocused { get; set; } = false;
        private bool MoveCursorOnMouseUp { get; set; } = false;
        private int MaskCursorPosition { get; set; }

        public (ChangeType changeType, int? selectionStart, int? selectionEnd) ChangeSelectionData { get; set; }
        [Inject] private IJSRuntime? JSRuntime { get; set; }
        /// <summary>
        /// The masking string that defines the mask's behavior. A backslash will escape any character. Special format characters are: '9': [0-9] 'a': [a-zA-Z] '*': [a-zA-Z0-9]
        /// </summary>
        [Parameter] public string Mask { get; set; } = "";

        /// <summary>
        /// The character to show in place of unfilled characters of the mask.
        /// </summary>
        [Parameter] public char MaskChar { get; set; } = '_';

        /// <summary>
        /// An object defining the format characters and corresponding regexp values. Default format string: "9:[0-9], a:[a-zA-Z], *:[a-zA-Z0-9]"
        /// </summary>
        [Parameter] public string MaskFormat { get; set; } = DEFAULT_MASK_FORMAT;


        [Parameter] public string? DefaultValue { get; set; }

        [Parameter] public string? Value { get; set; }

        /// <summary>
        /// Gets or sets a callback that updates the bound value.
        /// </summary>
        [Parameter] public EventCallback<string> ValueChanged { get; set; }

        /// <summary>
        /// Gets or sets an expression that identifies the bound value.
        /// </summary>
        [Parameter] public Expression<Func<string>>? ValueExpression { get; set; }


        [Parameter] public EventCallback<MouseEventArgs> OnMouseDown { get; set; }

        [Parameter] public EventCallback<MouseEventArgs> OnMouseUp { get; set; }

        [Parameter] public EventCallback<ChangeEventArgs> OnInput { get; set; }
        [Parameter] public Func<string, string>? OnGetErrorMessage { get; set; }
        [Parameter] public Action<string, string>? OnNotifyValidationResult { get; set; }

        /// <summary>
        /// Gets the <see cref="FieldIdentifier"/> for the bound value.
        /// </summary>
        protected internal FieldIdentifier FieldIdentifier { get; set; }
        [CascadingParameter] EditContext CascadedEditContext { get; set; } = default!;

        private string? latestValidatedValue = default;
        private readonly EventHandler<ValidationStateChangedEventArgs> _validationStateChangedHandler;
        //private bool _previousParsingAttemptFailed;
        //private ValidationMessageStore? _parsingValidationMessages;

        private ElementReference? Element;

        private readonly static string DEFAULT_MASK_FORMAT = "9:[0-9], a:[a-zA-Z], *:[a-zA-Z0-9]";

        private Dictionary<char, Regex> ParsedMaskFormat = new();

        private const string BasePath = "./_content/BlazorFluentUI.CoreComponents/baseComponent.js";
        private IJSObjectReference? baseModule;


        private string? displayValue;

        public string? DisplayValue
        {
            get => displayValue;

            set
            {
                if (!string.IsNullOrEmpty(value))
                    displayValue = value;
            }
        }

        public MaskedTextField()
        {
            AutoComplete = AutoComplete.Off;
            _validationStateChangedHandler = OnValidateStateChanged;
        }

        private void OnValidateStateChanged(object? sender, ValidationStateChangedEventArgs eventArgs)
        {
            UpdateAdditionalValidationAttributes();

            StateHasChanged();
        }

        protected override Task OnInitializedAsync()
        {
            MaskCharData = ParseMask(Mask, ParsedMaskFormat);

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


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            baseModule = await JSRuntime!.InvokeAsync<IJSObjectReference>("import", BasePath);

            await base.OnAfterRenderAsync(firstRender);
        }

        protected override async Task OnParametersSetAsync()
        {


            //if (!EqualityComparer<string>.Default.Equals(DefaultValue, default))
            //{
            //    Value = DefaultValue;
            //    DefaultValue = default;
            //}

            //if (ValidateAllChanges())
            //{
            //    Validate(CurrentValue);
            //}
            if (Element != null)
            {
                if (MaskCursorPosition > 0)
                {
                    await baseModule!.InvokeVoidAsync("setSelectionRange", Element, MaskCursorPosition, MaskCursorPosition);
                }
            }
            MoveCursorOnMouseUp = false;

            await base.OnParametersSetAsync();
            return;
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {


            parameters.SetParameterProperties(this);

            ParsedMaskFormat = ParseMaskFormat(MaskFormat);


            Value = GetMaskDisplay(Mask, MaskCharData, MaskChar);



            if (CascadedEditContext != null && ValueExpression != null)
            {
                CascadedEditContext.OnValidationStateChanged += _validationStateChangedHandler;
                FieldIdentifier = FieldIdentifier.Create(ValueExpression);


                UpdateAdditionalValidationAttributes();
            }
            // For derived components, retain the usual lifecycle with OnInit/OnParametersSet/etc.
            base.SetParametersAsync(ParameterView.Empty);
            return Task.CompletedTask;
        }

        private void CascadedEditContext_OnValidationStateChanged(object? sender, ValidationStateChangedEventArgs e)
        {
            InvokeAsync(() =>
            {
                StateHasChanged();
            });
        }

        private async Task HandleOnFocusAsync(FocusEventArgs args)
        {

            isFocused = true;

            // Move the cursor position to the leftmost unfilled position
            for (int i = 0; i < MaskCharData.Count; i++)
            {
                if (MaskCharData[i].Value == null)
                {
                    MaskCursorPosition = (MaskCharData[i].DisplayIndex);
                    Debug.WriteLine($"OnFocus: cursor pos - {MaskCursorPosition}");
                    await baseModule!.InvokeVoidAsync("setSelectionRange", Element, MaskCursorPosition, MaskCursorPosition);
                    break;
                }
            }
            await OnFocus.InvokeAsync(args);
        }

        private async Task HandleOnBlurAsync(FocusEventArgs args)
        {

            isFocused = false;
            MoveCursorOnMouseUp = true;
            await OnBlur.InvokeAsync(args);
        }

        private async Task HandleOnMouseDownAsync(MouseEventArgs args)
        {


            if (!isFocused)
            {
                MoveCursorOnMouseUp = true;
            }

            await OnMouseDown.InvokeAsync(args);
        }

        private async Task HandleOnMouseUpAsync(MouseEventArgs args)
        {


            // Move the cursor on mouseUp after focusing the textField
            if (MoveCursorOnMouseUp)
            {
                MoveCursorOnMouseUp = false;
                // Move the cursor position to the rightmost unfilled position
                for (int i = 0; i < MaskCharData.Count; i++)
                {
                    if (MaskCharData[i].Value == null)
                    {
                        MaskCursorPosition = MaskCharData[i].DisplayIndex;
                        break;
                    }
                }
            }

            await OnMouseUp.InvokeAsync(args);
        }

        private async Task HandleOnInputAsync(ChangeEventArgs args)
        {
            string? inputValue = args.Value!.ToString();

            if (Element != null && ChangeSelectionData.selectionStart <= 0 && ChangeSelectionData.selectionEnd <= 0)
            {
                int? ss = await GetSelectionStart(Element);
                int? se = await GetSelectionEnd(Element);
                ChangeType ct = ChangeSelectionData.changeType;

                ChangeSelectionData = (changeType: ct != ChangeType.Default ? ct : ChangeType.Default,
                                       selectionStart: ss != null ? ss : -1,
                                       selectionEnd: se != null ? se : -1);


                if (ChangeSelectionData == (default, 0, 0))
                {
                    return; // inputValue;
                }
            }

            int cursorPos = 0;
            ChangeType changeType = ChangeSelectionData.changeType;
            int selectionStart = (int)ChangeSelectionData.selectionStart!;
            int selectionEnd = (int)ChangeSelectionData.selectionEnd!;

            if (changeType == ChangeType.TextPasted)
            {
                int charsSelected = selectionEnd - selectionStart;
                int charCount = inputValue!.Length + charsSelected - Value!.Length;
                int startPos = selectionStart;

                string pastedString = inputValue.Substring(startPos!, charCount);

                // Clear any selected characters
                if (charsSelected > 0)
                {
                    MaskCharData = ClearRange(MaskCharData, selectionStart, charsSelected);
                }
                cursorPos = InsertString(MaskCharData, startPos, pastedString);
            }
            else if (changeType == ChangeType.Delete || changeType == ChangeType.Backspace)
            {
                // isDel is true If the characters are removed LTR, otherwise RTL
                bool isDel = changeType == ChangeType.Delete;
                int charCount = selectionEnd - selectionStart;

                if (charCount > 0)
                {
                    // charCount is > 0 if range was deleted
                    MaskCharData = ClearRange(MaskCharData, selectionStart, charCount);
                    cursorPos = GetRightFormatIndex(MaskCharData, selectionStart);
                }
                else
                {
                    // If charCount == 0, there was no selection and a single character was deleted
                    if (isDel)
                    {
                        MaskCharData = ClearNext(MaskCharData, selectionStart);
                        cursorPos = GetRightFormatIndex(MaskCharData, selectionStart);
                    }
                    else
                    {
                        MaskCharData = ClearPrev(MaskCharData, selectionStart);
                        cursorPos = GetLeftFormatIndex(MaskCharData, selectionStart);
                    }
                }
            }
            else if (inputValue!.Length > Value!.Length)
            {
                // This case is if the user added characters
                int charCount = inputValue.Length - Value.Length;
                int startPos = (selectionEnd!) - charCount;
                string enteredString = inputValue.Substring(startPos, charCount);

                cursorPos = InsertString(MaskCharData, startPos, enteredString);
            }
            else if (inputValue.Length <= Value.Length)
            {
                /**
                 * This case is reached only if the user has selected a block of 1 or more
                 * characters and input a character replacing the characters they've selected.
                 */
                int charCount = 1;
                int selectCount = Value.Length + charCount - inputValue.Length;
                int startPos = (selectionEnd!) - charCount;
                string enteredString = inputValue.Substring(startPos, charCount);

                // Clear the selected range
                MaskCharData = ClearRange(MaskCharData, startPos, selectCount);
                // Insert the printed character
                cursorPos = InsertString(MaskCharData, startPos, enteredString);
            }

            ChangeSelectionData = (default, 0, 0);

            string newValue = GetMaskDisplay(Mask, MaskCharData, MaskChar);

            MaskCursorPosition = cursorPos;


            await OnInput.InvokeAsync(new ChangeEventArgs { Value = newValue });



            Value = newValue;

            StateHasChanged();
            await ValueChanged.InvokeAsync(Value);

            StateHasChanged();
            Validate(Value);
            return;
        }


        private async Task HandleOnKeyDownAsync(KeyboardEventArgs args)
        {
            Console.WriteLine("In KeyDown");

            ChangeSelectionData = (default, 0, 0);
            if (Element != null && displayValue != null)
            {
                var keyCode = args.Key;
                var ctrlKey = args.CtrlKey;
                var metaKey = args.MetaKey;

                if (ctrlKey || metaKey)
                    return;
                if ((keyCode == "Delete" || keyCode == "Backspace") && Element != null)
                {
                    int? selectionStart = await GetSelectionStart(Element);
                    int? selectionEnd = await GetSelectionEnd(Element);

                    if (!(keyCode == "Backspace" && selectionEnd != null && selectionEnd >= 0) && !(keyCode == "Delete" && selectionStart != null && selectionStart < displayValue?.Length))
                    {
                        return;
                    }
                    ChangeSelectionData = (changeType: keyCode == "Backspace" ? ChangeType.Backspace : ChangeType.Delete,
                                    selectionStart: selectionStart != null ? selectionStart : -1,
                                    selectionEnd: selectionEnd != null ? selectionEnd : -1);
                }
            }

            await OnKeyDown.InvokeAsync(args);
            return;

        }

        // ToDo
        private async Task HandleOnPasteAsync(ClipboardEventArgs args)
        {
            if (Element != null)
            {
                int selectionStart = await GetSelectionStart(Element);
                int selectionEnd = await GetSelectionEnd(Element);

                ChangeSelectionData = (changeType: ChangeType.TextPasted,
                                selectionStart: selectionStart > 0 ? selectionStart : -1,
                                selectionEnd: selectionEnd > 0 ? selectionEnd : -1);
            }
            await OnPaste.InvokeAsync(args);
        }

        private void Validate(string? value)
        {
            if (CascadedEditContext != null && ValueExpression != null)
            {
                if (value != null)
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
                if (value != null)
                {
                    if (latestValidatedValue != null && latestValidatedValue.Equals(value))
                        return;

                    latestValidatedValue = value;
                    string? errorMessage = OnGetErrorMessage?.Invoke(value);
                    if (errorMessage != null)
                    {
                        ErrorMessage = errorMessage;
                    }
                    OnNotifyValidationResult?.Invoke(value, errorMessage!);

                    StateHasChanged();
                }
            }
        }
        public override async ValueTask DisposeAsync()
        {
            //OnChange -= StateHasChanged;
            if (baseModule != null)
                await baseModule.DisposeAsync();
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

        private Dictionary<char, Regex> ParseMaskFormat(string maskFormat)
        {
            Dictionary<char, Regex> result = new();

            string[]? masks = maskFormat.Split(',', System.StringSplitOptions.TrimEntries);
            foreach (string mask in masks)
            {
                string[]? maskParts = mask.Split(':');
                if (maskParts == null || maskParts.Length != 2)
                    continue;

                result.Add(maskParts[0].ToCharArray()[0], new Regex(maskParts[1], RegexOptions.Compiled));
            }
            return result;
        }

        /// <summary>
        /// Takes in the mask string and the formatCharacters and returns an array of MaskValues
        /// Example:
        /// mask = 'Phone Number: (999) - 9999'
        /// return = [
        ///    { Value: null, DisplayIndex: 16, Format: /[0-9]/ },
        ///    { Value: null, DisplayIndex: 17, Format: /[0-9]/ },
        ///    { Value: null, DisplayIndex: 18, Format: /[0-9]/ },
        ///    { Value: null, DisplayIndex: 22, Format: /[0-9]/ },
        /// </summary>
        /// <param name="mask">The string use to define the format of the displayed maskedValue.</param>
        /// <param name="formatChars">An object defining how certain characters in the mask should accept input.</param>
        /// <returns>An list of MaskValues.</returns>
        private List<MaskValue> ParseMask(string mask, Dictionary<char, Regex> formatChars)
        {
            if (mask == null)
                return new List<MaskValue>();

            if (formatChars.Count == 0)
                formatChars = ParsedMaskFormat;

            List<MaskValue> maskCharData = new(mask.Length);

            // Count the escape characters in the mask string.
            int escapedChars = 0;
            for (int i = 0; i + escapedChars < mask.Length; i++)
            {
                char maskChar = mask[i + escapedChars];
                if (maskChar == '\\')
                {
                    escapedChars++;
                }
                else
                {
                    // Check if the maskChar is a format character.

                    if (formatChars.ContainsKey(maskChar))
                    {
                        Regex? maskFormat = formatChars[maskChar];
                        /**
                        ///Do not add escapedChars to the displayIndex.
                        ///The index refers to a position in the mask's displayValue.
                        ///Since the backslashes don't appear in the displayValue,
                        ///we do not add them to the charData displayIndex.
                         */
                        maskCharData.Add(new() { DisplayIndex = i, Format = maskFormat });

                    }
                }
            }
            return maskCharData;
        }

        /// <summary>
        /// Takes in the mask string, an array of MaskValues, and the maskCharacter
        /// returns the mask string formatted with the input values and maskCharacter.
        /// If the maskChar is undefined, the maskDisplay is truncated to the last filled format character.
        /// Example:
        /// mask = 'Phone Number: (999) 999 - 9999'
        /// maskCharData = '12345'
        /// maskChar = '_'
        /// return = 'Phone Number: (123) 45_ - ___'
        ///
        /// Example:
        /// mask = 'Phone Number: (999) 999 - 9999'
        /// value = '12345'
        /// maskChar = undefined
        /// return = 'Phone Number: (123) 45'
        /// </summary>
        /// <param name="mask">The string use to define the format of the displayed maskedValue.</param>
        /// <param name="maskCharData">The input values to insert into the mask string for displaying</param>
        /// <param name="maskChar">A character to display in place of unfilled mask format characters.</param>
        /// <returns></returns>
        private string GetMaskDisplay(string? mask, List<MaskValue> maskCharData, char? maskChar)
        {

            string? maskDisplay = mask;

            if (maskDisplay == null)
            {
                return "";
            }

            // Remove all backslashes
            maskDisplay = maskDisplay.Replace("\\", string.Empty);

            // lastDisplayIndex is is used to truncate the string if necessary.
            int lastDisplayIndex = 0;
            if (maskCharData.Count > 0)
            {
                lastDisplayIndex = maskCharData[0].DisplayIndex - 1;
            }

            /**
            ///For each input value, replace the character in the maskDisplay with the value.
            ///If there is no value set for the format character, use the maskChar.
             */
            foreach (MaskValue charData in maskCharData)
            {
                char? nextChar = ' ';
                if (charData.Value != null)
                {
                    nextChar = charData.Value;
                    if (charData.DisplayIndex > lastDisplayIndex)
                    {
                        lastDisplayIndex = charData.DisplayIndex;
                    }
                }
                else
                {
                    if (maskChar != null)
                    {
                        nextChar = maskChar;
                    }
                }

                // Insert the character into the maskdisplay at its corresponding index
                maskDisplay = maskDisplay[0..charData.DisplayIndex] + nextChar + maskDisplay[(charData.DisplayIndex + 1)..];
            }

            // Cut off all mask characters after the last filled format value
            if (maskChar == null)
            {
                maskDisplay = maskDisplay[0..(lastDisplayIndex + 1)];
            }

            return maskDisplay;
        }

        /// <summary>
        /// Get the next format index right of a specified index.
        /// If no index exists, returns the rightmost index.
        /// </summary>
        /// <param name="maskCharData">The input values to insert into the mask string for displaying</param>
        /// <param name="index"></param>
        /// <returns></returns>
        private int GetRightFormatIndex(List<MaskValue> maskCharData, int index)
        {
            for (int i = 0; i < maskCharData.Count; i++)
            {
                if (maskCharData[i].DisplayIndex >= index)
                {
                    return maskCharData[i].DisplayIndex;
                }
            }
            return maskCharData[0].DisplayIndex;
        }

        /// <summary>
        /// Get the next format index left of a specified index.
        /// If no index exists, returns the leftmost index.
        /// </summary>
        /// <param name="maskCharData">The input values to insert into the mask string for displaying</param>
        /// <param name="index"></param>
        /// <returns></returns>
        private int GetLeftFormatIndex(List<MaskValue> maskCharData, int index)
        {
            for (int i = maskCharData.Count - 1; i >= 0; i--)
            {
                if (maskCharData[i].DisplayIndex <= index)
                {
                    return maskCharData[i].DisplayIndex;
                }
            }
            return maskCharData[0].DisplayIndex;
        }

        /// <summary>
        /// Deletes all values in maskCharData with a displayIndex that falls inside the specified range.
        /// maskCharData is modified inline and also returned.
        /// </summary>
        /// <param name="maskCharData">The input values to insert into the mask string for displaying</param>
        /// <param name="selectionStart">The start of the range to clear</param>
        /// <param name="selectionCount">The end of the range to clear</param>
        /// <returns></returns>
        private List<MaskValue> ClearRange(List<MaskValue> maskCharData, int selectionStart, int selectionCount)
        {
            for (int i = 0; i < maskCharData.Count; i++)
            {
                if (maskCharData[i].DisplayIndex >= selectionStart)
                {
                    if (maskCharData[i].DisplayIndex >= selectionStart + selectionCount)
                    {
                        break;
                    }
                    maskCharData[i].Value = null;
                }
            }
            return maskCharData;
        }


        /// <summary>
        /// Deletes the input character at or after a specified index and returns the new array of charData
        /// maskCharData is modified inline and also returned.
        /// <param name="maskCharData">The input values to insert into the mask string for displaying</param>
        /// <param name="selectionStart">Start at or after this index</param>
        /// <returns></returns>

        private List<MaskValue> ClearNext(List<MaskValue> maskCharData, int selectionStart)
        {
            for (int i = 0; i < maskCharData.Count; i++)
            {
                if (maskCharData[i].DisplayIndex >= selectionStart)
                {
                    maskCharData[i].Value = null;
                    break;
                }
            }
            return maskCharData;
        }

        /// <summary>
        /// Deletes the input character before a specified index and returns the new list of charData
        /// maskCharData is modified inline and also returned.
        /// </summary>
        /// <param name="maskCharData">The input values to insert into the mask string for displaying</param>
        /// <param name="selectionStart">Start before this index</param>
        /// <returns>The new lisk of character data</returns>
        private List<MaskValue> ClearPrev(List<MaskValue> maskCharData, int selectionStart)
        {
            for (int i = maskCharData.Count - 1; i >= 0; i--)
            {
                if (maskCharData[i].DisplayIndex <= selectionStart)
                {
                    maskCharData[i].Value = null;
                    break;
                }
            }
            return maskCharData;
        }

        /// <summary>
        ///Deletes all values in maskCharData with a displayIndex that falls inside the specified range.
        ///Modifies the maskCharData inplace with the passed string and returns the display index of the
        ///next format character after the inserted string.
        /// </summary>
        /// <param name="maskCharData">The input values to insert into the mask string for displaying</param>
        /// <param name="selectionStart">The start of the selection</param>
        /// <param name="newString">The new string to investigate</param>
        /// <returns>The DisplayIndex of the next format character</returns>
        private int InsertString(List<MaskValue> maskCharData, int selectionStart, string newString)
        {
            int stringIndex = 0;
            int nextIndex = 0;
            bool isStringInserted = false;

            // Iterate through _maskCharData finding values with a displayIndex after the specified range start
            for (int i = 0; i < maskCharData.Count && stringIndex < newString.Length; i++)
            {
                if (maskCharData[i].DisplayIndex >= selectionStart)
                {
                    isStringInserted = true;
                    nextIndex = maskCharData[i].DisplayIndex;
                    // Find the next character in the newString that matches the format
                    while (stringIndex < newString.Length)
                    {
                        // If the character matches the format regexp, set the maskCharData to the new character
                        if (maskCharData[i].Format != null && maskCharData[i].Format!.IsMatch(newString.Substring(stringIndex, 1)))
                        {
                            maskCharData[i].Value = newString[stringIndex++];
                            // Set the nextIndex to the display index of the next mask format character.
                            if (i + 1 < maskCharData.Count)
                            {
                                nextIndex = maskCharData[i + 1].DisplayIndex;
                            }
                            else
                            {
                                nextIndex++;
                            }
                            break;
                        }
                        stringIndex++;
                    }
                }
            }
            return isStringInserted ? nextIndex : selectionStart;
        }

        public async ValueTask<int> GetSelectionStart(ElementReference? element) => await baseModule!.InvokeAsync<int>("getSelectionStart", element);

        public async ValueTask<int> GetSelectionEnd(ElementReference? element) => await baseModule!.InvokeAsync<int>("getSelectionEnd", Element);

    }
}
