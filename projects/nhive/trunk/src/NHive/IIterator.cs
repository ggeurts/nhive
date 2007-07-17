namespace NHive
{
    using System;

    /// <summary>
    /// Interface to be implemented by all input and output iterators. Iterators 
    /// are a sort of generic bookmark or cursor into a collection of items. They indicate 
    /// a position in a collection and provide operations to either read or write the items 
    /// stored at this positon.
    /// </summary>
    /// <typeparam name="T">The type of objects that are read and/or written by the iterator.</typeparam>
    /// <typeparam name="TIterator">The type of the iterator itself.</typeparam>
    /// <remarks>
    /// <para>
    /// The iterator concept is borrowed from the C++ Standard Template Library (STL) and a port of
    /// the STL to the .NET Framework (the <a href="http://www.sf.net/cstl">CSTL project</a>).
    /// </para>
    /// <para>
    /// Iterators should be value types. Typically they are passed to generic algorithms that manipulate
    /// (increment and/or decrement) the iterator to access ranges of values in the parent hive of the
    /// iterator. However, operations on iterators by generic algorithms should not modify the iterator value
    /// as seen by callers of the generic algorithms. By forcing iterators to be value types, this behaviour 
    /// is guaranteed under all circumstances. The alternative would be to support cloning of iterators
    /// and ensure that this is doen consistently by all generic algorithms. However, the usage of cloning 
    /// is prone to coding errors that may be hard to detect.
    /// </para>
    /// </remarks>
    public interface IIterator<T, TIterator>
        where TIterator : struct, IIterator<T, TIterator>
    {
        /// <summary>
        /// Returns the hive that contains the items that are referenced by this iterator.
        /// </summary>
        IHive<T> Parent { get; }

        /// <summary>
        /// Retrieves optional iterator property.
        /// </summary>
        /// <typeparam name="P">Property type.</typeparam>
        /// <returns>An instance of property type <typeparamref name="P"/> if the iterator 
        /// or its parent hive (see <see cref="Parent"/>) supports the property. Otherwise 
        /// returns <c>null</c>.</returns>
        P GetProperty<P>();

        void Increment();
    }

    /// <summary>
    /// Interface to be implemented by all output iterators. Output iterators support the 
    /// inserting and/or updating of an item at the position in the parent hive that is
    /// currently pointed to by the iterator.
    /// </summary>
    public interface IOutputIterator<T, TIterator>
        : IIterator<T, TIterator>
        where TIterator : struct, IOutputIterator<T, TIterator>
    {
        /// <summary>
        /// Inserts or updates a value at the current position of the iterator 
        /// in the parent hive.
        /// </summary>
        /// <param name="value">The item to be stored in the hive.</param>
        void Write(T value);
    }

    /// <summary>
    /// Interface to be implemented by all input iterators. Input iterators support the 
    /// reading of an item at the position in the parent hive that is currently pointed to 
    /// by the iterator. This interface guarantees only that the a the items in the parent 
    /// collection can be iterated once in forward direction.
    /// </summary>
    public interface IInputIterator<T, TSize, TIterator>
        : IIterator<T, TIterator>
        , IEquatable<TIterator>
        where TIterator: struct, IInputIterator<T, TSize, TIterator>
        where TSize : struct, IConvertible
    {
        /// <summary>
        /// Reads the item at the current position of the iterator in the parent hive.
        /// </summary>
        /// <returns>The read item.</returns>
        T Read();
    }

    /// <summary>
    /// Interface to be implemented by all input iterators over hives that support repeatable 
    /// forward-only iteration. This interface guarantees that the a the items in the parent 
    /// collection can be iterated an arbitrary number of times in forward direction.
    /// </summary>
    public interface IForwardIterator<T, TSize, TIterator>
        : IInputIterator<T, TSize, TIterator>
        where TIterator : struct, IForwardIterator<T, TSize, TIterator>
        where TSize: struct, IConvertible
    { }

    /// <summary>
    /// Interface to be implemented by all input iterators over hives that support repeatable 
    /// bidirectional iteration. This interface guarantees that the a the items in the parent 
    /// collection can be iterated an arbitrary number of times in forward or backward direction.
    /// </summary>
    public interface IBidirectionalIterator<T, TSize, TIterator>
        : IForwardIterator<T, TSize, TIterator>
        where TIterator : struct, IBidirectionalIterator<T, TSize, TIterator>
        where TSize : struct, IConvertible
    {
        void Decrement();
    }

    /// <summary>
    /// Interface to be implemented by all input iterators over hives that support random access
    /// iteration. This interface guarantees that the a the items in the parent collection can be 
    /// accessed an arbitrary number of times in random order.
    /// </summary>
    public interface IRandomAccessIterator<T, TSize, TIterator> 
        : IBidirectionalIterator<T, TSize, TIterator>
        , IOutputIterator<T, TIterator>
        , IComparable<TIterator>
        where TIterator : struct, IRandomAccessIterator<T, TSize, TIterator>
        where TSize : struct, IConvertible
    {
        TIterator OffsetBy(TSize distance);
        TSize DistanceFrom(TIterator from);
    }
}
