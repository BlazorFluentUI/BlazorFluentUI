using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public interface IBackground
    {
        string NeutralTertiaryAlt { get; }
        string NeutralDark { get; }
        string NeutralQuaternaryAlt { get; }
        string NeutralLight { get; }
        string NeutralLighter { get; }
        string NeutralLighterAlt { get; }
    }
}
