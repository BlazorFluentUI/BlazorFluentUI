using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorFabric.Test.HostedClient.Client.Models
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
