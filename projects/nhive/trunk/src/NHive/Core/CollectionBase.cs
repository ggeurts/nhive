namespace NHive.Core
{
    using System;
    using System.Collections.Generic;
    using NHive.Core.Events;

    public abstract class CollectionBase<T, TSize, TSizeOperations, TIterator>
        : HiveBase<T, TSize, TSizeOperations, TIterator>
        , ICollection<T, TSize, TIterator>
        where TIterator : struct, IForwardIterator<T, TSize, TIterator>
        where TSize : struct, IConvertible
        where TSizeOperations : ISizeOperations<TSize>, new()
    {
        #region Static fields

        protected static CountDelegate<T, TSize, TIterator> CountImpl =
            CountAlgorithm.LinearSpeed<TIterator>;

        #endregion

        #region Fields

        /// <summary>
        /// The current collection version
        /// </summary>
        private long _revision = 0;

        private HiveEventPublisher<T, TSize, TIterator> _events =
            new HiveEventPublisher<T, TSize, TIterator>(HiveEvents.All);

        #endregion

        #region Constructor(s)

        protected CollectionBase(IEqualityComparer<T> itemEqualityComparer)
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

        public void AddRange(IEnumerable<T> itemsToAdd)
        {
            AddRange(itemsToAdd.GetEnumerator());
        }

        protected internal virtual void AddRange<TInputSize, TInput>(IHive<T, TInputSize, TInput> itemsToAdd)
            where TInput : struct, IInputIterator<T, TInputSize, TInput>
            where TInputSize : struct, IConvertible
        {
            AddRange(new HiveEnumerator<T, TInputSize, TInput>(itemsToAdd));
        }

        private void AddRange<TEnumerator>(TEnumerator itemsToAdd)
            where TEnumerator: IEnumerator<T>
        {
            if (!itemsToAdd.MoveNext()) return;

            BeginRevision();
            bool hasAddEventSubscribers = Events.HasAddEventSubscribers;
            do
            {
                TIterator position;
                OnAdd(itemsToAdd.Current, out position);
                if (hasAddEventSubscribers)
                {
                    Events.PublishAddedEvent(position);
                }
            } while (itemsToAdd.MoveNext());
            EndRevision();
        }

        protected abstract void OnAdd(T item, out TIterator position);

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

        public void RemoveRange(IHive<T> itemsToRemove)
        {
            RemoveRange(itemsToRemove.GetEnumerator());
        }

        protected internal void RemoveRange<TInputSize, TInput>(IHive<T, TInputSize, TInput> itemsToRemove)
            where TInput : struct, IInputIterator<T, TInputSize, TInput>
            where TInputSize : struct, IConvertible
        {
            RemoveRange(new HiveEnumerator<T, TInputSize, TInput>(itemsToRemove));
        }

        protected void RemoveRange<TEnumerator>(TEnumerator itemsToRemove)
            where TEnumerator: IEnumerator<T>
        {
            if (!itemsToRemove.MoveNext()) return;

            BeginRevision();
            bool hasRemoveEventSubscribers = Events.HasRemovingEventSubscribers;
            do
            {
                TIterator position;
                if (TryFind(itemsToRemove.Current, out position))
                {
                    if (hasRemoveEventSubscribers)
                    {
                        _events.PublishRemovingEvent(position);
                    }
                    OnRemoveAt(position);
                }
            } while (itemsToRemove.MoveNext());
            EndRevision();
        }

        protected abstract void OnRemoveAt(TIterator position);

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

        #region Protected internal methods - Version management

        public long Revision
        {
            get { return _revision; }
        }

        protected void BeginRevision()
        {
            ThrowIfReadOnly();
        }

        protected void EndRevision()
        {
            _revision++;
        }

        /// <summary>
        /// Check if the collection has been modified since a specified revision.
        /// </summary>
        /// <param name="revision">The revision that will be compared to the 
        /// current revision of the collection.</param>
        /// <exception cref="HiveModifiedException">The revision of this collection is greater than
        /// <paramref name="sinceRevision"/>.</exception>
        protected internal void ThrowIfModifiedSince(int revision)
        {
            if (_revision > revision)
                throw new HiveModifiedException();
        }

        #endregion
    }
}
