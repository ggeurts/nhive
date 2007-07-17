namespace NHive
{
    using System;

    public interface IIteratable<T, TSize, TIterator>
        where TIterator : struct, IIterator<T, TIterator>
        where TSize: struct, IConvertible
    {
        TIterator Begin { get; }
        TIterator End { get; }
    }

    public interface IInputIteratable<T, TSize, TIterator>
        where TIterator : struct, IInputIterator<T, TSize, TIterator>
        where TSize : struct, IConvertible
    {
        TIterator Begin { get; }
        TIterator End { get; }
    }

    public interface IForwardIteratable<T, TSize, TIterator> 
        : IInputIteratable<T, TSize, TIterator>
        where TIterator : struct, IForwardIterator<T, TSize, TIterator>
        where TSize : struct, IConvertible
    { }

    public interface IBidirectionalIteratable<T, TSize, TIterator> 
        : IForwardIteratable<T, TSize, TIterator>
        where TIterator : struct, IBidirectionalIterator<T, TSize, TIterator>
        where TSize : struct, IConvertible
    { }

    public interface IRandomAccessIteratable<T, TSize, TIterator> 
        : IBidirectionalIteratable<T, TSize, TIterator>
        where TIterator : struct, IRandomAccessIterator<T, TSize, TIterator>
        where TSize : struct, IConvertible
    { }
}
