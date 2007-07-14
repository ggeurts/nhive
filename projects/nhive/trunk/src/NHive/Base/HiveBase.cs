namespace NHive.Base
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using NHive.Base.Iterators;
    using NHive.Base.Size;

    public abstract class HiveBase<T, TSize, TSizeOperations, TIterator> 
        : IHive<T, TSize, TIterator>
        , IInputIteratable<T, TSize, TIterator>
        where TIterator : struct, IInputIterator<T, TSize, TIterator>
        where TSize : struct, IConvertible
        where TSizeOperations : ISizeOperations<TSize>, new()
    {
        protected static readonly TSizeOperations Size = new TSizeOperations();

        #region Fields

        /// <summary>
        /// The current collection version
        /// </summary>
        private long _revision = 0;

        /// <summary>
        /// The item equalityComparer of the collection
        /// </summary>
        protected readonly IEqualityComparer<T> ItemEqualityComparer;

        #endregion

        #region Constructor(s)

        protected HiveBase(IEqualityComparer<T> itemEqualityComparer)
        {
            if (itemEqualityComparer == null)
            {
                throw new ArgumentNullException("itemEqualityComparer");
            }
            this.ItemEqualityComparer = itemEqualityComparer;
        }

        #endregion

        #region Public properties

        public bool IsEmpty 
        {
            get { return this.Begin.Equals(this.End); }
        }

        public abstract bool IsReadOnly { get; }
        public abstract TIterator Begin { get; }
        public abstract TIterator End { get; }

        #endregion

        #region Public methods - Query methods

        public virtual bool TryGetCount(out TSize count)
        {
            count = Size.Const(-1);
            return false;
        }

        public virtual P GetProperty<P>()
        {
            return default(P);
        }

        public void CopyTo(T[] array, TSize startIndex)
        {
            Algorithms.Copy<T, TSize, TIterator>(this, array, startIndex);
        }

        #endregion

        #region Public methods - GetEnumerator

        public Range<T, TSize, TIterator>.Enumerator GetEnumerator()
        {
            return new Range<T, TSize, TIterator>(this).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Protected internal methods - Access control

        protected internal void ThrowIfReadOnly()
        {
            if (this.IsReadOnly)
            {
                throw new NotSupportedException("Hive is readonly.");
            }
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

        protected static class CountAlgorithm
        {
            public static TSize LinearSpeed<TInput>(TInput begin, TInput end)
                where TInput : struct, IInputIterator<T, TSize, TInput>
            {
                TSize count = Size.Zero;
                for (TInput i = begin; !i.Equals(end); i.Increment())
                {
                    Size.Increment(ref count);
                }
                return count;
            }

            public static TSize ConstantSpeed<TInput>(TInput begin, TInput end)
                where TInput : struct, IRandomAccessIterator<T, TSize, TInput>
            {
                return end.DistanceFrom(begin);
            }
        }

    }
}
