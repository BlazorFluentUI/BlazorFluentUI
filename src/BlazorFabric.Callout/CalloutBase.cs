using BlazorFabric.BaseComponent;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric.Callout
{
    public class CalloutBase : FabricComponentBase
    {
        internal CalloutBase() { }

        [Parameter] protected RenderFragment ChildContent { get; set; }
        [Parameter] protected ElementRef ElementTarget { get; set; }  // not working yet
        [Parameter] protected FabricComponentBase FabricComponentTarget { get; set; }

        [Parameter] protected bool DoNotLayer { get; set; }
        [Parameter] protected bool IsBeakVisible { get; set; } = true;
        [Parameter] protected int GapSpace { get; set; } = 0;
        [Parameter] protected int BeakWidth { get; set; } = 16;
        [Parameter] protected int CalloutWidth { get; set; } = 0;
        [Parameter] protected int CalloutMaxWidth { get; set; } = 0;
        [Parameter] protected string BackgroundColor { get; set; } = null;
        [Parameter] protected string Bounds { get; set; } = "????";
        [Parameter] protected int MinPagePadding { get; set; } = 8;
        [Parameter] protected bool PreventDismissOnScroll { get; set; } = false;
        [Parameter] protected bool PreventDismissOnResize { get; set; } = false;
        [Parameter] protected bool PreventDismissOnLosFocus { get; set; } = false;
        [Parameter] protected bool CoverTarget { get; set; } = false;
        [Parameter] protected bool AlignTargetEdge { get; set; } = false;
        [Parameter] protected string Role { get; set; }
        [Parameter] protected string AriaLabel { get; set; }
        [Parameter] protected string AriaLabelledBy { get; set; }
        [Parameter] protected string AriaDescribedBy { get; set; }
        [Parameter] protected bool Hidden { get; set; } = false;

        [Parameter] protected Rectangle Position { get; set; } = new Rectangle();

        protected int contentMaxHeight = 0;
        protected bool overflowYHidden = false;

        protected bool isMeasured = false;

        protected override async Task OnAfterRenderAsync()
        {
            if (!isMeasured)
            {
                
            }
            await base.OnAfterRenderAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (this.FabricComponentTarget != null && !isMeasured)
            {
                Rectangle targetBounds = null;
                targetBounds = await this.FabricComponentTarget.GetBoundsAsync();
                this.Position = targetBounds;

                isMeasured = true;
                StateHasChanged();
            }
            await base.OnParametersSetAsync();
        }
    }
}
