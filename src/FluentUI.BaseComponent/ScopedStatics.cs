using System;
using System.Collections.Generic;

namespace FluentUI
{
    public class ScopedStatics
    {
        public bool FocusRectsInitialized { get; set; } = false;

        public bool AssembliesScanned { get; set; } = false;
        public List<Type> ComponentTypes { get; set; }
    }
}
