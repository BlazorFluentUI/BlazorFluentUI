using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorFluentUI
{
    public partial class LocalCS : ComponentBase, ILocalCSSheet, IDisposable
    {
        private string? css;
        private ICollection<IRule>? rules;

        protected long ScopeId { get; set; }

        [Inject]
        public IComponentStyle? ComponentStyle { get; set; }
        [Inject]
        public ObjectIDGenerator? IdGenerator { get; set; }

        [Parameter]
        public ICollection<IRule> Rules
        {
            get => rules!;
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
            ScopeId = IdGenerator.GetId(this, out _);
            ComponentStyle!.LocalCSSheets.Add(this);
            SetSelectorNames();
            await base.OnInitializedAsync();
        }

        protected override void OnParametersSet()
        {
            css = "";
            css = string.Join(string.Empty, Rules.Select(x => ComponentStyle!.PrintRule(x)));
            //foreach(var rule in rules)
            //{
            //    css += ComponentStyle.PrintRule(rule);
            //}
            base.OnParametersSet();

        }

        //Private functionality

        private void SetSelectorNames()
        {
            foreach (IRule? rule in rules!)
            {
                Rule? innerRule = rule as Rule;
                if (innerRule!.Selector?.GetType() == typeof(IdSelector) || innerRule.Selector?.GetType() == typeof(MediaSelector))
                    continue;
                if (string.IsNullOrWhiteSpace(innerRule.Selector?.SelectorName))
                {
                    innerRule.Selector!.SelectorName = $"css-{ScopeId}-{rules.ToList().IndexOf(innerRule)}";
                }
                else
                {
                    innerRule.Selector.SelectorName = $"{innerRule.Selector.SelectorName}-{ScopeId}-{rules.ToList().IndexOf(innerRule)}";
                }
            }
            RulesChanged.InvokeAsync(rules);
        }

        public void Dispose()
        {
            ComponentStyle!.LocalCSSheets.Remove(this);
        }
    }
}
