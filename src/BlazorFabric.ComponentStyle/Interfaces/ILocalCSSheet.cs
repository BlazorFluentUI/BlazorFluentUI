using System.Collections.Generic;

namespace BlazorFabric
{
    public interface ILocalCSSheet
    {
        ICollection<LocalRule> Rules { get; set; }
    }
}