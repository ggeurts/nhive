namespace NHive
{
    using System;
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
}
