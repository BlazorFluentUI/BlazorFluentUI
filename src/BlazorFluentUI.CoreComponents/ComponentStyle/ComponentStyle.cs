using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace BlazorFluentUI
{
    public class ComponentStyle : IComponentStyle
    {
        private static readonly Dictionary<Type, List<PropertyInfo>> _propertyDictionary = new();
        private static readonly Dictionary<PropertyInfo, List<Attribute>> _attributeDictionary = new();
        private static readonly Dictionary<PropertyInfo, Func<object, object>> _rulePropertiesGetters = new();

        public bool IsClient { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.Create("WEBASSEMBLY"));

        public ICollection<ILocalCSSheet> LocalCSSheets { get; set; }

        public ComponentStyle()
        {
            LocalCSSheets = new List<ILocalCSSheet>();
        }

        public void SetDisposedAction()
        {
        }

        public void Disposed()
        {
            LocalCSSheets.Clear();
        }

        public string PrintRule(IRule rule)
        {
            if (rule == null)
                return "";
            if (rule.Properties == null)
                return "";
            string? ruleAsString = "";

            ruleAsString += $"{(rule as Rule)?.Selector?.GetSelectorAsString()}{{";

            if (rule.Properties is CssString)
            {
                return ruleAsString + (rule.Properties as CssString)?.Css + "}";
            }
            else
            {
                Type? type = rule.Properties.GetType();
                foreach (PropertyInfo? property in GetCachedProperties(type))
                {
                    string cssProperty = "";
                    string? cssValue = "";
                    Attribute? attribute = null;

                    //Catch Ignore Propertie
                    attribute = GetCachedCustomAttribute(property, typeof(CsIgnoreAttribute));
                    if (attribute != null)
                        continue;

                    if (property.Name == "CssString")
                    {
                        ruleAsString += GetCachedGetter(property, _rulePropertiesGetters).Invoke(rule.Properties)?.ToString();
                        continue;
                    }

                    attribute = GetCachedCustomAttribute(property, typeof(CsPropertyAttribute));
                    if (attribute != null)
                    {
                        if (attribute is CsPropertyAttribute propAttribute)
                        {
                            if (propAttribute.IsCssStringProperty)
                            {
                                ruleAsString += GetCachedGetter(property, _rulePropertiesGetters).Invoke(rule.Properties)?.ToString();
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

        private static List<PropertyInfo> GetCachedProperties(Type type)
        {
            if (_propertyDictionary.TryGetValue(type, out List<PropertyInfo> properties) == false)
            {
                properties = type.GetProperties().ToList();
                _propertyDictionary.Add(type, properties);
            }

            return properties;
        }

        private static Attribute? GetCachedCustomAttribute(PropertyInfo property, Type attributeType)
        {
            Attribute? attribute = null;
            if (_attributeDictionary.TryGetValue(property, out List<Attribute> attributes) == false)
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
            long start = DateTime.Now.Ticks;
            if (cache.TryGetValue(property, out Func<object, object>? getter) == false)
            {
                DynamicMethod dynamicMethod = new(property.Name + "_DynamicMethod", typeof(Func<object, object>), new Type[] { typeof(object) });
                ILGenerator IL = dynamicMethod.GetILGenerator();
                IL.Emit(OpCodes.Ldarg_0);
                IL.Emit(OpCodes.Castclass, property.DeclaringType!);
                IL.Emit(OpCodes.Call, property.GetGetMethod()!);
                IL.Emit(OpCodes.Ret);

                getter = (Func<object, object>?)dynamicMethod.CreateDelegate(typeof(Func<object, object>));
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
