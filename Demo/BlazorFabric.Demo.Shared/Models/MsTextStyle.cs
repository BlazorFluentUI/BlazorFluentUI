using System.ComponentModel.DataAnnotations;

namespace BlazorFabric.Demo.Shared.Models
{
    public class MsTextStyle : IMsText
    {
        public string Color { get; set; }
        [Display(Name = "font-family")]
        public string FontFamily { get; set; }
        [Display(Name = "font-size")]
        public string FontSize { get; set; }
        [Display(Name = "font-weight")]
        public string FontWeight { get; set; }
        [Display(Name = "-webkit-font-smoothing")]
        public string WebkitFontSmoothing { get; set; }
        [Display(Name = "-moz-osx-font-smoothing")]
        public string MozOsxFontSmoothing { get; set; }
    }
}
