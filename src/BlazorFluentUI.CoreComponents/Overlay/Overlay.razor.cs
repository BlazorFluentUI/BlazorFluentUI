using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorFluentUI
{
    public partial class Overlay : FluentUIComponentBase, IAsyncDisposable
    {
        [Inject]
        private IJSRuntime? JSRuntime { get; set; }
        private const string BasePath = "./_content/BlazorFluentUI.CoreComponents/baseComponent.js";
        private IJSObjectReference? baseModule;


        [Parameter]
        public bool IsDarkThemed { get; set; } = false;

        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            baseModule = await JSRuntime!.InvokeAsync<IJSObjectReference>("import", BasePath);
            if (firstRender)
            {
                await baseModule!.InvokeVoidAsync("disableBodyScroll");
            }

        }

        public override async ValueTask DisposeAsync()
        {
            if (baseModule != null)
            {
                await baseModule!.InvokeVoidAsync("enableBodyScroll");
                await baseModule.DisposeAsync();
            }
        }
    }
}
