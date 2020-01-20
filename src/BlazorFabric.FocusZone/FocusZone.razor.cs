using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{

    public partial class FocusZone : FabricComponentBase, IDisposable
    {
        [Inject] private IJSRuntime jsRuntime { get; set; }

        [Parameter] public bool AllowFocusRoot { get; set; }
        //[Parameter] public FabricComponentBase As { get; set; }
        [Parameter] public bool CheckForNoWrap { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public string DefaultActiveElement { get; set; }
        [Parameter] public FocusZoneDirection Direction { get; set; } = FocusZoneDirection.Bidirectional;
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public bool DoNotAllowFocusEventToPropagate { get; set; }
        [Parameter] public FocusZoneTabbableElements HandleTabKey { get; set; }
        [Parameter] public bool IsCircularNavigation { get; set; }
        [Parameter] public List<ConsoleKey> InnerZoneKeystrokeTriggers { get; set; }
        [Parameter] public EventCallback OnActiveElementChanged { get; set; }
        [Parameter] public Func<bool> OnBeforeFocus { get; set; }  // This is likely not having an effect because of asynchronous code allowing the event to propagate.
        [Parameter] public EventCallback OnFocusNotification { get; set; }
        [Parameter] public Func<bool> ShouldInputLoseFocusOnArrowKey { get; set; } // This is likely not having an effect because of asynchronous code allowing the event to propagate.
        
        [Parameter] public bool IsFocusable { get; set; }

        public async void Focus()
        {
            await jsRuntime.InvokeVoidAsync("BlazorFabricBaseComponent.focusElement", RootElementReference);
        }

        public async void FocusFirstElement()
        {
            await jsRuntime.InvokeVoidAsync("BlazorFabricBaseComponent.focusFirstElementChild", RootElementReference);
        }

        protected string Id = Guid.NewGuid().ToString();
        //private int[] _lastIndexPath;
        private bool _jsAvailable;
        private int _registrationId = -1;

        private Task<int> _registrationTask = null;
        
        protected override Task OnInitializedAsync()
        {

            return base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (_registrationTask == null)
                {
                    _registrationTask = RegisterFocusZoneAsync();
                    _registrationId = await _registrationTask;
                }
                else if (!_registrationTask.IsCompleted)
                {
                    await _registrationTask;
                }
                _jsAvailable = true;
            }
            else
            {
                //update focusZone
                if (_registrationTask != null && !_registrationTask.IsCompleted)
                    await _registrationTask;

                await UpdateFocusZoneAsync();
                
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        protected override async Task OnParametersSetAsync()
        {
            if (_registrationId == -1 && _jsAvailable)
            {
                _registrationId = await RegisterFocusZoneAsync();
            }
            await base.OnParametersSetAsync();
        }

        private async Task UpdateFocusZoneAsync()
        {
            try
            {
                var props = FocusZoneProps.GenerateProps(this, Id, RootElementReference);
                await jsRuntime.InvokeVoidAsync("BlazorFabricFocusZone.updateFocusZone", _registrationId, props);
            }
            catch (TaskCanceledException ex)
            {

            }
        }


        private async Task<int> RegisterFocusZoneAsync()
        {
            var props = FocusZoneProps.GenerateProps(this, Id, RootElementReference);
            return await jsRuntime.InvokeAsync<int>("BlazorFabricFocusZone.register", props, DotNetObjectReference.Create(this));
        }

        private async Task UnregisterFocusZoneAsync()
        {
            try
            {
                await jsRuntime.InvokeVoidAsync("BlazorFabricFocusZone.unregister", _registrationId);
            }
            catch (TaskCanceledException ex)
            {
                var i = ex;
            }
        }

        [JSInvokable]
        public bool JSOnBeforeFocus()
        {
            return OnBeforeFocus();
        }

        [JSInvokable]
        public bool JSShouldInputLoseFocusOnArrowKey()
        {
            return ShouldInputLoseFocusOnArrowKey();
        }

        [JSInvokable]
        public void JSOnFocusNotification()
        {
            OnFocusNotification.InvokeAsync(null);
        }

        [JSInvokable]
        public void JSOnActiveElementChanged()
        {
            OnActiveElementChanged.InvokeAsync(null);
        }


        public async void Dispose()
        {
            if (_registrationId != -1)
            {
                _registrationId = -1;
                Debug.WriteLine("Trying to unregister focuszone");
                await UnregisterFocusZoneAsync();
            }
        }
    }
}
