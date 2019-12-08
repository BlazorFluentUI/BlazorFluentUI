using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public enum TextType
    {
        Tiny,
        XSmall,
        Small,
        SmallPlus,
        Medium,
        MediumPlus,
        Large,
        XLarge,
        XxLarge,
        Mega
    }

    public static partial class CssUtils
    {
        public static Dictionary<TextType, string> TextTypeMap = new Dictionary<TextType, string>
        {
            [TextType.Tiny] = "tiny",
            [TextType.XSmall] = "xSmal",
            [TextType.Small] = "small",
            [TextType.SmallPlus] = "smallPlus",
            [TextType.Medium] = "medium",
            [TextType.MediumPlus] = "mediumPlus",
            [TextType.Large] = "large",
            [TextType.XLarge] = "xLarge",
            [TextType.XxLarge] = "xxLarge",
            [TextType.Mega] = "mega"
        };
    }
}
