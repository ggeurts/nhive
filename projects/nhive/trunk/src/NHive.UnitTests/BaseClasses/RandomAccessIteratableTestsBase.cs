namespace NHive.UnitTests
{
    using System;
    using System.Collections.Generic;
    using MbUnit.Framework;

    public interface IRandomAccessIterableTestArgs<T, TSize, THive, TIterator>
        : IForwardIterableTestArgs<T, TSize, THive, TIterator>
        where THive : IRandomAccessIteratable<T, TSize, TIterator>
        where TIterator : struct, IRandomAccessIterator<T, TSize, TIterator>
        where TSize : struct, IConvertible
    { }

    public abstract class RandomAccessIteratableTestsBase<T, TSize, THive, TIterator>
        : ForwardIteratableTestsBase<T, TSize, THive, TIterator>
        where THive : IRandomAccessIteratable<T, TSize, TIterator>
        where TIterator : struct, IRandomAccessIterator<T, TSize, TIterator>
        where TSize : struct, IConvertible
    { }
}
