namespace NHive
{
    using System;
    using System.Collections.Generic;

    public interface ICollection<T, TSize>
        : IBufferedHive<T, TSize>
        where TSize : struct, IConvertible
    {
        /// <summary>
        /// Revision number of hive. The revision number is incremented when items
        /// are added to, removed from or updated in the hive.
        /// </summary>
        long Revision { get; }

        void Add(T item);
        void AddRange(IEnumerable<T> range);
        void Clear();
        bool Remove(T item);
    }
}
