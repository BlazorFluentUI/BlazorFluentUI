using System.Collections.Generic;

namespace BlazorFluentUI.Style
{
    public class FocusStyleMergeRules
    {
        public string MergeRules { get; set; }
        public List<Rule> AddRules { get; set; } = new List<Rule>();
    }
}
