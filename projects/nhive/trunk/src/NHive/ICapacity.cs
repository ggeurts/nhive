namespace NHive
{
    using System;

    public interface ICapacity<TSize>
        where TSize: struct, IConvertible
    {
        TSize Capacity { get; }
        TSize MaxCapacity { get; }
    }
}
