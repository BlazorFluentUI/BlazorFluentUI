using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorFabric.Test.ClientSide.Models
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
