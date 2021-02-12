using System;
using System.Collections.Generic;
using System.Text;

namespace FluentUI
{
    public class SemanticTextColors : ISemanticTextColors
    {
        public string BodyText { get; set; }
        public string BodyTextChecked { get; set; }
        public string BodySubtext { get; set; }
        public string ActionLink { get; set; }
        public string ActionLinkHovered { get; set; }
        public string Link { get; set; }
        public string LinkHovered { get; set; }
        public string DisabledText { get; set; }
        public string DisabledBodyText { get; set; }
        public string DisabledSubtext { get; set; }
        public string DisabledBodySubtext { get; set; }
        public string ErrorText { get; set; }
        public string WarningText { get; set; }
        public string SuccessText { get; set; }
        public string InputText { get; set; }
        public string InputTextHovered { get; set; }
        public string InputPlaceholderText { get; set; }
        public string ButtonText { get; set; }
        public string ButtonTextHovered { get; set; }
        public string ButtonTextChecked { get; set; }
        public string ButtonTextCheckedHovered { get; set; }
        public string ButtonTextPressed { get; set; }
        public string ButtonTextDisabled { get; set; }
        public string PrimaryButtonText { get; set; }
        public string PrimaryButtonTextHovered { get; set; }
        public string PrimaryButtonTextPressed { get; set; }
        public string PrimaryButtonTextDisabled { get; set; }
        public string AccentButtonText { get; set; }
        public string ListText { get; set; }
        public string MenuItemText { get; set; }
        public string MenuItemTextHovered { get; set; }
    }
}
