using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;

namespace FluentUI
{
    public partial class Modal : FluentUIComponentBase, IDisposable
    {
        [Parameter]
        public string ContainerClass { get; set; }

        [Parameter]
        public bool IsOpen { get; set; }

        [Parameter]
        public bool IsModeless { get; set; }

        [Parameter]
        public bool IsBlocking { get; set; }

        [Parameter]
        public bool IsDarkOverlay { get; set; } = true;

        [Parameter]
        public string TitleAriaId { get; set; }

        [Parameter]
        public string SubtitleAriaId { get; set; }

        [Parameter]
        public bool TopOffsetFixed { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }
       
        [Parameter]
        public EventCallback<EventArgs> OnDismiss { get; set; }

        // from IAccessiblePopupProps
        [Parameter]
        public ElementReference ElementToFocusOnDismiss { get; set; }

        [Parameter]
        public bool IgnoreExternalFocusing { get; set; }

        [Parameter]
        public bool ForceFocusInsideTrap { get; set; } = true;

        [Parameter]
        public string FirstFocusableSelector { get; set; }

        [Parameter]
        public string CloseButtonAriaLabel { get; set; }

        [Parameter]
        public bool IsClickableOutsideFocusTrap { get; set; }

        private bool _isOpenDelayed = false;

        private ElementReference allowScrollOnModal;

        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        private bool isAnimating = false;
        private bool animationRenderStart = false;
        private ModalVisibilityState previousVisibility = ModalVisibilityState.Closed;
        private ModalVisibilityState currentVisibility = ModalVisibilityState.Closed;
        private Timer _animationTimer;
        private Action _clearExistingAnimationTimer;
        private Action<ModalVisibilityState> _animateTo;
        private Action _onTransitionComplete;
        private ElapsedEventHandler _handler = null;
        private bool _jsAvailable;
        private string _keydownRegistration;

        public Modal()
        {
            _animationTimer = new Timer();
            _clearExistingAnimationTimer = () =>
            {
                if (_animationTimer.Enabled)
                {
                    _animationTimer.Stop();
                    _animationTimer.Elapsed -= _handler;
                }
            };

            _animateTo = (animationState) =>
            {
                _animationTimer.Interval = 200;
                _handler = null;
                _handler = (s, e) =>
                {
                    _animationTimer.Elapsed -= _handler;
                    _animationTimer.Stop();
                    InvokeAsync(() =>
                    {
                        //Debug.WriteLine("Inside invokeAsync from animateTo timer elapsed");
                        previousVisibility = currentVisibility;
                        currentVisibility = animationState;
                        _onTransitionComplete();
                    });
                };
                _animationTimer.Elapsed += _handler;
                _animationTimer.Start();
            };

            _onTransitionComplete = () =>
            {
                isAnimating = false;
                StateHasChanged();
            };
        }

        protected override async Task OnParametersSetAsync()
        {
            previousVisibility = currentVisibility;

            if (IsOpen && (currentVisibility == ModalVisibilityState.Closed || currentVisibility == ModalVisibilityState.AnimatingClosed))
            {
                currentVisibility = ModalVisibilityState.AnimatingOpen;
            }
            if (!IsOpen && (currentVisibility == ModalVisibilityState.Open || currentVisibility == ModalVisibilityState.AnimatingOpen))
            {
                currentVisibility = ModalVisibilityState.AnimatingClosed;
                // This StateHasChanged call was added because using a custom close button in NavigationTemplate did not cause a state change to occur.
                // The result was that the animation class would not get added and the close transition would not show.  This is a hack to make it work.
                StateHasChanged();
            }

            Debug.WriteLine($"Was: {previousVisibility}  Current:{currentVisibility}");

            if (_jsAvailable)
            {
                if (currentVisibility != previousVisibility)
                {
                    Debug.WriteLine("Clearing animation timer");
                    _clearExistingAnimationTimer();
                    if (currentVisibility == ModalVisibilityState.AnimatingOpen)
                    {
                        isAnimating = true;
                        animationRenderStart = true;
                        _animateTo(ModalVisibilityState.Open);
                    }
                    else if (currentVisibility == ModalVisibilityState.AnimatingClosed)
                    {
                        isAnimating = true;
                        _animateTo(ModalVisibilityState.Closed);
                    }
                }
            }

            await base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _jsAvailable = true;
                // 27 is Escape code
                _keydownRegistration = await JSRuntime.InvokeAsync<string>("FluentUIBaseComponent.registerWindowKeyDownEvent", DotNetObjectReference.Create(this), "27", "ProcessKeyDown");
            }
            
            await base.OnAfterRenderAsync(firstRender);
        }

        

        [JSInvokable]
        public void ProcessKeyDown(string keyCode)
        {
            if (keyCode == "27")
                OnDismiss.InvokeAsync(null);
        }

        protected override bool ShouldRender()
        {
            if (isAnimating && !animationRenderStart)
                return false;
            else
            {
                animationRenderStart = false;
                return true;
            }
        }
               
        private bool GetDelayedIsOpened()
        {

            //System.Timers.Timer timer = new System.Timers.Timer();
            //timer.Interval = 16;
            //timer.Elapsed

             return IsOpen;
        }

        public async void Dispose()
        {
            _clearExistingAnimationTimer();
            if (_keydownRegistration != null)
            {
                await JSRuntime.InvokeVoidAsync("FluentUIBaseComponent.deregisterWindowKeyDownEvent", _keydownRegistration);
                _keydownRegistration = null;
            }
        }
    }
}
