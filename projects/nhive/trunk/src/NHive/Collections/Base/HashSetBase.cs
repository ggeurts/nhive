namespace NHive.Collections.Base
{
    using System;
    using System.Collections.Generic;
    using NHive.Core;

    public abstract class HashSetBase<T, TSize, TSizeOperations>
        : CollectionBase<T, TSize, TSizeOperations, HashSetBase<T, TSize, TSizeOperations>.Iterator>
        where TSize : struct, IConvertible
        where TSizeOperations: struct, ISizeOperations<TSize>
    {
        #region Constants

        private const int DEFAULT_CAPACITY = 16;
        private const int MAX_BIT_COUNT = 8 * sizeof(int);
        private const double FILLFACTOR_DEFAULT = 0.66;
        private const double FILLFACTOR_MIN = 0.1;
        private const double FILLFACTOR_MAX = 0.9;

        #endregion

        #region Static fields

        private static Random _randomFactor = new Random();

        #endregion

        #region Fields

        private Bucket[] _table;
        private int _bitCount;
        private int _unusedBitCount;
        private int _indexMask;     // _indexMask == (1 << _bits) - 1;
        private int _resizeThreshhold;

        private readonly int _initialBitCount;
        private readonly double _fillFactor;
        private readonly uint _randomHashFactor;

        private TSize _count;
        private Iterator _begin;
        private readonly Iterator _end;

        #endregion

        #region Constructor(s)

        protected HashSetBase()
            : this(Size.From(DEFAULT_CAPACITY), FILLFACTOR_DEFAULT, EqualityComparer<T>.Default)
        { }

        protected HashSetBase(IEqualityComparer<T> itemEqualityComparer)
            : this(Size.From(DEFAULT_CAPACITY), FILLFACTOR_DEFAULT, itemEqualityComparer)
        { }

        /// <summary>
        /// Create a hash set with external item equalityComparer and default fill threshold (66%)
        /// </summary>
        /// <param name="capacity">Initial table size (rounded to power of 2, at least 16)</param>
        /// <param name="itemEqualityComparer">The external item equalityComparer</param>
        public HashSetBase(TSize capacity, IEqualityComparer<T> itemEqualityComparer)
            : this(capacity, FILLFACTOR_DEFAULT, itemEqualityComparer) 
        { }

        /// <summary>
        /// Create a hash set with external item equalityComparer.
        /// </summary>
        /// <param name="capacity">Initial table size (rounded to power of 2, at least 16)</param>
        /// <param name="fillFactor">Fill threshold (in range 10% to 90%)</param>
        /// <param name="itemEqualityComparer">The external item equalityComparer</param>
        public HashSetBase(TSize capacity, double fillFactor, IEqualityComparer<T> itemEqualityComparer)
            : base(itemEqualityComparer)
        {
            if (fillFactor < FILLFACTOR_MIN || fillFactor > FILLFACTOR_MAX)
                throw new ArgumentException("Fill outside valid range [0.1, 0.9]");
            if (Size.Compare(capacity, Size.Zero) <= 0)
                throw new ArgumentException("Capacity must be greater than zero");

            _fillFactor = fillFactor;
            _randomHashFactor = (2 * (uint) _randomFactor.Next() + 1) * 1529784659;

            _initialBitCount = CalculateBitCount(capacity);
            ResetBucketTable(_initialBitCount);

            _count = Size.Zero;
            _end = new Iterator(this, null);
            _begin = _end;
        }

        private int CalculateBitCount(TSize capacity)
        {
            Size.Decrement(ref capacity);

            int initialBitCount = 4;
            while (Size.Compare(Size.ShiftRight(capacity, initialBitCount), Size.Zero) > 0)
            {
                initialBitCount++;
            }
            
            return Math.Min(initialBitCount, MAX_BIT_COUNT);
        }

        #endregion

        #region Public properties

        public sealed override Iterator Begin
        {
            get { return new Iterator(this, _table[0]); }
        }

        public sealed override Iterator End
        {
            get { return _end; }
        }

        public override TSize Count
        {
            get { return _count; }
        }

        #endregion

        #region Contains/Find operations

        public override bool Contains(T item)
        {
            return FindBucketForItem(item) != null;
        }

        protected override bool TryFind(T item, out Iterator position)
        {
            Bucket bucket = FindBucketForItem(item);
            if (bucket != null)
            {
                position = new Iterator(this, bucket);
                return true;
            }
            
            position = _end;
            return false;
        }

        #endregion

        #region Add/Remove/Clear operations

        protected override void OnAdd(T item, out Iterator position)
        {
            int hashValue = ItemEqualityComparer.GetHashCode(item);
            int tableIndex = GetTableIndexForHashValue(hashValue);

            Bucket bucket = _table[tableIndex];
            if (bucket != null)
            {
                do
                {
                    if (hashValue == bucket.HashValue && ItemEqualityComparer.Equals(item, bucket.Item))
                    {
                        break;
                    }
                    bucket = bucket.Next;
                } while (bucket != null);
            }

            if (bucket == null)
            {
                bucket = new Bucket(item, hashValue, _table[tableIndex]);
                _table[tableIndex] = bucket;
                Size.Increment(ref _count);
            }

            position = new Iterator(this, bucket);
        }

        protected override void OnRemoveAt(Iterator position)
        {
            int tableIndex = GetTableIndexForHashValue(position.Bucket.HashValue);
            Bucket bucket = _table[tableIndex];
            
            if (bucket == position.Bucket)
            {
                _table[tableIndex] = bucket.Next;
                Size.Decrement(ref _count);
                return;
            }

            while (bucket != null)
            {
                Bucket nextBucket = bucket.Next;
                if (nextBucket == position.Bucket)
                {
                    bucket.Next = nextBucket.Next;
                    Size.Decrement(ref _count);
                    return;
                }
                bucket = nextBucket;
            }
        }

        protected override void OnClear()
        {
            ResetBucketTable(_initialBitCount);
            _count = Size.Zero;
        }

        #endregion

        #region Private operations

        private int GetTableIndexForHashValue(int hashValue)
        {
            return (int) (((uint) hashValue * _randomHashFactor) >> _unusedBitCount);
        }

        /// <summary>
        /// Searches for bucket that contains specified item.
        /// </summary>
        /// <param name="item">The item to be found.</param>
        /// <returns>Returns bucket that contains the item or <c>null</c> if the
        /// item is not present in the set.</returns>
        private Bucket FindBucketForItem(T item)
        {
            int hashValue = ItemEqualityComparer.GetHashCode(item);
            int tableIndex = GetTableIndexForHashValue(hashValue);

            Bucket bucket = _table[tableIndex];
            if (bucket != null)
            {
                do
                {
                    if (hashValue == bucket.HashValue && ItemEqualityComparer.Equals(item, bucket.Item))
                    {
                        break;
                    }
                    bucket = bucket.Next;
                } while (bucket != null);
            }

            return bucket;
        }

        /// <summary>
        /// Attempts to find first bucket after a specified bucket.
        /// </summary>
        /// <param name="bucket">The bucket for which the next bucket in the bucket table 
        /// must be found. A <c>null</c> value indicates that the first bucket in the bucket 
        /// table must be found.</param>
        /// <param name="nextBucket">The found bucket or <c>null</c> if no bucket was found.</param>
        /// <returns>Returns <c>true</c> if the next bucket was found. Otherwise 
        /// returns <c>false</c>.</returns>
        private bool TryFindBucketAfter(Bucket bucket, out Bucket nextBucket)
        {
            int tableIndex;
            if (bucket != null)
            {
                if (bucket.Next != null)
                {
                    nextBucket = bucket.Next;
                    return true;
                }
                tableIndex = GetTableIndexForHashValue(bucket.HashValue);
            }
            else
            {
                tableIndex = 0;
            }

            for (; tableIndex < _table.Length; tableIndex++)
            {
                if (_table[tableIndex] != null)
                {
                    nextBucket = _table[tableIndex];
                    return true;
                }
            }

            nextBucket = null;
            return false;
        }

        private void ExpandBucketTable()
        {
            ResizeBucketTable(_bitCount + 1);
        }

        private void ShrinkBucketTable()
        {
            if (_bitCount > 3)
            {
                ResizeBucketTable(_bitCount - 1);
            }
        }

        private void ResizeBucketTable(int bits)
        {
            Bucket[] oldTable = _table;

            ResetBucketTable(bits);
            for (int oldTableIndex = 0; oldTableIndex < oldTable.Length; oldTableIndex++)
            {
                Bucket bucket = oldTable[oldTableIndex];
                while (bucket != null)
                {
                    int newTableIndex = GetTableIndexForHashValue(bucket.HashValue);

                    _table[newTableIndex] = new Bucket(bucket.Item, bucket.HashValue, _table[newTableIndex]);
                    bucket = bucket.Next;
                }
            }
        }

        private void ResetBucketTable(int newBitCount)
        {
            if (newBitCount > MAX_BIT_COUNT) return;

            int tableLength = 1 << newBitCount;
            _table = new Bucket[tableLength];
            _bitCount = newBitCount;
            _unusedBitCount = MAX_BIT_COUNT - newBitCount;
            _indexMask = tableLength - 1;
            _resizeThreshhold = (int)(tableLength * _fillFactor);
        }

        #endregion

        #region Iterator implementation

        public struct Iterator : IForwardIterator<T, TSize, Iterator>
        {
            private HashSetBase<T, TSize, TSizeOperations> _parent;
            private Bucket _bucket;

            #region Constructor(s)

            internal Iterator(HashSetBase<T, TSize, TSizeOperations> parent, Bucket bucket)
            {
                _parent = parent;
                _bucket = bucket;
            }

            #endregion

            #region Properties

            internal Bucket Bucket
            {
                get { return _bucket; }
            }

            public IHive<T> Parent
            {
                get { return _parent; }
            }

            #endregion

            #region IIterator<T,Iterator> Members

            public P GetProperty<P>()
            {
                return _parent.GetProperty<P>();
            }

            public void Increment()
            {
                if (_bucket == null) return;

                if (!_parent.TryFindBucketAfter(_bucket, out _bucket))
                {
                    _bucket = _parent.End.Bucket;
                }
            }

            public T Read()
            {
                return _bucket.Item;
            }

            #endregion

            #region IEquatable<Iterator> Members

            public bool Equals(Iterator other)
            {
                return object.ReferenceEquals(_bucket, other._bucket)
                    && object.ReferenceEquals(_parent, other._parent);
            }

            #endregion
        }

        #endregion

        #region Bucket class

        [Serializable]
        protected internal class Bucket
        {
            public readonly T Item;
            public readonly int HashValue; //Cache!
            public Bucket Next;

            public Bucket(T item, int hashval, Bucket overflow)
            {
                this.Item = item;
                this.HashValue = hashval;
                this.Next = overflow;
            }
        }

        #endregion
    }
}
