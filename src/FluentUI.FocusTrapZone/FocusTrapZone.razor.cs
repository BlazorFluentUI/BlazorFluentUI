using FluentUI.FocusTrapZoneInternal;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class FocusTrapZone : FluentUIComponentBase, IDisposable
    {
        protected static Stack<FocusTrapZone> _focusStack = new Stack<FocusTrapZone>();

        [Inject]
        private IJSRuntime jsRuntime { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public bool DisableFirstFocus { get; set; }

        [Parameter]
        public ElementReference ElementToFocusOnDismiss { get; set; }

        [Parameter]
        public string FirstFocusableSelector { get; set; }

        [Parameter]
        public bool FocusPreviouslyFocusedInnerElement { get; set; }

        [Parameter]
        public bool ForceFocusInsideTrap { get; set; }

        [Parameter]
        public bool IgnoreExternalFocusing { get; set; }

        [Parameter]
        public bool IsClickableOutsideFocusTrap { get; set; }


        protected ElementReference _firstBumper;
        protected ElementReference _lastBumper;

        //private bool _prevDisabled = true;
        //private bool _isElementToFocusOnDismissNotNull = false;
        //private bool _hasFocus = false;
        //private bool _isPreviouslyFocusedElementOutsideTrapZoneNotNull = false;
        //private ElementReference _previouslyFocusedElementOutsideTrapZone;
        private int _id = -1;


        public async Task FocusAsync()
        {
            if (_id != -1)
                await jsRuntime.InvokeVoidAsync("FluentUIFocusTrapZone.focus", _id);
        }
    
        protected override async Task OnParametersSetAsync()
        {
            if (_id != -1)
            {
                var props = new FocusTrapZoneProps(this, _firstBumper, _lastBumper);
                await jsRuntime.InvokeVoidAsync("FluentUIFocusTrapZone.updateProps", _id, props);
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
            var props = new FocusTrapZoneProps(this, _firstBumper, _lastBumper);
            _id = await jsRuntime.InvokeAsync<int>("FluentUIFocusTrapZone.register", props, DotNetObjectReference.Create(this));
        }
     

        public async void Dispose()
        {
            if (_id != -1)
                await jsRuntime.InvokeVoidAsync("FluentUIFocusTrapZone.unregister", _id);
        }
    }

}

