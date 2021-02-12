using System;
using System.Collections.Generic;
using System.Text;

namespace FluentUI
{
    public class KeyFramesSelector : ISelector
    {
        public string SelectorName { get; set; }

        public string GetSelectorAsString()
        {
            return $"@keyframes {(SelectorName != null ? SelectorName : "")}";
        }
    }
}
