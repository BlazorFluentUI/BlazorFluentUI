using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public interface IComponentStyle
    {
        ICollection<ILocalCSSheet> LocalCSSheets { get; set; }

        ICollection<IGlobalCSSheet> GlobalCSSheets { get; set; }

        IDictionary<string, string> GetGlobalCSRules();
        
        void Subscribe(IGlobalRules globalRules);
        void UpdateSubscribers();
    }
}
