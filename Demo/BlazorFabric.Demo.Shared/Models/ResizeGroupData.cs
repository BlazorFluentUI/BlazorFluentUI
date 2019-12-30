using System.Collections.Generic;

namespace BlazorFabric.Demo.Shared.Models
{
    public class ResizeGroupData<TObject>
    {
        public IEnumerable<TObject> Items { get; set; }
        public IEnumerable<TObject> OverflowItems { get; set; }
        public string CacheKey { get; set; }

        public ResizeGroupData(IEnumerable<TObject> items, IEnumerable<TObject> overflowItems, string cacheKey)
        {
            this.Items = items;
            this.OverflowItems = overflowItems;
            this.CacheKey = cacheKey;
        }
    }
}
