using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    public class DynamicAccessor<TItem>
    {
        public object Value
        {
            get => _getter(_item);
            set => _setter(_item, value);
        }

        private Func<TItem, object> _getter;
        private Action<TItem, object> _setter;
        private TItem _item;

        public DynamicAccessor(TItem item, Expression<Func<TItem,object>> getter)
        {
            _item = item;
            _getter = getter.Compile();
            _setter = DetailsRowUtils.GetSetter(getter);
        }
    }
}
