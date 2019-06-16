using BlazorFabric.BaseComponent;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric.Dropdown
{
    public class DropdownBase : FabricComponentBase
    {
        [Parameter] protected string AriaLabel { get; set; }
        [Parameter] protected bool Disabled { get; set; }
        [Parameter] protected int DropdownWidth { get; set; } = 0;
        [Parameter] protected string ErrorMessage { get; set; }
        [Parameter] protected bool IsOpen { get; set; }
        [Parameter] protected string Label { get; set; }
        [Parameter] protected bool Required { get; set; }
        [Parameter] protected ResponsiveMode ResponsiveMode { get; set; }

        protected string id = Guid.NewGuid().ToString();
        protected bool isSmall = false;

        protected List<string> selectedOptions = new List<string>();
    }
}
