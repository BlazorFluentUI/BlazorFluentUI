using System;
using System.Collections.Generic;
using System.Text;

namespace FluentUI
{
    public static class DoubleExtension
    {
        public static string ToCssValue(this double value)
        {
            return value.ToString().Replace(',', '.');
        }
    }
}
