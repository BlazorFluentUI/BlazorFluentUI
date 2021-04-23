using BlazorFluentUI.Themes.Default;

namespace BlazorFluentUI
{
    public partial class Theme : ITheme
    {
        public IPalette Palette { get; set; } = new DefaultPalette();
        public ISemanticTextColors SemanticTextColors { get; set; } = new SemanticTextColors();
        public ISemanticColors SemanticColors { get; set; } = new SemanticColors();
        public IFontStyle FontStyle { get; set; } = new FontStyle();
        public ICommonStyle CommonStyle { get; set; } = new DefaultCommonStyle();
        public IZIndex ZIndex { get; set; } = new DefaultZIndex();
        public IEffects Effects { get; set; } = new DefaultEffects();
        public IDepths Depths { get; set; } = new DefaultDepths();
        public IAnimation Animation { get; set; } = new DefaultAnimation();
    }
}
