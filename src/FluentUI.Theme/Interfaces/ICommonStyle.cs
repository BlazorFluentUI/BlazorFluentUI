using System;
using System.Collections.Generic;
using System.Text;

namespace FluentUI.Interfaces
{
    public interface ICommonStyle
    {
        int ScreenWidthMinSmall { get; }
        int ScreenWidthMinMedium { get; }
        int ScreenWidthMinLarge { get; }
        int ScreenWidthMinXLarge { get; }
        int ScreenWidthMinXXLarge { get; }
        int ScreenWidthMinXXXLarge { get; }
        int ScreenWidthMaxSmall { get; }
        int ScreenWidthMaxMedium { get; }
        int ScreenWidthMaxLarge { get; }
        int ScreenWidthMaxXLarge { get; }
        int ScreenWidthMaxXXLarge { get; }
        int ScreenWidthMinUhfMobile { get; }
    }
}
