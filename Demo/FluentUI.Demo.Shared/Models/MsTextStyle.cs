namespace FluentUI.Demo.Shared.Models
{
    public class MsTextStyle : IMsText
    {
        public string Color { get; set; }
        [CsProperty(PropertyName = "font-family")]
        public string FontFamily { get; set; }
        [CsProperty(PropertyName = "font-size")]
        public string FontSize { get; set; }
        [CsProperty(PropertyName = "font-weight")]
        public string FontWeight { get; set; }
        [CsProperty(PropertyName = "-webkit-font-smoothing")]
        public string WebkitFontSmoothing { get; set; }
        [CsProperty(PropertyName = "-moz-osx-font-smoothing")]
        public string MozOsxFontSmoothing { get; set; }
    }
}
