using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFluentUI
{
    public interface IRule
    {
        public IRuleProperties Properties { get; set; }
    }
}
