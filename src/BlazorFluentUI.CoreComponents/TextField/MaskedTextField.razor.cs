using System;
using System.Collections.Generic;
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



    public partial class MaskedTextField : TextFieldBase<string?>
    {
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

        [Parameter] public EventCallback<MouseEventArgs> OnMouseDown { get; set; }

        [Parameter] public EventCallback<MouseEventArgs> OnMouseUp { get; set; }

        private TextFieldBase<string?>? TextFieldReference;
        private new ElementReference? Element;

        private readonly static string DEFAULT_MASK_FORMAT = "9:[0-9], a:[a-zA-Z], *:[a-zA-Z0-9]";

        private static Dictionary<char, Regex> ParsedMaskFormat = new();

        private const string BasePath = "./_content/BlazorFluentUI.CoreComponents/baseComponent.js";
        private IJSObjectReference? baseModule;

        //STATE
        private List<MaskValue> maskCharData = new();
        //bool isFocused;
        bool moveCursorOnMouseUp;
        int maskCursorPosition;
        (ChangeType changeType, int selectionStart, int selectionEnd) ChangeSelectionData;


        private string? DisplayValue
        {
            get => GetMaskDisplay(Mask, maskCharData, MaskChar);

            set
            {

            }
        }


        public MaskedTextField()
        {
            AutoComplete = AutoComplete.Off;


        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            baseModule = await JSRuntime!.InvokeAsync<IJSObjectReference>("import", BasePath);

            await base.OnAfterRenderAsync(firstRender);

            // Move the cursor to the start of the mask format after values update.
            if (TextFieldReference != null && TextFieldReference.Element != null)
            {
                Element = TextFieldReference.Element;

                if (isFocused && maskCursorPosition > 0 )
                {
                    await baseModule.InvokeVoidAsync("setSelectionRange", Element, maskCursorPosition, maskCursorPosition);
        }
            }

        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            if (!EqualityComparer<string>.Default.Equals(DefaultValue, default))
            {
                Value = DefaultValue;
                DefaultValue = default;
            }

            //if (ValidateAllChanges())
            //{
            //    Validate(CurrentValue);
            //}
            if (TextFieldReference != null && TextFieldReference.Element != null)
            {
                Element = TextFieldReference.Element;
                if (maskCursorPosition > 0 )
                {
                    await baseModule!.InvokeVoidAsync("setSelectionRange", Element, maskCursorPosition, maskCursorPosition);
                }
            }

            return;
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            base.SetParametersAsync(parameters);

            parameters.SetParameterProperties(this);

            ParsedMaskFormat = ParseMaskFormat(MaskFormat);

            maskCharData = ParseMask(Mask, ParsedMaskFormat);

            DefaultValue = DisplayValue;
            //if (CascadedEditContext != null && ValueExpression != null)
            //{
            //    CascadedEditContext.OnValidationStateChanged += _validationStateChangedHandler;
            //    FieldIdentifier = FieldIdentifier.Create(ValueExpression);

            //    _nullableUnderlyingType = Nullable.GetUnderlyingType(typeof(TValue));

            //    UpdateAdditionalValidationAttributes();
            //}
            // For derived components, retain the usual lifecycle with OnInit/OnParametersSet/etc.
            return Task.CompletedTask;
        }

        private void CascadedEditContext_OnValidationStateChanged(object? sender, ValidationStateChangedEventArgs e)
        {
            InvokeAsync(() =>
            {
                StateHasChanged();
            });
        }

        private void HandleValueChanged(string? value)
        {
            maskCharData = ParseMask(Mask, ParsedMaskFormat);
            if (value != null)
            {
                SetValue(value);
            }
            DisplayValue = GetMaskDisplay(Mask, maskCharData, MaskChar);

        }

        private void SetValue(string newValue)
        {
            int valueIndex = 0;
            int charDataIndex = 0;

            while (valueIndex < newValue.Length && charDataIndex < maskCharData.Count)
            {
                // Test if the next character in the new value fits the next format character
                char testVal = newValue[valueIndex];
                if (maskCharData[charDataIndex].Format!.IsMatch(testVal.ToString()))
                {
                    maskCharData[charDataIndex].Value = testVal;
                    charDataIndex++;
                }
                valueIndex++;
            }
        }

        private async Task HandleOnFocusAsync(FocusEventArgs args)
        {
            isFocused = true;

            // Move the cursor position to the leftmost unfilled position
            for (int i = 0; i < maskCharData.Count; i++)
            {
                if (maskCharData[i].Value == null)
                {
                    maskCursorPosition = (maskCharData[i].DisplayIndex);
                    break;
                }
            }
            await OnFocus.InvokeAsync(args);
        }

        private async Task HandleOnBlurAsync(FocusEventArgs args)
        {
            isFocused = false;
            moveCursorOnMouseUp = true;
            await OnBlur.InvokeAsync(args);
        }

        private async Task HandleOnMouseDownAsync(MouseEventArgs args)
        {
            if (!isFocused)
            {
                moveCursorOnMouseUp = true;
            }
            await OnMouseDown.InvokeAsync(args);
        }

        private async Task HandleOnMouseUpAsync(MouseEventArgs args)
        {
            // Move the cursor on mouseUp after focusing the textField
            if (moveCursorOnMouseUp)
            {
                moveCursorOnMouseUp = false;
                // Move the cursor position to the rightmost unfilled position
                for (int i = 0; i < maskCharData.Count; i++)
                {
                    if (maskCharData[i].Value != null)
                    {
                        maskCursorPosition = maskCharData[i].DisplayIndex;
                        break;
                    }
                }
            }
            await OnMouseUp.InvokeAsync(args);
        }

        private async Task HandleOnChangeAsync(string? inputValue)
        {
            if (TextFieldReference != null && TextFieldReference.Element != null && ChangeSelectionData == (default, 0, 0))
            {
                int ss = await baseModule!.InvokeAsync<int>("getSelectionStart", TextFieldReference.Element);
                int se = await baseModule!.InvokeAsync<int>("getSelectionEnd", TextFieldReference.Element);

                ChangeSelectionData = (changeType: ChangeType.Default,
                                       selectionStart: ss > 0 ? ss : -1,
                                       selectionEnd: se > 0 ? se : -1);


                if (ChangeSelectionData != (default, 0 ,0))
                {
                    return;
                }
            }

            int cursorPos = 0;
            var (changeType, selectionStart, selectionEnd) = ChangeSelectionData;

            if (changeType == ChangeType.TextPasted)
            {
                int charsSelected = selectionEnd - selectionStart;
                int charCount = inputValue!.Length + charsSelected - DisplayValue!.Length;
                int startPos = selectionStart;
                string pastedString = inputValue.Substring(startPos, charCount);

                // Clear any selected characters
                if (charsSelected > 0)
                {
                    maskCharData = ClearRange(maskCharData, selectionStart, charsSelected);
                }
                cursorPos = InsertString(maskCharData, startPos, pastedString);
            }
            else if (changeType == ChangeType.Delete || changeType == ChangeType.Backspace)
            {
                // isDel is true If the characters are removed LTR, otherwise RTL
                bool isDel = changeType == ChangeType.Delete;
                int charCount = selectionEnd - selectionStart;

                if (charCount > 0)
                {
                    // charCount is > 0 if range was deleted
                    maskCharData = ClearRange(maskCharData, selectionStart, charCount);
                    cursorPos = GetRightFormatIndex(maskCharData, selectionStart);
                }
                else
                {
                    // If charCount == 0, there was no selection and a single character was deleted
                    if (isDel)
                    {
                        maskCharData = ClearNext(maskCharData, selectionStart);
                        cursorPos = GetRightFormatIndex(maskCharData, selectionStart);
                    }
                    else
                    {
                        maskCharData = ClearPrev(maskCharData, selectionStart);
                        cursorPos = GetLeftFormatIndex(maskCharData, selectionStart);
                    }
                }
        }
            else if (inputValue!.Length > DisplayValue!.Length)
            {
                // This case is if the user added characters
                int charCount = inputValue.Length - DisplayValue.Length;
                int startPos = selectionEnd - charCount;
                string enteredString = inputValue.Substring(startPos, charCount);

                cursorPos = InsertString(maskCharData, startPos, enteredString);
            }
            else if (inputValue.Length <= DisplayValue.Length)
        {
                /**
                 * This case is reached only if the user has selected a block of 1 or more
                 * characters and input a character replacing the characters they've selected.
                 */
                int charCount = 1;
                int selectCount = DisplayValue.Length + charCount - inputValue.Length;
                int startPos = selectionEnd - charCount;
                string enteredString = inputValue.Substring(startPos, charCount);

                // Clear the selected range
                maskCharData = ClearRange(maskCharData, startPos, selectCount);
                // Insert the printed character
                cursorPos = InsertString(maskCharData, startPos, enteredString);
            }

            ChangeSelectionData = (default, 0, 0);

            string newValue = GetMaskDisplay(Mask, maskCharData, MaskChar);

            DisplayValue = newValue;
            maskCursorPosition = cursorPos;

            await OnChange.InvokeAsync(newValue);
            //await ValueChanged.InvokeAsync((string?)args.Value);
        }

            //// Check if backspace or delete press is valid.
            //if (!(keyCode === KeyCodes.backspace && selectionEnd && selectionEnd > 0) &&
            //    !(keyCode === KeyCodes.del && selectionStart !== null && selectionStart < textField.current.value.length)
            //                    )
            //                    {
            //                        return;
            //                    }

        private async Task HandleOnKeyDownAsync(KeyboardEventArgs args)
        {
            Console.WriteLine("In KeyDown)");

            //ChangeSelectionData = null;
            var keyCode = args.Key;
            var ctrlKey = args.CtrlKey;
            var metaKey = args.MetaKey;

            if (ctrlKey || metaKey)
                return;
            if ((keyCode == "Delete" || keyCode == "Backspace") && TextFieldReference != null )
            {
                int selectionStart = await baseModule!.InvokeAsync<int>("getSelectionStart", TextFieldReference.Element).ConfigureAwait(false);
                int selectionEnd = await baseModule!.InvokeAsync<int>("getSelectionEnd", TextFieldReference.Element).ConfigureAwait(false);

                if (!(keyCode == "Backspace" && selectionEnd > 0) && !(keyCode == "Delete" && selectionStart < Value?.Length))
                {
                    return;
                }
                ChangeSelectionData = (changeType: keyCode == "Backspace" ? ChangeType.Backspace : ChangeType.Delete,
                                selectionStart: selectionStart > 0 ? selectionStart : -1,
                                selectionEnd: selectionEnd > 0 ? selectionEnd : -1);
            }

            await OnKeyDown.InvokeAsync(args);

        }



        // ToDo
        private async Task HandleOnPasteAsync(ClipboardEventArgs args)
        {
            if (TextFieldReference != null)
            {
                int selectionStart = await baseModule!.InvokeAsync<int>("getSelectionStart", TextFieldReference.Element);
                int selectionEnd = await baseModule!.InvokeAsync<int>("getSelectionEnd", TextFieldReference.Element); ;

                ChangeSelectionData = (changeType: ChangeType.TextPasted,
                                selectionStart: selectionStart > 0 ? selectionStart : -1,
                                selectionEnd: selectionEnd > 0 ? selectionEnd : -1);
            }
            await OnPaste.InvokeAsync(args);
        }




        private static Dictionary<char, Regex> ParseMaskFormat(string maskFormat)
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
        private static List<MaskValue> ParseMask(string mask, Dictionary<char, Regex> formatChars)
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
        private static string GetMaskDisplay(string? mask, List<MaskValue> maskCharData, char? maskChar)
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
        private static int GetRightFormatIndex(List<MaskValue> maskCharData, int index)
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
        private static int GetLeftFormatIndex(List<MaskValue> maskCharData, int index)
        {
            for (int i = maskCharData.Count - 1; i >= 0; i--)
            {
                if (maskCharData[i].DisplayIndex < index)
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
        private static List<MaskValue> ClearRange(List<MaskValue> maskCharData, int selectionStart, int selectionCount)
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

        private static List<MaskValue> ClearNext(List<MaskValue> maskCharData, int selectionStart)
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
        private static List<MaskValue> ClearPrev(List<MaskValue> maskCharData, int selectionStart)
        {
            for (int i = maskCharData.Count - 1; i >= 0; i--)
            {
                if (maskCharData[i].DisplayIndex < selectionStart)
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
        private static int InsertString(List<MaskValue> maskCharData, int selectionStart, string newString)
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
    }
}
