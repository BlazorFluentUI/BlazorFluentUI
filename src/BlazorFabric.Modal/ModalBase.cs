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

        protected ElementReference allowScrollOnModal;
    }
}
