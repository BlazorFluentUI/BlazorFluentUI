using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorFluentUI
{
    public static class HashSetExtensions
    {
        public static IRule AddCssStringSelector(this ICollection<IRule> rules, string selectorName)
        {
            if (string.IsNullOrWhiteSpace(selectorName))
                throw new ArgumentNullException(nameof(selectorName));

            Rule? rule = new()
            {
                Selector = new CssStringSelector { SelectorName = selectorName },
                Properties = new CssString(),
            };

            rules.Add(rule);
            return rule;
        }
        
        public static IRule AddCssClassSelector(this ICollection<IRule> rules, string selectorName)
        {
            if (string.IsNullOrWhiteSpace(selectorName))
                throw new ArgumentNullException(nameof(selectorName));

            if (selectorName.StartsWith("."))
                selectorName = selectorName[1..];

            Rule? rule = new()
            {
                Selector = new ClassSelector { SelectorName = selectorName },
                Properties = new CssString(),
            };

            rules.Add(rule);
            return rule;
        }
    }
}
