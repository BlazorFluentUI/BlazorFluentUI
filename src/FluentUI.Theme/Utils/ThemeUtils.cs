using System;
using System.Collections.Generic;
using System.Text;

namespace FluentUI.Utils
{
    internal static class ThemeUtils
    {
        internal static ISemanticColors CopyTo(this ISemanticColors source, ISemanticColors target)
        {
            if (source.BodyBackground != null) { target.BodyBackground = source.BodyBackground; }
            if (source.BodyBackgroundHovered != null) { target.BodyBackgroundHovered = source.BodyBackgroundHovered; }
            if (source.BodyBackgroundChecked != null) { target.BodyBackgroundChecked = source.BodyBackgroundChecked; }
            if (source.BodyStandoutBackground != null) { target.BodyStandoutBackground = source.BodyStandoutBackground; }
            if (source.BodyFrameBackground != null) { target.BodyFrameBackground = source.BodyFrameBackground; }
            if (source.BodyFrameDivider != null) { target.BodyFrameDivider = source.BodyFrameDivider; }
            if (source.BodyDivider != null) { target.BodyDivider = source.BodyDivider; }
            if (source.DisabledBackground != null) { target.DisabledBackground = source.DisabledBackground; }
            if (source.DisabledBorder != null) { target.DisabledBorder = source.DisabledBorder; }
            if (source.FocusBorder != null) { target.FocusBorder = source.FocusBorder; }
            if (source.VariantBorder != null) { target.VariantBorder = source.VariantBorder; }
            if (source.VariantBorderHovered != null) { target.VariantBorderHovered = source.VariantBorderHovered; }
            if (source.DefaultStateBackground != null) { target.DefaultStateBackground = source.DefaultStateBackground; }
            if (source.ErrorBackground != null) { target.ErrorBackground = source.ErrorBackground; }
            if (source.BlockingBackground != null) { target.BlockingBackground = source.BlockingBackground; }
            if (source.WarningBackground != null) { target.WarningBackground = source.WarningBackground; }
            if (source.WarningHighlight != null) { target.WarningHighlight = source.WarningHighlight; }
            if (source.SuccessBackground != null) { target.SuccessBackground = source.SuccessBackground; }
            if (source.InputBorder != null) { target.InputBorder = source.InputBorder; }
            if (source.SmallInputBorder != null) { target.SmallInputBorder = source.SmallInputBorder; }
            if (source.InputBorderHovered != null) { target.InputBorderHovered = source.InputBorderHovered; }
            if (source.InputBackground != null) { target.InputBackground = source.InputBackground; }
            if (source.InputBackgroundChecked != null) { target.InputBackgroundChecked = source.InputBackgroundChecked; }
            if (source.InputBackgroundCheckedHovered != null) { target.InputBackgroundCheckedHovered = source.InputBackgroundCheckedHovered; }
            if (source.InputPlaceholderBackgroundChecked != null) { target.InputPlaceholderBackgroundChecked = source.InputPlaceholderBackgroundChecked; }
            if (source.InputForegroundChecked != null) { target.InputForegroundChecked = source.InputForegroundChecked; }
            if (source.InputFocusBorderAlt != null) { target.InputFocusBorderAlt = source.InputFocusBorderAlt; }
            if (source.InputIconDisabled != null) { target.InputIconDisabled = source.InputIconDisabled; }
            if (source.InputIcon != null) { target.InputIcon = source.InputIcon; }
            if (source.InputIconHovered != null) { target.InputIconHovered = source.InputIconHovered; }
            if (source.ButtonBackground != null) { target.ButtonBackground = source.ButtonBackground; }
            if (source.ButtonBackgroundChecked != null) { target.ButtonBackgroundChecked = source.ButtonBackgroundChecked; }
            if (source.ButtonBackgroundHovered != null) { target.ButtonBackgroundHovered = source.ButtonBackgroundHovered; }
            if (source.ButtonBackgroundCheckedHovered != null) { target.ButtonBackgroundCheckedHovered = source.ButtonBackgroundCheckedHovered; }
            if (source.ButtonBackgroundDisabled != null) { target.ButtonBackgroundDisabled = source.ButtonBackgroundDisabled; }
            if (source.ButtonBackgroundPressed != null) { target.ButtonBackgroundPressed = source.ButtonBackgroundPressed; }
            if (source.ButtonBorder != null) { target.ButtonBorder = source.ButtonBorder; }
            if (source.ButtonBorderDisabled != null) { target.ButtonBorderDisabled = source.ButtonBorderDisabled; }
            if (source.PrimaryButtonBackground != null) { target.PrimaryButtonBackground = source.PrimaryButtonBackground; }
            if (source.PrimaryButtonBackgroundHovered != null) { target.PrimaryButtonBackgroundHovered = source.PrimaryButtonBackgroundHovered; }
            if (source.PrimaryButtonBackgroundPressed != null) { target.PrimaryButtonBackgroundPressed = source.PrimaryButtonBackgroundPressed; }
            if (source.PrimaryButtonBackgroundDisabled != null) { target.PrimaryButtonBackgroundDisabled = source.PrimaryButtonBackgroundDisabled; }
            if (source.PrimaryButtonBorder != null) { target.PrimaryButtonBorder = source.PrimaryButtonBorder; }
            if (source.AccentButtonBackground != null) { target.AccentButtonBackground = source.AccentButtonBackground; }
            if (source.MenuBackground != null) { target.MenuBackground = source.MenuBackground; }
            if (source.MenuDivider != null) { target.MenuDivider = source.MenuDivider; }
            if (source.MenuIcon != null) { target.MenuIcon = source.MenuIcon; }
            if (source.MenuHeader != null) { target.MenuHeader = source.MenuHeader; }
            if (source.MenuItemBackgroundHovered != null) { target.MenuItemBackgroundHovered = source.MenuItemBackgroundHovered; }
            if (source.MenuItemBackgroundPressed != null) { target.MenuItemBackgroundPressed = source.MenuItemBackgroundPressed; }
            if (source.ListBackground != null) { target.ListBackground = source.ListBackground; }
            if (source.ListItemBackgroundHovered != null) { target.ListItemBackgroundHovered = source.ListItemBackgroundHovered; }
            if (source.ListItemBackgroundChecked != null) { target.ListItemBackgroundChecked = source.ListItemBackgroundChecked; }
            if (source.ListItemBackgroundCheckedHovered != null) { target.ListItemBackgroundCheckedHovered = source.ListItemBackgroundCheckedHovered; }
            if (source.ListHeaderBackgroundHovered != null) { target.ListHeaderBackgroundHovered = source.ListHeaderBackgroundHovered; }
            if (source.ListHeaderBackgroundPressed != null) { target.ListHeaderBackgroundPressed = source.ListHeaderBackgroundPressed; }

            return target;
        }

