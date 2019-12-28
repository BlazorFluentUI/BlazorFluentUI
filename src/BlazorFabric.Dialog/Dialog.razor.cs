using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public partial class Dialog : FabricComponentBase
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

        public Dialog()
        {
            Id = Guid.NewGuid().ToString();
            DefaultTitleTextId = Id = "-title";
            DefaultSubTextId = Id = "-subText";
        }
    }
}
