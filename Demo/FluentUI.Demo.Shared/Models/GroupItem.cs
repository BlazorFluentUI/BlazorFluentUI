using FluentUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FluentUI.Demo.Shared.Models
{
    class GroupItem: IGroup
    {
        public ICollection<IRibbonItem> ItemsSource { get; set; } = new ObservableCollection<IRibbonItem>();
    }
}
