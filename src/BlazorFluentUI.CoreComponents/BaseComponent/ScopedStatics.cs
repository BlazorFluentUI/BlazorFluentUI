using System;
using System.Collections.Generic;

namespace BlazorFluentUI
{
    public class ScopedStatics
    {
        public bool FocusRectsInitialized { get; set; } = false;

        public bool AssembliesScanned { get; set; } = false;
        public List<Type> ComponentTypes { get; set; }
    }
}
