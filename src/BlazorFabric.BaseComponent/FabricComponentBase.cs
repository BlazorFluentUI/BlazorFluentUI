using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric.BaseComponent
{
    public class FabricComponentBase: ComponentBase
    {
        [Inject] private IComponentContext ComponentContext { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }

        [Parameter] protected string ClassName { get; set; }
        [Parameter] protected string Style { get; set; }

        public ElementReference RootElementReference;

        public async Task<Rectangle> GetBoundsAsync()
        {
            if (ComponentContext.IsConnected)
            {
                var rectangle = await JSRuntime.InvokeAsync<Rectangle>("BlazorFabricBaseComponent.measureElementRect", RootElementReference);
                return rectangle;
            }
            else
                return new Rectangle();
        }
                

    }
}
