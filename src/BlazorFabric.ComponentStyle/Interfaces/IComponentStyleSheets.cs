using System.Collections.Generic;

namespace BlazorFabric
{
    public interface IComponentStyleSheets
    {
        ICollection<IComponentStyleSheet> CStyleSheets { get; set; }
    }
}