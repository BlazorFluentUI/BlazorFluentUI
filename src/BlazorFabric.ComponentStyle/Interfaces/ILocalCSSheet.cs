using System.Collections.Generic;

namespace BlazorFabric
{
    public interface ILocalCSSheet
    {
        ICollection<IRule> Rules { get; set; }
    }
}