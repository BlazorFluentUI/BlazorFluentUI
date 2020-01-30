using System.Collections.Generic;

namespace BlazorFabric
{
    public interface IStaticCSSheet
    {
        ICollection<Rule> Rules { get; set; }
    }
}