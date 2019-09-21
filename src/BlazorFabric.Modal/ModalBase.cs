using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public class ModalBase: FabricComponentBase
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
        public bool ForceFocusInsideTrap { get; set; }

        [Parameter]
        public string FirstFocusableSelector { get; set; }

        [Parameter]
        public string CloseButtonAriaLabel { get; set; }

        [Parameter]
        public bool IsClickableOutsideFocusTrap { get; set; }

        protected bool _isOpenDelayed = false;

        protected ElementReference allowScrollOnModal;

        protected bool GetDelayedIsOpened()
        {

            //System.Timers.Timer timer = new System.Timers.Timer();
            //timer.Interval = 16;
            //timer.Elapsed

             return IsOpen;
        }
    }
}
