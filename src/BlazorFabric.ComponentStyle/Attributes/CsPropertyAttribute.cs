using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public class CsPropertyAttribute : Attribute
    {
        public string PropertyName { get; set; }
        public bool IsCssStringProperty { get; set; } = false;

        public CsPropertyAttribute()
        {

        }
    }
}
