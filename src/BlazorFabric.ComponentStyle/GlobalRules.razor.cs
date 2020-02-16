using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class GlobalRules : IDisposable
    {
        [Inject]
        public IComponentStyle ComponentStyle { get; set; }
        private bool _isDisposed = false;

        protected override Task OnInitializedAsync()
        {
            ComponentStyle.GlobalCSRules.CollectionChanged += UpdateComponent;
            return base.OnInitializedAsync();
        }

        private void UpdateComponent(object sender, NotifyCollectionChangedEventArgs e)
        {   
            if(!_isDisposed)
                InvokeAsync(() => StateHasChanged());
        }

        public void Dispose()
        {
            _isDisposed = true;
        }
    }
}
