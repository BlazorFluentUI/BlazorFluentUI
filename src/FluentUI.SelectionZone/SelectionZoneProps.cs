using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI
{
    public class SelectionZoneProps
    {
        public bool DisableAutoSelectOnInputElements { get; set; }    
        public bool EnableTouchInvocationTarget { get; set; }
        public bool EnterModalOnTouch { get; set; }
        public bool IsModal { get; set; }
        public bool OnItemInvokeSet { get; set; }
        public SelectionMode SelectionMode { get; set; }

    }
}
