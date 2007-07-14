/*
 Copyright (c) 2003-2006 Niels Kokholm and Peter Sestoft
 Permission is hereby granted, free of charge, to any person obtaining a copy
 of this software and associated documentation files (the "Software"), to deal
 in the Software without restriction, including without limitation the rights
 to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:
 
 The above copyright notice and this permission notice shall be included in
 all copies or substantial portions of the Software.
 
 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 SOFTWARE.
*/

using System;
using System.Diagnostics;
using SCG = System.Collections.Generic;

namespace C5
{
    internal abstract class MappedDirectedCollectionValue<T, V> : DirectedCollectionValueBase<V>, IDirectedCollectionValue<V>
    {
        private IDirectedCollectionValue<T> _directedCollectionValue;
        abstract public V Map(T item);

        public MappedDirectedCollectionValue(IDirectedCollectionValue<T> directedCollectionValue)
        {
            _directedCollectionValue = directedCollectionValue;
        }

        public override V Choose() 
        { 
            return Map(_directedCollectionValue.Choose()); 
        }

        public override bool IsEmpty 
        { 
            get { return _directedCollectionValue.IsEmpty; } 
        }

        public override int Count 
        { 
            get { return _directedCollectionValue.Count; } 
        }

        public override Speed CountSpeed 
        { 
            get { return _directedCollectionValue.CountSpeed; } 
        }

        public override IDirectedCollectionValue<V> Backwards()
        {
            MappedDirectedCollectionValue<T, V> retval = (MappedDirectedCollectionValue<T, V>) MemberwiseClone();
            retval._directedCollectionValue = _directedCollectionValue.Backwards();
            return retval;
        }

        public override SCG.IEnumerator<V> GetEnumerator()
        {
            foreach (T item in _directedCollectionValue)
            {
                yield return Map(item);
            }
        }

        public override EnumerationDirection Direction
        {
            get { return _directedCollectionValue.Direction; }
        }

        IDirectedEnumerable<V> IDirectedEnumerable<V>.Backwards()
        {
            return Backwards();
        }


    }

    abstract class MappedCollectionValue<T, V> : CollectionValueBase<V>, ICollectionValue<V>
    {
        ICollectionValue<T> _collectionValue;

        abstract public V Map(T item);

        public MappedCollectionValue(ICollectionValue<T> collectionValue)
        {
            _collectionValue = collectionValue;
        }

        public override V Choose() 
        { 
            return Map(_collectionValue.Choose()); 
        }

        public override bool IsEmpty 
        { 
            get { return _collectionValue.IsEmpty; } 
        }

        public override int Count 
        { 
            get { return _collectionValue.Count; } 
        }

        public override Speed CountSpeed 
        { 
            get { return _collectionValue.CountSpeed; } 
        }

        public override SCG.IEnumerator<V> GetEnumerator()
        {
            foreach (T item in _collectionValue)
            {
                yield return Map(item);
            }
        }
    }

    class MultiplicityOne<T> : MappedCollectionValue<T, Multiplicity<T>>
    {
        public MultiplicityOne(ICollectionValue<T> coll) 
            : base(coll) 
        { }

        public override Multiplicity<T> Map(T k) 
        {
            return new Multiplicity<T>(k, 1); 
        }
    }

    class DropMultiplicity<T> : MappedCollectionValue<Multiplicity<T>, T>
    {
        public DropMultiplicity(ICollectionValue<Multiplicity<T>> coll) 
            : base(coll) 
        { }

        public override T Map(Multiplicity<T> kvp) 
        { 
            return kvp.Value; 
        }
    }

    abstract class MappedDirectedEnumerable<T, V> : EnumerableBase<V>, IDirectedEnumerable<V>
    {
        private IDirectedEnumerable<T> _directedEnumerable;

        abstract public V Map(T item);

        public MappedDirectedEnumerable(IDirectedEnumerable<T> directedEnumerable)
        {
            _directedEnumerable = directedEnumerable;
        }

        public IDirectedEnumerable<V> Backwards()
        {
            MappedDirectedEnumerable<T, V> retval = (MappedDirectedEnumerable<T, V>) MemberwiseClone();
            retval._directedEnumerable = _directedEnumerable.Backwards();
            return retval;
        }

        public override SCG.IEnumerator<V> GetEnumerator()
        {
            foreach (T item in _directedEnumerable)
            {
                yield return Map(item);
            }
        }

        public EnumerationDirection Direction
        {
            get { return _directedEnumerable.Direction; }
        }
    }
}