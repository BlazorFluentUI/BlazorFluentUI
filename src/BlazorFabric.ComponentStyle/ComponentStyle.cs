using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BlazorFabric
{
    public class ComponentStyle : IComponentStyle
    {
        public ICollection<IDynamicCSSheet> DynamicCSSheets { get; set; }
        public ICollection<IStaticCSSheet> StaticCSSheets { get; set; }

        public ICollection<IGlobalRules> SubscribedGlobalRules { get; set; }

        public ComponentStyle()
        {
            DynamicCSSheets = new HashSet<IDynamicCSSheet>();
            StaticCSSheets = new HashSet<IStaticCSSheet>();
            SubscribedGlobalRules = new HashSet<IGlobalRules>();
        }

        public IDictionary<string, string> GetGlobalCSRules()
        {
            var globalCSRules = new Dictionary<string, string>();
            foreach(var styleSheet in StaticCSSheets)
            {
                foreach(var rule in styleSheet.Rules)
                {
                    var ruleAsString = PrintRule(rule);
                    var key = Convert.ToBase64String(Encoding.Unicode.GetBytes(ruleAsString));
                    if (!globalCSRules.ContainsKey(key))
                    {
                        globalCSRules.Add(key, ruleAsString);
                    }
                }
            }
            return globalCSRules;
        }

        public void Subscribe(IGlobalRules globalRules)
        {
            SubscribedGlobalRules.Add(globalRules);
        }

        public void UpdateSubscribers()
        {
            foreach(var subscriber in SubscribedGlobalRules)
            {
                subscriber.Update();
            }
        }

        public string PrintRule(Rule rule)
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
