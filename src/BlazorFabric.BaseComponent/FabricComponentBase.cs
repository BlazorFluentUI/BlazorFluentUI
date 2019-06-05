using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric.BaseComponent
{
    public class FabricComponentBase: ComponentBase
    {
        [Parameter] protected string ClassName { get; set; }
    }
}
