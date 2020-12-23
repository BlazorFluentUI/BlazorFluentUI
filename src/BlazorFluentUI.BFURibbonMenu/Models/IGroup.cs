using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFluentUI.Models
{
    public interface IGroup
    {
        ICollection<IRibbonItem> ItemsSource { get; set; }

    }
}
