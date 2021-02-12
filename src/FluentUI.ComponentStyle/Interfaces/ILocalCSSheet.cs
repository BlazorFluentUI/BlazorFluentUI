using System.Collections.Generic;

namespace FluentUI
{
    public interface ILocalCSSheet
    {
        ICollection<IRule> Rules { get; set; }
    }
}