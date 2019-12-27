using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System;

namespace BlazorFabric
{
    public partial class ComponentStyle : ComponentBase, IComponentStyleSheet
    {
        [Inject]
        public IComponentStyleSheets ComponentStyleSheets { get; set; }
        private string css;
        private ICollection<UniqueRule> rules;

        [Parameter]
        public ICollection<UniqueRule> Rules
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
        [Parameter] public EventCallback<ICollection<UniqueRule>> RulesChanged { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ComponentStyleSheets.CStyleSheets.Add(this);
            SetSelectorNames();
            await base.OnInitializedAsync();
        }

        protected override void OnParametersSet()
        {
            PrintRules();
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
                    rule.Selector.SelectorName = $"css-{ComponentStyleSheets.CStyleSheets.ToList().IndexOf(this)}-{rules.ToList().IndexOf(rule)}";
                }
                else
                {
                    rule.Selector.SelectorName = $"{rule.Selector.SelectorName}-{ComponentStyleSheets.CStyleSheets.ToList().IndexOf(this)}-{rules.ToList().IndexOf(rule)}";
                }
            }
            RulesChanged.InvokeAsync(rules);
        }

        private void PrintRules()
        {
            css = "";
            foreach (var rule in rules)
            {
                css += $"{rule.Selector.ToString()}{{";
                foreach (var property in rule.Properties.GetType().GetProperties())
                {
                    string cssProperty = "";
                    string cssValue = "";
                    var attribute = property.GetCustomAttribute(typeof(DisplayAttribute));
                    if (attribute != null)
                    {
                        cssProperty = (attribute as DisplayAttribute).GetName();
                    }
                    else
                    {
                        cssProperty = property.Name;
                    }

                    cssValue = property.GetValue(rule.Properties)?.ToString();
                    if (!string.IsNullOrWhiteSpace(cssValue))
                    {
                        css += $"{cssProperty.ToLower()}:{cssValue};";
                    }
                }
                css += "}";
            }
        }
    }
}