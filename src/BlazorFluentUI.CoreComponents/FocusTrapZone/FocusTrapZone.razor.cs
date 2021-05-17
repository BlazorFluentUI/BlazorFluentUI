using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorFluentUI
{
    public partial class FocusTrapZone : FluentUIComponentBase, IAsyncDisposable
    {
        [Inject]
        private IJSRuntime? JSRuntime { get; set; }
        private const string ScriptPath = "./_content/BlazorFluentUI.CoreComponents/focusTrapZone.js";
        private IJSObjectReference? scriptModule;

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
        private DotNetObjectReference<FocusTrapZone>? selfReference;
        private int _id = -1;


        public async Task FocusAsync()
        {
            if (_id != -1)
                await scriptModule!.InvokeVoidAsync("focus", _id);
        }

        protected override async Task OnParametersSetAsync()
        {
            if (_id != -1)
            {
                try
                {
                    FocusTrapZoneProps? props = new(this, _firstBumper, _lastBumper);
                    await scriptModule!.InvokeVoidAsync("updateProps", _id, props);
                }
                catch { }
            }

            await base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (scriptModule == null)
                scriptModule = await JSRuntime!.InvokeAsync<IJSObjectReference>("import", ScriptPath);

            if (firstRender)
            {
                RegisterFocusTrapZone();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async void RegisterFocusTrapZone()
        {
            FocusTrapZoneProps? props = new(this, _firstBumper, _lastBumper);

            selfReference = DotNetObjectReference.Create(this);
            try
            {
                _id = await scriptModule!.InvokeAsync<int>("register", cancellationTokenSource.Token, props, selfReference);
            }
            catch { }
        }


        public override async ValueTask DisposeAsync()
        {
            try
            {
                if (_id != -1 && scriptModule != null)
                {
                    await scriptModule.InvokeVoidAsync("unregister", _id);
                    await scriptModule.DisposeAsync();
                }
                selfReference?.Dispose();

                await base.DisposeAsync();
            }
            catch (TaskCanceledException)
            {
            }
        }
    }
}

