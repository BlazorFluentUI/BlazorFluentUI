using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class FontIcon : FabricComponentBase
    {
        private string icon;

        [Parameter] public string IconName { get; set; }

        protected override Task OnParametersSetAsync()
        {
            MappedFontIcons.Icons.TryGetValue(IconName, out icon);

            return base.OnParametersSetAsync();
        }
    }
}
