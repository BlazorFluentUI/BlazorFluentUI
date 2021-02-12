using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace FluentUI
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
        internal IJSRuntime? jSRuntime { get; set; }

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

        public static Dictionary<string, string> GlobalClassNames = new Dictionary<string, string>()
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
                    await jSRuntime.InvokeVoidAsync("open", OnClickHref, OnClickTarget, "noreferrer noopener nofollow").ConfigureAwait(false);
                }
                else
                {
                    await jSRuntime.InvokeVoidAsync("open", OnClickHref, "_blank", "noreferrer nofollow").ConfigureAwait(false);
                }
            }
        }

        private async void MouseClickHandler(MouseEventArgs mouseEventArgs)
        {
            await OnClickHandler().ConfigureAwait(false);
        }

        public ICollection<IRule> CreateGlobalCss(ITheme theme)
        {
            var documentCardRules = new HashSet<IRule>();

            documentCardRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DocumentCard" },
                Properties = new CssString()
                {
                    Css = "webkit-font-smoothing:antialiased;" +
                          $"background-color:{theme.Palette.White};" +
                          $"border:1px solid {theme.Palette.NeutralLight};" +
                          "max-width:320px;" +
                          "min-width:240px;" +
                          "user-select: none;" +
                          "position: relative;"
                }
            });

            documentCardRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $".{GlobalClassNames["rootActionable"]}:hover" },
                Properties = new CssString()
                {
                    Css = "cursor:pointer;" +
                          $"border-color:{theme.Palette.NeutralTertiaryAlt}"
                }
            });            
            documentCardRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $".{GlobalClassNames["rootActionable"]}:hover:after" },
                Properties = new CssString()
                {
                    Css = "content:' ';" +
                          $"position: absolute;" +
                          "top: 0;" +
                          "right: 0;" +
                          "bottom: 0;" +
                          "left: 0;" +
                          $"border: 1px solid ${theme.Palette.NeutralTertiaryAlt};" +
                          "pointer-events: none"
                }
            });

            documentCardRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DocumentCard--compact" },
                Properties = new CssString()
                {
                    Css = "display: flex;" +
                          "max-width:480px;" +
                          "height:108px;"
                }
            });
            documentCardRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $".ms-DocumentCard--compact.{DocumentCardPreview.GlobalClassNames["root"]}" },
                Properties = new CssString()
                {
                    Css = $"border-right:1px solid {theme.Palette.NeutralLight};" +
                          "border-bottom:0;" +
                          "max-height: 106px;" +
                          "max-width:144px;"
                }
            });
            documentCardRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $".ms-DocumentCard--compact.{DocumentCardPreview.GlobalClassNames["icon"]}" },
                Properties = new CssString()
                {
                    Css = "max-height: 32px;" +
                          "max-width: 32px;"
                }
            });

            documentCardRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $".ms-DocumentCard--compact.{DocumentCardActivity.GlobalClassNames["root"]}" },
                Properties = new CssString()
                {
                    Css = "padding-bottom: 12px"
                }
            });

            documentCardRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $".ms-DocumentCard--compact.{DocumentCardTitle.GlobalClassNames["root"]}" },
                Properties = new CssString()
                {
                    Css = "padding-bottom: 12px 16px 8px 16px;" +
                          $"font-size: {theme.FontStyle.FontSize.Medium};" +
                          "line-height: 16px;"
                }
            });

            return documentCardRules;
        }

        public async Task Focus()
        {
            await jSRuntime.InvokeVoidAsync("FluentUIBaseComponent.focusElement", RootElementReference).ConfigureAwait(false);
        }
    }
}
