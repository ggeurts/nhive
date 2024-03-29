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
using SCG = System.Collections.Generic;

namespace C5
{
    /// <summary>
    /// A sorted generic dictionary based on a red-black tree set.
    /// </summary>
    [Serializable]
    public class TreeDictionary<K, V> : SortedDictionaryBase<K, V>, IDictionary<K, V>, ISortedDictionary<K, V>
    {

        #region Constructors

        /// <summary>
        /// Create a red-black tree dictionary using the natural comparer for keys.
        /// <exception cref="ArgumentException"/> if the key type K is not comparable.
        /// </summary>
        public TreeDictionary()
            : this(Comparer<K>.Default, EqualityComparer<K>.Default)
        { }

        /// <summary>
        /// Create a red-black tree dictionary using an external comparer for keys.
        /// </summary>
        /// <param name="comparer">The external comparer</param>
        public TreeDictionary(SCG.IComparer<K> comparer)
            : this(comparer, new ComparerZeroHashCodeEqualityComparer<K>(comparer))
        { }

        private TreeDictionary(SCG.IComparer<K> comparer, SCG.IEqualityComparer<K> equalityComparer)
            : base(
                new TreeSet<SCG.KeyValuePair<K, V>>(
                    new KeyValuePairComparer<K, V>(comparer)),
                comparer, equalityComparer)
        { }

        /// <summary>
        /// Creates shallow clone or snapshot of other <see cref="T:TreeDictionary"/> instance.
        /// </summary>
        /// <param name="template">Dictionary to be cloned.</param>
        /// <param name="isSnapshot">Indicates whether clone must be a read-only snapshot.</param>
        private TreeDictionary(TreeDictionary<K, V> template, bool isSnapshot)
            : base(ClonePairs(template, isSnapshot), template.Comparer, template.EqualityComparer)
        { }

        private static ISorted<SCG.KeyValuePair<K, V>> ClonePairs(TreeDictionary<K, V> template, bool isSnapshot)
        {
            TreeSet<SCG.KeyValuePair<K, V>> originalPairs = (TreeSet<SCG.KeyValuePair<K, V>>) template.Pairs;
            return isSnapshot
                ? originalPairs.Snapshot()
                : (ISorted<SCG.KeyValuePair<K, V>>) originalPairs.Clone();
        }

        #endregion

        //TODO: put in interface
        /// <summary>
        /// Make a snapshot of the current state of this dictionary
        /// </summary>
        /// <returns>The snapshot</returns>
        [Tested]
        public SCG.IEnumerable<SCG.KeyValuePair<K, V>> Snapshot()
        {
            return new TreeDictionary<K, V>(this, true);
        }

        public override object Clone()
        {
            return new TreeDictionary<K, V>(this, false);
        }

    }
}