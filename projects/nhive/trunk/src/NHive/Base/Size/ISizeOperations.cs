namespace NHive.Base.Size
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Operations that are supported on size arguments of type <typeparamref name="TSize"/>.
    /// </summary>
    /// <typeparam name="TSize">A signed or unsigned ordinal type that is used to measure the number 
    /// of items in a data structure or operation.</typeparam>
    public interface ISizeOperations<TSize> : IComparer<TSize>, IEqualityComparer<TSize>
        where TSize: struct, IConvertible
    {
        TSize Zero { get; }
        TSize Const(int x);
        void Decrement(ref TSize x);
        void Increment(ref TSize x);
        TSize Add(TSize x, TSize y);
        TSize Add(TSize x, int y);
        TSize AddWith(ref TSize x, TSize y);
        TSize AddWith(ref TSize x, int y);
        TSize Subtract(TSize x, TSize y);
        TSize Subtract(TSize x, int y);
        TSize SubtractWith(ref TSize x, TSize y);
        TSize SubtractWith(ref TSize x, int y);
        TSize Multiply(TSize x, TSize y);
        TSize Multiply(TSize x, int y);
        TSize MultiplyWith(ref TSize x, TSize y);
        TSize MultiplyWith(ref TSize x, int y);
        TSize Divide(TSize x, TSize y);
        TSize Divide(TSize x, int y);
        TSize DivideWith(ref TSize x, TSize y);
        TSize DivideWith(ref TSize x, int y);
        TSize From<T>(T x) where T : struct, IConvertible;
        TSize FromInt32(int x);
        TSize FromInt64(long x);
        int ToInt32(TSize x);
        long ToInt64(TSize x);
        T[] CreateArray<T>(TSize length);
        void CopyArray<T>(T[] sourceArray, TSize sourceBeginIndex, TSize sourceEndIndex, T[] destinationArray, TSize destinationIndex);
        void ClearArray<T>(T[] array, TSize beginIndex, TSize endIndex);
        T GetArrayElement<T>(T[] array, TSize index);
        void SetArrayElement<T>(T[] array, TSize index, T item);
    }
}
