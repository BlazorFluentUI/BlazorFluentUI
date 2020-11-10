using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class BFUFontIcon : BFUComponentBase
    {
        private string icon;

        [Parameter] 
        public string IconName { get; set; }

        [Parameter(CaptureUnmatchedValues = true)] 
        public Dictionary<string, object> ExtraParameters { get; set; }

        protected override Task OnParametersSetAsync()
        {
            MappedFontIcons.Icons.TryGetValue(IconName, out icon);

            return base.OnParametersSetAsync();
        }
    }
}
