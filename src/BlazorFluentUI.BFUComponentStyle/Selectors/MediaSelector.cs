using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFluentUI
{
    public class MediaSelector : ISelector
    {
        public string SelectorName { get; set; }

        public string GetSelectorAsString()
        {
            return SelectorName;
        }
    }
}
