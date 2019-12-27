using System.Collections.Generic;

namespace BlazorFabric
{
    public class ComponentStyleSheets : IComponentStyleSheets
    {
        public ComponentStyleSheets()
        {
            CStyleSheets = new HashSet<IComponentStyleSheet>();
        }
        public ICollection<IComponentStyleSheet> CStyleSheets { get; set; }
    }
}