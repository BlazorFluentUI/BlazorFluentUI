namespace BlazorFabric
{
    public interface ITheme
    {
        IPalette Palette { get; set; }
        ISemanticTextColors SemanticTextColors { get; set; }
        ISemanticColors SemanticColors { get; set; }
        IFontStyle FontStyle { get; set; }

    }
}
