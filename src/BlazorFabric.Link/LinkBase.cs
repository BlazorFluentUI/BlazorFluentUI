using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public class LinkBase : FabricComponentBase
    {
        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public string Href{ get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }


    }
}
