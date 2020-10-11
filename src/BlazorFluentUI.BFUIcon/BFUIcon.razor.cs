using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BlazorFluentUI
{
    public partial class BFUIcon : BFUComponentBase
    {
        [Parameter] public string IconName { get; set; }
        [Parameter] public IconType IconType { get; set; }
              
    }
}
