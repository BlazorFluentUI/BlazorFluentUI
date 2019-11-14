using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public class MessageBarBase : FabricComponentBase
    {
        [Parameter]
        public bool IsMultiline { get; set; }

        [Parameter]
        public MessageBarType MessageBarType { get; set; } = MessageBarType.Info;

        [Parameter]
        public bool Truncated { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }


        protected override Task OnInitializedAsync()
        {


            return base.OnInitializedAsync();
        }

    }
}
