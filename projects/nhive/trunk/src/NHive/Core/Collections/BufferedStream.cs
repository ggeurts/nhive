namespace NHive.Base.Iterators
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using NHive.Base.Size;

    /// <summary>
    /// Adapter class that upgrades a streamed hive (any <see cref="IEnumerable{T}"/> 
    /// or <see cref="IIteratable{T}"/> implementation) into read-only buffered hive.
    /// </summary>
    /// <typeparam name="T">Hive item type.</typeparam>
    /// <typeparam name="TSize">Hive size.</typeparam>
    /// <typeparam name="TSizeOperations">Hive size operations.</typeparam>
    /// <remarks>
    /// <para>This class is useful if the exact number of items in a stream must
    /// be known for a certain hive operation. An example is the inserting of
    /// a stream into an array list.
    /// </para>
    /// </remarks>
    internal class BufferedStream<T, TSize, TSizeOperations>
        : IBufferedHive<T, TSize, BufferedStream<T, TSize, TSizeOperations>.Iterator>
        where TSize : struct, IConvertible
        where TSizeOperations : ISizeOperations<TSize>, new()
    {
        private const int SEGMENT_CAPACITY_FIRST = 8;
        private const int SEGMENT_CAPACITY_MAX = int.MaxValue;

        #region Static fields

        private static TSizeOperations Size = new TSizeOperations();

        protected static CountDelegate<T, TSize, Iterator> CountImpl = Iterator.Count;

        #endregion

        #region Fields

        /// <summary>
        /// First segment in linked list of buffer segments.
        /// </summary>
        private BufferSegment _firstSegment;

        /// <summary>
        /// Total number of items in buffer.
        /// </summary>
        private TSize _count;

        /// <summary>
        /// Iterator for first item in buffer.
        /// </summary>
        private Iterator _begin;

        /// <summary>
        /// Iterator for end of buffer (one position after last item in buffer).
        /// </summary>
        private Iterator _end;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Constructs a new <see cref="BufferedStream{T, TSize, TSizeOperations}"/> instance. Is only
        /// intended for use by the static factory methods on this class.
        /// </summary>
        /// <param name="firstSegment">First segment in buffer segment list.</param>
        /// <param name="count">Total number of items in buffer.</param>
        private BufferedStream(BufferSegment firstSegment, TSize count)
        {
            _firstSegment = firstSegment;
            _count = count;
            _begin = new Iterator(this, _firstSegment, 0);
            _end = new Iterator(this, null, 0);
        }

        #endregion

        #region Static factory methods

        /// <summary>
        /// Creates a read-only, forward iterable, buffered hive for an iteratable range of items.
        /// </summary>
        /// <typeparam name="TInput">Input iterator type.</typeparam>
        /// <param name="range">The range of items to be loaded into the buffer.</param>
        /// <returns>A <see cref="BufferedStream{T, TSize, TSizeOperations}"/> instance that contains
        /// all range items.</returns>
        public static BufferedStream<T, TSize, TSizeOperations> Create<TInput>(Range<T, TSize, TInput> range)
            where TInput : struct, IInputIterator<T, TSize, TInput>
        {
            BufferSegment firstSegment = new BufferSegment(SEGMENT_CAPACITY_FIRST);

            BufferSegment segment = firstSegment;
            TSize count = Size.Zero;
            for (TInput i = range.Begin; !i.Equals(range.End); i.Increment())
            {
                AddSegmentItem(ref segment, ref count, i.Read());
            }
            Size.AddWith(ref count, segment.Count);

            return new BufferedStream<T, TSize, TSizeOperations>(firstSegment, count);
        }

        /// <summary>
        /// Creates a read-only, forward iterable, buffered hive for an enumerable range of items.
        /// </summary>
        /// <param name="range">The range of items to be loaded into the buffer.</param>
        /// <returns>A <see cref="BufferedStream{T, TSize, TSizeOperations}"/> instance that contains
        /// all range items.</returns>
        public static BufferedStream<T, TSize, TSizeOperations> Create(IEnumerable<T> range)
        {
            BufferSegment firstSegment = new BufferSegment(SEGMENT_CAPACITY_FIRST);

            BufferSegment segment = firstSegment;
            TSize count = Size.Zero;
            foreach (T item in range)
            {
                AddSegmentItem(ref segment, ref count, item);
            }
            Size.AddWith(ref count, segment.Count);

            return new BufferedStream<T, TSize, TSizeOperations>(firstSegment, count);
        }

        private static void AddSegmentItem(ref BufferSegment segment, ref TSize count, T item)
        {
            if (segment.IsFull)
            {
                Size.AddWith(ref count, segment.Count);
                int segmentCapacity = CalculateNextNodeCapacity(segment.Capacity);
                segment.NextSegment = new BufferSegment(segmentCapacity);
                segment = segment.NextSegment;
            }
            segment.Add(item);
        }

        private static int CalculateNextNodeCapacity(int currentNodeCapacity)
        {
            return SEGMENT_CAPACITY_MAX - currentNodeCapacity > currentNodeCapacity
                ? currentNodeCapacity * 2
                : SEGMENT_CAPACITY_MAX;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// See <see cref="IForwardIterable{T}.Begin"/>.
        /// </summary>
        public Iterator Begin
        {
            get { return _begin; }
        }

        /// <summary>
        /// See <see cref="IForwardIterable{T}.End"/>.
        /// </summary>
        public Iterator End
        {
            get { return _end; }
        }

        /// <summary>
        /// See <see cref="IHive.IsEmpty"/>.
        /// </summary>
        public bool IsEmpty
        {
            get { return _begin.Equals(_end); }
        }

        /// <summary>
        /// See <see cref="IHive{T}.IsReadOnly"/>.
        /// </summary>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// The total number of buffered items.
        /// </summary>
        public TSize Count
        {
            get { return _count; }
        }

        #endregion

        #region Public operations

        public bool Contains(T item)
        {
            for (BufferSegment node = _firstSegment; node != null; node = node.NextSegment)
            {
                if (node.Contains(item)) return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to determine total number of buffered items. 
        /// </summary>
        /// <param name="count">Returns total number of buffered items.</param>
        /// <returns>Always returns <c>true</c>.</returns>
        /// <seealso cref="IHive{T}.TryGetCount"/>
        public bool TryGetCount(out TSize count)
        {
            count = _count;
            return true;
        }

        /// <summary>
        /// Copies buffer contents to an array. 
        /// </summary>
        /// <param name="array">The array to which the buffer items must be copied.</param>
        /// <param name="arrayIndex">The array index where the first buffered item is to be inserted.</param>
        /// <seealso cref="ICollection.CopyTo"/>
        public void CopyTo(T[] array, TSize arrayIndex)
        {
            for (BufferSegment node = _firstSegment; node != null; node = node.NextSegment)
            {
                node.CopyTo(array, arrayIndex);
                Size.AddWith(ref arrayIndex, node.Count);
            }
        }

        /// <summary>
        /// Retrieves optional hive property.
        /// </summary>
        /// <typeparam name="P">Property type.</typeparam>
        /// <returns>An instance of property type <typeparamref name="P"/> if the hive
        /// supports the property. Otherwise returns <c>null</c>.</returns>
        /// <seealso cref="IHive{T}.GetProperty{P}"/>
        public P GetProperty<P>()
        {
            if (typeof(P) == typeof(CountDelegate<T, TSize, Iterator>))
            {
                return (P) (object) CountImpl;
            }
            return default(P);
        }

        #endregion

        #region IEnumerable, IEnumerable<T> Members

        public Range<T, TSize, Iterator>.Enumerator GetEnumerator()
        {
            return new Range<T, TSize, Iterator>(this).GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Forward buffer iterator.
        /// </summary>
        public struct Iterator : IForwardIterator<T, TSize, Iterator>
        {
            private BufferedStream<T, TSize, TSizeOperations> _parent;
            private BufferSegment _segment;
            private int _indexInSegment;

            internal Iterator(BufferedStream<T, TSize, TSizeOperations> parent,
                BufferSegment segment, int indexInSegment)
            {
                _parent = parent;
                _segment = segment;
                _indexInSegment = indexInSegment;
            }

            internal static TSize Count(Iterator begin, Iterator end)
            {
                BufferedStream<T, TSize, TSizeOperations> parent = begin._parent;
                if (begin.Equals(parent.Begin) && end.Equals(parent.End))
                {
                    return parent.Count;
                }

                BufferSegment segment = begin._segment;
                TSize count = Size.From(-begin._indexInSegment);

                if (segment != null)
                {
                    while (segment != end._segment)
                    {
                        Size.AddWith(ref count, segment.Capacity);
                        segment = segment.NextSegment;
                    }
                    Size.AddWith(ref count, end._indexInSegment);
                }
                return count;
            }

            public IHive<T> Parent
            {
                get { return _parent; }
            }

            public Iterator Next
            {
                get
                {
                    Iterator result = this;
                    result.Increment();
                    return result;
                }
            }

            public bool Equals(Iterator other)
            {
                return object.ReferenceEquals(this._segment, other._segment)
                    && this._indexInSegment == other._indexInSegment
                    && object.ReferenceEquals(this._parent, other.Parent);
            }

            public void Increment()
            {
                if (_segment == null) return;

                _indexInSegment++;
                if (_indexInSegment >= _segment.Count)
                {
                    _segment = _segment.NextSegment;
                    _indexInSegment = 0;
                }
            }

            public T Read()
            {
                return _segment[_indexInSegment];
            }

            public P GetProperty<P>()
            {
                return _parent.GetProperty<P>();
            }
        }

        /// <summary>
        /// Sequence of buffered items that can be composed together to form a linked list
        /// of buffer segments.
        /// </summary>
        internal class BufferSegment
        {
            private readonly T[] _items;
            private int _count;
            public BufferSegment NextSegment;

            internal BufferSegment(int capacity)
            {
                _items = new T[capacity];
            }

            public T this[int index]
            {
                get { return _items[index]; }
            }

            public int Capacity
            {
                get { return _items.Length; }
            }

            public int Count
            {
                get { return _count; }
            }

            public bool IsFull
            {
                get { return _count >= _items.Length; }
            }

            public void Add(T item)
            {
                _items[_count++] = item;
            }

            public bool Contains(T item)
            {
                return Array.IndexOf<T>(_items, item) >= 0;
            }

            public void CopyTo(T[] array, TSize arrayIndex)
            {
                if (typeof(TSize) == typeof(int))
                {
                    Array.Copy(_items, 0, array, Size.ToInt32(arrayIndex), _count);
                }
                else
                {
                    long targetIndex = Size.ToInt64(arrayIndex);
                    for (int i = 0; i < _count; i++, targetIndex++)
                    {
                        array[targetIndex] = _items[i];
                    }
                }
            }
        }
    }
}
