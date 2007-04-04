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
using System.Text;

namespace C5
{
    /// <summary>
    /// Static helper methods for creation and display of (key, value) pairs.
    /// </summary>
    internal static class KeyValuePair
    {
        public static SCG.KeyValuePair<K, V> Create<K, V>(K key)
        {
            return new SCG.KeyValuePair<K, V>(key, default(V));
        }

        public static bool Show<K, V>(SCG.KeyValuePair<K, V> pair, StringBuilder sb, ref int rest, IFormatProvider formatProvider)
        {
            if (rest < 0)
                return false;
            if (!Showing.Show(pair.Key, sb, ref rest, formatProvider))
                return false;
            sb.Append(" => ");
            rest -= 4;
            if (!Showing.Show(pair.Value, sb, ref rest, formatProvider))
                return false;
            return rest >= 0;
        }
    }

    /// <summary>
    /// Default comparer for dictionary entries in a sorted dictionary.
    /// Entry comparisons only look at keys and uses an externally defined comparer for that.
    /// </summary>
    [Serializable]
    public sealed class KeyValuePairComparer<K, V> : SCG.IComparer<SCG.KeyValuePair<K, V>>
    {
        private SCG.IComparer<K> _comparer;

        /// <summary>
        /// Create an entry comparer for a item comparer of the keys
        /// </summary>
        /// <param name="comparer">Comparer of keys</param>
        public KeyValuePairComparer(SCG.IComparer<K> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            _comparer = comparer;
        }

        /// <summary>
        /// Compare two entries
        /// </summary>
        /// <param name="entry1">First entry</param>
        /// <param name="entry2">Second entry</param>
        /// <returns>The result of comparing the keys</returns>
        [Tested]
        public int Compare(SCG.KeyValuePair<K, V> entry1, SCG.KeyValuePair<K, V> entry2)
        {
            return _comparer.Compare(entry1.Key, entry2.Key);
        }
    }

    /// <summary>
    /// Default equalityComparer for dictionary entries.
    /// Operations only look at keys and uses an externaly defined equalityComparer for that.
    /// </summary>
    [Serializable]
    public sealed class KeyValuePairEqualityComparer<K, V> : SCG.IEqualityComparer<SCG.KeyValuePair<K, V>>
    {
        SCG.IEqualityComparer<K> _keyEqualityComparer;

        #region Constructors

        /// <summary>
        /// Create an entry equalityComparer using the default equalityComparer for keys
        /// </summary>
        public KeyValuePairEqualityComparer()
        {
            _keyEqualityComparer = EqualityComparer<K>.Default;
        }

        /// <summary>
        /// Create an entry equalityComparer from a specified item equalityComparer for the keys
        /// </summary>
        /// <param name="keyEqualityComparer">The key equalityComparer</param>
        public KeyValuePairEqualityComparer(SCG.IEqualityComparer<K> keyEqualityComparer)
        {
            if (keyEqualityComparer == null)
            {
                throw new ArgumentNullException("keyEqualityComparer");
            }
            _keyEqualityComparer = keyEqualityComparer;
        }

        #endregion

        #region IEqualityComparer<SCG.KeyValuePair<K,V>> Members

        /// <summary>
        /// Test two entries for equality
        /// </summary>
        /// <param name="x">First entry</param>
        /// <param name="y">Second entry</param>
        /// <returns>True if keys are equal</returns>
        public bool Equals(SCG.KeyValuePair<K, V> x, SCG.KeyValuePair<K, V> y)
        {
            return _keyEqualityComparer.Equals(x.Key, y.Key);
        }

        /// <summary>
        /// Get the hash code of the entry
        /// </summary>
        /// <param name="obj">The entry</param>
        /// <returns>The hash code of the key</returns>
        public int GetHashCode(SCG.KeyValuePair<K, V> obj)
        {
            return _keyEqualityComparer.GetHashCode(obj.Key);
        }

        #endregion
    }

    /// <summary>
    /// A base class for implementing a dictionary based on a set collection implementation.
    /// <i>See the source code for <see cref="T:C5.HashDictionary`2"/> for an example</i>
    /// </summary>
    [Serializable]
    public abstract class DictionaryBase<K, V> : CollectionValueBase<SCG.KeyValuePair<K, V>>, IDictionary<K, V>, SCG.IDictionary<K, V>
    {
        #region Fields

        private readonly ICollection<SCG.KeyValuePair<K, V>> _pairs;
        private readonly SCG.IEqualityComparer<K> _keyEqualityComparer;

        protected ICollection<SCG.KeyValuePair<K, V>> Pairs
        {
            get { return _pairs; }
        }

        #endregion

        #region Events
        
        private ProxyEventBlock<SCG.KeyValuePair<K, V>> _eventBlock;

        private ProxyEventBlock<SCG.KeyValuePair<K, V>> EventBlock
        {
            get
            {
                if (_eventBlock == null)
                {
                    _eventBlock = new ProxyEventBlock<SCG.KeyValuePair<K, V>>(this, _pairs);
                }
                return _eventBlock;
            }
        }

        /// <summary>
        /// The change event. Will be raised for every change operation on the collection.
        /// </summary>
        public override event CollectionChangedHandler<SCG.KeyValuePair<K, V>> CollectionChanged
        {
            add { EventBlock.CollectionChanged += value; }
            remove { EventBlock.CollectionChanged -= value; }
        }

