using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public partial class SelectionZone<TItem> : FabricComponentBase
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public bool DisableAutoSelectOnInputElements { get; set; }

        [Parameter]
        public bool EnterModalOnTouch { get; set; }

        [Parameter]
        public bool IsSelectedOnFocus { get; set; } = true;

        [Parameter]
        public EventCallback<(TItem item, int index)> OnItemContextMenu { get; set; }

        [Parameter]
        public EventCallback<(TItem item,int index)> OnItemInvoked { get; set; }

        [Parameter]
        public Selection<TItem> Selection { get; set; }

        [Parameter]
        public SelectionMode SelectionMode { get; set; }

        [Parameter]
        public bool SelectionPreservedOnEmptyClick { get; set; }


    }
}
