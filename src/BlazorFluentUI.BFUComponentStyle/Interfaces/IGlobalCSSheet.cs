using System;
using System.Collections.Generic;

namespace BlazorFluentUI
{
    public interface IGlobalCSSheet
    {
        //object Component { get; set; }
        Type ComponentType { get; set; }
        bool IsGlobal { get; set; }
        bool FixStyle { get; set; }
        Func<ICollection<IRule>> CreateGlobalCss { get; set; }
    }
}