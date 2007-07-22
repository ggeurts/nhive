namespace NHive.Core
{
    using System;
    using System.Collections.Generic;

    public abstract class ListBase<T, TSize, TSizeOperations>
        : RandomAccessCollectionBase<T, TSize, TSizeOperations>
        , IList<T, TSize>
        where TSize : struct, IConvertible
        where TSizeOperations : ISizeOperations<TSize>, new()
    {
        #region Constructor(s)

        protected ListBase(IEqualityComparer<T> itemEqualityComparer)
            : base(itemEqualityComparer)
        { }

        #endregion

        #region Find operations

        public abstract TSize IndexOf(T item);

        public sealed override bool Contains(T item)
        {
            return Size.Compare(IndexOf(item), Size.Zero) >= 0;
        }

        protected sealed override bool TryFind(T item, out Iterator position)
        {
            TSize index = IndexOf(item);
            if (Size.Compare(index, Size.Zero) >= 0)
            {
                position = new Iterator(this, index);
                return true;
            }
            else
            {
                position = default(Iterator);
                return false;
            }
        }

        #endregion

        #region Add operations

        protected internal override void AddRange<TInputSize, TInput>(IHive<T, TInputSize, TInput> itemsToAdd)
        {
            InsertRange(Count, itemsToAdd);
        }

        #endregion

        #region Insert operations

        /// <summary>
        /// Insert an item at a specific index, moving items to the right
        /// upwards and expanding the array if necessary.
        /// </summary>
        /// <param name="index">The index at which to insert.</param>
        /// <param name="item">The item to insert.</param>
        /// <exception cref="ArgumentOutOfRangeException">Parameter <see cref="index"/> is less than
        /// <c>0</c> or greater than <see cref="Count"/>.</exception>
        public void Insert(TSize index, T item)
        {
            if (Size.Compare(index, this.Count) > 0 || Size.Compare(index, Size.Zero) < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            BeginRevision();

            Iterator position = new Iterator(this, index);
            OnInsert(position, item);
            if (Events.HasAddEventSubscribers)
            {
                Events.PublishAddedEvent(position);
            }

            EndRevision();
        }

        public void InsertRange(TSize index, IEnumerable<T> items)
        {
            if (Size.Compare(index, this.Count) > 0 || Size.Compare(index, Size.Zero) < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            WrappedEnumerable<T, TSize, TSizeOperations> range = 
                new WrappedEnumerable<T, TSize, TSizeOperations>(items);
            if (range.IsEmpty) return;

            BeginRevision();

            Iterator position = new Iterator(this, index);
            Range<T, TSize, Iterator> insertedRange;
            OnInsertRange<TSize, WrappedEnumerable<T, TSize, TSizeOperations>.Iterator>(
                position, range, out insertedRange);
            if (Events.HasAddEventSubscribers)
            {
                Events.PublishAddedEvent(new Range<T, TSize, Iterator>(insertedRange));
            }

            EndRevision();
        }

        protected abstract void OnInsert(Iterator position, T item);
        protected abstract void OnInsertRange<TInputSize, TInput>
            (Iterator insertBegin, IHive<T, TInputSize, TInput> range, out Range<T, TSize, Iterator> insertedRange)
            where TInput : struct, IInputIterator<T, TInputSize, TInput>
            where TInputSize : struct, IConvertible;

        #endregion
    }
}
