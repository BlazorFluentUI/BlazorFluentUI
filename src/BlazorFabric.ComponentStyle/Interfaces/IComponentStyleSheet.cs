using System.Collections.Generic;

namespace BlazorFabric
{
    public interface IComponentStyleSheet
    {
        ICollection<DynamicRule> Rules { get; set; }
    }
}