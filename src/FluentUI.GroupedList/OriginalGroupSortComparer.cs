using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI
{
    public class OriginalGroupSortComparer<TItem> : IComparer<IGroupedListItem3<TItem>>
    {
        IList<TItem> _original;

        public OriginalGroupSortComparer(IList<TItem> original)
        {
            _original = original;
        }

        public int Compare(IGroupedListItem3<TItem> x, IGroupedListItem3<TItem> y)
        {
            if (_original.IndexOf(x.Item) > _original.IndexOf(y.Item))
                return 1;
            else if (_original.IndexOf(x.Item) < _original.IndexOf(y.Item))
                return -1;
            else
                return 0;
        }

    }
}
