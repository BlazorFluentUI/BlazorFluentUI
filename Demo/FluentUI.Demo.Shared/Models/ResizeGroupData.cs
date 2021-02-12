using System.Collections.Generic;

namespace FluentUI.Demo.Shared.Models
{
    public class ResizeGroupData<TObject>
    {
        public IEnumerable<TObject> Items { get; set; }
        public IEnumerable<TObject> OverflowItems { get; set; }
        public string CacheKey { get; set; }

        public ResizeGroupData(IEnumerable<TObject> items, IEnumerable<TObject> overflowItems, string cacheKey)
        {
            Items = items;
            OverflowItems = overflowItems;
            CacheKey = cacheKey;
        }
    }
}
