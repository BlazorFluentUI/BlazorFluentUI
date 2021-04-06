using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BlazorFluentUI
{
    public enum DocumentCardType
    {
        /// <summary>
        /// Standard DocumentCard.
        /// </summary>
        Normal = 0,
        /// <summary>
        /// Compact layout. Displays the preview beside the details, rather than above.
        /// </summary>
        Compact = 1
    }

    public partial class DocumentCard : FluentUIComponentBase
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Compact layout. Displays the preview beside the details, rather than above.
        /// </summary>
        [Parameter]
        public DocumentCardType Type { get; set; }

        /// <summary>
        /// Function to call when the card is clicked or keyboard Enter/Space is pushed.
        /// </summary>
        [Parameter] public EventCallback OnClick { get; set; }

        /// <summary>
        /// A URL to navigate to when the card is clicked. If a function has also been provided, it will be used instead of the URL.
        /// </summary>
        [Parameter] public string? OnClickHref { get; set; }

        /// <summary>
        /// A target browser context for opening the link. If not specified, will open in the same tab/window.
        /// </summary>
        [Parameter] public string? OnClickTarget { get; set; }

        [Inject]
        internal IJSRuntime? JSRuntime { get; set; }

        private const string scriptPath = "./_content/BlazorFluentUI.CoreComponents/documentCard.js";
        private IJSObjectReference? scriptModule;

        [Inject]
        internal NavigationManager? NavigationManager { get; set; }

        public string? BaseClassName { get; set; }

        /// <summary>
        /// True when the card has a click action.
        /// </summary>
        /// <value>
        ///   <c>true</c> if actionable; otherwise, <c>false</c>.
        /// </value>
        public bool Actionable { get; set; }

        public static Dictionary<string, string> GlobalClassNames = new()
        {
            {"root", "ms-DocumentCard"},
            {"rootActionable", "ms-DocumentCard--actionable"},
            {"rootCompact", "ms-DocumentCard--compact"}
        };

        protected override void OnInitialized()
        {
            SetBaseClassName();
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            SetActionable();
            SetBaseClassName();
            base.OnParametersSet();
            if (!string.IsNullOrWhiteSpace(AriaRoleDescription))
            {
                // Aria role assigned to the documentCard (Eg. button, link).
                AriaRoleDescription = OnClick.HasDelegate ? "button" : "link";
            }
        }

        private void SetActionable()
        {
            Actionable = (!string.IsNullOrWhiteSpace(OnClickHref) || OnClick.HasDelegate);
        }

        private void SetBaseClassName()
        {
            BaseClassName = $"{GlobalClassNames["root"]} {(Type == DocumentCardType.Compact ? GlobalClassNames["rootCompact"] : "")} {(Actionable ? GlobalClassNames["rootActionable"] : "")}";
        }

        private async void KeyDownHandler(KeyboardEventArgs keyboardEventArgs)
        {
            if (keyboardEventArgs.Code == "Space" || keyboardEventArgs.Code == "Enter")
            {
                await OnClickHandler().ConfigureAwait(false);
            }
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            scriptModule = await JSRuntime!.InvokeAsync<IJSObjectReference>("import", scriptPath);
        }

        private async Task OnClickHandler()
        {
            if (OnClick.HasDelegate)
            {
                await OnClick.InvokeAsync(null).ConfigureAwait(false);
            }
            else if (!OnClick.HasDelegate && !string.IsNullOrWhiteSpace(OnClickHref))
            {
                if (!string.IsNullOrWhiteSpace(OnClickTarget))
                {
                    await scriptModule!.InvokeVoidAsync("open", OnClickHref, OnClickTarget, "noreferrer noopener nofollow").ConfigureAwait(false);
                }
                else
                {
                    await scriptModule!.InvokeVoidAsync("open", OnClickHref, "_blank", "noreferrer nofollow").ConfigureAwait(false);
                }
            }
        }

        private async void MouseClickHandler(MouseEventArgs mouseEventArgs)
        {
            await OnClickHandler().ConfigureAwait(false);
        }

        public async Task Focus()
        {
            await RootElementReference.FocusAsync();
            //await JSRuntime.InvokeVoidAsync("FluentUIBaseComponent.focusElement", RootElementReference).ConfigureAwait(false);
        }
    }
}
