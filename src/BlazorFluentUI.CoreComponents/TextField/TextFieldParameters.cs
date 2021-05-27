using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorFluentUI
{
    public class TextFieldParameters : FluentUIComponentBase
    {
        [Parameter] public bool Multiline { get; set; }
        [Parameter] public InputType InputType { get; set; } = InputType.Text;

        /// <summary>
        /// Gets or sets a collection of additional attributes that will be applied to the created element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)] public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }

       
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

        
        [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }  // expose click event for Combobox and pickers
        [Parameter] public EventCallback<FocusEventArgs> OnBlur { get; set; }
        [Parameter] public EventCallback<FocusEventArgs> OnFocus { get; set; }

        

        [Parameter] public EventCallback<ClipboardEventArgs> OnPaste { get; set; }

        protected ICollection<IRule> TextFieldLocalRules { get; set; } = new List<IRule>();
        protected string id = $"id_{Guid.NewGuid().ToString().Replace("-", "")}";
        protected string descriptionId = $"d_{Guid.NewGuid().ToString().Replace("-", "")}";
        protected bool hasIcon;
        protected bool hasLabel;
        protected bool isFocused = false;
        protected bool defaultErrorMessageIsSet;

        private readonly Rule TextField_Field_HasIcon = new();

        protected override void OnThemeChanged()
        {
            if (hasIcon)
            {
                SetStyle();
            }
        }

        protected void CreateLocalCss()
        {
            if (hasIcon)
            {
                TextField_Field_HasIcon.Selector = new ClassSelector() { SelectorName = "ms-TextField-field" };

                TextFieldLocalRules.Add(TextField_Field_HasIcon);
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

        /// <summary>
        /// Returns a dictionary with the same values as the specified <paramref name="source"/>.
        /// </summary>
        /// <returns>true, if a new dictrionary with copied values was created. false - otherwise.</returns>
        protected static bool ConvertToDictionary(IReadOnlyDictionary<string, object>? source, out Dictionary<string, object> result)
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
    }
}
