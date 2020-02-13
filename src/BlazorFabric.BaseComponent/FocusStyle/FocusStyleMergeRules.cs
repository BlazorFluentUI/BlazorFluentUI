using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric.BaseComponent.FocusStyle
{
    public class FocusStyleMergeRules
    {
        public string MergeRules { get; set; }
        public List<Rule> AddRules { get; set; } = new List<Rule>();
    }
}
