using System.Collections.Generic;

namespace BlazorFluentUI
{
    public interface IHasPreloadableGlobalStyle
    {
        ICollection<IRule> CreateGlobalCss(ITheme theme);
    }
}
