using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public partial class BFUFontIcon : BFUComponentBase
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
