using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public class MessageBarBase : FabricComponentBase
    {
        [Parameter]
        public bool IsMultiline { get; set; } = true;

        [Parameter]
        public MessageBarType MessageBarType { get; set; } = MessageBarType.Info;

        [Parameter]
        public bool Truncated { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public RenderFragment Actions { get; set; }

        [Parameter]
        public string DismissButtonAriaLabel { get; set; }

        [Parameter]
        public string OverflowButtonAriaLabel { get; set; }

        [Parameter]
        public EventCallback OnDismiss { get; set; }

        protected bool HasDismiss { get => (OnDismiss.HasDelegate); }

        protected bool HasExpand { get => (Truncated && Actions == null); }

        protected bool ExpandSingelLine { get; set; }

        protected void Truncate()
        {
            ExpandSingelLine = !ExpandSingelLine;
        }

    }
}
