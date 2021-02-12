using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.InteropServices;
using Sigil.NonGeneric;
using Sigil;
using System.Diagnostics;

namespace FluentUI
{
    public class ComponentStyle : IComponentStyle
    {
        private static Dictionary<Type, List<PropertyInfo>> _propertyDictionary = new Dictionary<Type, List<PropertyInfo>>();
        private static Dictionary<PropertyInfo, List<Attribute>> _attributeDictionary = new Dictionary<PropertyInfo, List<Attribute>>();
        private static Dictionary<PropertyInfo, Func<object, object>> _rulePropertiesGetters = new Dictionary<PropertyInfo, Func<object, object>>();

        public bool ClientSide { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.Create("WEBASSEMBLY"));

        public GlobalRules? GlobalRules { get; set; }

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
            if (GlobalRules != null)
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
                        //Debug.WriteLine($"Removed StyleSheet for {((IGlobalCSSheet)item).ComponentType}");
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
                        //Debug.WriteLine($"Added StyleSheet for {((IGlobalCSSheet)item).ComponentType}");
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
                GlobalRules?.UpdateGlobalRules();
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

        private ICollection<string>? GetAllGlobalCSRules()
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
            if (rule == null)
                return "";
            if (rule.Properties == null)
                return "";
            var ruleAsString = "";
            
            ruleAsString += $"{(rule as Rule)?.Selector?.GetSelectorAsString()}{{";

            if (rule.Properties is CssString)
            {
                return ruleAsString + (rule.Properties as CssString)?.Css + "}";
            }
            else
            {
                var type = rule.Properties.GetType();
                foreach (var property in GetCachedProperties(type))
                {
                    string cssProperty = "";
                    string cssValue = "";
                    Attribute? attribute = null;

                    //Catch Ignore Propertie
                    attribute = GetCachedCustomAttribute(property, typeof(CsIgnoreAttribute));  // property.GetCustomAttribute(typeof(CsIgnoreAttribute));
                    if (attribute != null)
                        continue;

                    if (property.Name == "CssString")
                    {
                        ruleAsString += GetCachedGetter(property, _rulePropertiesGetters).Invoke(rule.Properties)?.ToString();//property.GetValue(rule.Properties)?.ToString();
                        continue;
                    }

                    attribute = GetCachedCustomAttribute(property, typeof(CsPropertyAttribute));  //property.GetCustomAttribute(typeof(CsPropertyAttribute));
                    if (attribute != null)
                    {
                        var propAttribute = attribute as CsPropertyAttribute;
                        if (propAttribute != null)
                        {
                            if (propAttribute.IsCssStringProperty)
                            {
                                ruleAsString += GetCachedGetter(property, _rulePropertiesGetters).Invoke(rule.Properties)?.ToString(); //property.GetValue(rule.Properties)?.ToString();
                                continue;
                            }
                            cssProperty = propAttribute.PropertyName;
                        }
                    }
                    else
                    {
                        cssProperty = property.Name;
                    }
                    // getter could return null so check for null before ToString call.
                    cssValue = GetCachedGetter(property, _rulePropertiesGetters).Invoke(rule.Properties)?.ToString(); //property.GetValue(rule.Properties)?.ToString();
                    if (cssValue != null)
                    {
                        ruleAsString += $"{cssProperty.ToLower()}:{(string.IsNullOrEmpty(cssValue) ? "\"\"" : cssValue)};";
                    }
                }
            }
            ruleAsString += "}";
            return ruleAsString;
        }

        private List<PropertyInfo> GetCachedProperties(Type type)
        {
            List<PropertyInfo> properties;
            if (_propertyDictionary.TryGetValue(type, out properties) == false)
            {
                properties = type.GetProperties().ToList();
                _propertyDictionary.Add(type, properties);
            }

            return properties;
        }

        private Attribute? GetCachedCustomAttribute(PropertyInfo property, Type attributeType)
        {
            Attribute? attribute = null;
            List<Attribute> attributes;
            if (_attributeDictionary.TryGetValue(property, out attributes) == false)
            {
                attributes = property.GetCustomAttributes().ToList();
                _attributeDictionary.Add(property, attributes);
            }
            if (attributes != null)
            {
                attribute = attributes.FirstOrDefault(x => x.GetType() ==  attributeType);
            }

            return attribute;
        }

        private static Func<object, object> GetCachedGetter(PropertyInfo property, Dictionary<PropertyInfo, Func<object,object>> cache) 
        {
            Func<object,object> getter;
            var start = DateTime.Now.Ticks;
            if (cache.TryGetValue(property, out getter) == false)
            {
                var getterEmitter = Emit<Func<object, object>>.NewDynamicMethod().LoadArgument(0).CastClass(property.DeclaringType).Call(property.GetGetMethod()).Return();
                getter = getterEmitter.CreateDelegate();
                cache.Add(property, getter);
                //Debug.WriteLine($"Emit creation took: {TimeSpan.FromTicks(DateTime.Now.Ticks - start).TotalMilliseconds}ms");
            }
            else
            {
                //Debug.WriteLine($"Cached getter took: {TimeSpan.FromTicks(DateTime.Now.Ticks-start).TotalMilliseconds}ms");
            }

            return getter;
        }
    }
}
