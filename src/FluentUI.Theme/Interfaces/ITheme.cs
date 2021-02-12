using FluentUI.Interfaces;

namespace FluentUI
{
    public interface ITheme
    {
        IPalette Palette { get; set; }
        ISemanticTextColors SemanticTextColors { get; set; }
        ISemanticColors SemanticColors { get; set; }
        IFontStyle FontStyle { get; set; }
        ICommonStyle CommonStyle { get; set; }
        IZIndex ZIndex { get; set; }
        IDepths Depths { get; set; }
        IEffects Effects { get; set; }
        IAnimation Animation { get; set; }
    }
}
