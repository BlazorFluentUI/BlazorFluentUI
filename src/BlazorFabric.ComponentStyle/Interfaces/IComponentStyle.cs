using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public interface IComponentStyle
    {
        ICollection<IDynamicCSSheet> DynamicCSSheets { get; set; }

        ICollection<IStaticCSSheet> StaticCSSheets { get; set; }

        IDictionary<string, string> GetGlobalCSRules();
        
        void Subscribe(IGlobalRules globalRules);
        void UpdateSubscribers();
    }
}
