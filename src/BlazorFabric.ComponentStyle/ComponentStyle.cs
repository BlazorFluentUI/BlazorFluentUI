using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.InteropServices;

namespace BlazorFabric
{
    public class ComponentStyle : IComponentStyle
    {
        public bool ClientSide { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.Create("WEBASSEMBLY"));


        public GlobalRules GlobalRules { get; set; }

        public ICollection<ILocalCSSheet> LocalCSSheets { get; set; }
        public ObservableCollection<IGlobalCSSheet> GlobalCSSheets { get; set; }
        public ICollection<IGlobalCSSheet> GlobalRulesSheets { get; set; }
        public ICollection<string> GlobalCSRules { get; set; }

        public ComponentStyle()
        {
            LocalCSSheets = new List<ILocalCSSheet>();
            GlobalCSSheets = new ObservableCollection<IGlobalCSSheet>();
            GlobalCSSheets.CollectionChanged += CollectionChanged;
            GlobalRulesSheets = new List<IGlobalCSSheet>();
            GlobalCSRules = new List<string>();
        }

        public void SetDisposedAction()
        {
            GlobalRules.OnDispose = Disposed;
        }

        public void Disposed()
        {
            LocalCSSheets.Clear();
            GlobalCSSheets.Clear();
            GlobalRulesSheets.Clear();
            GlobalCSRules.Clear();
        }

        //public bool ComponentStyleExist(object component)
        //{
        //    if (component == null)
        //        return false;
        //    var componentType = component.GetType();
        //    return GlobalRulesSheets.Any(x => x.Component?.GetType() == componentType);
        //}

        public bool ComponentStyleExist(Type componentType)
        {
            if (componentType == null)
                return false;
            return GlobalRulesSheets.Any(x => x.ComponentType == componentType);
        }

        //public bool StyleSheetIsNeeded(object component)
        //{
        //    if (component == null)
        //        return false;
        //    var componentType = component.GetType();
        //    return GlobalCSSheets.Any(x => x.Component?.GetType() == componentType);
        //}

        public bool StyleSheetIsNeeded(Type componentType)
        {
            if (componentType == null)
                return false;
            //var componentType = component.GetType();
            return GlobalCSSheets.Any(x => x.ComponentType == componentType);
        }


        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    //if (!((IGlobalCSSheet)item).FixStyle && ((IGlobalCSSheet)item).Component != null && !StyleSheetIsNeeded(((IGlobalCSSheet)item).Component))
                    //{
                    //    GlobalRulesSheets.Remove(GlobalRulesSheets.First(x => x.Component?.GetType() == ((IGlobalCSSheet)item).Component.GetType()));
                    //    RemoveOneStyleSheet((IGlobalCSSheet)item);
                    //    GlobalRules.UpdateGlobalRules();
                    //}
                    //else if (!((IGlobalCSSheet)item).FixStyle && ((IGlobalCSSheet)item).Component != null && ((IGlobalCSSheet)item).IsGlobal)
                    //{
                    //    GlobalCSSheets.First(x => x.Component?.GetType() == ((IGlobalCSSheet)item).Component.GetType()).IsGlobal = true;
                    //}
                    if (!((IGlobalCSSheet)item).FixStyle && ((IGlobalCSSheet)item).ComponentType != null && !StyleSheetIsNeeded(((IGlobalCSSheet)item).ComponentType))
                    {
                        GlobalRulesSheets.Remove(GlobalRulesSheets.First(x => x.ComponentType == ((IGlobalCSSheet)item).ComponentType));
                        RemoveOneStyleSheet((IGlobalCSSheet)item);
                        GlobalRules?.UpdateGlobalRules();
                    }
                    else if (!((IGlobalCSSheet)item).FixStyle && ((IGlobalCSSheet)item).ComponentType != null && ((IGlobalCSSheet)item).IsGlobal)
                    {
                        GlobalCSSheets.First(x => x.ComponentType == ((IGlobalCSSheet)item).ComponentType).IsGlobal = true;
                    }
                }
            }

            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    if (!ComponentStyleExist(((IGlobalCSSheet)item).ComponentType))
                    {
                        GlobalRulesSheets.Add((IGlobalCSSheet)item);
                        ((IGlobalCSSheet)item).IsGlobal = true;
                        AddOneStyleSheet((IGlobalCSSheet)item);
                        GlobalRules?.UpdateGlobalRules();
                    }
                }
            }
        }
        public void RulesChanged(IGlobalCSSheet globalCSSheet)
        {
            if (globalCSSheet.IsGlobal || globalCSSheet.FixStyle)
                UpdateGlobalRules();
            return;

        }

        private void UpdateGlobalRules()
        {
            var newRules = GetAllGlobalCSRules();
            if (newRules?.Count > 0)
            {
                GlobalCSRules.Clear();
                GlobalCSRules = newRules;
                GlobalRules.UpdateGlobalRules();
            }
        }

        private void AddOneStyleSheet(IGlobalCSSheet sheet)
        {
            if (sheet.CreateGlobalCss == null)
            {
                return;
            }
            var rules = sheet.CreateGlobalCss.Invoke();

            foreach (var rule in rules)
            {
                var ruleAsString = PrintRule(rule);
                if (!GlobalCSRules.Contains(ruleAsString))
                {
                    GlobalCSRules.Add(ruleAsString);
                }
            }
        }

        private void RemoveOneStyleSheet(IGlobalCSSheet sheet)
        {
            if (sheet.CreateGlobalCss == null)
            {
                return;
            }
            var rules = sheet.CreateGlobalCss.Invoke();

            foreach (var rule in rules)
            {
                var ruleAsString = PrintRule(rule);
                if (GlobalCSRules.Contains(ruleAsString))
                {
                    GlobalCSRules.Remove(ruleAsString);
                }
            }
        }

        private ICollection<string> GetAllGlobalCSRules()
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

        public string PrintRule(IRule rule)
        {
            if (rule?.Properties == null)
                return "";
            var ruleAsString = "";
            ruleAsString += $"{(rule as Rule).Selector.GetSelectorAsString()}{{";

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

                    if (property.Name == "CssString")
                    {
                        ruleAsString += property.GetValue(rule.Properties)?.ToString();
                        continue;
                    }

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
            }
            ruleAsString += "}";
            return ruleAsString;
        }
    }
}
