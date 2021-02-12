using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Reflection;
using System;
using System.Text.Encodings.Web;

namespace FluentUI
{
    public partial class LocalCS : ComponentBase, ILocalCSSheet, IDisposable
    {
        private string css;
        private ICollection<IRule> rules;

        [Inject]
        public IComponentStyle ComponentStyle { get; set; }

        [Parameter]
        public ICollection<IRule> Rules
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

        [Parameter] public EventCallback<ICollection<IRule>> RulesChanged { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ComponentStyle.LocalCSSheets.Add(this);
            SetSelectorNames();
            await base.OnInitializedAsync();
        }

        protected override void OnParametersSet()
        {
            css = "";
            css = string.Join(string.Empty, Rules.Select(x=>ComponentStyle.PrintRule(x)));
            //foreach(var rule in rules)
            //{
            //    css += ComponentStyle.PrintRule(rule);
            //}
            base.OnParametersSet();

        }

        //Private functionality

        private void SetSelectorNames()
        {
            foreach (var rule in rules)
            {
                var innerRule = rule as Rule;
                if (innerRule.Selector.GetType() == typeof(IdSelector) || innerRule.Selector.GetType() == typeof(MediaSelector))
                    continue;
                if (string.IsNullOrWhiteSpace(innerRule.Selector.SelectorName))
                {
                    innerRule.Selector.SelectorName = $"css-{ComponentStyle.LocalCSSheets.ToList().IndexOf(this)}-{rules.ToList().IndexOf(innerRule)}";
                }
                else
                {
                    innerRule.Selector.SelectorName = $"{innerRule.Selector.SelectorName}-{ComponentStyle.LocalCSSheets.ToList().IndexOf(this)}-{rules.ToList().IndexOf(innerRule)}";
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