using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorFluentUI
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

        public readonly static List<int> RESPONSIVE_MAX_CONSTRAINT = new() { 479, 639, 1023, 1365, 1919, 99999999 };

        public static async Task<ResponsiveMode> GetResponsiveModeAsync(IJSRuntime JSRuntime)
        {
            string BasePath = "./_content/BlazorFluentUI.CoreComponents/baseComponent.js";

            IJSObjectReference? baseModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", BasePath);

            ResponsiveMode responsiveMode = ResponsiveMode.Small;
            Rectangle? windowRect = await baseModule.InvokeAsync<Rectangle>("getWindowRect");
            try
            {
                while (windowRect.Width > RESPONSIVE_MAX_CONSTRAINT[(int)responsiveMode])
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
