﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorFluentUI
{
    /// <summary>
    /// Represents the result of a <see cref="ItemsProviderDelegate{TItem}"/>.
    /// </summary>
    /// <typeparam name="TItem">The type of the context for each item in the list.</typeparam>
    public readonly struct ItemsProviderResult<TItem>
    {
        /// <summary>
        /// The items to provide.
        /// </summary>
        public IEnumerable<TItem> Items { get; }

        /// <summary>
        /// The total item count in the source generating the items provided.
        /// </summary>
        public int TotalItemCount { get; }

        /// <summary>
        /// Instantiates a new <see cref="ItemsProviderResult{TItem}"/> instance.
        /// </summary>
        /// <param name="items">The items to provide.</param>
        /// <param name="totalItemCount">The total item count in the source generating the items provided.</param>
        public ItemsProviderResult(IEnumerable<TItem> items, int totalItemCount)
        {
            Items = items;
            TotalItemCount = totalItemCount;
        }
    }
}
