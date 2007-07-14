namespace NHive.Base.Size
{
    using System;

    public struct Int64Operations : ISizeOperations<long>
    {
        public long Zero
        {
            get { return 0; }
        }

        public long Const(int x)
        {
            return x;
        }

        public long From<TSize2>(TSize2 x) where TSize2 : struct, IConvertible
        {
            return Convert.ToInt64(x);
        }

        public long FromInt32(int x)
        {
            return x;
        }

        public long FromInt64(long x)
        {
            return x;
        }

        public int ToInt32(long x)
        {
            return (int) x;
        }

        public long ToInt64(long x)
        {
            return x;
        }

        public void Decrement(ref long x)
        {
            x--;
        }

        public void Increment(ref long x)
        {
            x++;
        }

        public long Add(long x, long y)
        {
            return x + y;
        }

        public long Add(long x, int y)
        {
            return x + y;
        }

        public long AddWith(ref long x, long y)
        {
            return x += y;
        }

        public long AddWith(ref long x, int y)
        {
            return x += y;
        }

        public long Subtract(long x, long y)
        {
            return x - y;
        }

        public long Subtract(long x, int y)
        {
            return x - y;
        }

        public long SubtractWith(ref long x, long y)
        {
            return x -= y;
        }

        public long SubtractWith(ref long x, int y)
        {
            return x -= y;
        }

        public long Multiply(long x, long y)
        {
            return x * y;
        }

        public long Multiply(long x, int y)
        {
            return x * y;
        }

        public long MultiplyWith(ref long x, long y)
        {
            return x *= y;
        }

        public long MultiplyWith(ref long x, int y)
        {
            return x *= y;
        }

        public long Divide(long x, long y)
        {
            return x / y;
        }

        public long Divide(long x, int y)
        {
            return x / y;
        }

        public long DivideWith(ref long x, long y)
        {
            return x /= y;
        }

        public long DivideWith(ref long x, int y)
        {
            return x /= y;
        }

        public T[] CreateArray<T>(long length)
        {
            return new T[length];
        }

        public void CopyArray<T>(T[] sourceArray, long sourceBeginIndex, long sourceEndIndex,
            T[] destinationArray, long destinationIndex)
        {
            Array.Copy(sourceArray, sourceBeginIndex, destinationArray, destinationIndex,
                sourceEndIndex - sourceBeginIndex);
        }

        public void ClearArray<T>(T[] array, long beginIndex, long endIndex)
        {
            if (beginIndex < int.MaxValue)
            {
                if (endIndex > int.MaxValue)
                {
                    endIndex = int.MaxValue;
                }
                Array.Clear(array, (int) beginIndex, (int) (endIndex - beginIndex));
            }
        }

        public T GetArrayElement<T>(T[] array, long index)
        {
            return array[index];
        }

        public void SetArrayElement<T>(T[] array, long index, T item)
        {
            array[index] = item;
        }

        public int Compare(long x, long y)
        {
            return x.CompareTo(y);
        }

        public bool Equals(long x, long y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(long obj)
        {
            return obj.GetHashCode();
        }
    }
}
