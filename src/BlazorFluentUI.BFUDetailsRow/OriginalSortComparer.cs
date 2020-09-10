using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlazorFluentUI
{
    public class OriginalSortComparer<TItem> : IComparer<TItem>
    {
        IList<TItem> _original;

        public OriginalSortComparer(IList<TItem> original)
        {
            _original = original;
        }

        public int Compare(TItem x, TItem y)
        {
            if (_original.IndexOf(x) > _original.IndexOf(y))
                return 1;
            else if (_original.IndexOf(x) < _original.IndexOf(y))
                return -1;
            else
                return 0;
        }
    }
}
