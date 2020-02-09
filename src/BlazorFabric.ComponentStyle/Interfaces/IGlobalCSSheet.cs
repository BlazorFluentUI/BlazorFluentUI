using System.Collections.Generic;

namespace BlazorFabric
{
    public interface IGlobalCSSheet
    {
        ICollection<Rule> Rules { get; set; }
    }
}