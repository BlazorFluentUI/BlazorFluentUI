using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public class GlobalCS : ComponentBase, IGlobalCSSheet, IDisposable
    {
        [Inject]
        public IComponentStyle ComponentStyle { get; set; }

        [Parameter]
        public object Component { get; set; }

        [Parameter]
        public bool ReloadStyle { get; set; } = false;

        [Parameter]
        public Func<ICollection<Rule>> CreateGlobalCss { get; set; }

        [Parameter]
        public EventCallback<bool> ReloadStyleChanged { get; set; }

        public bool IsGlobal { get; set; }

        public void Dispose()
        {
            ComponentStyle.GlobalCSSheets.Remove(this);
        }

        protected override Task OnInitializedAsync()
        {
            ComponentStyle.GlobalCSSheets.Add(this);
            return base.OnInitializedAsync();
        }

        protected override void OnParametersSet()
        {
            if (ReloadStyle && IsGlobal)
            {
                ReloadStyle = false;
                ReloadStyleChanged.InvokeAsync(false);
                ComponentStyle.ItemsChanged(this);
            }
            base.OnParametersSet();
        }
    }
}

