using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFluentUI
{
    public class BFUDropdownChangeArgs
    {
        public string Key { get; set; } 
        public bool IsAdded { get; set; }

        public BFUDropdownChangeArgs(string key, bool isAdded)
        {
            Key = key;
            IsAdded = isAdded;
        }
    }
}
