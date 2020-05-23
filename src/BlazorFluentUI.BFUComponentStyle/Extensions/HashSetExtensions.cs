using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorFluentUI
{
    public static class HashSetExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="selectorName"></param>
        /// <returns></returns>
        [Obsolete("We can remove this once IHasPreloadableGlobalStyle uses IRule instead of Rule in the CreateGlobalCss method.")]
        public static IRule AddCssStringSelector(this HashSet<Rule> rules, string selectorName)
        {
            if (string.IsNullOrWhiteSpace(selectorName))
                throw new ArgumentNullException(nameof(selectorName));

            if (!selectorName.StartsWith("."))
                selectorName = $".{selectorName}";

            var rule = new Rule
            {
                Selector = new CssStringSelector { SelectorName = selectorName },
                Properties = new CssString(),
            };

            rules.Add(rule);
            return rule;
        }
        public static IRule AddCssStringSelector(this HashSet<IRule> rules, string selectorName)
        {
            if (string.IsNullOrWhiteSpace(selectorName))
                throw new ArgumentNullException(nameof(selectorName));

            if (!selectorName.StartsWith("."))
                selectorName = $".{selectorName}";

            var rule = new Rule
            {
                Selector = new CssStringSelector { SelectorName = selectorName },
                Properties = new CssString(),
            };

            rules.Add(rule);
            return rule;
        }
    }
}
