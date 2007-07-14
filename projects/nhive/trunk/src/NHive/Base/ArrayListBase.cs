namespace NHive.Base
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using NHive.Base.Events;
    using NHive.Base.Iterators;
    using NHive.Base.Size;

    public class ArrayListBase<T, TSize, TSizeOperations> 
        : ListBase<T, TSize, TSizeOperations>
        where TSize : struct, IConvertible
        where TSizeOperations : ISizeOperations<TSize>, new()
    {
        protected static readonly TSize DEFAULT_CAPACITY = Size.Const(8);

        #region Fields

        /// <summary>
        /// The actual internal array container. Will be extended on demand.
        /// </summary>
        private T[] _innerArray;
        
        /// <summary>
        /// Number of items in _innerArray.
        /// </summary>
        private TSize _count = Size.Zero;

        /// <summary>
        /// Maximum number of items that _innerArray can contain without need for expansion.
        /// </summary>
        private TSize _capacity = Size.Zero;

        #endregion

        #region Constructor(s)

        protected ArrayListBase()
            : this(DEFAULT_CAPACITY, EqualityComparer<T>.Default)
        { }

        protected ArrayListBase(TSize capacity)
            : this(capacity, EqualityComparer<T>.Default)
        { }

        /// <summary>
        /// Create an empty <see cref="ArrayListHive{T}"/> object.
        /// </summary>
        /// <param name="capacity">The initial capacity of the internal array container.
        /// Will be rounded upwards to the nearest power of 2 greater than or equal to 8.</param>
        /// <param name="itemEqualityComparer">The item EqualityComparer to use, primarily for item equality</param>
        protected ArrayListBase(TSize capacity, IEqualityComparer<T> itemEqualityComparer)
            : base(itemEqualityComparer)
        {
            if (Size.Compare(capacity, Size.Zero) <= 0)
            {
                throw new ArgumentOutOfRangeException("capacity");
            }

            TSize newLength = DEFAULT_CAPACITY;
            while (Size.Compare(newLength, capacity) < 0)
            {
                Size.MultiplyWith(ref newLength, 2);
            }
            _innerArray = Size.CreateArray<T>(newLength);
            _capacity = newLength;
        }

        #endregion

        #region Public properties

        public TSize Capacity
        {
            get { return _capacity; }
        }

        public override TSize Count
        {
            get { return _count; }
        }

        #endregion

        #region Find operations

        public override TSize IndexOf(T item)
        {
            for (long i = 0; i < Size.ToInt64(_count); i++)
            {
                if (ItemEqualityComparer.Equals(item, _innerArray[i]))
                {
                    return Size.FromInt64(i);
                }
            }
            return Size.Const(-1);
        }

        #endregion

        #region Add, Insert operations

        protected override void OnAdd(T item, out Iterator position)
        {
            position = End;

            TSize index = position.Key;
            EnsureSpaceExists(index, Size.Add(index, 1));
            _innerArray[Size.ToInt64(index)] = item;
        }

        protected override void OnAddRange<TInputSize, TInput>(
            ref TInput rangeBegin, TInput rangeEnd, out Range<T, TSize, Iterator> addedItems)
        {
            OnInsertRange(End, new Range<T, TInputSize, TInput>(rangeBegin, rangeEnd), out addedItems);
            rangeBegin = rangeEnd;
        }

        protected override void OnInsert(Iterator position, T item)
        {
            TSize index = position.Key;
            EnsureSpaceExists(index, Size.Add(index, 1));
            _innerArray[Size.ToInt64(index)] = item;
        }

        protected override void OnInsertRange<TInputSize, TInput>
            (Iterator position, IHive<T, TInputSize, TInput> range, out Range<T, TSize, Iterator> insertedRange)
        {
            TSize beginIndex = position.Key;
            TSize endIndex;

            TInputSize itemCount;
            if (range.TryGetCount(out itemCount))
            {
                endIndex = Size.Add(beginIndex, Size.From<TInputSize>(itemCount));
                EnsureSpaceExists(beginIndex, endIndex);

                long targetIndex = Size.ToInt64(beginIndex);
                for (TInput i = range.Begin; !i.Equals(range.End); i.Increment())
                {
                    _innerArray[targetIndex++] = i.Read();
                }
            }
            else
            {
                BufferedStream<T, TSize, TSizeOperations> snapshot =
                    BufferedStream<T, TSize, TSizeOperations>.Create(range);
                endIndex = Size.Add(beginIndex, snapshot.Count);

                if (Size.Compare(snapshot.Count, Size.Zero) > 0)
                {
                    EnsureSpaceExists(beginIndex, endIndex);
                    snapshot.CopyTo(_innerArray, beginIndex);
                }
            }

            insertedRange = new Range<T, TSize, Iterator>(position, new Iterator(this, endIndex));
        }

        private void EnsureSpaceExists(TSize fromIndex, TSize toIndex)
        {
            TSize itemCount = Size.Subtract(toIndex, fromIndex);
            TSize newEndIndex = Size.Add(_count, itemCount);

            if (Size.Compare(newEndIndex, _capacity) > 0)
            {
                ExpandAndInsertSpace(fromIndex, toIndex);
                return;
            }

            if (Size.Compare(fromIndex, _count) < 0)
            {
                Size.CopyArray(_innerArray, fromIndex, _count, _innerArray, toIndex);
            }
            _count = newEndIndex;
        }

        private void ExpandAndInsertSpace(TSize fromIndex, TSize toIndex)
        {
            TSize itemCount = Size.Subtract(toIndex, fromIndex);

            TSize minCapacity = Size.Add(_capacity, itemCount);
            TSize newCapacity = _capacity;
            while (Size.Compare(newCapacity, minCapacity) < 0)
            {
                Size.MultiplyWith(ref newCapacity, 2);
            }
            T[] newArray = Size.CreateArray<T>(newCapacity);

            if (Size.Compare(itemCount, Size.Zero) > 0 && Size.Compare(fromIndex, _count) < 0)
            {
                Size.CopyArray(_innerArray, Size.Zero, fromIndex, newArray, Size.Zero);
                Size.CopyArray(_innerArray, fromIndex, _count, newArray, toIndex);
            }
            else
            {
                Size.CopyArray(_innerArray, Size.Zero, _count, newArray, Size.Zero);
            }

            _innerArray = newArray;
            _capacity = newCapacity;
            Size.AddWith(ref _count, itemCount);
        }

        #endregion

        #region Update operations

        protected override void OnUpdate(Iterator position, T newItem)
        {
            _innerArray[Size.ToInt64(position.Key)] = newItem;
        }

        #endregion

        #region Remove, Clear operations

        protected override void OnRemoveAt(Iterator position)
        {
            TSize itemIndex = position.Key;
            Clear(itemIndex, Size.Add(itemIndex, 1));
        }

        protected override void OnRemoveRange(Range<T, TSize, Iterator> range)
        {
            Clear(range.Begin.Key, range.End.Key);
        }

        /// <summary>
        /// Remove all items and reset size of internal array container.
        /// </summary>
        protected sealed override void OnClear()
        {
            _innerArray = Size.CreateArray<T>(DEFAULT_CAPACITY);
            _capacity = DEFAULT_CAPACITY;
            _count = Size.Zero;
        }

        private void Clear(TSize fromIndex, TSize toIndex)
        {
            TSize itemCount = Size.Subtract(toIndex, fromIndex);
            TSize newEndIndex = Size.Subtract(_count, itemCount);
            if (Size.Compare(fromIndex, newEndIndex) < 0)
            {
                Size.CopyArray(_innerArray, toIndex, _count, _innerArray, fromIndex);
            }
            Size.ClearArray(_innerArray, newEndIndex, _count);
            _count = newEndIndex;
        }

        #endregion

        #region Read operations

        protected sealed override T OnRead(TSize index)
        {
            return _innerArray[Size.ToInt32(index)];
        }

        #endregion
    }
}
