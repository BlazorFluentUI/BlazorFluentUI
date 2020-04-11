using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFluentUI
{
    public class Selection<TItem>
    {
        private IEnumerable<TItem> _items;

        public IEnumerable<TItem> SelectedItems
        {
            get => _items;
            set => _items = value;
        }

        

        public Selection()
        {
            _items = new List<TItem>();
        }

        public Selection(IEnumerable<TItem> items)
        {
            _items = items;
        }

        public void ClearSelection()
        {
            _items = new List<TItem>();
        }
    }
}
