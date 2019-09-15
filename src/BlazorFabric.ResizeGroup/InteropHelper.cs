using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BlazorFabric.ResizeGroupInternal
{
    public class InteropHelper
    {
        private Action<bool> _resizeHappenedTrigger;

        public InteropHelper(Action<bool> resizeHappenedTrigger)
        {
            _resizeHappenedTrigger = resizeHappenedTrigger;
        }

        [JSInvokable]
        public void ResizeHappenedAsync()
        {
            _resizeHappenedTrigger(true);
        }

    }
}
