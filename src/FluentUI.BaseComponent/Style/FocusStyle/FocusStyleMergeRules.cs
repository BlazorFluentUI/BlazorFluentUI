using System.Collections.Generic;

namespace FluentUI.Style
{
    public class FocusStyleMergeRules
    {
        public string? MergeRules { get; set; }
        public List<string> MergeRulesList { get; set; } = new List<string>();
        public List<Rule> AddRules { get; set; } = new List<Rule>();
    }
}
