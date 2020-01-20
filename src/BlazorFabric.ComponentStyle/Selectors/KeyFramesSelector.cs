using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public class KeyFramesSelector : IUniqueSelector
    {
        public bool UniqueName { get; set; }
        public string SelectorName { get; set; }

        public string GetSelectorAsString()
        {
            return $"@keyframes {(SelectorName != null ? SelectorName : "")}";
        }
    }
}
