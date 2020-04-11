using System.Collections.Generic;

namespace BlazorFluentUI
{
    public interface IHasPreloadableGlobalStyle
    {
        ICollection<Rule> CreateGlobalCss(ITheme theme);
    }
}
