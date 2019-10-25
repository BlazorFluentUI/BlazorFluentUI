using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public class ResponsiveLayoutItem : ComponentBase
    {
        [CascadingParameter] public ResponsiveLayout CascadingResponsiveLayout { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public CssValue MaxWidth { get; set; }
        [Parameter] public CssValue MinWidth { get; set; }

        private string mediaQuery = "";

        protected override async Task OnParametersSetAsync()
        {
            GenerateMediaQuery();
            await CascadingResponsiveLayout.AddQueryAsync(this, mediaQuery);
            await base.OnParametersSetAsync();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            // only render if they are the active item from ResponsiveLayout
            if (CascadingResponsiveLayout != null && CascadingResponsiveLayout.ActiveItem == this)
            {
                builder.AddContent(0, ChildContent);
            }
        }

        private void GenerateMediaQuery()
        {
            mediaQuery = "";
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
        }
    }
}
