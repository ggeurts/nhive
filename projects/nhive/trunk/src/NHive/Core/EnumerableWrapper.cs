namespace NHive.Core
{
    using System;
    using System.Collections.Generic;
    using NHive.Core;

    internal sealed class EnumerableWrapper<T, TSize, TSizeOperations> 
        : StreamedHiveBase<T, TSize, TSizeOperations>
        where TSize: struct, IConvertible
        where TSizeOperations: ISizeOperations<TSize>, new()
    {
        private IEnumerable<T> _items;
        private IEnumerator<T> _innerStream;

        public EnumerableWrapper(IEnumerable<T> items)
            : this(EqualityComparer<T>.Default, items)
        { }

        public EnumerableWrapper(EqualityComparer<T> itemEqualityComparer, IEnumerable<T> items)
            : base(itemEqualityComparer)
        {
            _items = items;
            _innerStream = items.GetEnumerator();
        }

        public override bool TryGetCount(out TSize count)
        {
            return Algorithms.TryCount<T, TSize>(_items, out count);
        }

        protected override bool OnTryRead(out T value)
        {
            if (_innerStream.MoveNext())
            {
                value = _innerStream.Current;
                return true;
            }

            value = default(T);
            return false;
        }
    }
}