        /// <summary>
        /// The change event. Will be raised for every change operation on the collection.
        /// </summary>
        public override event CollectionClearedHandler<SCG.KeyValuePair<K, V>> CollectionCleared
        {
            add { EventBlock.CollectionCleared += value; }
            remove { EventBlock.CollectionCleared -= value; }
        }

        /// <summary>
        /// The item added  event. Will be raised for every individual addition to the collection.
        /// </summary>
        public override event ItemsAddedHandler<SCG.KeyValuePair<K, V>> ItemsAdded
        {
            add { EventBlock.ItemsAdded += value; }
            remove { EventBlock.ItemsAdded -= value; }
        }

        /// <summary>
        /// The item added  event. Will be raised for every individual removal from the collection.
        /// </summary>
        public override event ItemsRemovedHandler<SCG.KeyValuePair<K, V>> ItemsRemoved
        {
            add { EventBlock.ItemsRemoved += value; }
            remove { EventBlock.ItemsRemoved -= value; }
        }

        public override EventTypeEnum ListenableEvents
        {
            get { return EventTypeEnum.Basic; }
        }

        public override EventTypeEnum ActiveEvents
        {
            get { return _pairs.ActiveEvents; }
        }

        #endregion

        #region Constructors

        protected DictionaryBase(ICollection<SCG.KeyValuePair<K, V>> pairs, SCG.IEqualityComparer<K> keyEqualityComparer)
        {
            if (pairs == null)
            {
                throw new ArgumentNullException("pairs");
            }
            if (keyEqualityComparer == null)
            {
                throw new ArgumentNullException("keyEqualityComparer");
            }

            _pairs = pairs;
            _keyEqualityComparer = keyEqualityComparer;
        }

        #endregion

        #region IDictionary<K,V> Members

        public virtual SCG.IEqualityComparer<K> EqualityComparer
        {
            get { return _keyEqualityComparer; }
        }

        /// <summary>
        /// Add a new (key, value) pair (a mapping) to the dictionary.
        /// </summary>
        /// <exception cref="DuplicateNotAllowedException"> if there already is an entry with the same key. </exception>
        /// <param name="key">Key to add</param>
        /// <param name="value">Value to add</param>
        [Tested]
        public void Add(K key, V value)
        {
            Add(new SCG.KeyValuePair<K, V>(key, value));
        }

        /// <summary>
        /// Add a new (key, value) pair (a mapping) to the dictionary.
        /// </summary>
        /// <exception cref="DuplicateNotAllowedException"> if there already is an entry with the same key. </exception>
        /// <param name="pair">(key, value) pair to add</param>
        private void Add(SCG.KeyValuePair<K, V> pair)
        {
            if (!_pairs.Add(pair))
            {
                throw new DuplicateNotAllowedException("Key being added: '" + pair.Key + "'");
            }
        }

        /// <summary>
        /// Add the entries from a collection of <see cref="T:C5.KeyValuePair`2"/> pairs to this dictionary.
        /// <para><b>TODO: add restrictions L:K and W:V when the .Net SDK allows it </b></para>
        /// </summary>
        /// <exception cref="DuplicateNotAllowedException"> 
        /// If the input contains duplicate keys or a key already present in this dictionary.</exception>
        /// <param name="entries"></param>
        public virtual void AddAll<L, W>(SCG.IEnumerable<SCG.KeyValuePair<L, W>> entries)
            where L : K
            where W : V
        {
            foreach (SCG.KeyValuePair<L, W> pair in entries)
            {
                Add(new SCG.KeyValuePair<K, V>(pair.Key, pair.Value));
            }
        }

        /// <summary>
        /// Remove an entry with a given key from the dictionary
        /// </summary>
        /// <param name="key">The key of the entry to remove</param>
        /// <returns>True if an entry was found (and removed)</returns>
        [Tested]
        public virtual bool Remove(K key)
        {
            return _pairs.Remove(KeyValuePair.Create<K, V>(key));
        }

        /// <summary>
        /// Remove an entry with a given key from the dictionary and report its value.
        /// </summary>
        /// <param name="key">The key of the entry to remove</param>
        /// <param name="value">On exit, the value of the removed entry</param>
        /// <returns>True if an entry was found (and removed)</returns>
        [Tested]
        public virtual bool Remove(K key, out V value)
        {
            SCG.KeyValuePair<K, V> p = KeyValuePair.Create<K, V>(key);

            if (_pairs.Remove(p, out p))
            {
                value = p.Value;
                return true;
            }
            else
            {
                value = default(V);
                return false;
            }
        }

        /// <summary>
        /// Remove all entries from the dictionary
        /// </summary>
        [Tested]
        public virtual void Clear()
        {
            _pairs.Clear();
        }

        public virtual Speed ContainsSpeed
        {
            get { return _pairs.ContainsSpeed; }
        }

        /// <summary>
        /// Check if there is an entry with a specified key
        /// </summary>
        /// <param name="key">The key to look for</param>
        /// <returns>True if key was found</returns>
        [Tested]
        public virtual bool ContainsKey(K key)
        {
            return _pairs.Contains(KeyValuePair.Create<K, V>(key));
        }

        private class LiftedEnumerable<H> : SCG.IEnumerable<SCG.KeyValuePair<K, V>> where H : K
        {
            private SCG.IEnumerable<H> _keys;

            public LiftedEnumerable(SCG.IEnumerable<H> keys)
            {
                _keys = keys;
            }

