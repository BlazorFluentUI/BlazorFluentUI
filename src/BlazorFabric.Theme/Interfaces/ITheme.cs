using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public interface ITheme
    {
        IPrimary Primary { get; }
        IForeground Foreground { get; }
        IBackground Background { get; }

    }
}
