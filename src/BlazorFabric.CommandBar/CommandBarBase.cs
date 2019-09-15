using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public class CommandBarBase : ComponentBase
    {

        [Parameter] public RenderFragment Items { get; set; }
        [Parameter] public RenderFragment OverflowItems { get; set; }
        [Parameter] public RenderFragment FarItems { get; set; }



    }
}
