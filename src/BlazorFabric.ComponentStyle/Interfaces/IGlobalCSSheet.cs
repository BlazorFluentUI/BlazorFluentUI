using System.Collections.Generic;

namespace BlazorFabric
{
    public interface IGlobalCSSheet
    {
        object Component { get; set; }
        ICollection<Rule> Rules { get; set; }
        bool HasEvent { get; set; }
    }
}