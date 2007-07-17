namespace NHive.Base
{
    using System;
    using System.Collections.Generic;
    using NHive.Base.Size;

    public abstract class StreamedHiveBase<T, TSize, TSizeOperations>
        : HiveBase<T, TSize, TSizeOperations, StreamedHiveBase<T, TSize, TSizeOperations>.Iterator>
        where TSize: struct, IConvertible
        where TSizeOperations : ISizeOperations<TSize>, new()
    {
        private Iterator _current;
        private Iterator _end;
        private T _value;
        bool _isBof;

        protected StreamedHiveBase(IEqualityComparer<T> itemEqualityComparer)
            : base(itemEqualityComparer)
        {
            _isBof = true;
            _end = new Iterator(this, true);
        }

        #region Public properties

        public sealed override Iterator Begin
        {
            get 
            {
                if (_isBof)
                {
                    _isBof = false;
                    _current = OnTryRead(out _value)
                        ? new Iterator(this, false)
                        : _end;
                }
                return _current;
            }
        }

        public sealed override Iterator End
        {
            get { return _end; }
        }

        public override bool IsReadOnly
        {
            get { return true; }
        }

        #endregion

        #region Read operations

        private T Value
        {
            get 
            {
                return _value; 
            }
        }

        private bool TryReadNext()
        {
            return !_current.IsEnd && OnTryRead(out _value);
        }

        protected abstract bool OnTryRead(out T value);

        #endregion

        #region Iterator struct

        public struct Iterator: IInputIterator<T, TSize, Iterator>
        {
            private StreamedHiveBase<T, TSize, TSizeOperations> _parent;
            private bool _isEnd;

            internal Iterator(StreamedHiveBase<T, TSize, TSizeOperations> parent, bool isEnd)
            {
                _parent = parent;
                _isEnd = isEnd;
            }

            internal bool IsEnd
            {
                get { return _isEnd; }
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

            public IHive<T> Parent
            {
                get { return _parent; }
            }

            public bool Equals(Iterator other)
            {
                return object.ReferenceEquals(this._parent, other._parent)
                    && this._isEnd == other._isEnd;
            }

            public override int GetHashCode()
            {
                return _parent == null ? 0 : _parent.GetHashCode();
            }

            public P GetProperty<P>()
            {
                return _parent == null ? default(P) : _parent.GetProperty<P>();
            }

            public void Increment()
            {
                if (_parent == null) return;
                if (!_parent.TryReadNext())
                {
                    _isEnd = true;
                }
            }

            public T Read()
            {
                return _parent == null ? default(T) : _parent.Value;
            }
        }

        #endregion
    }
}
