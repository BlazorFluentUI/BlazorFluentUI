using System;
using System.Collections.Generic;
using System.Text;

namespace FluentUI
{
    public interface INavItem
    {
        string Name { get; set; }
        string Icon { get; set; }
        string Link { get; set; }
        
    }
}
