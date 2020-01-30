using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public class StaticCS : ComponentBase, IStaticCSSheet, IDisposable
    {
        private ICollection<Rule> rules;

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
                //RulesChanged.InvokeAsync(value);
            }
        }

        public void Dispose()
        {
            ComponentStyle.StaticCSSheets.Remove(this);
            ComponentStyle.UpdateSubscribers();
        }

        protected override Task OnInitializedAsync()
        {
            ComponentStyle.StaticCSSheets.Add(this);
            ComponentStyle.UpdateSubscribers();
            return base.OnInitializedAsync();
        }

        
    }
}

