using System;
using System.Linq;

namespace BlazorFluentUI
{
    public static class RuleExtensions
    {
        public static IRule AppendCssStyles(this IRule rule, params string[] cssStyles)
        {
            var cssString = rule.Properties as CssString;
            if (cssString != null)
                cssString.AppendCss(cssStyles);

            return rule;
        }

        public static CssString AppendCss(this CssString cssString, params string[] cssStyles)
        {
            if (!cssStyles.Any())
                return cssString;

            var totalCharLength = cssStyles.Select(x => x.Length).Sum() + cssStyles.Count();

            // Use string.Create and Spans to highly optimize the string concatenation (no allocations)
            var combinedString = string.Create(totalCharLength, cssStyles, (chars, state) =>
            {
                var position = 0;
                foreach(var cssString in state)
                {
                    cssString.AsSpan().CopyTo(chars.Slice(position));
                    position += cssString.Length;
                    chars[position++] = ';'; // Append a semi-colon after each fragment
                }
            });

            cssString.Css += combinedString;
            return cssString;
        }
    }
}
