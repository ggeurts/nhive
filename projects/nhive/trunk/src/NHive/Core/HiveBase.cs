namespace NHive.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

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
            count = Size.From(-1);
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

        public HiveEnumerator<T, TSize, TIterator> GetEnumerator()
        {
            return new HiveEnumerator<T, TSize, TIterator>(this);
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
