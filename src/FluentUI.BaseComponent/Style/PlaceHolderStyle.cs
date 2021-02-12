using System.Collections.Generic;

namespace FluentUI.Style
{
    public static class PlaceHolderStyle
    {
        public static IList<Rule> GetPlaceholderStyle(string selectorName ,IRuleProperties properties)
        {
            var placeholderRules = new List<Rule>();

            placeholderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $"{selectorName}::placeholder" },
                Properties = properties
            });

            placeholderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $"{selectorName}:-ms-input-placeholder" },
                Properties = properties
            });

            placeholderRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $"{selectorName}::-ms-input-placeholder" },
                Properties = properties
            });

            return placeholderRules;
        }
    }
}
