using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric.Nav
{
    public interface INavItem
    {
        string Name { get; set; }
        string Icon { get; set; }
        string Link { get; set; }
    }
}
