using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public partial class GlobalRules : IGlobalRules
    {
        private bool isRendered;
        public void Update()
        {
            if(isRendered)
                StateHasChanged();
        }

        protected override Task OnInitializedAsync()
        {
            ComponentStyle.GlobalRules = this;
            return base.OnInitializedAsync();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
                isRendered = true;
            base.OnAfterRender(firstRender);
        }
    }
}
