using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace FluentUI
{
    public partial class DialogContent : FluentUIComponentBase
    {
        [Parameter] public string CloseButtonAriaLabel { get; set; } = "Close";  //need to localize
        [Parameter] public RenderFragment ContentTemplate { get; set; }
        [Parameter] public DialogType DialogType { get; set; } = DialogType.Normal;
        [Parameter] public string DraggableHeaderClassName { get; set; }
        [Parameter] public RenderFragment FooterTemplate { get; set; }
        [Parameter] public bool IsMultiline { get; set; }
        [Parameter] public EventCallback<EventArgs> OnDismiss { get; set; }
        [Parameter] public bool ShowCloseButton { get; set; }
        [Parameter] public string SubText { get; set; }
        [Parameter] public string SubTextId { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public string TitleId { get; set; }



    }
}
