using FluentUI.Interfaces;

namespace FluentUI.Themes.Default
{
    public class DefaultCommonStyle : ICommonStyle
    {
        public int ScreenWidthMinSmall => 320;
        public int ScreenWidthMinMedium => 480;
        public int ScreenWidthMinLarge => 640;
        public int ScreenWidthMinXLarge => 1024;
        public int ScreenWidthMinXXLarge => 1366;
        public int ScreenWidthMinXXXLarge => 1920;
        public int ScreenWidthMaxSmall => ScreenWidthMinMedium - 1;
        public int ScreenWidthMaxMedium => ScreenWidthMinLarge - 1;
        public int ScreenWidthMaxLarge => ScreenWidthMinXLarge - 1;
        public int ScreenWidthMaxXLarge => ScreenWidthMinXXLarge - 1;
        public int ScreenWidthMaxXXLarge => ScreenWidthMinXXXLarge - 1;
        public int ScreenWidthMinUhfMobile => 768;
    }
}
