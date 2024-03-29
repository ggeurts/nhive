namespace NHive.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    public abstract class RandomAccessCollectionBase<T, TSize, TSizeOperations>
        : CollectionBase<T, TSize, TSizeOperations, RandomAccessCollectionBase<T, TSize, TSizeOperations>.Iterator>
        , IRandomAccessIteratable<T, TSize, RandomAccessCollectionBase<T, TSize, TSizeOperations>.Iterator>
        where TSize : struct, IConvertible
        where TSizeOperations : ISizeOperations<TSize>, new()
    {
        #region Static fields

        protected static new CountDelegate<T, TSize, Iterator> CountImpl =
            CountAlgorithm.ConstantSpeed<Iterator>;

        #endregion

        #region Constructor(s)

        protected RandomAccessCollectionBase(IEqualityComparer<T> itemEqualityComparer)
            : base(itemEqualityComparer)
        { }

        #endregion

        #region Public properties

        public sealed override Iterator Begin
        {
            get { return new Iterator(this, Size.Zero); }
        }

        public sealed override Iterator End
        {
            get { return new Iterator(this, this.Count); }
        }

        public T this[TSize key]
        {
            get
            {
                return OnRead(key);
            }
            set
            {
                Update(new Iterator(this, key), value);
            }
        }

        #endregion

        #region Update operations

        protected void Update(Iterator position, T newItem)
        {
            BeginRevision();

            if (Events.HasRemovingEventSubscribers)
            {
                Events.PublishRemovingEvent(position);
            }
            OnUpdate(position, newItem);
            if (Events.HasAddEventSubscribers)
            {
                Events.PublishAddedEvent(position);
            }

            EndRevision();
        }

        protected abstract void OnUpdate(Iterator position, T newItem);

        #endregion

        #region Remove operations

        public void RemoveAt(TSize index)
        {
            base.RemoveAt(new Iterator(this, index));
        }

        public void RemoveRange(TSize startIndex, TSize count)
        {
            RemoveRange(
                new Range<T, TSize, Iterator>(
                    new Iterator(this, startIndex),
                    new Iterator(this, Size.Add(startIndex, count))));
        }

        protected void RemoveRange(Range<T, TSize, Iterator> itemsToRemove)
        {
            if (itemsToRemove.IsEmpty) return;

            BeginRevision();
            Events.PublishRemovingEvent(itemsToRemove);
            OnRemoveRange(itemsToRemove);
            EndRevision();
        }

        protected abstract void OnRemoveRange(Range<T, TSize, Iterator> range);

        #endregion

        #region GetProperty<P>

        public override P GetProperty<P>()
        {
            if (typeof(P) == typeof(CountDelegate<T, TSize, Iterator>))
            {
                return (P) (object) CountImpl;
            }
            return base.GetProperty<P>();
        }

        #endregion

        #region Read operations

        protected abstract T OnRead(TSize key);

        #endregion

        public struct Iterator : IRandomAccessIterator<T, TSize, Iterator>
        {
            #region Static fields

            private static readonly Iterator Null = new Iterator();

            #endregion

            #region Fields

            private readonly RandomAccessCollectionBase<T, TSize, TSizeOperations> _parent;
            private TSize _key;

            #endregion

            #region Constructor(s) and Factory methods

            public Iterator(RandomAccessCollectionBase<T, TSize, TSizeOperations> parent, TSize key)
            {
                _parent = parent;
                _key = key;
            }

            private static Iterator CreateWithBoundaryCheck(
                RandomAccessCollectionBase<T, TSize, TSizeOperations> parent, TSize key)
            {
                if (parent == null)
                {
                    return Iterator.Null;
                }
                else if (Size.Compare(key, parent.End._key) >= 0)
                {
                    return parent.End;
                }
                else if (Size.Compare(key, parent.Begin._key) <= 0)
                {
                    return parent.Begin;
                }
                else
                {
                    return new Iterator(parent, key);
                }
            }

            #endregion

            #region Properties

            private bool IsNull
            {
                get { return _parent == null; }
            }

            public TSize Key
            {
                get { return _key; }
            }

            #endregion

            #region IRandomAccessIterator members

            public IHive<T> Parent
            {
                get { return _parent; }
            }

            public bool IsEnd
            {
                get { return IsNull ? true : Size.Equals(_key, _parent.Count); }
            }

            public Iterator Next
            {
                get { return CreateWithBoundaryCheck(_parent, Size.Add(_key, 1)); }
            }

            public Iterator Previous
            {
                get { return CreateWithBoundaryCheck(_parent, Size.Subtract(_key, 1)); }
            }

            public int CompareTo(Iterator other)
            {
                return Size.Compare(_key, other._key);
            }

            public bool Equals(Iterator other)
            {
                return Size.Equals(_key, other._key)
                    && object.ReferenceEquals(_parent, other._parent);
            }

            public P GetProperty<P>()
            {
                return _parent.GetProperty<P>();
            }

            public Iterator OffsetBy(TSize offset)
            {
                return CreateWithBoundaryCheck(_parent, Size.Add(_key, offset));
            }

            public TSize DistanceFrom(Iterator from)
            {
                return IsNull ? Size.Zero : Size.Subtract(_key, from._key);
            }

            public void Increment()
            {
                if (Size.Compare(_key, _parent.End._key) < 0)
                {
                    Size.Increment(ref _key);
                }
            }

            public void Decrement()
            {
                if (Size.Compare(_key, _parent.Begin._key) > 0)
                {
                    Size.Decrement(ref _key);
                }
            }

            public T Read()
            {
                return IsNull
                    ? default(T)
                    : _parent[_key];
            }

            public void Write(T value)
            {
                if (!IsNull)
                {
                    _parent.Update(this, value);
                }
            }

            #endregion
        }
    }
}
