using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FluentUI
{
    public interface IComponentStyle
    {
        bool ClientSide { get; }

        GlobalRules GlobalRules { get; set; }

        ICollection<ILocalCSSheet> LocalCSSheets { get; set; }

        ObservableCollection<IGlobalCSSheet> GlobalCSSheets { get; set; }

        ICollection<string> GlobalCSRules { get; set; }

        void RulesChanged(IGlobalCSSheet globalCSSheet);

        string PrintRule(IRule rule);

        void SetDisposedAction();
    }
}
