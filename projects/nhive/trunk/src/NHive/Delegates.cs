namespace NHive
{
    using System;

    public delegate TSize CountDelegate<T, TSize, TIterator>(TIterator begin, TIterator end)
        where TIterator : struct, IInputIterator<T, TSize, TIterator>
        where TSize : struct, IConvertible;

    public delegate TSize GetCapacityDelegate<TSize>()
        where TSize: struct, IConvertible;
}
