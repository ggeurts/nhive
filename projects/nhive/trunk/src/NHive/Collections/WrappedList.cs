namespace NHive
{
    using System.Collections.Generic;
    using NHive.Base;

    public class WrappedList<T>: ListBase32<T>
    {
        #region Fields

        private IList<T> _innerList;

        #endregion

        #region Constructor(s)

        public WrappedList(IList<T> innerList)
            : base(EqualityComparer<T>.Default)
        {
            _innerList = innerList;
        }

        #endregion

        #region Public properties

        public override int Count
        {
            get { return _innerList.Count; }
        }

        public override bool IsReadOnly
        {
            get { return _innerList.IsReadOnly; }
        }

        #endregion

        #region Public overrides

        public sealed override int IndexOf(T item)
        {
            return _innerList.IndexOf(item);
        }

        #endregion

        #region Protected overrides

        protected sealed override T OnRead(int key)
        {
            return _innerList[key];
        }

        protected override void OnAdd(T item, out Iterator position)
        {
            position = this.End;
            _innerList.Add(item);
        }

        protected override void OnAddRange<TInputSize, TInput>(
            ref TInput rangeBegin, TInput rangeEnd, out Range<T, int, Iterator> addedItems)
        {
            Iterator addedBegin = End;
            for (TInput i = rangeBegin; !i.Equals(rangeEnd); i.Increment())
            {
                _innerList.Add(i.Read());
            }

            rangeBegin = rangeEnd;
            addedItems = new Range<T, int, Iterator>(addedBegin, End);
        }

        protected override void OnInsert(Iterator position, T item)
        {
            _innerList.Insert(position.Key, item);
        }

        protected override void OnInsertRange<TInputSize, TInput>(
            Iterator position, IHive<T, TInputSize, TInput> range, out Range<T, int, Iterator> insertedRange)
        {
            Iterator insertedEnd;
            if (position.IsEnd)
            {
                for (TInput i = range.Begin; !i.Equals(range.End); i.Increment())
                {
                    _innerList.Add(i.Read());
                }
                insertedEnd = End;
            }
            else
            {
                int targetIndex = position.Key;
                for (TInput i = range.Begin; !i.Equals(range.End); i.Increment())
                {
                    _innerList.Insert(targetIndex++, i.Read());
                }
                insertedEnd = new Iterator(this, targetIndex);
            }
            insertedRange = new Range<T, int, Iterator>(position, insertedEnd);
        }

        protected override void OnUpdate(Iterator position, T newItem)
        {
            _innerList[position.Key] = newItem;
        }

        protected override void OnRemoveAt(Iterator position)
        {
            _innerList.RemoveAt(position.Key);
        }

        protected override void OnRemoveRange(Range<T, int, Iterator> range)
        {
            for (int i = range.End.Previous.Key; i >= range.Begin.Key; i--)
            {
                _innerList.RemoveAt(i);
            }
        }

        protected override void OnClear()
        {
            _innerList.Clear();
        }

        #endregion
    }
}
