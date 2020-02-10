using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public class GlobalCS : ComponentBase, IGlobalCSSheet, IDisposable
    {
        private ICollection<Rule> rules;
        private bool shouldRender;

        [Inject]
        public IComponentStyle ComponentStyle { get; set; }

        [Parameter]
        public ICollection<Rule> Rules
        {
            get => rules;
            set
            {
                if (value == rules)
                {
                    return;
                }
                rules = value;
                shouldRender = true;
                //RulesChanged.InvokeAsync(value);
            }
        }

        public void Dispose()
        {
            ComponentStyle.GlobalCSSheets.Remove(this);
            ComponentStyle.UpdateSubscribers();
        }

        protected override Task OnInitializedAsync()
        {
            ComponentStyle.GlobalCSSheets.Add(this);
            return base.OnInitializedAsync();
        }

        protected override bool ShouldRender()
        {
            if (shouldRender)
            {
                shouldRender = false;
                return true;
            }
            return false;
            //return base.ShouldRender();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            ComponentStyle.UpdateSubscribers();
            base.OnAfterRender(firstRender);
        }

    }
}

