using BlazorFluentUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BlazorFluentUI.Demo.Shared.Models
{
    class TabItem<TItem>
    {
        public string? Header { get; set; }
        public ObservableCollection<IGroup> Groups {get;set;}
    }
}
