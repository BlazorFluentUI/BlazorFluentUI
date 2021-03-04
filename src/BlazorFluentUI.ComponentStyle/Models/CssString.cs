using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFluentUI
{
    public class CssString : IRuleProperties
    {
        [CsProperty(IsCssStringProperty = true)]
        public string Css { get; set; }
    }
}
