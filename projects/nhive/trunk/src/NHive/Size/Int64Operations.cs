namespace NHive.Size
{
    using System;

    public struct Int64Operations : ISizeOperations<long>
    {
        #region Predefined constants

        public long Zero
        {
            get { return 0; }
        }

        #endregion

        #region Conversion operations

        public long From<TSize2>(TSize2 x) where TSize2 : struct, IConvertible
        {
            return Convert.ToInt64(x);
        }

        public long From(int x)
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

        #endregion

        #region Increment/Decrement operations

        public void Decrement(ref long x)
        {
            x--;
        }

        public void Increment(ref long x)
        {
            x++;
        }

        #endregion

        #region Mathematical operations

        public long Add(long x, long y)
        {
            checked { return x + y; }
        }

        public long Add(long x, int y)
        {
            checked { return x + y; }
        }

        public long AddWith(ref long x, long y)
        {
            checked { return x += y; }
        }

        public long AddWith(ref long x, int y)
        {
            checked { return x += y; }
        }

        public long Subtract(long x, long y)
        {
            checked { return x - y; }
        }

        public long Subtract(long x, int y)
        {
            checked { return x - y; }
        }

        public long SubtractWith(ref long x, long y)
        {
            checked { return x -= y; }
        }

        public long SubtractWith(ref long x, int y)
        {
            checked { return x -= y; }
        }

        public long Multiply(long x, long y)
        {
            checked { return x * y; }
        }

        public long Multiply(long x, int y)
        {
            checked { return x * y; }
        }

        public long MultiplyWith(ref long x, long y)
        {
            checked { return x *= y; }
        }

        public long MultiplyWith(ref long x, int y)
        {
            checked { return x *= y; }
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

        #endregion

        #region Array operations

        public T[] CreateArray<T>(long length)
        {
            return new T[length];
        }

        public void CopyArray<T>(T[] sourceArray, long sourceBeginIndex, long sourceEndIndex,
            T[] targetArray, long targetIndex)
        {
            if (sourceEndIndex > sourceArray.Length || sourceEndIndex < sourceBeginIndex)
            {
                throw new ArgumentOutOfRangeException("sourceEndIndex");
            }

            Array.Copy(sourceArray, sourceBeginIndex, targetArray, targetIndex,
                sourceEndIndex - sourceBeginIndex);
        }

        public void ClearArray<T>(T[] array, long beginIndex, long endIndex)
        {
            if (beginIndex < 0)
            {
                throw new ArgumentOutOfRangeException("beginIndex");
            }
            if (endIndex > array.Length || endIndex < beginIndex)
            {
                throw new ArgumentOutOfRangeException("endIndex");
            }

            // Delegate to default Array.Clear implementation if possible.
            if (endIndex <= int.MaxValue)
            {
                Array.Clear(array, (int)beginIndex, (int)(endIndex - beginIndex));
            }
            else
            {
                if (beginIndex < int.MaxValue)
                {
                    Array.Clear(array, (int)beginIndex, int.MaxValue - (int)beginIndex);
                    beginIndex = (long) int.MaxValue + 1;
                }
                for (long i = beginIndex; beginIndex < endIndex; beginIndex++)
                {
                    array[i] = default(T);
                }
            }
        }

        public T GetValueFromArray<T>(T[] array, long index)
        {
            return array[index];
        }

        public void SetValueInArray<T>(T[] array, T item, long index)
        {
            array[index] = item;
        }

        #endregion

        #region IEqualityComparer<long> implementation

        public bool Equals(long x, long y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(long obj)
        {
            return obj.GetHashCode();
        }

        #endregion

        #region IComparer<long> implementaion

        public int Compare(long x, long y)
        {
            return x.CompareTo(y);
        }

        #endregion
    }
}