            public SCG.IEnumerator<SCG.KeyValuePair<K, V>> GetEnumerator()
            {
                foreach (H key in _keys)
                {
                    yield return KeyValuePair.Create<K, V>(key);
                }
            }

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion
        }

        public virtual bool ContainsAll<H>(SCG.IEnumerable<H> keys) where H : K
        {
            return _pairs.ContainsAll(new LiftedEnumerable<H>(keys));
        }

        /// <summary>
        /// Check if there is an entry with a specified key and report the corresponding
        /// value if found. This can be seen as a safe form of "val = this[key]".
        /// </summary>
        /// <param name="key">The key to look for</param>
        /// <param name="value">On exit, the value of the entry</param>
        /// <returns>True if key was found</returns>
        [Tested]
        public virtual bool TryGetValue(K key, out V value)
        {
            return Find(ref key, out value);
        }

        /// <summary>
        /// Check if there is an entry with a specified key and report the corresponding
        /// value if found. This can be seen as a safe form of "val = this[key]".
        /// </summary>
        /// <param name="key">The key to look for</param>
        /// <param name="value">On exit, the value of the entry</param>
        /// <returns>True if key was found</returns>
        private bool Find(ref K key, out V value)
        {
            SCG.KeyValuePair<K, V> p = KeyValuePair.Create<K, V>(key);

            if (_pairs.Find(ref p))
            {
                value = p.Value;
                return true;
            }
            else
            {
                value = default(V);
                return false;
            }
        }

        /// <summary>
        /// Look for a specific key in the dictionary and if found replace the value with a new one.
        /// This can be seen as a non-adding version of "this[key] = val".
        /// </summary>
        /// <param name="key">The key to look for</param>
        /// <param name="value">The new value</param>
        /// <returns>True if key was found</returns>
        [Tested]
        public virtual bool Update(K key, V value)
        {
            SCG.KeyValuePair<K, V> p = new SCG.KeyValuePair<K, V>(key, value);
            return _pairs.Update(p);
        }

        /// <summary>
        /// Look for a specific key in the dictionary and if found replace the value with a new one.
        /// This can be seen as a non-adding version of "this[key] = val".
        /// </summary>
        /// <param name="key">The key to look for</param>
        /// <param name="value">The new value</param>
        /// <param name="oldvalue"></param>
        /// <returns>True if key was found</returns>
        public virtual bool Update(K key, V value, out V oldvalue)
        {
            SCG.KeyValuePair<K, V> p = new SCG.KeyValuePair<K, V>(key, value);

            bool retval = _pairs.Update(p, out p);
            oldvalue = p.Value;
            return retval;
        }

        /// <summary>
        /// Look for a specific key in the dictionary. If found, report the corresponding value,
        /// else add an entry with the key and the supplied value.
        /// </summary>
        /// <param name="key">On entry the key to look for</param>
        /// <param name="value">On entry the value to add if the key is not found.
        /// On exit the value found if any.</param>
        /// <returns>True if key was found</returns>
        [Tested]
        public virtual bool FindOrAdd(K key, ref V value)
        {
            SCG.KeyValuePair<K, V> p = new SCG.KeyValuePair<K, V>(key, value);

            if (!_pairs.FindOrAdd(ref p))
                return false;
            else
            {
                value = p.Value;
                return true;
            }
        }

        /// <summary>
        /// Update value in dictionary corresponding to key if found, else add new entry.
        /// More general than "this[key] = val;" by reporting if key was found.
        /// </summary>
        /// <param name="key">The key to look for</param>
        /// <param name="value">The value to add or replace with.</param>
        /// <returns>True if entry was updated.</returns>
        [Tested]
        public virtual bool UpdateOrAdd(K key, V value)
        {
            return _pairs.UpdateOrAdd(new SCG.KeyValuePair<K, V>(key, value));
        }

        /// <summary>
        /// Update value in dictionary corresponding to key if found, else add new entry.
        /// More general than "this[key] = val;" by reporting if key was found and the old value if any.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="oldvalue"></param>
        /// <returns></returns>
        public virtual bool UpdateOrAdd(K key, V value, out V oldvalue)
        {
            SCG.KeyValuePair<K, V> p = new SCG.KeyValuePair<K, V>(key, value);
            bool retval = _pairs.UpdateOrAdd(p, out p);
            oldvalue = p.Value;
            return retval;
        }

        #region Keys, Values support classes

        private class ValuesCollection : CollectionValueBase<V>, ICollectionValue<V>, SCG.ICollection<V>
        {
            private IDictionary<K, V> _dict;

            internal ValuesCollection(IDictionary<K, V> dict)
            {
                _dict = dict;
            }

            public override V Choose()
            {
                return _dict.Choose().Value;
            }

            [Tested]
            public override SCG.IEnumerator<V> GetEnumerator()
            {
                //Updatecheck is performed by the pairs enumerator
                foreach (SCG.KeyValuePair<K, V> p in _dict)
                {
                    yield return p.Value;
                }
            }

            public override bool IsEmpty
            {
                get { return _dict.IsEmpty; }
            }

            [Tested]
            public override int Count
            {
                [Tested]
                get { return _dict.Count; }
            }

            public override Speed CountSpeed
            {
                get { return _dict.CountSpeed; }
            }

            #region SCG.ICollection<V> Members

