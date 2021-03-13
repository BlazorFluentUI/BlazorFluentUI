// from https://github.com/mattmc3/dotmore/tree/master/dotmore/Collections/Generic

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BlazorFluentUI
{
    public class KeyedCollection2<TKey, TItem> : KeyedCollection<TKey, TItem>
    {
        private const string DelegateNullExceptionMessage = "Delegate passed cannot be null";
        private Func<TItem, TKey> _getKeyForItemFunction;

        public KeyedCollection2(Func<TItem, TKey> getKeyForItemFunction) : base()
        {
            if (getKeyForItemFunction == null) throw new ArgumentNullException(DelegateNullExceptionMessage);
            _getKeyForItemFunction = getKeyForItemFunction;
        }

        public KeyedCollection2(Func<TItem, TKey> getKeyForItemDelegate, IEqualityComparer<TKey> comparer) : base(comparer)
        {
            if (getKeyForItemDelegate == null) throw new ArgumentNullException(DelegateNullExceptionMessage);
            _getKeyForItemFunction = getKeyForItemDelegate;
        }

        protected override TKey GetKeyForItem(TItem item)
        {
            return _getKeyForItemFunction(item);
        }

        public void SortByKeys()
        {
            Comparer<TKey>? comparer = Comparer<TKey>.Default;
            SortByKeys(comparer);
        }

        public void SortByKeys(IComparer<TKey> keyComparer)
        {
            Comparer2<TItem>? comparer = new((x, y) => keyComparer.Compare(GetKeyForItem(x), GetKeyForItem(y)));
            Sort(comparer);
        }

        public void SortByKeys(Comparison<TKey> keyComparison)
        {
            Comparer2<TItem>? comparer = new((x, y) => keyComparison(GetKeyForItem(x), GetKeyForItem(y)));
            Sort(comparer);
        }

        public void Sort()
        {
            Comparer<TItem>? comparer = Comparer<TItem>.Default;
            Sort(comparer);
        }

        public void Sort(Comparison<TItem> comparison)
        {
            Comparer2<TItem>? newComparer = new((x, y) => comparison(x, y));
            Sort(newComparer);
        }

        public void Sort(IComparer<TItem> comparer)
        {
            List<TItem> list = base.Items as List<TItem>;
            if (list != null)
            {
                list.Sort(comparer);
            }
        }
    }
}