namespace NHive
{
    using System;

    /// <summary>
    /// A variable-sized container whose elements are arranged in a strict linear order. 
    /// It supports insertion and removal of elements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISequentialHive<T> : IHive<T>
    {
        T First { get; }

        /// <summary>Inserts item before iterator <paramref name="where"/>.</summary>
        /// <param name="insertPosition"></param>
        /// <param name="item"></param>
        void InsertAt(Cell<T> position, T item);
        void InsertAt(Cell<T> position, int count, T item);
        void InsertAt(Cell<T> position, ForwardView<T> range);

        void Delete(Cell<T> position);
        void Delete(ForwardView<T> range);
    }

    public interface IIndexedHive<T> : ISequentialHive<T>
    {
    }

    public interface ISortedHive<T> : ISequentialHive<T>
    {
    }
}
