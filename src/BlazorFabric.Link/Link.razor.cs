using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class Link : FabricComponentBase
    {
        [Parameter]
        public LinkType? Type { get; set; }

        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public string Href { get; set; }

        [Parameter]
        public string Target { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }


    }
}
