using System.Collections.Generic;

namespace BlazorFabric
{
    public interface IDynamicCSSheet
    {
        ICollection<DynamicRule> Rules { get; set; }
    }
}