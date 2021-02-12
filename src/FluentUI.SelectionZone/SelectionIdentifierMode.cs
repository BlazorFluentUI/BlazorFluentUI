using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI
{
    /// <summary>
    /// Index mode will store selected items by their index.  This is more useful for simple lists that don't need keys for their items. 
    /// Key mode will store items by their key.
    /// </summary>
    public enum SelectionIdentifierMode
    {
        Index,
        Key
    }
}
