using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BlazorFabric
{
    public interface IComponentStyle
    {
        GlobalRules GlobalRules { get; set; }

        ICollection<ILocalCSSheet> LocalCSSheets { get; set; }

        ObservableCollection<IGlobalCSSheet> GlobalCSSheets { get; set; }

        ICollection<string> GlobalCSRules { get; set; }

        void RulesChanged(IGlobalCSSheet globalCSSheet);

        string PrintRule(Rule rule);

        void SetDisposedAction();
    }
}
