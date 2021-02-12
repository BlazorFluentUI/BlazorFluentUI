namespace FluentUI
{
    public interface ISemanticTextColors
    {
        string BodyText { get; set; }
        string BodyTextChecked { get; set; }
        string BodySubtext { get; set; }
        string ActionLink { get; set; }
        string ActionLinkHovered { get; set; }
        string Link { get; set; }
        string LinkHovered { get; set; }
        string DisabledText { get; set; }
        string DisabledBodyText { get; set; }
        string DisabledSubtext { get; set; }
        string DisabledBodySubtext { get; set; }
        string ErrorText { get; set; }
        string WarningText { get; set; }
        string SuccessText { get; set; }
        string InputText { get; set; }
        string InputTextHovered { get; set; }
        string InputPlaceholderText { get; set; }
        string ButtonText { get; set; }
        string ButtonTextHovered { get; set; }
        string ButtonTextChecked { get; set; }
        string ButtonTextCheckedHovered { get; set; }
        string ButtonTextPressed { get; set; }
        string ButtonTextDisabled { get; set; }
        string PrimaryButtonText { get; set; }
        string PrimaryButtonTextHovered { get; set; }
        string PrimaryButtonTextPressed { get; set; }
        string PrimaryButtonTextDisabled { get; set; }
        string AccentButtonText { get; set; }
        string ListText { get; set; }
        string MenuItemText { get; set; }
        string MenuItemTextHovered { get; set; }
    }
}
