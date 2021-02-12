using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class LayerPortal : FluentUIComponentBase, IDisposable
    {
        [Parameter] public RenderFragment? ChildContent { get; set; }
        [Parameter] public string? Id { get; set; }
        [Parameter] public ElementReference? VirtualParent { get; set; }
        [Parameter] public bool IsFixed { get; set; } = true;

        [Inject] private IJSRuntime? JSRuntime { get; set; }

        //protected Dictionary<string, (RenderFragment fragment, bool isFixed)> fragments = new Dictionary<string, (RenderFragment fragment, bool isFixed)>();


        protected bool shouldRender = false;

        protected override bool ShouldRender()
        {
            if (shouldRender)
            {
                shouldRender = false;
                return true;
            }
            return false;
        }

        public void Rerender()
        {
            shouldRender = true;
            InvokeAsync(StateHasChanged);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {

            await base.OnAfterRenderAsync(firstRender);
        }



        public void Dispose()
        {

        }
    }
}
