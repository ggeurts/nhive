namespace NHive
{
    using System;
    using System.Collections.Generic;

    public interface IList<T, TSize>
        : ICollection<T, TSize>
        where TSize : struct, IConvertible
    {
        T this[TSize index] { get; set; }
        TSize IndexOf(T item);
        void Insert(TSize index, T item);
        void InsertRange(TSize index, IEnumerable<T> range);
    }
}