            /// <summary>Not supported.</summary>
            /// <exception cref="NotSupportedException">Always thrown</exception>
            void SCG.ICollection<V>.Add(V key)
            {
                throw new NotSupportedException();
            }

            /// <summary>Not supported.</summary>
            /// <exception cref="NotSupportedException">Always thrown</exception>
            void SCG.ICollection<V>.Clear()
            {
                throw new NotSupportedException();
            }

            /// <summary>
            /// Determines whether the collection contains a specific value.
            /// </summary>
            /// <param name="value">Object to locate in the collection.</param>
            /// <returns><c>true</c> if item is found in collection, otherwise <c>false</c>.</returns>
            /// <remarks>
            /// This method is an O(n) operation, where n is <see cref="Count"/>.
            /// </remarks>
            bool SCG.ICollection<V>.Contains(V value)
            {
                SCG.KeyValuePair<K, V> pair;
                return _dict.Find(
                    delegate(SCG.KeyValuePair<K, V> p)
                    {
                        return EqualityComparer<V>.Default.Equals(p.Value, value);
                    }, out pair);
            }

            bool SCG.ICollection<V>.IsReadOnly
            {
                get { return true; }
            }

            /// <summary>Not supported.</summary>
            /// <exception cref="NotSupportedException">Always thrown</exception>
            bool SCG.ICollection<V>.Remove(V key)
            {
                throw new NotSupportedException();
            }

            #endregion
        }

        private class KeysCollection : CollectionValueBase<K>, ICollectionValue<K>, SCG.ICollection<K>
        {
            private IDictionary<K, V> _dict;

            internal KeysCollection(IDictionary<K, V> dict)
            {
                _dict = dict;
            }

            public override K Choose()
            {
                return _dict.Choose().Key;
            }

            [Tested]
            public override SCG.IEnumerator<K> GetEnumerator()
            {
                foreach (SCG.KeyValuePair<K, V> p in _dict)
                {
                    yield return p.Key;
                }
            }

            public override bool IsEmpty
            {
                get { return _dict.IsEmpty; }
            }

            [Tested]
            public override int Count
            {
                [Tested]
                get { return _dict.Count; }
            }

            public override Speed CountSpeed
            {
                get { return _dict.CountSpeed; }
            }

            #region SCG.ICollection<K> Members

            /// <summary>Not supported.</summary>
            /// <exception cref="NotSupportedException">Always thrown</exception>
            void SCG.ICollection<K>.Add(K key)
            {
                throw new NotSupportedException();
            }

            /// <summary>Not supported.</summary>
            /// <exception cref="NotSupportedException">Always thrown</exception>
            void SCG.ICollection<K>.Clear()
            {
                throw new NotSupportedException();
            }

            /// <summary>
            /// Determines whether the colection contains a specific key.
            /// </summary>
            /// <param name="key">Object to locate in the collection.</param>
            /// <returns><c>true</c> if item is found in collection, otherwise <c>false</c>.</returns>
            bool SCG.ICollection<K>.Contains(K key)
            {
                return _dict.ContainsKey(key);
            }

            bool SCG.ICollection<K>.IsReadOnly
            {
                get { return true; }
            }

            /// <summary>Not supported.</summary>
            /// <exception cref="NotSupportedException">Always thrown</exception>
            bool SCG.ICollection<K>.Remove(K key)
            {
                throw new NotSupportedException();
            }

            #endregion
        }

        #endregion

        /// <summary>
        /// A collection containing the all the keys of the dictionary.
        /// </summary>
        [Tested]
        public virtual ICollectionValue<K> Keys
        {
            [Tested]
            get { return new KeysCollection(this); }
        }

        /// <summary>
        /// A collection containing all the values of the dictionary.
        /// </summary>
        [Tested]
        public virtual ICollectionValue<V> Values
        {
            [Tested]
            get { return new ValuesCollection(this); }
        }

        /// <summary>
        /// Returns delegate for indexer by key for this dictionary instance. 
        /// </summary>
        public virtual Fun<K, V> Fun
        {
            get { return delegate(K k) { return this[k]; }; }
        }

        /// <summary>
        /// Indexer by key for dictionary. 
        /// <para>The get method will throw an exception if no entry is found. </para>
        /// <para>The set method behaves like <see cref="M:C5.DictionaryBase`2.UpdateOrAdd(`0,`1)"/>.</para>
        /// </summary>
        /// <exception cref="NoSuchItemException"> On get if no entry is found. </exception>
        /// <value>The value corresponding to the key</value>
        [Tested]
        public virtual V this[K key]
        {
            [Tested]
            get
            {
                SCG.KeyValuePair<K, V> p = KeyValuePair.Create<K, V>(key);
                if (_pairs.Find(ref p))
                {
                    return p.Value;
                }
                else
                {
                    throw new NoSuchItemException("Key '" + key.ToString() + "' not present in Dictionary");
                }
            }

            [Tested]
            set
            {
                _pairs.UpdateOrAdd(new SCG.KeyValuePair<K, V>(key, value));
            }
        }

        /// <summary>
        /// Indicates whether dictionary is read only.
        /// </summary>
        /// <value>True if dictionary is read only</value>
        [Tested]
        public virtual bool IsReadOnly
        {
            [Tested]
            get { return _pairs.IsReadOnly; }
        }

        /// <summary>
        /// Check the integrity of the internal data structures of this dictionary.
        /// </summary>
        /// <returns>True if check does not fail.</returns>
        [Tested]
        public virtual bool Check()
        {
            return _pairs.Check();
        }

