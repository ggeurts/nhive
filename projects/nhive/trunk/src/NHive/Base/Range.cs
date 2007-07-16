namespace NHive.Base
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using NHive.Base.Size;

    /// <summary>
    /// Helper type that provides an efficient <see cref="IEnumerable{T}"/> implementation
    /// for any iteratable collection or range of items within a collection.
    /// </summary>
    public struct Range<T, TSize, TIterator> : IHive<T, TSize, TIterator>
        where TIterator : struct, IInputIterator<T, TSize, TIterator>
        where TSize : struct, IConvertible
    {
        private readonly TIterator _begin;
        private readonly TIterator _end;

        public Range(IIteratable<T, TSize, TIterator> hive)
        {
            _begin = hive.Begin;
            _end = hive.End;
        }

        public Range(TIterator begin, TIterator end)
        {
            if (!object.ReferenceEquals(begin.Parent, end.Parent))
            {
                throw new ArgumentException("Iterators must have same parent collection.");
            }

            _begin = begin;
            _end = end;
        }

        public TIterator Begin
        {
            get { return _begin; }
        }

        public TIterator End
        {
            get { return _end; }
        }

        public bool IsEmpty
        {
            get { return _begin.Equals(_end); }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public P GetProperty<P>()
        {
            return _begin.Parent != null
                ? _begin.Parent.GetProperty<P>()
                : default(P);
        }

        public bool TryGetCount(out TSize count)
        {
            return Algorithms.TryCount<T, TSize, TIterator>(_begin, _end, out count);
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        public struct Enumerator : IEnumerator<T>
        {
            private TIterator _current;
            private TIterator _end;

            private State _state;
            private enum State
            {
                Bof = 0,
                Iterating = 1,
                Eof = 2
            }

            public Enumerator(Range<T, TSize, TIterator> range)
            {
                _current = range._begin;
                _end = range._end;
                _state = State.Bof;
            }

            object IEnumerator.Current
            {
                get { return this.Current; }
            }

            public T Current
            {
                get
                {
                    if (_state == State.Bof)
                    {
                        throw new InvalidOperationException(
                            "Cannot retrieve current enumerator value before MoveNext method " +
                            "has been called at least once.");
                    }
                    return _current.Read();
                }
            }

            public void Dispose()
            {
                // nop
            }

            public bool MoveNext()
            {
                switch (_state)
                {
                    case State.Bof:
                        _state = State.Iterating;
                        break;
                    case State.Iterating:
                        _current.Increment();
                        break;
                    case State.Eof:
                        return false;
                }

                if (_current.Equals(_end))
                {
                    _state = State.Eof;
                    return false;
                }

                return true;
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }
    }
}
