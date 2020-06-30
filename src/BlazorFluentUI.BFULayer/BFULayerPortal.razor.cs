using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class BFULayerPortal : BFUComponentBase, IDisposable
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

        //public async Task SetVirtualParentAsync(ElementReference parent)
        //{
        //    await JSRuntime.InvokeVoidAsync("BlazorFluentUiBaseComponent.setVirtualParent", RootElementReference, parent);
        //}

        //public void SetChildContent(string layerId, RenderFragment renderFragment, bool isFixed)
        //{
        //    if (fragments.ContainsKey(layerId))
        //    {
        //        fragments[layerId] = (renderFragment, isFixed);
        //    }
        //    else
        //    {
        //        fragments.Add(layerId, (renderFragment, isFixed));
        //    }
        //    //this.ChildContent = renderFragment;
        //    //this.IsFixed = isFixed;
        //    Rerender();
        //}

        //public void RemoveChildContent(string layerId)
        //{
        //    fragments.Remove(layerId);
        //    //this.ChildContent = null;
        //    Rerender();
        //}

        public void Dispose()
        {
            
        }
    }
}
