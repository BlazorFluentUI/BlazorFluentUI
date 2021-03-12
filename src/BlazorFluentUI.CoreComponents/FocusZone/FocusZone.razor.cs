using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BlazorFluentUI
{

    public partial class FocusZone : FluentUIComponentBase, IAsyncDisposable
    {
        [Inject] private IJSRuntime? JSRuntime { get; set; }

        [Parameter] public bool AllowFocusRoot { get=>allowFocusRoot; set { if (value != allowFocusRoot) { updateFocusZone = true; allowFocusRoot = value; } } }
        //[Parameter] public ComponentBase As { get; set; }
        [Parameter] public bool CheckForNoWrap { get => checkForNoWrap; set { if (value != checkForNoWrap) { updateFocusZone = true; checkForNoWrap = value; } } }
        [Parameter] public RenderFragment? ChildContent { get; set; }
        [Parameter] public string DefaultActiveElement { get => defaultActiveElement; set { if (value != defaultActiveElement) { updateFocusZone = true; defaultActiveElement = value; } } }
        [Parameter] public FocusZoneDirection Direction { get => direction; set { if (value != direction) { updateFocusZone = true; direction = value; } } }
        [Parameter] public bool Disabled { get => disabled; set { if (value != disabled) { updateFocusZone = true; disabled = value; } } }
        [Parameter] public bool DoNotAllowFocusEventToPropagate { get => doNotAllowFocusEventToPropagate; set { if (value != doNotAllowFocusEventToPropagate) { updateFocusZone = true; doNotAllowFocusEventToPropagate = value; } } }
        [Parameter] public FocusZoneTabbableElements HandleTabKey { get => handleTabKey; set { if (value != handleTabKey) { updateFocusZone = true; handleTabKey = value; } } }
        [Parameter] public bool IsCircularNavigation { get => isCircularNavigation; set { if (value != isCircularNavigation) { updateFocusZone = true; isCircularNavigation = value; } } }
        [Parameter] public List<ConsoleKey> InnerZoneKeystrokeTriggers { get => innerZoneKeystrokeTriggers; set { if (value != innerZoneKeystrokeTriggers) { updateFocusZone = true; innerZoneKeystrokeTriggers = value; } } }
        [Parameter] public EventCallback OnActiveElementChanged { get; set; }
        [Parameter] public Func<bool> OnBeforeFocus { get => onBeforeFocus; set { if (value != onBeforeFocus) { updateFocusZone = true; onBeforeFocus = value; } } }   // This is likely not having an effect because of asynchronous code allowing the event to propagate.

        [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
        [Parameter] public EventCallback OnFocusNotification { get; set; }

        [Parameter] public string Role { get; set; } = "presentation";
        [Parameter] public Func<bool> ShouldInputLoseFocusOnArrowKey { get => shouldInputLoseFocusOnArrowKey; set { if (value != shouldInputLoseFocusOnArrowKey) { updateFocusZone = true; shouldInputLoseFocusOnArrowKey = value; } } } // This is likely not having an effect because of asynchronous code allowing the event to propagate.
        [Parameter] public bool IsFocusable { get; set; }

        [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> UnknownAttributes { get; set; }


        bool allowFocusRoot;
        bool checkForNoWrap;
        string defaultActiveElement;
        FocusZoneDirection direction;
        bool disabled;
        bool doNotAllowFocusEventToPropagate;
        FocusZoneTabbableElements handleTabKey;
        bool isCircularNavigation;
        List<ConsoleKey> innerZoneKeystrokeTriggers;
        Func<bool> onBeforeFocus;
        Func<bool> shouldInputLoseFocusOnArrowKey;

        bool updateFocusZone = false;

        public async void Focus()
        {
            await JSRuntime!.InvokeVoidAsync("FluentUIBaseComponent.focusElement", RootElementReference);
        }

        public async void FocusFirstElement()
        {
            await JSRuntime!.InvokeVoidAsync("FluentUIBaseComponent.focusFirstElementChild", RootElementReference);
        }

        protected string Id = Guid.NewGuid().ToString();
        //private int[] _lastIndexPath;
        //private bool _jsAvailable;
        private int _registrationId = -1;

        private readonly Task<int>? _registrationTask = null;

        public event PropertyChangedEventHandler? PropertyChanged;
        //private bool parametersUpdated = false;

        protected override Task OnInitializedAsync()
        {
            Direction = FocusZoneDirection.Bidirectional;
            return base.OnInitializedAsync();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                if (_registrationTask == null)
                {
                    _ = RegisterFocusZoneAsync();
                }
                else if (!_registrationTask.IsCompleted)
                {
                    //await _registrationTask;
                }
                //_jsAvailable = true;
                updateFocusZone = false;
            }
            else
            {
                if (_registrationId != -1 && updateFocusZone)
                {
                    updateFocusZone = false;
                    _ = UpdateFocusZoneAsync();
                }
            }
            base.OnAfterRender(firstRender);
        }


        protected override async Task OnParametersSetAsync()
        {

            await base.OnParametersSetAsync();
        }

        private async Task UpdateFocusZoneAsync()
        {
            try
            {
                FocusZoneProps? props = FocusZoneProps.GenerateProps(this, Id, RootElementReference);
                await JSRuntime!.InvokeVoidAsync("BlazorFluentUIFocusZone.updateFocusZone", _registrationId, props);
            }
            catch (TaskCanceledException)
            {

            }
        }


        private async Task RegisterFocusZoneAsync()
        {
            FocusZoneProps? props = FocusZoneProps.GenerateProps(this, Id, RootElementReference);
            _registrationId = await JSRuntime!.InvokeAsync<int>("BlazorFluentUIFocusZone.register", props, DotNetObjectReference.Create(this));
        }

        private async Task UnregisterFocusZoneAsync()
        {
            try
            {
                await JSRuntime!.InvokeVoidAsync("BlazorFluentUIFocusZone.unregister", _registrationId);
                _registrationId = -1;
            }
            catch (TaskCanceledException)
            {
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


        //public async void Dispose()
        //{
        //    if (_registrationId != -1)
        //    {
        //        //Debug.WriteLine("Trying to unregister focuszone");
        //        UnregisterFocusZoneAsync();

        //    }
        //}

        public async ValueTask DisposeAsync()
        {
            if (_registrationId != -1)
            {
                //Debug.WriteLine("Trying to unregister focuszone");
                await UnregisterFocusZoneAsync();

            }
            GC.SuppressFinalize(this);
        }

        //public ICollection<IRule> CreateGlobalCss(ITheme theme)
        //{
        //    var rules = new HashSet<IRule>();

        //    rules.AddCssClassSelector(".ms-FocusZone:focus").AppendCssStyles("outline:none;");

        //    return rules;
        //}
    }
}
