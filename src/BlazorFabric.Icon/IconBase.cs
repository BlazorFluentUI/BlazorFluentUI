
using BlazorFabric.BaseComponent;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric.Icon
{
    public class IconBase : FabricComponentBase
    {
        [Parameter] protected string IconName { get; set; }
        [Parameter] protected string AriaLabel { get; set; }
        [Parameter] protected IconType IconType { get; set; }
        //[Parameter] protected bool IsPlaceholder { get; set; }


    }
}
