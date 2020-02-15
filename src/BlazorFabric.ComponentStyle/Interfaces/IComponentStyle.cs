using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BlazorFabric
{
    public interface IComponentStyle
    {
        ICollection<ILocalCSSheet> LocalCSSheets { get; set; }

        ObservableCollection<IGlobalCSSheet> GlobalCSSheets { get; set; }

        ObservableRangeCollection<string> GlobalCSRules { get; set; }

        bool ComponentStyleExist(object component);
    }
}
