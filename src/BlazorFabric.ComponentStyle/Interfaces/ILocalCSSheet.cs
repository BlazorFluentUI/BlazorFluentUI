using System.Collections.Generic;

namespace BlazorFabric
{
    public interface ILocalCSSheet
    {
        ICollection<Rule> Rules { get; set; }
    }
}