using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    internal class TextSizeMapper
    {
        public static Dictionary<TextType, string> TextSizeMap = new Dictionary<TextType, string>
        {
            [TextType.None] = "",
            [TextType.Tiny] = "10px",
            [TextType.XSmall] = "10px",
            [TextType.Small] = "12px",
            [TextType.SmallPlus] = "12px",
            [TextType.Medium] = "14px",
            [TextType.MediumPlus] = "16px",
            [TextType.Large] = "18px",
            [TextType.XLarge] = "20px",
            [TextType.XLargePlus] = "24px",
            [TextType.XxLarge] = "28px",
            [TextType.XxLargePlus] = "32px",
            [TextType.SuperLarge] = "42px",
            [TextType.Mega] = "68px"
        };
    }
}
