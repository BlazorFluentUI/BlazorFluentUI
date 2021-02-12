using Microsoft.AspNetCore.Components;	
using Microsoft.AspNetCore.Components.Web;	
using Microsoft.JSInterop;	
using System;	
using System.Collections.Generic;	
using System.Text;	
using System.Threading.Tasks;	

namespace FluentUI	
{	
    /**	
     * This adds accessibility to Dialog and Panel controls	
     */	
    public partial class Popup : FluentUIComponentBase, IDisposable	
    {	
        [Parameter] public RenderFragment ChildContent { get; set; }	

        [Parameter] public string Role { get; set; }	
        //[Parameter] public string AriaLabel { get; set; }	
        //[Parameter] public string AriaLabelledBy { get; set; }	
        //[Parameter] public string AriaDescribedBy { get; set; }	
        [Parameter] public bool ShouldRestoreFocus { get; set; } = true;  //THIS DOES NOTHING AT THE MOMENT.  	

        [Parameter] public EventCallback<EventArgs> OnDismiss { get; set; }	


        // Come back to this later if needed!	
        // Line needed on razor page:	
        // style=@("overflowY: {(needsVerticalScrollBar ? "scroll" : "hidden")}; outline: none")	
        private bool needsVerticalScrollBar = false;	

        private string _handleToLastFocusedElement;	

        [Inject] private IJSRuntime jSRuntime { get; set; }	

        protected override async Task OnAfterRenderAsync(bool firstRender)	
        {	
            if (firstRender)	
            {	
                _handleToLastFocusedElement = await jSRuntime.InvokeAsync<string>("FluentUIBaseComponent.storeLastFocusedElement");	
            }	
            await base.OnAfterRenderAsync(firstRender);	
        }	

        private async Task KeyDownHandler(KeyboardEventArgs args)	
        {	
            switch (args.Key)	
            {	
                case "Escape":	
                    await OnDismiss.InvokeAsync(args);	
                    break;	
            }	

            //return Task.CompletedTask;	
        }	

        public async void Dispose()	
        {	
            if (_handleToLastFocusedElement != null)	
            {	
                await jSRuntime.InvokeVoidAsync("FluentUIBaseComponent.restoreLastFocus", _handleToLastFocusedElement, ShouldRestoreFocus);	
            }	
        }	
    }	
}
