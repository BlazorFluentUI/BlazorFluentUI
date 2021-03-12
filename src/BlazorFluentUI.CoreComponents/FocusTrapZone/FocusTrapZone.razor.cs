using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class FocusTrapZone : FluentUIComponentBase, IDisposable
    {
        //protected static Stack<FocusTrapZone> _focusStack = new();

        [Inject]
        private IJSRuntime? JSRuntime { get; set; }

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool DisableFirstFocus { get; set; }

        [Parameter]
        public ElementReference ElementToFocusOnDismiss { get; set; }

        [Parameter]
        public string? FirstFocusableSelector { get; set; }

        [Parameter]
        public bool FocusPreviouslyFocusedInnerElement { get; set; }

        [Parameter]
        public bool FocusTriggerOnOutsideClick { get; set; }

        [Parameter]
        public bool ForceFocusInsideTrap { get; set; }

        [Parameter]
        public bool IgnoreExternalFocusing { get; set; }

        [Parameter]
        public bool IsClickableOutsideFocusTrap { get; set; }


        protected ElementReference _firstBumper;
        protected ElementReference _lastBumper;

        private int _id = -1;


        public async Task FocusAsync()
        {
            if (_id != -1)
                await JSRuntime!.InvokeVoidAsync("BlazorFluentUIFocusTrapZone.focus", _id);
        }

        protected override async Task OnParametersSetAsync()
        {
            if (_id != -1)
            {
                FocusTrapZoneProps? props = new(this, _firstBumper, _lastBumper);
                await JSRuntime!.InvokeVoidAsync("BlazorFluentUIFocusTrapZone.updateProps", _id, props);
            }

            await base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                RegisterFocusTrapZone();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async void RegisterFocusTrapZone()
        {
            FocusTrapZoneProps? props = new(this, _firstBumper, _lastBumper);
            _id = await JSRuntime!.InvokeAsync<int>("BlazorFluentUIFocusTrapZone.register", props, DotNetObjectReference.Create(this));
        }


        public async void Dispose()
        {
            if (_id != -1)
                await JSRuntime!.InvokeVoidAsync("BlazorFluentUIFocusTrapZone.unregister", _id);
            
            GC.SuppressFinalize(this);
        }

    }

}

