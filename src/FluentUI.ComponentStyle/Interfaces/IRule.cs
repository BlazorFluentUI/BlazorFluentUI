using System;
using System.Collections.Generic;
using System.Text;

namespace FluentUI
{
    public interface IRule
    {
        public IRuleProperties Properties { get; set; }
    }
}
