using Microsoft.AspNetCore.Components;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class GlobalRules
    {
        [Inject]
        public IComponentStyle ComponentStyle { get; set; }

        protected override Task OnInitializedAsync()
        {
            ComponentStyle.GlobalCSRules.CollectionChanged += UpdateComponent;
            return base.OnInitializedAsync();
        }

        private void UpdateComponent(object sender, NotifyCollectionChangedEventArgs e)
        {
                InvokeAsync(() => StateHasChanged());
        }

    }
}
