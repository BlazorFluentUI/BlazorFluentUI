﻿using Microsoft.JSInterop;
using System;

namespace BlazorFluentUI.ResizeGroupInternal
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
