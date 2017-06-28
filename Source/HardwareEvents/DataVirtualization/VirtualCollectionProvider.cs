using System;
using System.Collections.Generic;

namespace HardwareEvents.DataVirtualization
{
    /// <summary>
    /// Wrapper implementation for IItemsProvider
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class VirtualCollectionProvider<T> : IItemsProvider<T>
    {
        public Func<int, int, T[]> ItemsLoading
        {
            get;
            set;
        }

        public int VirtualItemCount { get; internal set; }

        /// <summary>
        /// Fetches the total number of items available.
        /// </summary>
        /// <returns></returns>
        public int FetchCount()
        {
            return VirtualItemCount;
        }

        /// <summary>
        /// Fetches a range of items.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">The number of items to fetch.</param>
        /// <returns></returns>
        public IList<T> FetchRange(int startIndex, int count)
        {
            return ItemsLoading(startIndex, count);
        }
    }
}
