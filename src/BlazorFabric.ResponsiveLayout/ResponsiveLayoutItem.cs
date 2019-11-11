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
    public class ResponsiveLayoutItem : ComponentBase
    {
        [CascadingParameter] public ResponsiveLayout CascadingResponsiveLayout { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public bool Default { get; set; } = false;

        [Parameter] public CssValue MaxWidth { get; set; }
        [Parameter] public CssValue MinWidth { get; set; }


        [Parameter] public string Id { get; set; }

        bool IsCurrentActive = false;
        bool jsAvailable = false;

        private string _mediaQuery = "";

        protected override async Task OnParametersSetAsync()
        {
            var tempQuery = GenerateMediaQuery();
            if (tempQuery != _mediaQuery)
            {
                _mediaQuery = tempQuery;
                if (jsAvailable)
                    await CascadingResponsiveLayout.AddQueryAsync(this, _mediaQuery);
            }
            await base.OnParametersSetAsync();
        }

        //public override Task SetParametersAsync(ParameterView parameters)
        //{
        //    parameters.TryGetValue("Id", out string id);
        //    if (!string.IsNullOrEmpty(id) && id == "Narrow Panel")
        //    {
        //        foreach (var p in parameters.ToDictionary())
        //        {
        //            Debug.WriteLine($"{p.Key}: {p.Value}");
        //        }
        //    }
        //    return base.SetParametersAsync(parameters);
        //}


        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {            
            // only render if they are the active item from ResponsiveLayout
            if (CascadingResponsiveLayout != null && (CascadingResponsiveLayout.ActiveItems.Contains(this) || (!CascadingResponsiveLayout.ActiveItems.Any() && this.Default == true) ))
            {
                Debug.WriteLine($"Rendering item ({Id})");
                IsCurrentActive = true;
                builder.AddContent(0, ChildContent);
            }
            //else if (CascadingResponsiveLayout == null && Default==true)
            //{
            //    Debug.WriteLine($"Rendering DEFAULT item ({Id})");
            //    IsCurrentActive = true;
            //    builder.AddContent(0, ChildContent);
            //}
            else
            {
                IsCurrentActive = false;
            }
        }

        public void NotifyStateChange()
        {
            Debug.WriteLine($"Notified should change: {Id}");
            this.StateHasChanged();
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                jsAvailable = true;
                await CascadingResponsiveLayout.AddQueryAsync(this, _mediaQuery);
            }

            await base.OnAfterRenderAsync(firstRender);
        }

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
    }
}
