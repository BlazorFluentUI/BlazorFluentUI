using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public interface IHasPreloadableGlobalStyle
    {
        ICollection<Rule> CreateGlobalCss(ITheme theme);
    }
}
