namespace FluentUI
{
	internal class MsText : IMsText
	{
		public string Color { get; set; }

		public string Display { get; set; }

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

		[CsProperty(PropertyName = "white-space")]
		public string WhiteSpace { get; set; }
		
		public string Overflow { get; set; }

		[CsProperty(PropertyName = "text-overflow")]
		public string TextOverflow { get; set; }
	}
}

