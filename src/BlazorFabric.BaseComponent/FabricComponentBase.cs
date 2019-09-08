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
        //[Inject] private IComponentContext ComponentContext { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }

        [Parameter] public string ClassName { get; set; }
        [Parameter] public string Style { get; set; }

        public ElementReference RootElementReference;

        public async Task<Rectangle> GetBoundsAsync()
        {
            //if (ComponentContext.IsConnected)
            //{
            try
            {
                var rectangle = await JSRuntime.InvokeAsync<Rectangle>("BlazorFabricBaseComponent.measureElementRect", RootElementReference);
                return rectangle;
            }
            catch
            {
                return new Rectangle();
            }
        }
                

    }
}
