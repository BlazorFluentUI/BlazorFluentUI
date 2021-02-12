using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace FluentUI
{
    public partial class Dialog : FluentUIComponentBase
    {
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public string ContainerClass { get; set; }
        [Parameter] public string ContentClass { get; set; }
        [Parameter] public DialogType DialogType { get; set; } = DialogType.Normal;
        [Parameter] public string DraggableHeaderClassName { get; set; }
        [Parameter] public RenderFragment FooterTemplate { get; set; }
        [Parameter] public bool IsBlocking { get; set; }
        [Parameter] public bool IsDarkOverlay { get; set; } = false;
        [Parameter] public bool IsMultiline { get; set; }
        [Parameter] public bool IsOpen { get; set; }
        [Parameter] public bool IsModeless { get; set; }
        [Parameter] public EventCallback<EventArgs> OnDismiss { get; set; }
        [Parameter] public string SubText { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public decimal MaxWidth { get; set; } = 340;
        [Parameter] public decimal MinWidth { get; set; } = 288;

        // from IAccessiblePopupProps
        [Parameter]
        public ElementReference ElementToFocusOnDismiss { get; set; }

        [Parameter]
        public bool IgnoreExternalFocusing { get; set; }

        [Parameter]
        public bool ForceFocusInsideTrap { get; set; }

        [Parameter]
        public string FirstFocusableSelector { get; set; }

        [Parameter]
        public string CloseButtonAriaLabel { get; set; }

        [Parameter]
        public bool IsClickableOutsideFocusTrap { get; set; }

        protected string Id;
        protected string DefaultTitleTextId;
        protected string DefaultSubTextId;
        private string _defaultMinWidth;
        private string _defaultMaxWidth;
        private ICollection<IRule> DialogLocalRules { get; set; } = new List<IRule>();
        private Rule DialogMainRule = new Rule();
        private Rule DialogMainMediaRule = new Rule();


        public Dialog()
        {
            Id = Guid.NewGuid().ToString();
            DefaultTitleTextId = Id = "-title";
            DefaultSubTextId = Id = "-subText";
        }

        protected override void OnInitialized()
        {
            _defaultMinWidth = $"{MinWidth.ToString().Replace(",", ".")}px";
            _defaultMaxWidth = $"{MaxWidth.ToString().Replace(",", ".")}px";
            CreateLocalCss();
            base.OnInitialized();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
                SetStyle();
            base.OnAfterRender(firstRender);
        }

        protected override void OnThemeChanged()
        {
            SetStyle();
        }

        private void CreateLocalCss()
        {
            DialogMainRule.Selector = new ClassSelector() { SelectorName = "ms-Dialog-main" };
            DialogMainMediaRule.Selector = new MediaSelector() { SelectorName = $"@media (min-width:{Theme.CommonStyle.ScreenWidthMinMedium}px)" };
            DialogLocalRules.Add(DialogMainRule);
            DialogLocalRules.Add(DialogMainMediaRule);
        }

        private void SetStyle()
        {
            DialogMainRule.Properties = new CssString()
            {
                Css = $"width:{_defaultMinWidth};" +
                            $"outline:3px solid transparent;" +
                            $"display:flex;"
            };

            DialogMainMediaRule.Selector = new MediaSelector() { SelectorName = $"@media (min-width:{Theme.CommonStyle.ScreenWidthMinMedium}px)" };
            DialogMainMediaRule.Properties = new CssString()
            {
                Css = $".{DialogMainRule.Selector.SelectorName}{{width:auto;max-width:{_defaultMaxWidth};min-width:{_defaultMinWidth};}}"
            };
        }
    }
}
