using System;
using System.Collections.Generic;
using System.Text;

namespace FluentUI
{
    internal class TextSizeMapper
    {
        public static string TextSizeMappper(TextType textType, ITheme theme)
        {
            switch (textType)
            {
                case TextType.Tiny:
                    return theme.FontStyle.FontSize.Tiny;
                case TextType.XSmall:
                    return theme.FontStyle.FontSize.XSmall;
                case TextType.Small:
                    return theme.FontStyle.FontSize.Small;
                case TextType.SmallPlus:
                    return theme.FontStyle.FontSize.SmallPlus;
                case TextType.Medium:
                    return theme.FontStyle.FontSize.Medium;
                case TextType.MediumPlus:
                    return theme.FontStyle.FontSize.MediumPlus;
                case TextType.Large:
                    return theme.FontStyle.FontSize.Large;
                case TextType.XLarge:
                    return theme.FontStyle.FontSize.XLarge;
                case TextType.XLargePlus:
                    return theme.FontStyle.FontSize.XLargePlus;
                case TextType.XxLarge:
                    return theme.FontStyle.FontSize.XxLarge;
                case TextType.XxLargePlus:
                    return theme.FontStyle.FontSize.XxLargePlus;
                case TextType.SuperLarge:
                    return theme.FontStyle.FontSize.SuperLarge;
                case TextType.Mega:
                    return theme.FontStyle.FontSize.Mega;
                default:
                    return "inherit";
            }
        }
    }
}