        #endregion

        #region ICollectionValue<SCG.KeyValuePair<K,V>> Members

        /// <summary>
        /// True if this collection is empty.
        /// </summary>
        public override bool IsEmpty
        {
            get { return _pairs.IsEmpty; }
        }

        /// <summary>
        /// The number of entrues in the dictionary.
        /// </summary>
        [Tested]
        public override int Count
        {
            [Tested]
            get { return _pairs.Count; }
        }

        [Tested]
        public override Speed CountSpeed
        {
            [Tested]
            get { return _pairs.CountSpeed; }
        }

        /// <summary>
        /// Choose some entry in this Dictionary. 
        /// </summary>
        /// <exception cref="NoSuchItemException">if collection is empty.</exception>
        /// <returns></returns>
        public override SCG.KeyValuePair<K, V> Choose()
        {
            return _pairs.Choose();
        }

        /// <summary>
        /// Create an enumerator for the collection of entries of the dictionary
        /// </summary>
        /// <returns>The enumerator</returns>
        [Tested]
        public override SCG.IEnumerator<SCG.KeyValuePair<K, V>> GetEnumerator()
        {
            return _pairs.GetEnumerator();
        }

        #endregion

        #region SCG.IDictionary<K,V> Members

        SCG.ICollection<K> SCG.IDictionary<K, V>.Keys
        {
            get { return new KeysCollection(this); }
        }

        SCG.ICollection<V> SCG.IDictionary<K, V>.Values
        {
            get { return new ValuesCollection(this); }
        }

        #endregion

        #region SCG.ICollection<KeyValuePair<K,V>> Members

        void SCG.ICollection<SCG.KeyValuePair<K, V>>.Add(SCG.KeyValuePair<K, V> item)
        {
            Add(item);
        }

        bool SCG.ICollection<SCG.KeyValuePair<K, V>>.Contains(SCG.KeyValuePair<K, V> item)
        {
            return Pairs.Contains(item);
        }

        void SCG.ICollection<SCG.KeyValuePair<K, V>>.CopyTo(SCG.KeyValuePair<K, V>[] array, int arrayIndex)
        {
            Pairs.CopyTo(array, arrayIndex);
        }

        bool SCG.ICollection<SCG.KeyValuePair<K, V>>.Remove(SCG.KeyValuePair<K, V> item)
        {
            return Pairs.Remove(item);
        }

        #endregion

        public override bool Show(StringBuilder sb, ref int rest, IFormatProvider formatProvider)
        {
            return Showing.ShowDictionary<K, V>(this, sb, ref rest, formatProvider);
        }

        public abstract object Clone();
    }

    /// <summary>
    /// A base class for implementing a sorted dictionary based on a sorted set collection implementation.
    /// <i>See the source code for <see cref="T:C5.TreeDictionary`2"/> for an example</i>
    /// </summary>
    public abstract class SortedDictionaryBase<K, V> : DictionaryBase<K, V>, ISortedDictionary<K, V>
    {
        #region Fields

        private ISorted<SCG.KeyValuePair<K, V>> _sortedPairs;
        private SCG.IComparer<K> _keyComparer;

        #endregion

        #region Constructors

        protected SortedDictionaryBase(
            ISorted<SCG.KeyValuePair<K, V>> sortedPairs,
            SCG.IComparer<K> keyComparer,
            SCG.IEqualityComparer<K> keyEqualityComparer)
            : base(sortedPairs, keyEqualityComparer)
        {
            _sortedPairs = sortedPairs;
            _keyComparer = keyComparer;
        }

        /// <summary>
        /// Creates clone of other <see cref="T:SortedDictionaryBase"/> instance.
        /// </summary>
        /// <param name="template">Dictionary to be cloned</param>
        protected SortedDictionaryBase(SortedDictionaryBase<K, V> template)
            : this((ISorted<SCG.KeyValuePair<K, V>>) template._sortedPairs.Clone(),
                template.Comparer, template.EqualityComparer)
        { }

        #endregion

        #region ISortedDictionary<K,V> Members

