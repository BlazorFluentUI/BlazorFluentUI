using System.Collections.Generic;

namespace BlazorFabric
{
    public interface IComponentStyleSheet
    {
        ICollection<UniqueRule> Rules { get; set; }
    }
}