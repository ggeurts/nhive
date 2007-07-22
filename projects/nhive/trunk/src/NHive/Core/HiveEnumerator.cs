namespace NHive.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public struct HiveEnumerator<T, TSize, TIterator> 
        : IEnumerator<T>
        where TSize: struct, IConvertible
        where TIterator: struct, IInputIterator<T, TSize, TIterator>
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

        internal HiveEnumerator(TIterator begin, TIterator end)
        {
            _current = begin;
            _end = end;
            _state = State.Bof;
        }

        public HiveEnumerator(IIteratable<T, TSize, TIterator> hive)
            : this(hive.Begin, hive.End)
        { }

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