        /// <summary>
        /// The key comparer used by this dictionary.
        /// </summary>
        /// <value></value>
        public SCG.IComparer<K> Comparer
        {
            get { return _keyComparer; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public new ISorted<K> Keys
        {
            get
            {
                return new SortedKeysCollection(this, _sortedPairs, _keyComparer, EqualityComparer);
            }
        }

        /// <summary>
        /// Get the entry in the dictionary whose key is the
        /// predecessor of a specified key.
        /// </summary>
        /// <exception cref="NoSuchItemException"></exception>
        /// <param name="key">The key</param>
        /// <returns>The entry</returns>
        [Tested]
        public SCG.KeyValuePair<K, V> Predecessor(K key)
        {
            return _sortedPairs.Predecessor(KeyValuePair.Create<K, V>(key));
        }

        /// <summary>
        /// Get the entry in the dictionary whose key is the
        /// weak predecessor of a specified key.
        /// </summary>
        /// <exception cref="NoSuchItemException"></exception>
        /// <param name="key">The key</param>
        /// <returns>The entry</returns>
        [Tested]
        public SCG.KeyValuePair<K, V> WeakPredecessor(K key)
        {
            return _sortedPairs.WeakPredecessor(KeyValuePair.Create<K, V>(key));
        }

        /// <summary>
        /// Get the entry in the dictionary whose key is the
        /// successor of a specified key.
        /// </summary>
        /// <exception cref="NoSuchItemException"></exception>
        /// <param name="key">The key</param>
        /// <returns>The entry</returns>
        [Tested]
        public SCG.KeyValuePair<K, V> Successor(K key)
        {
            return _sortedPairs.Successor(KeyValuePair.Create<K, V>(key));
        }

        /// <summary>
        /// Get the entry in the dictionary whose key is the
        /// weak successor of a specified key.
        /// </summary>
        /// <exception cref="NoSuchItemException"></exception>
        /// <param name="key">The key</param>
        /// <returns>The entry</returns>
        [Tested]
        public SCG.KeyValuePair<K, V> WeakSuccessor(K key)
        {
            return _sortedPairs.WeakSuccessor(KeyValuePair.Create<K, V>(key));
        }

        /// <summary>
        /// Returns the entry with minimal key from the sorted dictionary, if any. 
        /// </summary>
        /// <returns>Entry with minimal key</returns>
        /// <exception cref="NoSuchItemException">Dictionary is empty.</exception>
        public SCG.KeyValuePair<K, V> FindMin()
        {
            return _sortedPairs.FindMin();
        }

        /// <summary>
        /// Removes and returns the entry with minimal key from the sorted dictionary, if any. 
        /// Raises events ItemsRemoved and CollectionChanged. 
        /// </summary>
        /// <returns>Entry with minimal key.</returns>
        /// <exception cref="NoSuchItemException">Dictionary is empty.</exception>
        /// <exception cref="ReadOnlyCollectionException">Dictionary is read-only.</exception>
        public SCG.KeyValuePair<K, V> DeleteMin()
        {
            return _sortedPairs.DeleteMin();
        }

        /// <summary>
        /// Returns the entry with maximal key from the sorted dictionary, if any. 
        /// </summary>
        /// <returns>Entry with maximal key</returns>
        /// <exception cref="NoSuchItemException">Dictionary is empty.</exception>
        public SCG.KeyValuePair<K, V> FindMax()
        {
            return _sortedPairs.FindMax();
        }

        /// <summary>
        /// Removes and returns the entry with maximal key from the sorted dictionary, if any. 
        /// Raises events ItemsRemoved and CollectionChanged. 
        /// </summary>
        /// <returns>Entry with maximal key.</returns>
        /// <exception cref="NoSuchItemException">Dictionary is empty.</exception>
        /// <exception cref="ReadOnlyCollectionException">Dictionary is read-only.</exception>
        public SCG.KeyValuePair<K, V> DeleteMax()
        {
            return _sortedPairs.DeleteMax();
        }

        /// <summary></summary>
        /// <param name="cutter"></param>
        /// <param name="lowEntry"></param>
        /// <param name="lowIsValid"></param>
        /// <param name="highEntry"></param>
        /// <param name="highIsValid"></param>
        /// <returns></returns>
        public bool Cut(IComparable<K> cutter,
            out SCG.KeyValuePair<K, V> lowEntry, out bool lowIsValid,
            out SCG.KeyValuePair<K, V> highEntry, out bool highIsValid)
        {
            return _sortedPairs.Cut(new KeyValuePairComparable(cutter),
                out lowEntry, out lowIsValid,
                out highEntry, out highIsValid);
        }

        public IDirectedEnumerable<SCG.KeyValuePair<K, V>> RangeFrom(K bottom)
        {
            return _sortedPairs.RangeFrom(
                KeyValuePair.Create<K, V>(bottom));
        }

        /// <summary></summary>
        /// <param name="bottom"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public IDirectedEnumerable<SCG.KeyValuePair<K, V>> RangeFromTo(K bottom, K top)
        {
            return _sortedPairs.RangeFromTo(
                KeyValuePair.Create<K, V>(bottom),
                KeyValuePair.Create<K, V>(top));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        public IDirectedEnumerable<SCG.KeyValuePair<K, V>> RangeTo(K top)
        {
            return _sortedPairs.RangeTo(
                KeyValuePair.Create<K, V>(top));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDirectedCollectionValue<SCG.KeyValuePair<K, V>> RangeAll()
        {
            return _sortedPairs.RangeAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        public void AddSorted(SCG.IEnumerable<SCG.KeyValuePair<K, V>> items)
        {
            _sortedPairs.AddSorted(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lowKey"></param>
        public void RemoveRangeFrom(K lowKey)
        {
            _sortedPairs.RemoveRangeFrom(
                KeyValuePair.Create<K, V>(lowKey));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lowKey"></param>
        /// <param name="highKey"></param>
        public void RemoveRangeFromTo(K lowKey, K highKey)
        {
            _sortedPairs.RemoveRangeFromTo(
                KeyValuePair.Create<K, V>(lowKey),
                KeyValuePair.Create<K, V>(highKey));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="highKey"></param>
        public void RemoveRangeTo(K highKey)
        {
            _sortedPairs.RemoveRangeTo(
                KeyValuePair.Create<K, V>(highKey));
        }

        #endregion

        #region Classes

        private class KeyValuePairComparable : IComparable<SCG.KeyValuePair<K, V>>
        {
            IComparable<K> _cutter;

            internal KeyValuePairComparable(IComparable<K> cutter)
            {
                this._cutter = cutter;
            }

            public int CompareTo(SCG.KeyValuePair<K, V> other)
            {
                return _cutter.CompareTo(other.Key);
            }

            public bool Equals(SCG.KeyValuePair<K, V> other)
            {
                return _cutter.Equals(other.Key);
            }
        }

        private class ProjectedDirectedEnumerable : MappedDirectedEnumerable<SCG.KeyValuePair<K, V>, K>
        {
            public ProjectedDirectedEnumerable(IDirectedEnumerable<SCG.KeyValuePair<K, V>> directedPairs)
                : base(directedPairs)
            { }

            public override K Map(SCG.KeyValuePair<K, V> pair)
            {
                return pair.Key;
            }
        }

        private class ProjectedDirectedCollectionValue : MappedDirectedCollectionValue<SCG.KeyValuePair<K, V>, K>
        {
            public ProjectedDirectedCollectionValue(IDirectedCollectionValue<SCG.KeyValuePair<K, V>> directedPairs)
                : base(directedPairs)
            { }

            public override K Map(SCG.KeyValuePair<K, V> pair)
            {
                return pair.Key;
            }
        }

        private class SortedKeysCollection : SequencedBase<K>, ISorted<K>
        {
            private ISortedDictionary<K, V> _sortedDict;
            //TODO: eliminate this. Only problem is the Find method because we lack method on dictionary that also 
            //      returns the actual key.
            private ISorted<SCG.KeyValuePair<K, V>> _sortedPairs;
            private SCG.IComparer<K> _comparer;

            internal SortedKeysCollection(
                ISortedDictionary<K, V> sortedDict,
                ISorted<SCG.KeyValuePair<K, V>> sortedPairs,
                SCG.IComparer<K> comparer,
                SCG.IEqualityComparer<K> itemEqualityComparer)
                : base(itemEqualityComparer)
            {
                _sortedDict = sortedDict;
                _sortedPairs = sortedPairs;
                _comparer = comparer;
            }

            public override K Choose()
            {
                return _sortedDict.Choose().Key;
            }

            public override SCG.IEnumerator<K> GetEnumerator()
            {
                foreach (SCG.KeyValuePair<K, V> p in _sortedDict)
                {
                    yield return p.Key;
                }
            }

            public override bool IsEmpty
            {
                get { return _sortedDict.IsEmpty; }
            }

            public override int Count
            {
                [Tested]
                get { return _sortedDict.Count; }
            }

            public override Speed CountSpeed
            {
                get { return _sortedDict.CountSpeed; }
            }

            #region ISorted<K> Members

            public K FindMin()
            {
                return _sortedDict.FindMin().Key;
            }

            public K DeleteMin()
            {
                throw new ReadOnlyCollectionException();
            }

            public K FindMax()
            {
                return _sortedDict.FindMax().Key;
            }

            public K DeleteMax()
            {
                throw new ReadOnlyCollectionException();
            }

            public SCG.IComparer<K> Comparer
            {
                get { return _comparer; }
            }

            public K Predecessor(K item)
            {
                return _sortedDict.Predecessor(item).Key;
            }

            public K Successor(K item)
            {
                return _sortedDict.Successor(item).Key;
            }

            public K WeakPredecessor(K item)
            {
                return _sortedDict.WeakPredecessor(item).Key;
            }

            public K WeakSuccessor(K item)
            {
                return _sortedDict.WeakSuccessor(item).Key;
            }

            public bool Cut(IComparable<K> c, out K low, out bool lowIsValid, out K high, out bool highIsValid)
            {
                SCG.KeyValuePair<K, V> lowpair, highpair;
                bool retval = _sortedDict.Cut(c, out lowpair, out lowIsValid, out highpair, out highIsValid);
                low = lowpair.Key;
                high = highpair.Key;
                return retval;
            }

            public IDirectedEnumerable<K> RangeFrom(K bot)
            {
                return new ProjectedDirectedEnumerable(_sortedDict.RangeFrom(bot));
            }

            public IDirectedEnumerable<K> RangeFromTo(K bot, K top)
            {
                return new ProjectedDirectedEnumerable(_sortedDict.RangeFromTo(bot, top));
            }

            public IDirectedEnumerable<K> RangeTo(K top)
            {
                return new ProjectedDirectedEnumerable(_sortedDict.RangeTo(top));
            }

            public IDirectedCollectionValue<K> RangeAll()
            {
                return new ProjectedDirectedCollectionValue(_sortedDict.RangeAll());
            }

            public void AddSorted<U>(SCG.IEnumerable<U> items) where U : K
            {
                throw new ReadOnlyCollectionException();
            }

            public void RemoveRangeFrom(K low)
            {
                throw new ReadOnlyCollectionException();
            }

            public void RemoveRangeFromTo(K low, K hi)
            {
                throw new ReadOnlyCollectionException();
            }

            public void RemoveRangeTo(K hi)
            {
                throw new ReadOnlyCollectionException();
            }

            #endregion

            #region ICollection<K> Members

            public Speed ContainsSpeed { get { return _sortedDict.ContainsSpeed; } }

            public bool Contains(K key) { return _sortedDict.ContainsKey(key); ;      }

            public int ContainsCount(K item) { return _sortedDict.ContainsKey(item) ? 1 : 0; }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public virtual ICollectionValue<K> UniqueItems()
            {
                return this;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public virtual ICollectionValue<Multiplicity<K>> ItemMultiplicities()
            {
                return new MultiplicityOne<K>(this);
            }


            public bool ContainsAll<U>(SCG.IEnumerable<U> items) where U : K
            {
                //TODO: optimize?
                foreach (K item in items)
                    if (!_sortedDict.ContainsKey(item))
                        return false;
                return true;
            }

            public bool Find(ref K item)
            {
                SCG.KeyValuePair<K, V> p = KeyValuePair.Create<K, V>(item);
                return _sortedPairs.Find(ref p);
            }

            public bool FindOrAdd(ref K item)
            {
                throw new ReadOnlyCollectionException();
            }

            public bool Update(K item)
            {
                throw new ReadOnlyCollectionException();
            }

            public bool Update(K item, out K olditem)
            {
                throw new ReadOnlyCollectionException();
            }

            public bool UpdateOrAdd(K item)
            {
                throw new ReadOnlyCollectionException();
            }

            public bool UpdateOrAdd(K item, out K olditem)
            {
                throw new ReadOnlyCollectionException();
            }

            public bool Remove(K item)
            {
                throw new ReadOnlyCollectionException();
            }

            public bool Remove(K item, out K removeditem)
            {
                throw new ReadOnlyCollectionException();
            }

            public void RemoveAllCopies(K item)
            {
                throw new ReadOnlyCollectionException();
            }

            public void RemoveAll<U>(SCG.IEnumerable<U> items) where U : K
            {
                throw new ReadOnlyCollectionException();
            }

            public void Clear()
            {
                throw new ReadOnlyCollectionException();
            }

            public void RetainAll<U>(SCG.IEnumerable<U> items)
                where U : K
            {
                throw new ReadOnlyCollectionException();
            }

            #endregion

            #region IExtensible<K> Members

            public override bool IsReadOnly
            {
                get { return true; }
            }

            public bool AllowsDuplicates
            {
                get { return false; }
            }

            public bool DuplicatesByCounting
            {
                get { return true; }
            }

            public bool Add(K item)
            {
                throw new ReadOnlyCollectionException();
            }

            public void AddAll(System.Collections.Generic.IEnumerable<K> items)
            {
                throw new ReadOnlyCollectionException();
            }

            public void AddAll<U>(System.Collections.Generic.IEnumerable<U> items)
                where U : K
            {
                throw new ReadOnlyCollectionException();
            }

            public bool Check()
            {
                return _sortedDict.Check();
            }

            #endregion

            #region IDirectedCollectionValue<K> Members

            public override IDirectedCollectionValue<K> Backwards()
            {
                return RangeAll().Backwards();
            }

            #endregion

            #region IDirectedEnumerable<K> Members

            IDirectedEnumerable<K> IDirectedEnumerable<K>.Backwards()
            {
                return Backwards();
            }

            #endregion

            #region ICloneable Members

            //TODO: test
            /// <summary>
            /// Make a shallow copy of this SortedKeysCollection.
            /// </summary>
            /// <returns></returns>
            public virtual object Clone()
            {
                SortedArrayDictionary<K, V> dictClone =
                    new SortedArrayDictionary<K, V>(_sortedPairs.Count, _comparer, EqualityComparer);
                SortedArray<SCG.KeyValuePair<K, V>> sortedPairsClone =
                    (SortedArray<SCG.KeyValuePair<K, V>>) dictClone._sortedPairs;
                foreach (K key in _sortedDict.Keys)
                {
                    sortedPairsClone.Add(new SCG.KeyValuePair<K, V>(key, default(V)));
                }
                return new SortedKeysCollection(dictClone, sortedPairsClone, _comparer, EqualityComparer);
            }

            #endregion
        }

        #endregion

        public override bool Show(StringBuilder sb, ref int rest, IFormatProvider formatProvider)
        {
            return Showing.ShowDictionary<K, V>(this, sb, ref rest, formatProvider);
        }
    }

    internal class SortedArrayDictionary<K, V> : SortedDictionaryBase<K, V>, IDictionary<K, V>, ISortedDictionary<K, V>
    {
        #region Constructors

        public SortedArrayDictionary()
            : this(Comparer<K>.Default, EqualityComparer<K>.Default)
        { }

        /// <summary>
        /// Create a dictionary using an external comparer for keys.
        /// </summary>
        /// <param name="comparer">The external comparer</param>
        public SortedArrayDictionary(SCG.IComparer<K> comparer)
            : this(comparer, new ComparerZeroHashCodeEqualityComparer<K>(comparer))
        { }

        public SortedArrayDictionary(SCG.IComparer<K> comparer, SCG.IEqualityComparer<K> equalityComparer)
            : base(
                new SortedArray<SCG.KeyValuePair<K, V>>(
                    new KeyValuePairComparer<K, V>(comparer)),
                comparer, equalityComparer)
        { }

        public SortedArrayDictionary(int capacity, SCG.IComparer<K> comparer, SCG.IEqualityComparer<K> equalityComparer)
            : base(
                new SortedArray<SCG.KeyValuePair<K, V>>(capacity,
                    new KeyValuePairComparer<K, V>(comparer)),
                comparer, equalityComparer)
        { }

        /// <summary>
        /// Creates shallow clone of other <see cref="T:SortedArrayDictionary"/> instance.
        /// </summary>
        /// <param name="template">Dictionary to be cloned.</param>
        private SortedArrayDictionary(SortedArrayDictionary<K, V> template)
            : base(template)
        { }

        #endregion

        public override object Clone()
        {
            return new SortedArrayDictionary<K, V>(this);
        }

    }
}