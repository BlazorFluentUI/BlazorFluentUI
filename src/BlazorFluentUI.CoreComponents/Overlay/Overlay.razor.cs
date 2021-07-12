using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorFluentUI
{
    public partial class Overlay : FluentUIComponentBase, IAsyncDisposable
    {
        [Parameter]
        public bool IsDarkThemed { get; set; } = false;

        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //if (baseModule == null)
            //    baseModule = await JSRuntime!.InvokeAsync<IJSObjectReference>("import", BasePath);
            //if (firstRender)
            //{
            //    await baseModule!.InvokeVoidAsync("disableBodyScroll");
            //}

        }

        public override async ValueTask DisposeAsync()
        {
            try
            {
                //if (baseModule != null)
                //{
                //    await baseModule!.InvokeVoidAsync("enableBodyScroll");
                //    await baseModule.DisposeAsync();
                //}

                await base.DisposeAsync();
            }
            catch (TaskCanceledException)
            {
            }
        }
    }
}
