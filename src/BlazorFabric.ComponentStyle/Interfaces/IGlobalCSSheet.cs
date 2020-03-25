using System;
using System.Collections.Generic;

namespace BlazorFabric
{
    public interface IGlobalCSSheet
    {
        //object Component { get; set; }
        Type ComponentType { get; set; }
        bool IsGlobal { get; set; }
        bool FixStyle { get; set; }
        Func<ICollection<Rule>> CreateGlobalCss { get; set; }
    }
}