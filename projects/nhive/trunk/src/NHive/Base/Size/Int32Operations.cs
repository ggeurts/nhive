namespace NHive.Base.Size
{
    using System;

    public struct Int32Operations : ISizeOperations<int>
    {
        #region Predefined constants

        public int Zero 
        { 
            get { return 0; }
        }

        #endregion

        #region Conversion operations

        public int From<TSize2>(TSize2 x) where TSize2 : struct, IConvertible
        {
            return Convert.ToInt32(x);
        }

        public int From(int x)
        {
            return x;
        }

        public int FromInt64(long x)
        {
            return (int)x;
        }

        public int ToInt32(int x)
        {
            return x;
        }

        public long ToInt64(int x)
        {
            return x;
        }

        #endregion

        #region Increment/Decrement operations

        public void Decrement(ref int x)
        {
            x--;
        }

        public void Increment(ref int x)
        {
            x++;
        }

        #endregion

        #region Mathematical operations

        public int Add(int x, int y)
        {
            checked { return x + y; }
        }

        public int AddWith(ref int x, int y)
        {
            checked { return x += y; }
        }

        public int Subtract(int x, int y)
        {
            checked { return x - y; }
        }

        public int SubtractWith(ref int x, int y)
        {
            checked { return x -= y; }
        }

        public int Multiply(int x, int y)
        {
            checked { return x * y; }
        }

        public int MultiplyWith(ref int x, int y)
        {
            checked { return x *= y; }
        }

        public int Divide(int x, int y)
        {
            return x / y;
        }

        public int DivideWith(ref int x, int y)
        {
            return x /= y;
        }

        #endregion

        #region Array operations

        public T[] CreateArray<T>(int length)
        {
            return new T[length];
        }

        public void ClearArray<T>(T[] array, int beginIndex, int endIndex)
        {
            if (beginIndex < 0)
            {
                throw new ArgumentOutOfRangeException("beginIndex");
            }
            if (endIndex > array.Length || endIndex < beginIndex)
            {
                throw new ArgumentOutOfRangeException("endIndex");
            }

            Array.Clear(array, beginIndex, endIndex - beginIndex);
        }

        public void CopyArray<T>(T[] sourceArray, int sourceBeginIndex, int sourceEndIndex,
            T[] targetArray, int targetIndex)
        {
            if (sourceEndIndex > sourceArray.Length || sourceEndIndex < sourceBeginIndex)
            {
                throw new ArgumentOutOfRangeException("sourceEndIndex");
            }

            Array.Copy(sourceArray, sourceBeginIndex, targetArray, targetIndex,
                sourceEndIndex - sourceBeginIndex);
        }

        public T GetValueFromArray<T>(T[] array, int index)
        {
            return array[index];
        }

        public void SetValueInArray<T>(T[] array, T item, int index)
        {
            array[index] = item;
        }

        #endregion

        #region IEqualityComparer<int> implementation

        public bool Equals(int x, int y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(int obj)
        {
            return obj.GetHashCode();
        }

        #endregion

        #region IComparer<int> implementation

        public int Compare(int x, int y)
        {
            return x.CompareTo(y);
        }

        #endregion
    }
}
