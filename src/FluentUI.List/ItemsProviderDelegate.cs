using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentUI
{
    /// <summary>
    /// A function that provides items to a virtualized source.
    /// </summary>
    /// <typeparam name="TItem">The type of the context for each item in the list.</typeparam>
    /// <param name="request">The <see cref="ItemsProviderRequest"/> defining the request details.</param>
    /// <returns>A <see cref="ValueTask"/> whose result is a <see cref="ItemsProviderResult{TItem}"/> upon successful completion.</returns>
    public delegate ValueTask<ItemsProviderResult<TItem>> ItemsProviderDelegate<TItem>(ItemsProviderRequest request);
}
