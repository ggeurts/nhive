namespace NHive
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface IHive<T>
        : IEnumerable<T>
    {
        /// <summary>
        /// Indicates whether the hive is empty.
        /// </summary>
        /// <value>Is <c>true</c> if hive contains no items. Otherwise is <c>false</c>.</value>
        bool IsEmpty { get; }

        /// <summary>
        /// Indicates whether the hive can be modified or not.
        /// </summary>
        /// <value>Returns <c>true</c> if hive is read-only. Returns <c>false</c> if
        /// items can be added to, removed from and/or updated in the hive.</value>
        bool IsReadOnly { get; }

        /// <summary>
        /// Retrieves optional hive property.
        /// </summary>
        /// <typeparam name="P">Property type.</typeparam>
        /// <returns>An instance of property type <typeparamref name="P"/> if the hive
        /// supports the property. Otherwise returns <c>null</c>.</returns>
        P GetProperty<P>();
    }

    /// <summary>
    /// The interface that is to be implemented by all hives. A hive is an enumerable 
    /// stream or collection class. This interface guarantees that a hive can be
    /// iterated or enumerated once. However, it does not guarantee that the number of items
    /// in the hive is known in advance.
    /// </summary>
    /// <typeparam name="T">The type of objects stored in the hive.</typeparam>
    /// <remarks>
    /// <para>The NHive collection library makes a distinction between streamed and buffered hives.
    /// </para> 
    /// <para>A streamed hive is a collection of items, of which the size may be unknown. Streamed 
    /// hives only support once-only, forward-only enumeration or iteration. Large database query 
    /// resultsets or network streams are examples of where streamed hives are useful. Streamed hives
    /// must implement the <see cref="IHive{T}"/> interface.
    /// </para>
    /// <para>A buffered hive is a collection of items of which the size is always known and which may 
    /// be enumerated and/or iterated multiple times. Buffered hives support repeatable, forward-only 
    /// enumeration or iteration. Buffered hives must implement the <see cref="IBufferedHive{T, TSize}"/> 
    /// interface.
    /// </para>
    /// </remarks>
    /// <seealso cref="IBufferedHive{T, TSize}"/>
    public interface IHive<T, TSize> 
        : IHive<T>
        where TSize: struct, IConvertible
    {
        /// <summary>
        /// Attempts to determine total number of items in a hive. 
        /// </summary>
        /// <param name="count">Returns number of items in hive if the items can be counted. 
        /// Otherwise returns <c>-1</c>.</param>
        /// <returns>Returns <c>true</c> if the total number of items in the hive can be determined.
        /// Otherwise returns <c>false</c>.</returns>
        /// <remarks>
        /// The speed of this operations depends on the hive implementation.
        /// </remarks>
        bool TryGetCount(out TSize count);
    }

    /// <summary>
    /// The interface that is to be implemented by all iterable hives.
    /// </summary>
    public interface IHive<T, TSize, TIterator> 
        : IHive<T, TSize>
        , IIteratable<T, TSize, TIterator>
        where TIterator : struct, IInputIterator<T, TSize, TIterator>
        where TSize: struct, IConvertible
    {
    }

    /// <summary>
    /// The interface to be implemented by buffered hives. A buffered hive is a 
    /// collection of items of which the size is always known, and which can be 
    /// enumerated or iterated repeatedly without modifying the hive.
    /// </summary>
    /// <typeparam name="T">The type of items stored in the hive.</typeparam>
    /// <typeparam name="TSize">The integer type used to measure the hive size.</typeparam>
    /// <seealso cref="IHive{T}"/>
    public interface IBufferedHive<T, TSize>
        : IHive<T, TSize>
        where TSize : struct, IConvertible
    {
        /// <summary>
        /// Returns the total number of items that are currently stored in the hive.
        /// </summary>
        TSize Count { get; }

        /// <summary>
        /// Determines whether a buffered hive contains a specific value.
        /// </summary>
        /// <param name="item">The item to be found in the hive.</param>
        /// <returns>Returns <c>true</c> if <paramref name="item"/> is 
        /// present in the hive. Otherwise returns <c>false</c>.</returns>
        bool Contains(T item);
    }

    /// <summary>
    /// The interface to be implemented by buffered hives that support iteration. 
    /// See <see cref="IBufferedHive{T, TSize}"/> for more information on buffered hives.
    /// </summary>
    /// <typeparam name="T">The type of items stored in the hive.</typeparam>
    /// <typeparam name="TSize">The integer type used to measure the hive size.</typeparam>
    /// <typeparam name="TIterator">The type of iterators for the hive.</typeparam>
    public interface IBufferedHive<T, TSize, TIterator> 
        : IHive<T, TSize, TIterator>
        , IBufferedHive<T, TSize>
        where TIterator : struct, IForwardIterator<T, TSize, TIterator>
        where TSize : struct, IConvertible
    { }

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
