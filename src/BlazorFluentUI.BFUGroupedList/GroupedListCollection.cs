using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    /// <summary>
    /// Workaround for Blazor compiler not being able to use nested generics 3 deep
    /// </summary>
    public class GroupedListCollection<TItem>
    {
        public System.Collections.ObjectModel.ReadOnlyObservableCollection<GroupedListItem<TItem>> GroupedListItems { get; set; }
    }
}
