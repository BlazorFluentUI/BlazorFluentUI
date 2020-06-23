using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BlazorFluentUI
{
    public partial class BFUDocumentCard : BFUComponentBase
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }
        [Parameter]
        public bool Compact { get; set; }

        [Inject]
        private IJSRuntime jSRuntime { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
        [Parameter] public EventCallback<KeyboardEventArgs> OnKeyDown { get; set; }

        [Parameter] public string? OnClickHref { get; set; }
        [Parameter] public string? OnClickTarget { get; set; }
        public string? BaseClassName { get; set; }

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
        }

        private void SetActionable()
        {
            Actionable = (!string.IsNullOrWhiteSpace(OnClickHref) || OnClick.HasDelegate);
        }

        private void SetBaseClassName()
        {
            BaseClassName = $"{GlobalClassNames["root"]} {(Compact ? GlobalClassNames["rootCompact"] : "")} {(Actionable ? GlobalClassNames["rootActionable"] : "")}";
        }

        private async void KeyDownHandler(KeyboardEventArgs keyboardEventArgs)
        {
            if (keyboardEventArgs.Code == "Space" || keyboardEventArgs.Code == "Enter")
            {
                await OnKeyDown.InvokeAsync(keyboardEventArgs).ConfigureAwait(false);
            }
        }

        private async void OnClickHandler(MouseEventArgs mouseEventArgs)
        {
            if (OnClick.HasDelegate)
            {
                await OnClick.InvokeAsync(mouseEventArgs).ConfigureAwait(false);
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

        public ICollection<IRule> CreateGlobalCss(ITheme theme)
        {
            var documentCardRules = new HashSet<IRule>();

            documentCardRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = ".ms-DocumentCard" },
                Properties = new CssString()
                {
                    Css = "WebkitFontSmoothing:antialiased;" +
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
                    Css = "content:'\" \"';" +
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
                Selector = new CssStringSelector() { SelectorName = $".ms-DocumentCard--compact.{BFUDocumentCardPreview.GlobalClassNames["root"]}" },
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
                Selector = new CssStringSelector() { SelectorName = $".ms-DocumentCard--compact.{BFUDocumentCardPreview.GlobalClassNames["icon"]}" },
                Properties = new CssString()
                {
                    Css = "max-height: 32px;" +
                          "max-width: 32px;"
                }
            });

            documentCardRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $".ms-DocumentCard--compact.{BFUDocumentCardActivity.GlobalClassNames["root"]}" },
                Properties = new CssString()
                {
                    Css = "padding-bottom: 12px"
                }
            });

            documentCardRules.Add(new Rule()
            {
                Selector = new CssStringSelector() { SelectorName = $".ms-DocumentCard--compact.{BFUDocumentCardTitle.GlobalClassNames["root"]}" },
                Properties = new CssString()
                {
                    Css = "padding-bottom: 12px 16px 8px 16px;" +
                          $"font-size: {theme.FontStyle.FontSize.Medium};" +
                          "line-height: 16px;"
                }
            });

            return documentCardRules;
        }
    }
}
