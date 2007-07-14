namespace NHive.Base.Events
{
    using System;

    public interface IHiveEventSubscriber<T, TSize, TIterator>
        where TIterator : struct, IInputIterator<T, TSize, TIterator>
        where TSize: struct, IConvertible
    {
        void Added(TIterator at);
        void Added(Range<T, TSize, TIterator> range);
        void Removing(TIterator at);
        void Removing(Range<T, TSize, TIterator> range);
    }
}