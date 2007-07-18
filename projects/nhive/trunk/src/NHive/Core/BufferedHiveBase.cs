namespace NHive.Core
{
    using System;
    using System.Collections.Generic;
    using NHive.Core.Events;

    public abstract class BufferedHiveBase<T, TSize, TSizeOperations, TIterator>
        : HiveBase<T, TSize, TSizeOperations, TIterator>
        , IBufferedHive<T, TSize, TIterator>
        where TIterator: struct, IForwardIterator<T, TSize, TIterator>
        where TSize : struct, IConvertible
        where TSizeOperations: ISizeOperations<TSize>, new()
    {
        #region Static fields

        protected static CountDelegate<T, TSize, TIterator> CountImpl = 
            CountAlgorithm.LinearSpeed<TIterator>;

        #endregion

        #region Fields

        private HiveEventPublisher<T, TSize, TIterator> _events =
            new HiveEventPublisher<T, TSize, TIterator>(HiveEvents.All);

        #endregion

        #region Constructor(s)

        protected BufferedHiveBase(IEqualityComparer<T> itemEqualityComparer)
            : base(itemEqualityComparer)
        { }

        #endregion

        #region properties

        public override bool IsReadOnly
        {
            get { return false; }
        }

        protected HiveEventPublisher<T, TSize, TIterator> Events
        {
            get { return _events; }
        }

        #endregion

        #region Count

        public sealed override bool TryGetCount(out TSize count)
        {
            count = this.Count;
            return true;
        }

        public abstract TSize Count { get; }

        #endregion

        #region Find operations

        public abstract bool Contains(T item);
        protected abstract bool TryFind(T item, out TIterator position);

        #endregion

        #region Add operations

        public void Add(T item)
        {
            BeginRevision();

            TIterator position;
            OnAdd(item, out position);
            if (_events.HasAddEventSubscribers)
            {
                _events.PublishAddedEvent(position);
            }

            EndRevision();
        }

        public void AddRange(IEnumerable<T> items)
        {
            AddRange<TSize, EnumerableWrapper<T, TSize, TSizeOperations>.Iterator>(
                new EnumerableWrapper<T, TSize, TSizeOperations>(items));
        }

        protected void AddRange<TInputSize, TInput>(IHive<T, TInputSize, TInput> itemsToAdd)
            where TInput : struct, IInputIterator<T, TInputSize, TInput>
            where TInputSize: struct, IConvertible
        {
            if (itemsToAdd.IsEmpty) return;
            BeginRevision();

            TInput begin = itemsToAdd.Begin;
            TInput end = itemsToAdd.End;
            do
            {
                Range<T, TSize, TIterator> addedItems;
                OnAddRange<TInputSize, TInput>(ref begin, end, out addedItems);
                if (_events.HasAddEventSubscribers)
                {
                    _events.PublishAddedEvent(addedItems);
                }
            } while (!begin.Equals(end));

            EndRevision();
        }

        protected abstract void OnAdd(T item, out TIterator position);

        /// <summary>
        /// Adds one or more items to collection.
        /// </summary>
        /// <typeparam name="TInputSize">The range size type.</typeparam>
        /// <typeparam name="TInput">The range iterator type</typeparam>
        /// <param name="nextInRange">Iterator that both before and after invocation of this method must 
        /// point to first item in range that has not yet been added to the collection. Must be equal 
        /// to <paramref name="endOfRange"/> when all items in the range have been added to the collection.
        /// </param>
        /// <param name="endOfRange">Iterator that points to the end of the range.</param>
        /// <param name="addedItems">Returns the items that have been added to the collection. Must return at 
        /// least one item if the input range is not empty (that is <paramref name="nextInRange"/> is not 
        /// equal to <paramref name="endOfRange"/>).</param>
        protected abstract void OnAddRange<TInputSize, TInput>(ref TInput nextInRange, TInput endOfRange, out Range<T, TSize, TIterator> addedItems)
            where TInput : struct, IInputIterator<T, TInputSize, TInput>
            where TInputSize: struct, IConvertible;

        #endregion

        #region Remove operations

        public bool Remove(T item)
        {
            TIterator position;
            if (TryFind(item, out position))
            {
                RemoveAt(position);
                return true;
            }
            return false;
        }

        protected void RemoveAt(TIterator position)
        {
            BeginRevision();
            if (_events.HasRemovingEventSubscribers)
            {
                _events.PublishRemovingEvent(position);
            }
            OnRemoveAt(position);
            EndRevision();
        }

        protected void RemoveRange(Range<T, TSize, TIterator> range)
        {
            if (range.IsEmpty) return;

            BeginRevision();
            _events.PublishRemovingEvent(range);
            OnRemoveRange(range);
            EndRevision();
        }

        protected abstract void OnRemoveAt(TIterator position);
        protected abstract void OnRemoveRange(Range<T, TSize, TIterator> range);

        #endregion

        #region Update operations

        protected void Update(TIterator position, T newItem)
        {
            BeginRevision();

            if (_events.HasRemovingEventSubscribers)
            {
                _events.PublishRemovingEvent(position);
            }
            OnUpdate(position, newItem);
            if (_events.HasAddEventSubscribers)
            {
                _events.PublishAddedEvent(position);
            }

            EndRevision();
        }

        protected abstract void OnUpdate(TIterator position, T newItem);

        #endregion

        #region Clear

        public void Clear()
        {
            BeginRevision();
            OnClear();
            EndRevision();
        }

        protected abstract void OnClear();

        #endregion

        #region GetProperty<P>

        public override P GetProperty<P>()
        {
            if (typeof(P) == typeof(CountDelegate<T, TSize, TIterator>))
            {
                return (P) (object) CountImpl;
            }
            return base.GetProperty<P>();
        }

        #endregion
    }
}