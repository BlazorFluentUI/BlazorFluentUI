using System;
using System.Collections.Generic;
using System.Text;

namespace FluentUI
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
