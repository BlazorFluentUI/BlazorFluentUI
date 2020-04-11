using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFabric
{
    public class Page<TItem>
    {
        public double Height { get; set; }
        public int ItemCount { get; set; }
        public IEnumerable<TItem> Items { get; set; }
        public string Key { get; set; }
        public int StartIndex { get; set; }
        public double Top { get; set; }
        public object Data { get; set; }
        public bool IsSpacer { get; set; }
        public bool IsVisisble { get; set; }
    }
}
