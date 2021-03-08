using System.Collections.Generic;

namespace BlazorFluentUI.ResizeGroup
{
    public class ResizeGroupChange<TItem>
    {
        public IEnumerable<TItem> Primary { get; set; }
        public IEnumerable<TItem> Secondary { get; set; }
        public string CacheKey { get; set; }
    }
}
