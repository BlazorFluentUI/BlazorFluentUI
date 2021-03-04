using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BlazorFluentUI
{
    public interface IComponentStyle
    {
        bool ClientSide { get; }

        BFUGlobalRules GlobalRules { get; set; }

        ICollection<ILocalCSSheet> LocalCSSheets { get; set; }

        ObservableCollection<IGlobalCSSheet> GlobalCSSheets { get; set; }

        ICollection<string> GlobalCSRules { get; set; }

        void RulesChanged(IGlobalCSSheet globalCSSheet);

        string PrintRule(IRule rule);

        void SetDisposedAction();
    }
}
