using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI
{
    public partial class Callout : FluentUIComponentBase
    {
        //[Inject] private IComponentContext ComponentContext { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public ElementReference ElementTarget { get; set; }  // not working yet
        [Parameter] public FluentUIComponentBase FabricComponentTarget { get; set; }

        [Parameter] public DirectionalHint DirectionalHint { get; set; } = DirectionalHint.BottomAutoEdge;
        [Parameter] public bool DirectionalHintFixed { get; set; }
        [Parameter] public bool DoNotLayer { get; set; }
        [Parameter] public bool IsBeakVisible { get; set; } = true;
        [Parameter] public int GapSpace { get; set; } = 0;
        [Parameter] public int BeakWidth { get; set; } = 16;
        [Parameter] public int CalloutWidth { get; set; } = 0;
        [Parameter] public int CalloutMaxHeight { get; set; } = 0;
        [Parameter] public int CalloutMaxWidth { get; set; } = 0;
        [Parameter] public string BackgroundColor { get; set; } = null;
        [Parameter] public Rectangle Bounds { get; set; }
        [Parameter] public int MinPagePadding { get; set; } = 8;
        [Parameter] public bool PreventDismissOnScroll { get; set; } = false;
        [Parameter] public bool PreventDismissOnResize { get; set; } = false;
        [Parameter] public bool PreventDismissOnLostFocus { get; set; } = false;
        [Parameter] public bool CoverTarget { get; set; } = false;
        [Parameter] public bool AlignTargetEdge { get; set; } = false;
        [Parameter] public string Role { get; set; }

        [Parameter] public bool SetInitialFocus { get; set; }


        [Parameter] public bool HideOverflow { get; set; } = false;

        [Parameter] public EventCallback<bool> HiddenChanged { get; set; }
        [Parameter] public EventCallback OnDismiss { get; set; }
        [Parameter] public EventCallback<CalloutPositionedInfo> OnPositioned { get; set; }
        //protected Layer layerReference;
    }
}
