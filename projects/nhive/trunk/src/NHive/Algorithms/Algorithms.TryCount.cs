namespace NHive
{
    using System;
    using System.Collections.Generic;
    using NHive.Core;

    public static partial class Algorithms
    {
        internal static bool TryCount<T, TSize>(IEnumerable<T> items, out TSize count)
            where TSize: struct, IConvertible
        {
            IHive<T, TSize> hive = items as IHive<T, TSize>;
            if (hive != null)
            {
                return hive.TryGetCount(out count);
            }

            ICollection<T> collection = items as ICollection<T>;
            int itemCount = collection != null 
                ? collection.Count 
                : -1;
            count = SizeOperations<TSize>.Default.From(itemCount);
            return itemCount >= 0;
        }

        internal static bool TryCount<T, TSize, TInput>(TInput begin, TInput end, out TSize count)
            where TInput : struct, IInputIterator<T, TSize, TInput>
            where TSize : struct, IConvertible
        {
            CountDelegate<T, TSize, TInput> countImpl = begin.GetProperty<CountDelegate<T, TSize, TInput>>();
            if (countImpl != null)
            {
                count = countImpl(begin, end);
                return true;
            }
            else
            {
                count = SizeOperations<TSize>.Default.From(-1);
                return false;
            }
        }
    }
}
