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

    public class FocusZoneBase : FabricComponentBase, IDisposable
    {
        [Inject] private IJSRuntime jsRuntime { get; set; }

        [Parameter] public bool AllowFocusRoot { get; set; }
        //[Parameter] public FabricComponentBase As { get; set; }
        [Parameter] public bool CheckForNoWrap { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public ElementReference DefaultActiveElement { get; set; }
        [Parameter] public FocusZoneDirection Direction { get; set; } = FocusZoneDirection.Bidirectional;
        [Parameter] public bool Disabled { get; set; }
        [Parameter] public bool DoNotAllowFocusEventToPropagate { get; set; }
        [Parameter] public FocusZoneTabbableElements HandleTabKey { get; set; }
        [Parameter] public bool IsCircularNavigation { get; set; }
        [Parameter] public List<ConsoleKey> InnerZoneKeystrokeTriggers { get; set; }
        [Parameter] public EventCallback<ElementReference> OnActiveElementChanged { get; set; }
        [Parameter] public Func<object, bool> OnBeforeFocus { get; set; }  // This is likely not having an effect because of asynchronous code allowing the event to propagate.
        [Parameter] public EventCallback OnFocusNotification { get; set; }
        [Parameter] public Func<object, bool> ShouldInputLoseFocusOnArrowKey { get; set; } // This is likely not having an effect because of asynchronous code allowing the event to propagate.
        
        [Parameter] public bool IsFocusable { get; set; }

        protected string Id = Guid.NewGuid().ToString();
        //private int[] _lastIndexPath;
        private bool _jsAvailable;
        private int _registrationId = -1;

        
        protected override Task OnInitializedAsync()
        {

            return base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (_registrationId == -1)
                {
                    _registrationId = await RegisterFocusZoneAsync();
                }
                _jsAvailable = true;
            }
            else
            {
                //update focusZone
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
            Debug.WriteLine("Focuszone updating...");
            var props = FocusZoneProps.GenerateProps(this, Id, RootElementReference);
            await jsRuntime.InvokeVoidAsync("BlazorFabricFocusZone.updateFocusZone", _registrationId, props);
        }


        private async Task<int> RegisterFocusZoneAsync()
        {
            var props = FocusZoneProps.GenerateProps(this, Id, RootElementReference);
            return await jsRuntime.InvokeAsync<int>("BlazorFabricFocusZone.register", props, DotNetObjectReference.Create(this));
        }

        private async Task UnregisterFocusZoneAsync()
        {
            await jsRuntime.InvokeVoidAsync("BlazorFabricFocusZone.unregister", _registrationId);
        }

        [JSInvokable]
        public bool JSOnBeforeFocus(ElementReference element)
        {
            return OnBeforeFocus(element);
        }

        //[JSInvokable]
        //public bool JSIsInnerZoneKeystroke(KeyboardEventArgs args)
        //{
        //    return IsInnerZoneKeystroke(args);
        //}

        [JSInvokable]
        public bool JSShouldInputLoseFocusOnArrowKey(ElementReference element)
        {
            return ShouldInputLoseFocusOnArrowKey(element);
        }

        [JSInvokable]
        public void JSOnFocusNotification()
        {
            OnFocusNotification.InvokeAsync(null);
        }

        [JSInvokable]
        public void JSOnActiveElementChanged(ElementReference element)
        {
            OnActiveElementChanged.InvokeAsync(element);
        }


        public void Dispose()
        {
            if (_registrationId != -1)
            {
                _registrationId = -1;
                Debug.WriteLine("Trying to unregister focuszone");
                _ = UnregisterFocusZoneAsync();
            }
        }
    }
}
