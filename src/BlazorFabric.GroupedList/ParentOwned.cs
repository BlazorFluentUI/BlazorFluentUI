using System;
using System.Collections.Generic;
using System.Text;
using DynamicData;

namespace BlazorFabric
{
    public class ParentOwned<TItem,TKey>
    {
        public TKey OwnedBy { get; set; }
        public TItem Item { get; set; }
        public IObservable<IChangeSet<ParentOwned<TItem,TKey>,TKey>> ObservableChangeSet { get; }



        public ParentOwned(TItem item, TKey ownedBy)
        {
            Item = item;
            OwnedBy = ownedBy;
        }

        public ParentOwned(TItem item, TKey ownedBy, Func<TItem, TKey> keySelector, Func<TItem, IEnumerable<TItem>> subgroupSelector)
        {
            Item = item;
            OwnedBy = ownedBy;

            ObservableChangeSet = subgroupSelector(item)?
                .AsObservableChangeSet(keySelector)
                .Transform(x=> new ParentOwned<TItem,TKey>(x, keySelector(item), keySelector,subgroupSelector));
        }
    }
}
