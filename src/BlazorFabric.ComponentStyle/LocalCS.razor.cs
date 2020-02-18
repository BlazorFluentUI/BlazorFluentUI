using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Reflection;
using System;

namespace BlazorFabric
{
    public partial class LocalCS : ComponentBase, ILocalCSSheet, IDisposable
    {
        private string css;
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
                RulesChanged.InvokeAsync(value);
            }
        }

        [Parameter] public EventCallback<ICollection<Rule>> RulesChanged { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ComponentStyle.LocalCSSheets.Add(this);
            SetSelectorNames();
            await base.OnInitializedAsync();
        }

        protected override void OnParametersSet()
        {
            css = "";
            foreach(var rule in rules)
            {
                css += ComponentStyle.PrintRule(rule);
            }
            base.OnParametersSet();

        }

        //Private functionality

        private void SetSelectorNames()
        {
            foreach (var rule in rules)
            {
                if (rule.Selector.GetType() == typeof(IdSelector))
                    continue;
                if (string.IsNullOrWhiteSpace(rule.Selector.SelectorName))
                {
                    rule.Selector.SelectorName = $"css-{ComponentStyle.LocalCSSheets.ToList().IndexOf(this)}-{rules.ToList().IndexOf(rule)}";
                }
                else
                {
                    rule.Selector.SelectorName = $"{rule.Selector.SelectorName}-{ComponentStyle.LocalCSSheets.ToList().IndexOf(this)}-{rules.ToList().IndexOf(rule)}";
                }
            }
            RulesChanged.InvokeAsync(rules);
        }

        public void Dispose()
        {
            ComponentStyle.LocalCSSheets.Remove(this);
        }
    }
}