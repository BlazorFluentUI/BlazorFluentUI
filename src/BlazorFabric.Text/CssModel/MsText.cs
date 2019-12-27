using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlazorFabric
{
	internal class MsText : IMsText
	{
		public string Color { get; set; }

		public string Display { get; set; }

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

		[Display(Name = "white-space")]
		public string WhiteSpace { get; set; }
		
		public string Overflow { get; set; }

		[Display(Name = "text-overflow")]
		public string TextOverflow { get; set; }
	}
}

