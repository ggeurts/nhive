namespace NHive.UnitTests
{
    using System;
    using System.Collections.Generic;
    using MbUnit.Framework;

    public interface IForwardIterableTestArgs<T, TSize, THive, TIterator>
        : IInputIteratableTestArgs<T, TSize, THive, TIterator>
        where THive : IForwardIteratable<T, TSize, TIterator>
        where TIterator : struct, IForwardIterator<T, TSize, TIterator>
        where TSize : struct, IConvertible
    { }

    public abstract class ForwardIteratableTestsBase<T, TSize, THive, TIterator>
        : InputIteratableTestsBase<T, TSize, THive, TIterator>
        where THive : IForwardIteratable<T, TSize, TIterator>
        where TIterator : struct, IForwardIterator<T, TSize, TIterator>
        where TSize : struct, IConvertible
    { }
}