        internal static ISemanticTextColors CopyTo(this ISemanticTextColors source, ISemanticTextColors target)
        {
            if (source.BodyText != null) { target.BodyText = source.BodyText; }
            if (source.BodyTextChecked != null) { target.BodyTextChecked = source.BodyTextChecked; }
            if (source.BodySubtext != null) { target.BodySubtext = source.BodySubtext; }
            if (source.ActionLink != null) { target.ActionLink = source.ActionLink; }
            if (source.ActionLinkHovered != null) { target.ActionLinkHovered = source.ActionLinkHovered; }
            if (source.Link != null) { target.Link = source.Link; }
            if (source.LinkHovered != null) { target.LinkHovered = source.LinkHovered; }
            if (source.DisabledText != null) { target.DisabledText = source.DisabledText; }
            if (source.DisabledBodyText != null) { target.DisabledBodyText = source.DisabledBodyText; }
            if (source.DisabledSubtext != null) { target.DisabledSubtext = source.DisabledSubtext; }
            if (source.DisabledBodySubtext != null) { target.DisabledBodySubtext = source.DisabledBodySubtext; }
            if (source.ErrorText != null) { target.ErrorText = source.ErrorText; }
            if (source.WarningText != null) { target.WarningText = source.WarningText; }
            if (source.SuccessText != null) { target.SuccessText = source.SuccessText; }
            if (source.InputText != null) { target.InputText = source.InputText; }
            if (source.InputTextHovered != null) { target.InputTextHovered = source.InputTextHovered; }
            if (source.InputPlaceholderText != null) { target.InputPlaceholderText = source.InputPlaceholderText; }
            if (source.ButtonText != null) { target.ButtonText = source.ButtonText; }
            if (source.ButtonTextHovered != null) { target.ButtonTextHovered = source.ButtonTextHovered; }
            if (source.ButtonTextChecked != null) { target.ButtonTextChecked = source.ButtonTextChecked; }
            if (source.ButtonTextCheckedHovered != null) { target.ButtonTextCheckedHovered = source.ButtonTextCheckedHovered; }
            if (source.ButtonTextPressed != null) { target.ButtonTextPressed = source.ButtonTextPressed; }
            if (source.ButtonTextDisabled != null) { target.ButtonTextDisabled = source.ButtonTextDisabled; }
            if (source.PrimaryButtonText != null) { target.PrimaryButtonText = source.PrimaryButtonText; }
            if (source.PrimaryButtonTextHovered != null) { target.PrimaryButtonTextHovered = source.PrimaryButtonTextHovered; }
            if (source.PrimaryButtonTextPressed != null) { target.PrimaryButtonTextPressed = source.PrimaryButtonTextPressed; }
            if (source.PrimaryButtonTextDisabled != null) { target.PrimaryButtonTextDisabled = source.PrimaryButtonTextDisabled; }
            if (source.AccentButtonText != null) { target.AccentButtonText = source.AccentButtonText; }
            if (source.ListText != null) { target.ListText = source.ListText; }
            if (source.MenuItemText != null) { target.MenuItemText = source.MenuItemText; }
            if (source.MenuItemTextHovered != null) { target.MenuItemTextHovered = source.MenuItemTextHovered; }
            return target;
        }
    }
}
