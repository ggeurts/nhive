namespace NHive
{
    using System;
    using System.Collections.Generic;

    public static partial class Algorithms
    {
        public static void Copy<T, TSize, TInput, TOutput>(IIteratable<T, TSize, TInput> hive, TOutput target)
            where TInput : struct, IInputIterator<T, TSize, TInput>
            where TOutput : struct, IOutputIterator<T, TOutput>
            where TSize : struct, IConvertible
        {
            Copy<T, TSize, TInput, TOutput>(hive.Begin, hive.End, target);
        }

        internal static void Copy<T, TSize, TInput>(IIteratable<T, TSize, TInput> hive, T[] array, TSize startIndex)
            where TInput : struct, IInputIterator<T, TSize, TInput>
            where TSize : struct, IConvertible
        {
            long targetIndex = SizeOperations<TSize>.Default.ToInt64(startIndex);
            for (TInput i = hive.Begin; !i.Equals(hive.End); i.Increment())
            {
                array[targetIndex++] = i.Read();
            }
        }

        private static void Copy<T, TSize, TInput, TOutput>(TInput begin, TInput end, TOutput target)
            where TInput : struct, IInputIterator<T, TSize, TInput>
            where TOutput : struct, IOutputIterator<T, TOutput>
            where TSize : struct, IConvertible
        {
            for (TInput i = begin; !i.Equals(end); i.Increment(), target.Increment())
            {
                target.Write(i.Read());
            }
        }
    }
}
