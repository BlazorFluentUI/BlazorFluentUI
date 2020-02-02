using System.Collections.Generic;

namespace BlazorFabric
{
    public interface ILocalCSSheet
    {
        ICollection<DynamicRule> Rules { get; set; }
    }
}