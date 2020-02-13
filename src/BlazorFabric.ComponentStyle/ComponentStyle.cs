using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;

namespace BlazorFabric
{
    public class ComponentStyle : IComponentStyle
    {
        public ICollection<ILocalCSSheet> LocalCSSheets { get; set; }
        public ObservableCollection<IGlobalCSSheet> GlobalCSSheets { get; set; }
        public ObservableRangeCollection<string> GlobalCSRules { get; set; }

        public ComponentStyle()
        {
            LocalCSSheets = new HashSet<ILocalCSSheet>();
            GlobalCSSheets = new ObservableCollection<IGlobalCSSheet>();
            GlobalCSRules = new ObservableRangeCollection<string>();
            GlobalCSSheets.CollectionChanged += CollectionChanged;
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (INotifyPropertyChanged item in e.OldItems)
                {
                    item.PropertyChanged -= ItemChanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach (INotifyPropertyChanged item in e.NewItems)
                    item.PropertyChanged += ItemChanged;
            }
        }
        private void ItemChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateGlobalRules();
        }

        private void UpdateGlobalRules()
        {
            var newRules = GetGlobalCSRules();
            if (newRules?.Count > 0)
            {
                GlobalCSRules.ReplaceRange(newRules);
            }
        }

        private ICollection<string> GetNewRules(ICollection<Rule> rules)
        {
            var addRules = new Collection<string>();
            foreach (var rule in rules)
            {
                var ruleAsString = PrintRule(rule);
                if (!GlobalCSRules.Contains(ruleAsString))
                {
                    addRules.Add(ruleAsString);
                }
            }
            return addRules;
        }

        private ICollection<string> GetOldRules()
        {
            var addRules = new Collection<string>();
            foreach (var styleSheet in GlobalCSSheets)
            {
                foreach (var rule in styleSheet.Rules)
                {
                    var ruleAsString = PrintRule(rule);
                    if (!GlobalCSRules.Contains(ruleAsString))
                    {
                        addRules.Add(ruleAsString);
                    }
                }
            }
            return addRules;
        }

        private ICollection<string> GetGlobalCSRules()
        {
            var globalCSRules = new Collection<string>();
            var update = false;
            foreach (var styleSheet in GlobalCSSheets)
            {
                foreach (var rule in styleSheet.Rules)
                {
                    var ruleAsString = PrintRule(rule);
                    if (!globalCSRules.Contains(ruleAsString))
                    {
                        globalCSRules.Add(ruleAsString);
                    }
                    if (!GlobalCSRules.Contains(ruleAsString))
                    {
                        update = true;
                    }
                }
            }
            if (!update)
            {
                foreach (var rule in GlobalCSRules)
                {
                    if (!globalCSRules.Contains(rule))
                    {
                        update = true;
                    }
                }
            }
            if (update)
                return globalCSRules;
            return null;
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
