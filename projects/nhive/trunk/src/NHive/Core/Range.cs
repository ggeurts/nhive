namespace NHive.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Helper type that represents a sequence of items between a
    /// begin iterator (inclusive) and an end iterator (exclusive) and
    /// provides an efficient <see cref="IEnumerable{T}"/> implementation
    /// for the items in the range.
    /// </summary>
    public struct Range<T, TSize, TIterator> : IHive<T, TSize, TIterator>
        where TIterator : struct, IInputIterator<T, TSize, TIterator>
        where TSize : struct, IConvertible
    {
        private readonly TIterator _begin;
        private readonly TIterator _end;

        public Range(IIteratable<T, TSize, TIterator> hive)
        {
            _begin = hive.Begin;
            _end = hive.End;
        }

        public Range(TIterator begin, TIterator end)
        {
            if (!object.ReferenceEquals(begin.Parent, end.Parent))
            {
                throw new ArgumentException("Iterators must have same parent collection.");
            }

            _begin = begin;
            _end = end;
        }

        public TIterator Begin
        {
            get { return _begin; }
        }

        public TIterator End
        {
            get { return _end; }
        }

        public bool IsEmpty
        {
            get { return _begin.Equals(_end); }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public P GetProperty<P>()
        {
            return _begin.Parent != null
                ? _begin.Parent.GetProperty<P>()
                : default(P);
        }

        public bool TryGetCount(out TSize count)
        {
            return Algorithms.TryCount<T, TSize, TIterator>(_begin, _end, out count);
        }

        public HiveEnumerator<T, TSize, TIterator> GetEnumerator()
        {
            return new HiveEnumerator<T, TSize, TIterator>(_begin, _end);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
