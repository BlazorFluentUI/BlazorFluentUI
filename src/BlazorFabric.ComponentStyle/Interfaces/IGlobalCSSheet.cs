using System;
using System.Collections.Generic;

namespace BlazorFabric
{
    public interface IGlobalCSSheet
    {
        object Component { get; set; }
        bool IsGlobal { get; set; }
        Func<ICollection<Rule>> CreateGlobalCss { get; set; }
    }
}