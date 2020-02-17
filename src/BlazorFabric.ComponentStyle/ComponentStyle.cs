using System;
using System.Linq;
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
        public ObservableCollection<IGlobalCSSheet> GlobalRulesSheets { get; set; }
        public ObservableRangeCollection<string> GlobalCSRules { get; set; }

        public ComponentStyle()
        {
            LocalCSSheets = new HashSet<ILocalCSSheet>();
            GlobalCSSheets = new ObservableCollection<IGlobalCSSheet>();
            GlobalCSSheets.CollectionChanged += CollectionChanged;
            GlobalRulesSheets = new ObservableCollection<IGlobalCSSheet>();
            GlobalCSRules = new ObservableRangeCollection<string>();

        }

        public bool ComponentStyleExist(object component)
        {
            if (component == null)
                return false;
            var componentType = component.GetType();
            return GlobalRulesSheets.Any(x => x.Component?.GetType() == componentType);
        }

        public bool StyleSheetIsNeeded(object component)
        {
            if (component == null)
                return false;
            var componentType = component.GetType();
            return GlobalCSSheets.Any(x => x.Component?.GetType() == componentType);
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {

                    if (((IGlobalCSSheet)item).Component != null && !StyleSheetIsNeeded(((IGlobalCSSheet)item).Component))
                    {
                        GlobalRulesSheets.Remove(GlobalRulesSheets.First(x => x.Component?.GetType() == ((IGlobalCSSheet)item).Component.GetType()));
                        UpdateGlobalRules();
                    }
                    else if (((IGlobalCSSheet)item).Component != null && ((IGlobalCSSheet)item).IsGlobal)
                    {
                        GlobalCSSheets.First(x => x.Component?.GetType() == ((IGlobalCSSheet)item).Component.GetType()).IsGlobal = true;
                    }
                }
            }

            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    if (!ComponentStyleExist(((IGlobalCSSheet)item).Component))
                    {
                        GlobalRulesSheets.Add((IGlobalCSSheet)item);
                        ((IGlobalCSSheet)item).IsGlobal = true; ;
                        UpdateGlobalRules();
                    }
                }
            }
        }
        public void ItemsChanged(IGlobalCSSheet globalCSSheet)
        {
            if (!globalCSSheet.IsGlobal)
                return;
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

        private ICollection<string> GetGlobalCSRules()
        {
            var globalCSRules = new HashSet<string>();
            var update = false;
            foreach (var styleSheet in GlobalRulesSheets)
            {
                if (styleSheet.CreateGlobalCss == null)
                {
                    continue;
                }
                var rules = styleSheet.CreateGlobalCss.Invoke();

                foreach (var rule in rules)
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

            if (rule.Properties is CssString)
            {
                return ruleAsString + (rule.Properties as CssString).Css + "}";
            }
            else
            {
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
}
