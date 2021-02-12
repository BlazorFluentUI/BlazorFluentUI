using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentUI
{
    public enum ResponsiveMode
    {
        Small = 0,
        Medium = 1,
        Large = 2,
        XLarge = 3,
        XXLarge = 4,
        XXXLarge = 5,
        Unknown = 9999
    }

    public static class ResponsiveModeUtils
    {
        public static CssValue ToMaxCssValue(this ResponsiveMode responsiveMode)
        {
            return RESPONSIVE_MAX_CONSTRAINT[(int)responsiveMode];
        }
        public static CssValue ToMinCssValue(this ResponsiveMode responsiveMode)
        {
            return RESPONSIVE_MAX_CONSTRAINT[(int)responsiveMode] + 1;
        }

        public readonly static List<int> RESPONSIVE_MAX_CONSTRAINT = new List<int> { 479, 639, 1023, 1365, 1919, 99999999 };

        public static async Task<ResponsiveMode> GetResponsiveModeAsync(IJSRuntime jSRuntime)
        {
            var responsiveMode = ResponsiveMode.Small;
            var windowRect = await jSRuntime.InvokeAsync<Rectangle>("FluentUIBaseComponent.getWindowRect");
            try
            {
                while (windowRect.width > RESPONSIVE_MAX_CONSTRAINT[(int)responsiveMode])
                {
                    responsiveMode = (ResponsiveMode)((int)responsiveMode + 1);
                }
            }
            catch
            {
                responsiveMode = ResponsiveMode.Large;
            }
            return responsiveMode;
        }

    }
}
