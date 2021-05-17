using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class Image : FluentUIComponentBase, IAsyncDisposable
    {
        [Inject] private IJSRuntime? JSRuntime { get; set; }

        [Parameter] public string? Alt { get; set; }
        [Parameter] public ImageCoverStyle CoverStyle { get; set; } = ImageCoverStyle.None;
        [Parameter] public double Height { get; set; } = double.NaN;
        [Parameter] public ImageFit ImageFit { get; set; } = ImageFit.Unset;
        [Parameter] public bool MaximizeFrame { get; set; }
        [Parameter] public string? Role { get; set; }
        [Parameter] public bool ShouldFadeIn { get; set; } = true;
        [Parameter] public bool ShouldStartVisible { get; set; } = false;
        [Parameter] public string? Src { get; set; }
        [Parameter] public double Width { get; set; } = double.NaN;

        [Parameter] public EventCallback<ImageLoadState> OnLoadingStateChange { get; set; }

        private const string BasePath = "./_content/BlazorFluentUI.CoreComponents/baseComponent.js";
        private IJSObjectReference? baseModule;

        protected const string KEY_PREFIX = "fabricImage";
        private static Regex _svgRegex = new(@"\.svg$");

        protected ElementReference imageRef;

        private bool isLandscape = false;
        private ImageLoadState imageLoadState = ImageLoadState.NotLoaded;
        private bool hasRenderedOnce;

        private static Nullable<bool> SupportsObjectFit;

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            parameters.TryGetValue("Src", out string? src);
            if (Src != src)
                imageLoadState = ImageLoadState.NotLoaded;


            await base.SetParametersAsync(parameters);
        }

        protected override Task OnParametersSetAsync()
        {
            if (CoverStyle == ImageCoverStyle.Landscape)
                isLandscape = true;

            return base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (baseModule == null)
                baseModule = await JSRuntime!.InvokeAsync<IJSObjectReference>("import", BasePath);
            if (!SupportsObjectFit.HasValue)
            {
                SupportsObjectFit = await baseModule.InvokeAsync<bool>("supportsObjectFit");
            }
            if (firstRender)
                hasRenderedOnce = firstRender;
            else
                await ComputeCoverStyleAsync();

            await base.OnAfterRenderAsync(firstRender);
        }

        protected async Task OnImageLoaded(EventArgs eventArgs)
        {
            await ComputeCoverStyleAsync();

            if (Src != null)
            {
                imageLoadState = ImageLoadState.Loaded;
                await OnLoadingStateChange.InvokeAsync(imageLoadState);
            }
            StateHasChanged();
        }

        protected Task OnImageError(EventArgs eventArgs)
        {
            imageLoadState = ImageLoadState.Error;
            return OnLoadingStateChange.InvokeAsync(imageLoadState);
        }

        protected string GetRootClasses()
        {
            string classNames = "";
            classNames += MaximizeFrame ? " ms-Image--maximizeFrame" : "";
            if (imageLoadState == ImageLoadState.Loaded && ShouldFadeIn && !ShouldStartVisible)
                classNames += " fadeIn400";
            if (ImageFit == ImageFit.Center || ImageFit==ImageFit.CenterContain || ImageFit == ImageFit.CenterCover || ImageFit == ImageFit.Cover || ImageFit == ImageFit.Contain)
                classNames += " ms-Image--relative";

            return classNames;
        }

        protected string GetImageClasses()
        {
            string classNames = "";
            if (ImageFit == ImageFit.Center)
                classNames += " ms-Image-image--center ms-Image-image--fitStyles";
            if (ImageFit == ImageFit.Contain)
                classNames += " ms-Image-image--contain ms-Image-image--fitStyles";
            if (ImageFit == ImageFit.CenterContain)
                classNames += " ms-Image-image--centerContain ms-Image-image--fitStyles";
            if (ImageFit == ImageFit.Cover)
                classNames += " ms-Image-image--cover ms-Image-image--fitStyles";
            if (ImageFit == ImageFit.CenterCover)
                classNames += " ms-Image-image--centerCover ms-Image-image--fitStyles";
            if (ImageFit == ImageFit.None)
                classNames += " ms-Image-image--none";

            return classNames;
        }

        protected string GetImageStyles()
        {
            string styles = "";
            if (imageLoadState == ImageLoadState.Loaded)
                styles += "opacity:1;";
            else
                styles += "opacity:0;";

            if (!SupportsObjectFit.HasValue || SupportsObjectFit.Value == false)
            {
                if ((isLandscape && ImageFit == ImageFit.Contain) || (!isLandscape && ImageFit == ImageFit.Cover))
                {
                    styles += "width:100%;height:auto;";
                }
                else
                {
                    styles += "width:auto;height:100%;";
                }
            }

            if (ImageFit == ImageFit.CenterContain)
            {
                if (CoverStyle == ImageCoverStyle.Landscape)
                    styles += "max-width:100%;";
                else
                    styles += "max-height:100%;";
            }
            if (ImageFit == ImageFit.CenterCover)
            {
                if (CoverStyle == ImageCoverStyle.Landscape)
                    styles += "max-height:100%;";
                else
                    styles += "max-width:100%;";
            }
            if (ImageFit == ImageFit.Unset)
            {
                if (!double.IsNaN(Width) && double.IsNaN(Height))
                {
                    styles += "height:auto;width:100%;";
                }
                else if (double.IsNaN(Width) && !double.IsNaN(Height))
                {
                    styles += "height:100%;width:auto;";
                }
                else if (!double.IsNaN(Width) && !double.IsNaN(Height))
                {
                    styles += "height:100%;width:100%;";
                }

            }

            return styles;
        }


        private async Task ComputeCoverStyleAsync()
        {
            if (CoverStyle == ImageCoverStyle.None)
            {
                Rectangle? rootBounds = await GetBoundsAsync();
                Rectangle? imageNaturalBounds = await baseModule!.InvokeAsync<Rectangle>("getNaturalBounds", imageRef);

                if (imageNaturalBounds== null)
                {
                    return;
                }

                double desiredRatio;
                if (!double.IsNaN(Width) && !double.IsNaN(Height))
                {
                    desiredRatio = Width / Height;
                }
                else
                {
                    desiredRatio = rootBounds.Width / rootBounds.Height;
                }

                double naturalRatio = imageNaturalBounds.Width / imageNaturalBounds.Height;

                if (naturalRatio> desiredRatio)
                {
                    isLandscape = true;
                }
                else
                {
                    isLandscape = false;
                }
            }
        }

        public override async ValueTask DisposeAsync()
        {
            try
            {
                if (baseModule != null)
                    await baseModule.DisposeAsync();

                await base.DisposeAsync();
            }
            catch (TaskCanceledException)
            {
            }
        }
    }
}
