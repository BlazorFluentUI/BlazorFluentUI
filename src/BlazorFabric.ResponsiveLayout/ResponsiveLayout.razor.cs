using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class ResponsiveLayout : ComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        
        [Parameter]
        public CssValue MaxWidth { get; set; }
        
        [Parameter]
        public CssValue MinWidth { get; set; }

        bool IsCurrentActive = false;
        bool jsAvailable = false;

        private string _mediaQuery = "";

        private string _id = "B" + Guid.NewGuid().ToString();

        private string GenerateMediaQuery()
        {
            var mediaQuery = "";
            if (MinWidth != null)
                mediaQuery = $"(min-width: {MinWidth.AsLength})";
            if (MaxWidth != null)
            {
                if (string.IsNullOrWhiteSpace(mediaQuery))
                {
                    mediaQuery = $"(min-width: {MinWidth.AsLength})";
                }
                else
                {
                    mediaQuery += $" and (max-width: {MaxWidth.AsLength})";
                }
            }

            return mediaQuery;
        }

        private string GenerateCSSMediaQuery()
        {
            var mediaQuery = "";
            if (MinWidth != null)
                mediaQuery = $"(min-width: {MinWidth.AsLength})";
            if (MaxWidth != null)
            {
                if (string.IsNullOrWhiteSpace(mediaQuery))
                {
                    mediaQuery = $"(min-width: {MinWidth.AsLength})";
                }
                else
                {
                    mediaQuery += $" and (max-width: {MaxWidth.AsLength})";
                }
            }

            var css = "#" + _id + "{display: none;}";
            css += "@media " + mediaQuery + " {" + "#" + _id + "{display:block;}}";

            return css;
        }
    }
}
