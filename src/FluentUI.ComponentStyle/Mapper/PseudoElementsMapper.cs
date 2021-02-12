using System.Collections.Generic;

namespace FluentUI
{
    public static partial class PseudoMapper
    {
        public static Dictionary<PseudoElements, string> PseudoElementsMappper = new Dictionary<PseudoElements, string>
        {
            [PseudoElements.None] = "",
            [PseudoElements.After] = "::after",
            [PseudoElements.Backdrop] = "::backdrop",
            [PseudoElements.Before] = "::before",
            [PseudoElements.FirstLetter] = "::first-letter",
            [PseudoElements.FirstLine] = "::first-line",
            [PseudoElements.Placeholder] = "::placeholder",
            [PseudoElements.Selection] = "::selection"
        };

        public static Dictionary<PseudoClasses, string> PseudoClassesMappper = new Dictionary<PseudoClasses, string>
        {
            [PseudoClasses.None] = "",
            [PseudoClasses.Active] = ":active",
            [PseudoClasses.AnyLink] = ":any-link",
            [PseudoClasses.Checked] = ":checked",
            [PseudoClasses.Disabled] = ":disabled",
            [PseudoClasses.Empty] = ":empty",
            [PseudoClasses.Enabled] = ":enabled",
            [PseudoClasses.FirstChild] = ":first-child",
            [PseudoClasses.FirstOfType] = ":first-of-type",
            [PseudoClasses.Focus] = ":focus",
            [PseudoClasses.FocusWithin] = ":focus-within",
            [PseudoClasses.Hover] = ":hover",
            [PseudoClasses.InRange] = ":in-range",
            [PseudoClasses.Invalid] = ":invalid",
            [PseudoClasses.Lang] = ":lang",
            [PseudoClasses.LastChild] = ":last-child",
            [PseudoClasses.LastOfType] = ":last-of-type",
            [PseudoClasses.Link] = ":link",
            [PseudoClasses.Matches] = ":matches",
            [PseudoClasses.Not] = ":not",
            [PseudoClasses.NthChild] = ":nth-child",
            [PseudoClasses.NthLastChild] = ":nth-last-child",
            [PseudoClasses.NthLastOfType] = ":nth-last-of-type",
            [PseudoClasses.NthOfType] = ":nth-of-type",
            [PseudoClasses.OnlyOfType] = ":only-of-type",
            [PseudoClasses.OnlyChild] = ":only-child",
            [PseudoClasses.Optional] = ":optional",
            [PseudoClasses.OutOfRange] = ":out-of-range",
            [PseudoClasses.PlaceholderShown] = ":placeholder-shown",
            [PseudoClasses.Root] = ":root",
            [PseudoClasses.Target] = ":target",
            [PseudoClasses.Valid] = ":valid",
            [PseudoClasses.Visited] = ":visited"
        };
    }
}