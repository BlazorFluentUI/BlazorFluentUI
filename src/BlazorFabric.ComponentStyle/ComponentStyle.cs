using System.Collections.Generic;

namespace BlazorFabric
{
    public static class ComponentStyle
    {
        public static ICollection<IComponentStyleSheet> UniqueCSSheets { get; set; } = new HashSet<IComponentStyleSheet>();
        public static IDictionary<string,string> GlobalCSRules { get; set; } = new Dictionary<string,string>();
        public static IGlobalRules GlobalRules { get; set; }
    }
}
