namespace FluentUI
{
    public class CommonStyles
    {
        public const string HighContrastSelector = "@media screen and (-ms-high-contrast: active)";
        public const string HighContrastSelectorWhite = "@media screen and (-ms-high-contrast: black-on-white)";
        public const string HighContrastSelectorBlack = "@media screen and (-ms-high-contrast: white-on-black)";

        public const int ScreenWidthMinSmall = 320;
        public const int ScreenWidthMinMedium = 480;
        public const int ScreenWidthMinLarge = 640;
        public const int ScreenWidthMinXLarge = 1024;
        public const int ScreenWidthMinXXLarge = 1366;
        public const int ScreenWidthMinXXXLarge = 1920;
        public const int ScreenWidthMaxSmall = ScreenWidthMinMedium - 1;
        public const int ScreenWidthMaxMedium = ScreenWidthMinLarge - 1;
        public const int ScreenWidthMaxLarge = ScreenWidthMinXLarge - 1;
        public const int ScreenWidthMaxXLarge = ScreenWidthMinXXLarge - 1;
        public const int ScreenWidthMaxXXLarge = ScreenWidthMinXXXLarge - 1;

        public static string GetScreenSelector(int min, int max)
        {
            return $"@media only screen and (min-width: {min}px) and (max-width: {max}px)";
        }
    }
}
