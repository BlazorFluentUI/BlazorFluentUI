using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public interface IPrimary
    {
        string ThemeDarker { get; }
        string ThemeDark { get; }
        string ThemeDarkAlt { get; }
        string ThemePrimary { get; }
        string ThemeSecondary { get; }
        string ThemeTertiary { get; }
        string ThemeLight { get; }
        string ThemeLighter { get; }
        string ThemeLighterAlt { get; }
    }
}
