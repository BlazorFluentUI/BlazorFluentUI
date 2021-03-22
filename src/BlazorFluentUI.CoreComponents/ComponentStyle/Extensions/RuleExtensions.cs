using System;
using System.Linq;

namespace BlazorFluentUI
{
    public static class RuleExtensions
    {
        public static IRule AppendCssStyles(this IRule rule, params string[] cssStyles)
        {
            if (rule.Properties is CssString cssString)
                cssString.AppendCssStyles(cssStyles);

            return rule;
        }

        public static CssString AppendCssStyles(this CssString cssString, params string[] cssStyles)
        {
            if (!cssStyles.Any())
                return cssString;

            int totalCharLength = cssStyles.Select(x => x.Length).Sum() + cssStyles.Length;

            // Use string.Create and Spans to highly optimize the string concatenation (no allocations)
            string? combinedString = string.Create(totalCharLength, cssStyles, (chars, state) =>
            {
                int position = 0;
                foreach(string? cssString in state)
                {
                    cssString.AsSpan().CopyTo(chars[position..]);
                    position += cssString.Length;
                    chars[position++] = ';'; // Append a semi-colon after each fragment
                }
            });

            cssString.Css += combinedString;
            return cssString;
        }

        public static IRule SetCssStyles(this IRule rule, params string[] cssStyles)
        {
            if (rule.Properties is CssString cssString)
                cssString.SetCssStyles(cssStyles);

            return rule;
        }

        public static CssString SetCssStyles(this CssString cssString, params string[] cssStyles)
        {
            if (!cssStyles.Any())
                return cssString;

            int totalCharLength = cssStyles.Select(x => x.Length).Sum() + cssStyles.Length;

            // Use string.Create and Spans to highly optimize the string concatenation (no allocations)
            string? combinedString = string.Create(totalCharLength, cssStyles, (chars, state) =>
            {
                int position = 0;
                foreach (string? cssString in state)
                {
                    cssString.AsSpan().CopyTo(chars[position..]);
                    position += cssString.Length;
                    chars[position++] = ';'; // Append a semi-colon after each fragment
                }
            });

            cssString.Css = combinedString;
            return cssString;
        }
    }
}
