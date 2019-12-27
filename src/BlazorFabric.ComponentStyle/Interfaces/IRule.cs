using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public interface IRule
    {
        public IRuleProperties Properties { get; set; }
    }
}
