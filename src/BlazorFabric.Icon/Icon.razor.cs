using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class Icon : FabricComponentBase
    {
        [Parameter] public string IconName { get; set; }
        //[Parameter] public string AriaLabel { get; set; }
        [Parameter] public IconType IconType { get; set; }
        //[Parameter] protected bool IsPlaceholder { get; set; }


    }
}
