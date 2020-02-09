using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class GlobalRules : IGlobalRules, IDisposable
    {
        [Inject]
        public IComponentStyle ComponentStyle { get; set; }

        private bool isRendered;
        private bool isDisposed;

        public void Update()
        {
            if(isRendered && !isDisposed)
                InvokeAsync(() => StateHasChanged());
        }

        protected override Task OnInitializedAsync()
        {
            ComponentStyle.Subscribe(this);
            return base.OnInitializedAsync();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
                isRendered = true;
            base.OnAfterRender(firstRender);
        }

        public void Dispose()
        {
            isDisposed = true;
        }
    }
}
