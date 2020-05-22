using System.Collections.Generic;

namespace BlazorFluentUI
{
    public interface ILocalCSSheet
    {
        ICollection<IRule> Rules { get; set; }
    }
}