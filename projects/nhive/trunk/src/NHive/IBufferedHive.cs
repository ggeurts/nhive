namespace NHive
{
    using System;

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
}
