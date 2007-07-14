namespace NHive.Base.Size
{
    using System;

    public struct Int32Operations : ISizeOperations<int>
    {
        public int Zero 
        { 
            get { return 0; } 
        }

        public int Const(int x)
        {
            return x;
        }

        public int From<TSize2>(TSize2 x) where TSize2 : struct, IConvertible
        {
            return Convert.ToInt32(x);
        }

        public int FromInt32(int x)
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

        public void Decrement(ref int x)
        {
            x--;
        }

        public void Increment(ref int x)
        {
            x++;
        }

        public int Add(int x, int y)
        {
            return x + y;
        }

        public int AddWith(ref int x, int y)
        {
            return x += y;
        }

        public int Subtract(int x, int y)
        {
            return x - y;
        }

        public int SubtractWith(ref int x, int y)
        {
            return x -= y;
        }

        public int Multiply(int x, int y)
        {
            return x * y;
        }

        public int MultiplyWith(ref int x, int y)
        {
            return x *= y;
        }

        public int Divide(int x, int y)
        {
            return x / y;
        }

        public int DivideWith(ref int x, int y)
        {
            return x /= y;
        }

        public T[] CreateArray<T>(int length)
        {
            return new T[length];
        }

        public void CopyArray<T>(T[] sourceArray, int sourceBeginIndex, int sourceEndIndex, 
            T[] destinationArray, int destinationIndex)
        {
            Array.Copy(sourceArray, sourceBeginIndex, destinationArray, destinationIndex, 
                sourceEndIndex - sourceBeginIndex);
        }

        public void ClearArray<T>(T[] array, int beginIndex, int endIndex)
        {
            Array.Clear(array, beginIndex, endIndex - beginIndex);
        }

        public T GetArrayElement<T>(T[] array, int index)
        {
            return array[index];
        }

        public void SetArrayElement<T>(T[] array, int index, T item)
        {
            array[index] = item;
        }

        public int Compare(int x, int y)
        {
            return x.CompareTo(y);
        }

        public bool Equals(int x, int y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(int obj)
        {
            return obj.GetHashCode();
        }
    }
}
