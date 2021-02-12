using System;
using System.Collections.Generic;
using System.Text;

namespace FluentUI
{
    public class DropdownChangeArgs
    {
        public IDropdownOption Option { get; set; }
        //[Obsolete] public string? Key { get; set; } 
        public bool IsAdded { get; set; }

        //public DropdownChangeArgs(string key, bool isAdded)
        //{
        //    Key = key;
        //    IsAdded = isAdded;
        //}

        public DropdownChangeArgs(IDropdownOption option, bool isAdded)
        {
            Option = option;
            IsAdded = isAdded;
        }
    }
}
