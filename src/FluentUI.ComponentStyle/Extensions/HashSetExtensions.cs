using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentUI
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
        public static IRule AddCssStringSelector(this ICollection<Rule> rules, string selectorName)
        {
            if (string.IsNullOrWhiteSpace(selectorName))
                throw new ArgumentNullException(nameof(selectorName));

            var rule = new Rule
            {
                Selector = new CssStringSelector { SelectorName = selectorName },
                Properties = new CssString(),
            };

            rules.Add(rule);
            return rule;
        }
        public static IRule AddCssStringSelector(this ICollection<IRule> rules, string selectorName)
        {
            if (string.IsNullOrWhiteSpace(selectorName))
                throw new ArgumentNullException(nameof(selectorName));

            var rule = new Rule
            {
                Selector = new CssStringSelector { SelectorName = selectorName },
                Properties = new CssString(),
            };

            rules.Add(rule);
            return rule;
        }
        [Obsolete("We can remove this once IHasPreloadableGlobalStyle uses IRule instead of Rule in the CreateGlobalCss method.")]
        public static IRule AddCssClassSelector(this ICollection<Rule> rules, string selectorName)
        {
            if (string.IsNullOrWhiteSpace(selectorName))
                throw new ArgumentNullException(nameof(selectorName));

            if (selectorName.StartsWith("."))
                selectorName = selectorName.Substring(1);

            var rule = new Rule
            {
                Selector = new ClassSelector { SelectorName = selectorName },
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
                selectorName = selectorName.Substring(1);

            var rule = new Rule
            {
                Selector = new ClassSelector { SelectorName = selectorName },
                Properties = new CssString(),
            };

            rules.Add(rule);
            return rule;
        }
    }
}
