using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFabric
{
    public class StaticCS : ComponentBase
    {
        private ICollection<Rule> rules;

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

        protected override Task OnInitializedAsync()
        {
            var rulesUpdated = false;
            foreach (var rule in rules)
            {
                var ruleAsString = PrintRule(rule);
                var key = Convert.ToBase64String(Encoding.Unicode.GetBytes(ruleAsString));
                if (!ComponentStyle.GlobalCSRules.ContainsKey(key))
                {
                    ComponentStyle.GlobalCSRules.TryAdd(key, ruleAsString);
                    rulesUpdated = true;
                }
            }
            if(rulesUpdated)
                ComponentStyle.GlobalRules.Update();
            return base.OnInitializedAsync();
        }

        private string PrintRule(Rule rule)
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

