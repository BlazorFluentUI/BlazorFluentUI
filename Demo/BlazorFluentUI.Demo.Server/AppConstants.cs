﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFluentUI.Demo.Server
{
    public static class AppConstants
    {
#if DEBUG
        public const bool IS_DEBUG = true;
#else
        public const bool IS_DEBUG = false;
#endif
    }
}
