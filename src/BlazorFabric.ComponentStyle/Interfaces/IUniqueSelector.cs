using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public interface IUniqueSelector : ISelector
    {
        public bool UniqueName { get; set; }
    }
}
