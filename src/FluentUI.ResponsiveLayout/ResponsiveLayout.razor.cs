using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class ResponsiveLayout : ComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        
        [Parameter]
        public CssValue MaxWidth { get; set; }
        
        [Parameter]
        public CssValue MinWidth { get; set; }

        //[Parameter]
        //public ResponsiveMode ResponsiveMode { get; set; } = ResponsiveMode.Unknown;

        //public static readonly int[] Responsive_Max_Constraint = { 479, 639, 1023, 1365, 1919, 99999999 };

        private string _id = "B" + Guid.NewGuid().ToString();

        //private string GenerateMediaQuery()
        //{
        //    var mediaQuery = "";
        //    if (MinWidth != null)
        //        mediaQuery = $"(min-width: {MinWidth.AsLength})";
        //    if (MaxWidth != null)
        //    {
        //        if (string.IsNullOrWhiteSpace(mediaQuery))
        //        {
        //            mediaQuery = $"(min-width: {MinWidth.AsLength})";
        //        }
        //        else
        //        {
        //            mediaQuery += $" and (max-width: {MaxWidth.AsLength})";
        //        }
        //    }

        //    return mediaQuery;
        //}

        private string GenerateCSSMediaQuery()
        {
            var mediaQuery = "";
            if (MinWidth != null)
                mediaQuery = $"(min-width: {MinWidth.AsLength})";
            if (MaxWidth != null)
            {
                if (MinWidth == null)
                {
                    mediaQuery = $"(max-width: {MaxWidth.AsLength})";
                }
                else
                {
                    mediaQuery += $" and (max-width: {MaxWidth.AsLength})";
                }
            }
            //if (MinWidth == null && MaxWidth == null)
            //{
            //    mediaQuery = $"(max-width: {ResponsiveModeUtils.RESPONSIVE_MAX_CONSTRAINT[(int)ResponsiveMode]}px)";
            //}

            var css = "#" + _id + "{display: none;}";
            css += "@media " + mediaQuery + " {" + "#" + _id + "{display:block;}}";

            return css;
        }
    }
}
