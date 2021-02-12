using System;
using System.Collections.Generic;
using System.Text;

namespace FluentUI.Models
{
    public interface IRibbonItem
    {
        /// <summary>
        /// Hihger Priority means that it stays longer in the ribbon 
        /// and move later to the overflow items. Therefore the collapse 
        /// order is defined by the Priority
        /// </summary>
        double Priority { get; set; }
    }
}
