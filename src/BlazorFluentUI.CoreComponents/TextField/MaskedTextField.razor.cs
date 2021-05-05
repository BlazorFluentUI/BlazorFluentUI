using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        [Inject] private MaskedTextFieldInternalState internalState { get; set; }
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
        //{
        //    get => DisplayValue;
        //    set
        //    {
        //        if (displayValue == value)
        //            DisplayValue = value;
        //    }
        //}

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
            internalState = new MaskedTextFieldInternalState();

        }

        protected override Task OnInitializedAsync()
        {
            internalState.MaskCharData = ParseMask(Mask, ParsedMaskFormat);

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

            // Move the cursor to the start of the mask format after values update.
            //if (TextFieldReference != null && TextFieldReference.Element != null)
            //{
            //    //Value = GetMaskDisplay(Mask, internalState.MaskCharData, MaskChar);
            //    Element = TextFieldReference.Element;

            //    if (isFocused && internalState.MaskCursorPosition > 0)
            //    {
            //        await baseModule.InvokeVoidAsync("setSelectionRange", Element, internalState.MaskCursorPosition, internalState.MaskCursorPosition);
            //    }
            //}
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
                if (internalState.MaskCursorPosition > 0)
                {
                    await baseModule!.InvokeVoidAsync("setSelectionRange", Element, internalState.MaskCursorPosition, internalState.MaskCursorPosition);
                }
            }
            internalState.MoveCursorOnMouseUp = false;

            await base.OnParametersSetAsync();
            return;
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {


            parameters.SetParameterProperties(this);

            ParsedMaskFormat = ParseMaskFormat(MaskFormat);


            Value = GetMaskDisplay(Mask, internalState.MaskCharData, MaskChar);



            //if (CascadedEditContext != null && ValueExpression != null)
            //{
            //    CascadedEditContext.OnValidationStateChanged += _validationStateChangedHandler;
            //    FieldIdentifier = FieldIdentifier.Create(ValueExpression);

            //    _nullableUnderlyingType = Nullable.GetUnderlyingType(typeof(TValue));

            //    UpdateAdditionalValidationAttributes();
            //}
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

        //private void HandleValueChanged(string? value)
        //{
        //    DisplayValue = value;
        //    SetValue(value);
        //}

        //private void SetValue(string newValue)
        //{
        //    int valueIndex = 0;
        //    int charDataIndex = 0;

        //    while (valueIndex < newValue.Length && charDataIndex < internalState.MaskCharData.Count)
        //    {
        //        // Test if the next character in the new value fits the next format character
        //        char testVal = newValue[valueIndex];
        //        if (internalState.MaskCharData[charDataIndex].Format!.IsMatch(testVal.ToString()))
        //        {
        //            internalState.MaskCharData[charDataIndex].Value = testVal;
        //            charDataIndex++;
        //        }
        //        valueIndex++;
        //    }
        //}

        private async Task HandleOnFocusAsync(FocusEventArgs args)
        {

            isFocused = true;

            // Move the cursor position to the leftmost unfilled position
            for (int i = 0; i < internalState.MaskCharData.Count; i++)
            {
                if (internalState.MaskCharData[i].Value == null)
                {
                    internalState.MaskCursorPosition = (internalState.MaskCharData[i].DisplayIndex);
                    Debug.WriteLine($"OnFocus: cursor pos - {internalState.MaskCursorPosition}");
                    await baseModule!.InvokeVoidAsync("setSelectionRange", Element, internalState.MaskCursorPosition, internalState.MaskCursorPosition);
                    break;
                }
            }
            await OnFocus.InvokeAsync(args);
        }

        private async Task HandleOnBlurAsync(FocusEventArgs args)
        {

            isFocused = false;
            internalState.MoveCursorOnMouseUp = true;
            await OnBlur.InvokeAsync(args);
        }

        private async Task HandleOnMouseDownAsync(MouseEventArgs args)
        {


            if (!isFocused)
            {
                internalState.MoveCursorOnMouseUp = true;
            }

            await OnMouseDown.InvokeAsync(args);
        }

        private async Task HandleOnMouseUpAsync(MouseEventArgs args)
        {


            // Move the cursor on mouseUp after focusing the textField
            if (internalState.MoveCursorOnMouseUp)
            {
                internalState.MoveCursorOnMouseUp = false;
                // Move the cursor position to the rightmost unfilled position
                for (int i = 0; i < internalState.MaskCharData.Count; i++)
                {
                    if (internalState.MaskCharData[i].Value == null)
                    {
                        internalState.MaskCursorPosition = internalState.MaskCharData[i].DisplayIndex;
                        break;
                    }
                }
            }

            await OnMouseUp.InvokeAsync(args);
        }

        private async Task HandleOnInputAsync(ChangeEventArgs args)
        //private async Task<string?> HandleOnInputAsync(string? inputValue)
        {
            string? inputValue = args.Value!.ToString();

            if (Element != null && internalState.ChangeSelectionData.selectionStart <= 0 && internalState.ChangeSelectionData.selectionEnd <= 0)
            {
                int? ss = await GetSelectionStart(Element);
                int? se = await GetSelectionEnd(Element);
                ChangeType ct = internalState.ChangeSelectionData.changeType;

                internalState.ChangeSelectionData = (changeType: ct != ChangeType.Default ? ct : ChangeType.Default,
                                       selectionStart: ss != null ? ss : -1,
                                       selectionEnd: se != null ? se : -1);


                if (internalState.ChangeSelectionData == (default, 0, 0))
                {
                    return; // inputValue;
                }
            }

            int cursorPos = 0;
            ChangeType changeType = internalState.ChangeSelectionData.changeType;
            int selectionStart = (int)internalState.ChangeSelectionData.selectionStart!;
            int selectionEnd = (int)internalState.ChangeSelectionData.selectionEnd!;

            if (changeType == ChangeType.TextPasted)
            {
                int charsSelected = selectionEnd - selectionStart;
                int charCount = inputValue!.Length + charsSelected - displayValue!.Length;
                int startPos = selectionStart;

                string pastedString = inputValue.Substring(startPos!, charCount);

                // Clear any selected characters
                if (charsSelected > 0)
                {
                    internalState.MaskCharData = ClearRange(internalState.MaskCharData, selectionStart, charsSelected);
                }
                cursorPos = InsertString(internalState.MaskCharData, startPos, pastedString);
            }
            else if (changeType == ChangeType.Delete || changeType == ChangeType.Backspace)
            {
                // isDel is true If the characters are removed LTR, otherwise RTL
                bool isDel = changeType == ChangeType.Delete;
                int charCount = selectionEnd - selectionStart;

                if (charCount > 0)
                {
                    // charCount is > 0 if range was deleted
                    internalState.MaskCharData = ClearRange(internalState.MaskCharData, selectionStart, charCount);
                    cursorPos = GetRightFormatIndex(internalState.MaskCharData, selectionStart);
                }
                else
                {
                    // If charCount == 0, there was no selection and a single character was deleted
                    if (isDel)
                    {
                        internalState.MaskCharData = ClearNext(internalState.MaskCharData, selectionStart);
                        cursorPos = GetRightFormatIndex(internalState.MaskCharData, selectionStart);
                    }
                    else
                    {
                        internalState.MaskCharData = ClearPrev(internalState.MaskCharData, selectionStart);
                        cursorPos = GetLeftFormatIndex(internalState.MaskCharData, selectionStart);
                    }
                }
            }
            else if (inputValue!.Length > displayValue!.Length)
            {
                // This case is if the user added characters
                int charCount = inputValue.Length - displayValue.Length;
                int startPos = (selectionEnd!) - charCount;
                string enteredString = inputValue.Substring(startPos, charCount);

                cursorPos = InsertString(internalState.MaskCharData, startPos, enteredString);
            }
            else if (inputValue.Length <= displayValue.Length)
            {
                /**
                 * This case is reached only if the user has selected a block of 1 or more
                 * characters and input a character replacing the characters they've selected.
                 */
                int charCount = 1;
                int selectCount = displayValue.Length + charCount - inputValue.Length;
                int startPos = (selectionEnd!) - charCount;
                string enteredString = inputValue.Substring(startPos, charCount);

                // Clear the selected range
                internalState.MaskCharData = ClearRange(internalState.MaskCharData, startPos, selectCount);
                // Insert the printed character
                cursorPos = InsertString(internalState.MaskCharData, startPos, enteredString);
            }

            internalState.ChangeSelectionData = (default, 0, 0);

            string newValue = GetMaskDisplay(Mask, internalState.MaskCharData, MaskChar);

            internalState.MaskCursorPosition = cursorPos;


            await OnInput.InvokeAsync(new ChangeEventArgs { Value = newValue });



            Value = newValue;

            StateHasChanged();
            await ValueChanged.InvokeAsync(Value);

            StateHasChanged();
            return;
        }


        private async Task HandleOnKeyDownAsync(KeyboardEventArgs args)
        {
            Console.WriteLine("In KeyDown");

            internalState.ChangeSelectionData = (default, 0, 0);
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
                    internalState.ChangeSelectionData = (changeType: keyCode == "Backspace" ? ChangeType.Backspace : ChangeType.Delete,
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

                internalState.ChangeSelectionData = (changeType: ChangeType.TextPasted,
                                selectionStart: selectionStart > 0 ? selectionStart : -1,
                                selectionEnd: selectionEnd > 0 ? selectionEnd : -1);
            }
            await OnPaste.InvokeAsync(args);
        }


        public override async ValueTask DisposeAsync()
        {
            //internalState.OnChange -= StateHasChanged;
            if (baseModule != null)
                await baseModule.DisposeAsync();
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
