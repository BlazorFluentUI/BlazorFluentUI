using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public enum TextType
    {
        None,
        Tiny,
        XSmall,
        Small,
        SmallPlus,
        Medium,
        MediumPlus,
        Large,
        XLarge,
        XLargePlus,
        XxLarge,
        XxLargePlus,
        SuperLarge,
        Mega
    }

    public static partial class CssUtils
    {
        public static Dictionary<TextType, string> TextTypeMap = new Dictionary<TextType, string>
        {
            [TextType.None] = "",
            [TextType.Tiny] = "tiny",
            [TextType.XSmall] = "xSmall",
            [TextType.Small] = "small",
            [TextType.SmallPlus] = "smallPlus",
            [TextType.Medium] = "medium",
            [TextType.MediumPlus] = "mediumPlus",
            [TextType.Large] = "large",
            [TextType.XLarge] = "xLarge",
            [TextType.XLargePlus] = "xLargePlus",
            [TextType.XxLarge] = "xxLarge",
            [TextType.XxLargePlus] = "xxLargePlus",
            [TextType.SuperLarge] = "superLarge",
            [TextType.Mega] = "mega"
        };
    }
}
