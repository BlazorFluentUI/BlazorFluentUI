using Microsoft.JSInterop;
using System;

namespace BlazorFluentUI
{
    public class InteropHelper
    {
        private Action<bool> _onResizedTrigger;

        public InteropHelper(Action<bool> resizeHappenedTrigger)
        {
            _onResizedTrigger = resizeHappenedTrigger;
        }

        [JSInvokable]
        public void OnResizedAsync()
        {
            _onResizedTrigger(true);
        }

    }
}
