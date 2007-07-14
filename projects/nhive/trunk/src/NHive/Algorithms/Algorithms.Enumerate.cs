namespace NHive
{
    using System;
    using NHive.Base;
    
    public static partial class Algorithms
	{
        public static Range<T, TSize, TIterator> Enumerate<T, TSize, TIterator>(IIteratable<T, TSize, TIterator> hive)
            where TIterator : struct, IInputIterator<T, TSize, TIterator>
            where TSize : struct, IConvertible
        {
            return new Range<T, TSize, TIterator>(hive);
        }

        public static Range<T, TSize, TIterator> Enumerate<T, TSize, TIterator>(TIterator begin, TIterator end)
            where TIterator : struct, IInputIterator<T, TSize, TIterator>
            where TSize : struct, IConvertible
        {
            return new Range<T, TSize, TIterator>(begin, end);
        }
    }
}
