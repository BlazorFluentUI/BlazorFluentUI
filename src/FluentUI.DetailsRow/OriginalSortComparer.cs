using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentUI
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

    public class OriginalSortComparerPresortedByGroups<TItem> : IComparer<TItem>
    {
        IList<TItem> _original;
        IList<Func<TItem, object>> _preSort;
        bool _groupSortDescending;

        public OriginalSortComparerPresortedByGroups(IList<TItem> original, IList<Func<TItem,object>> preSort, bool groupSortDescending=false)
        {
            _original = original;
            _preSort = preSort;
            _groupSortDescending = groupSortDescending;

        }

        public int Compare(TItem? x, TItem? y)
        {
            foreach (var item in _preSort)
            {
                if (x == null && y == null)
                {
                    continue;
                }

                if (x == null)
                {
                    return -1;
                }

                if (y == null)
                {
                    return 1;
                }

                var xValue = item.Invoke(x);
                var yValue = item.Invoke(y);

                if (xValue == null && yValue == null)
                {
                    continue;
                }

                if (xValue == null)
                {
                    return -1;
                }

                if (yValue == null)
                {
                    return 1;
                }

                int result = (xValue as IComparable).CompareTo(yValue);
                if (result == 0)
                {
                    continue;
                }

                return !_groupSortDescending ? result : -result;
            }

            return FinalCompare(x, y);


        }

        public int FinalCompare(TItem x, TItem y)
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
