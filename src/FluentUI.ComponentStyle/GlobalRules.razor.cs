using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace FluentUI
{
    public partial class GlobalRules : IDisposable
    {
        [Inject]
        public IComponentStyle ComponentStyle { get; set; }
        public Action OnDispose { get; set; }
        private bool _isDisposed = false;

        protected override Task OnInitializedAsync()
        {
            ComponentStyle.GlobalRules = this;
            ComponentStyle.SetDisposedAction();
            return base.OnInitializedAsync();
        }

        public void UpdateGlobalRules()
        {
            if(!_isDisposed)
                InvokeAsync(() => StateHasChanged());
        }

        public void Dispose()
        {
            _isDisposed = true;
            OnDispose?.Invoke();
        }
    }
}
