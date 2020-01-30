using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public interface IForeground
    {
        string Black { get; }
        string NeutralDark { get; }
        string NeutralPrimary { get; }
        string NeutralPrimaryAlt { get; }
        string NeutralSecondary { get; }
        string NeutralTertiary { get; }
        string White { get; }
    }
}
