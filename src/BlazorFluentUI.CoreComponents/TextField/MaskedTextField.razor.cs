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


        private readonly static string DEFAULT_MASK_FORMAT = "9:[0-9], a:[a-zA-Z], *:[a-zA-Z0-9]";

        private static Dictionary<char, Regex> ParsedMaskFormat = new();

        private const string BasePath = "./_content/BlazorFluentUI.CoreComponents/baseComponent.js";
        private IJSObjectReference? baseModule;

        //STATE
        private List<MaskValue> maskCharData = new();
        bool isFocussed;
        bool moveCursorOnMouseUp;
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

        }

        protected override Task OnParametersSetAsync()
        {
            base.OnParametersSetAsync();

            if (!EqualityComparer<string>.Default.Equals(DefaultValue, default))
            {
                Value = DefaultValue;
                DefaultValue = default;
            }

            //if (ValidateAllChanges())
            //{
            //    Validate(CurrentValue);
            //}

            return Task.CompletedTask;
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

        private async Task HandleOnFocusAsync(FocusEventArgs args)
        {

            await OnFocus.InvokeAsync(args);
        }

        private async Task HandleOnBlurAsync(FocusEventArgs args)
        {

            await OnBlur.InvokeAsync(args);
        }

        private async Task HandleOnMouseDownAsync(MouseEventArgs args)
        {

            await OnMouseDown.InvokeAsync(args);
        }

        private async Task HandleOnMouseUpAsync(MouseEventArgs args)
        {

            await OnMouseUp.InvokeAsync(args);
        }

        private async Task HandleOnChangeAsync(string text)
        {

            await OnChange.InvokeAsync(text);
        }

        private async Task HandleOnKeyDownAsync(KeyboardEventArgs args)
        {
            Console.WriteLine("In KeyDown)");


            //            internalState.changeSelectionData = null;
            //            if (textField.current && textField.current.value)
            //            {
            //                const { keyCode, ctrlKey, metaKey } = ev;

            //                // Ignore ctrl and meta keydown
            //                if (ctrlKey || metaKey)
            //                {
            //                    return;
            //                }

            //// On backspace or delete, store the selection and the keyCode
            //if (keyCode === KeyCodes.backspace || keyCode === KeyCodes.del)
            //{
            //                    const selectionStart = (ev.target as HTMLInputElement).selectionStart;
            //                    const selectionEnd = (ev.target as HTMLInputElement).selectionEnd;

            //// Check if backspace or delete press is valid.
            //if (!(keyCode === KeyCodes.backspace && selectionEnd && selectionEnd > 0) &&
            //    !(keyCode === KeyCodes.del && selectionStart !== null && selectionStart < textField.current.value.length)
            //                    )
            //                    {
            //                        return;
            //                    }

            //                    internalState.changeSelectionData = {
            //                    changeType: keyCode === KeyCodes.backspace ? 'backspace' : 'delete',
            //            selectionStart: selectionStart !== null ? selectionStart : -1,
            //            selectionEnd: selectionEnd !== null ? selectionEnd : -1,
            //          };
            //                }
            //            }
            string KeyPressed = "";

            //ChangeSelectionData = null;
            var keyCode = args.Key;
            var ctrlKey = args.CtrlKey;
            var metaKey = args.MetaKey;

            if (ctrlKey || metaKey)
                return;

            int selectionStart;
            int selectionEnd;
            //KeyPressed = "Key Pressed is " + args.Key;

            if (keyCode == "Delete" || keyCode == "Backspace")
            {
                selectionStart = await baseModule!.InvokeAsync<int>("getSelectionStart", Element);
                selectionEnd = await baseModule!.InvokeAsync<int>("getSelectionEnd", Element);

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
            await OnPaste.InvokeAsync(args);
        }


        private async Task HandleOnInputChangeAsync(string? args)
        {
            await OnChange.InvokeAsync((string?)args);
            //await ValueChanged.InvokeAsync((string?)args.Value);
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
        /// Get the next format index left of a specified index.
        /// If no index exists, returns the leftmost index.
        /// </summary>
        /// <param name="maskCharData">The input values to insert into the mask string for displaying</param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static int GetRightFormatIndex(List<MaskValue> maskCharData, int index)
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
