using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFluentUI
{
    internal class TextSizeMapper
    {
        public static string TextSizeMappper(TextType textType, ITheme? theme)
        {
            if (theme != null)
            {
                return textType switch
                {
                    TextType.Tiny => theme.FontStyle.FontSize.Tiny,
                    TextType.XSmall => theme.FontStyle.FontSize.XSmall,
                    TextType.Small => theme.FontStyle.FontSize.Small,
                    TextType.SmallPlus => theme.FontStyle.FontSize.SmallPlus,
                    TextType.Medium => theme.FontStyle.FontSize.Medium,
                    TextType.MediumPlus => theme.FontStyle.FontSize.MediumPlus,
                    TextType.Large => theme.FontStyle.FontSize.Large,
                    TextType.XLarge => theme.FontStyle.FontSize.XLarge,
                    TextType.XLargePlus => theme.FontStyle.FontSize.XLargePlus,
                    TextType.XxLarge => theme.FontStyle.FontSize.XxLarge,
                    TextType.XxLargePlus => theme.FontStyle.FontSize.XxLargePlus,
                    TextType.SuperLarge => theme.FontStyle.FontSize.SuperLarge,
                    TextType.Mega => theme.FontStyle.FontSize.Mega,
                    _ => "inherit",
                };
            }
            return "inherit";
        }
      
    }
}
