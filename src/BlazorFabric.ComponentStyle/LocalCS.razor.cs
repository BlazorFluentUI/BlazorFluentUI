using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Reflection;
using System;

namespace BlazorFabric
{
    public partial class LocalCS : ComponentBase, ILocalCSSheet
    {
        private string css;
        private ICollection<DynamicRule> rules;

        [Inject]
        public IComponentStyle ComponentStyle { get; set; }

        [Parameter]
        public ICollection<DynamicRule> Rules
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

        [Parameter] public EventCallback<ICollection<DynamicRule>> RulesChanged { get; set; }

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
                css += PrintRule(rule);
            }
            base.OnParametersSet();

        }

        //Private functionality

        private void SetSelectorNames()
        {
            foreach (var rule in rules)
            {
                if (!rule.Selector.UniqueName)
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

        public string PrintRule(DynamicRule rule)
        {
            var ruleAsString = "";
            ruleAsString += $"{rule.Selector.GetSelectorAsString()}{{";
            foreach (var property in rule.Properties.GetType().GetProperties())
            {
                string cssProperty = "";
                string cssValue = "";
                Attribute attribute = null;

                //Catch Ignore Propertie
                attribute = property.GetCustomAttribute(typeof(CsIgnoreAttribute));
                if (attribute != null)
                    continue;

                attribute = property.GetCustomAttribute(typeof(CsPropertyAttribute));
                if (attribute != null)
                {
                    if ((attribute as CsPropertyAttribute).IsCssStringProperty)
                    {
                        ruleAsString += property.GetValue(rule.Properties)?.ToString();
                        continue;
                    }

                    cssProperty = (attribute as CsPropertyAttribute).PropertyName;
                }
                else
                {
                    cssProperty = property.Name;
                }

                cssValue = property.GetValue(rule.Properties)?.ToString();
                if (cssValue != null)
                {
                    ruleAsString += $"{cssProperty.ToLower()}:{(string.IsNullOrEmpty(cssValue) ? "\"\"" : cssValue)};";
                }
            }
            ruleAsString += "}";
            return ruleAsString;
        }
    }
}