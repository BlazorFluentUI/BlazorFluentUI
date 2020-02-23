namespace BlazorFabric.Style
{
    public class FocusStyleProps
    {
        public FocusStyleProps(ITheme theme)
        {
            BorderColor = theme.Palette.White;
            OutlineColor = theme.Palette.NeutralSecondary;
        }
        public double Inset = 0;
        public double Width = 1;
        public string Position = "relative";
        public string HighContrastStyle;
        public string BorderColor;
        public string BorderRadius = "var(--effects-RoundedCorner2)";
        public string OutlineColor;
        public bool IsFocusedOnly = true;
    }

}
