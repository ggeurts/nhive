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
    /// <summary>
    /// A priority queue class based on an interval heap data structure.
    /// </summary>
    /// <typeparam name="T">The item type</typeparam>
    [Serializable]
    public class IntervalHeap<T> : CollectionValueBase<T>, IPriorityQueue<T>
    {
        #region Events

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public override EventTypeEnum ListenableEvents { get { return EventTypeEnum.Basic; } }

        #endregion

        #region Fields

        [Serializable]
        struct Interval
        {
            internal T First;
            internal T Last;
            internal Handle FirstHandle;
            internal Handle LastHandle;

            public override string ToString() 
            { 
                return String.Format("[{0}; {1}]", First, Last); 
            }
        }

        private object _syncRoot = new object();
        private int _stamp;

        private SCG.IComparer<T> _comparer;
        private SCG.IEqualityComparer<T> _itemEqualityComparer;

        private Interval[] _heap;
        private int _size;

        #endregion

        #region Util
        bool heapifyMin(int i)
        {
            bool swappedroot = false;
            int cell = i, currentmin = cell;
            T currentitem = _heap[cell].First;
            Handle currenthandle = _heap[cell].FirstHandle;

            if (i > 0)
            {
                T other = _heap[cell].Last;
                if (2 * cell + 1 < _size && _comparer.Compare(currentitem, other) > 0)
                {
                    swappedroot = true;
                    Handle otherhandle = _heap[cell].LastHandle;
                    updateLast(cell, currentitem, currenthandle);
                    currentitem = other;
                    currenthandle = otherhandle;
                }
            }

            T minitem = currentitem;
            Handle minhandle = currenthandle;

            while (true)
            {
                int l = 2 * cell + 1, r = l + 1;
                T lv, rv;

                if (2 * l < _size && _comparer.Compare(lv = _heap[l].First, minitem) < 0)
                { currentmin = l; minitem = lv; }

                if (2 * r < _size && _comparer.Compare(rv = _heap[r].First, minitem) < 0)
                { currentmin = r; minitem = rv; }

                if (currentmin == cell)
                    break;

                minhandle = _heap[currentmin].FirstHandle;
                updateFirst(cell, minitem, minhandle);
                cell = currentmin;

                //Maybe swap first and last
                T other = _heap[cell].Last;
                if (2 * currentmin + 1 < _size && _comparer.Compare(currentitem, other) > 0)
                {
                    Handle otherhandle = _heap[cell].LastHandle;
                    updateLast(cell, currentitem, currenthandle);
                    currentitem = other;
                    currenthandle = otherhandle;
                }


                minitem = currentitem;
                minhandle = currenthandle;
            }

            if (cell != i || swappedroot)
                updateFirst(cell, minitem, minhandle);
            return swappedroot;
        }


        bool heapifyMax(int i)
        {
            bool swappedroot = false;
            int cell = i, currentmax = cell;
            T currentitem = _heap[cell].Last;
            Handle currenthandle = _heap[cell].LastHandle;

            if (i > 0)
            {
                T other = _heap[cell].First;
                if (_comparer.Compare(currentitem, other) < 0)
                {
                    swappedroot = true;
                    Handle otherhandle = _heap[cell].FirstHandle;
                    updateFirst(cell, currentitem, currenthandle);
                    currentitem = other;
                    currenthandle = otherhandle;
                }
            }

            T maxitem = currentitem;
            Handle maxhandle = currenthandle;

            while (true)
            {
                int l = 2 * cell + 1, r = l + 1;
                T lv, rv;

                if (2 * l + 1 < _size && _comparer.Compare(lv = _heap[l].Last, maxitem) > 0)
                { currentmax = l; maxitem = lv; }

                if (2 * r + 1 < _size && _comparer.Compare(rv = _heap[r].Last, maxitem) > 0)
                { currentmax = r; maxitem = rv; }

                if (currentmax == cell)
                    break;

                maxhandle = _heap[currentmax].LastHandle;
                updateLast(cell, maxitem, maxhandle);
                cell = currentmax;

                //Maybe swap first and last
                T other = _heap[cell].First;
                if (_comparer.Compare(currentitem, other) < 0)
                {
                    Handle otherhandle = _heap[cell].FirstHandle;
                    updateFirst(cell, currentitem, currenthandle);
                    currentitem = other;
                    currenthandle = otherhandle;
                }

                maxitem = currentitem;
                maxhandle = currenthandle;
            }

            if (cell != i || swappedroot) //Check could be better?
                updateLast(cell, maxitem, maxhandle);
            return swappedroot;
        }


        void bubbleUpMin(int i)
        {
            if (i > 0)
            {
                T min = _heap[i].First, iv = min;
                Handle minhandle = _heap[i].FirstHandle;
                int p = (i + 1) / 2 - 1;

                while (i > 0)
                {
                    if (_comparer.Compare(iv, min = _heap[p = (i + 1) / 2 - 1].First) < 0)
                    {
                        updateFirst(i, min, _heap[p].FirstHandle);
                        min = iv;
                        i = p;
                    }
                    else
                        break;
                }

                updateFirst(i, iv, minhandle);
            }
        }


        void bubbleUpMax(int i)
        {
            if (i > 0)
            {
                T max = _heap[i].Last, iv = max;
                Handle maxhandle = _heap[i].LastHandle;
                int p = (i + 1) / 2 - 1;

                while (i > 0)
                {
                    if (_comparer.Compare(iv, max = _heap[p = (i + 1) / 2 - 1].Last) > 0)
                    {
                        updateLast(i, max, _heap[p].LastHandle);
                        max = iv;
                        i = p;
                    }
                    else
                        break;
                }

                updateLast(i, iv, maxhandle);

            }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Create an interval heap with natural item comparer and default initial capacity (16)
        /// </summary>
        public IntervalHeap() 
            : this(16) 
        { }

        /// <summary>
        /// Create an interval heap with external item comparer and default initial capacity (16)
        /// </summary>
        /// <param name="comparer">The external comparer</param>
        public IntervalHeap(SCG.IComparer<T> comparer) 
            : this(16, comparer) 
        { }

        //TODO: maybe remove
        /// <summary>
        /// Create an interval heap with natural item comparer and prescribed initial capacity
        /// </summary>
        /// <param name="capacity">The initial capacity</param>
        public IntervalHeap(int capacity) 
            : this(capacity, Comparer<T>.Default, EqualityComparer<T>.Default) 
        { }

        /// <summary>
        /// Create an interval heap with external item comparer and prescribed initial capacity
        /// </summary>
        /// <param name="comparer">The external comparer</param>
        /// <param name="capacity">The initial capacity</param>
        public IntervalHeap(int capacity, SCG.IComparer<T> comparer) 
            : this(capacity, comparer, new ComparerZeroHashCodeEqualityComparer<T>(comparer)) 
        { }

        private IntervalHeap(int capacity, SCG.IComparer<T> comparer, SCG.IEqualityComparer<T> itemEqualityComparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            if (itemEqualityComparer == null)
            {
                throw new ArgumentNullException("itemEqualityComparer");
            }

            _comparer = comparer;
            _itemEqualityComparer = itemEqualityComparer;
            
            int length = 1;
            while (length < capacity)
            {
                length <<= 1;
            }
            _heap = new Interval[length];
        }

        #endregion

        #region IPriorityQueue<T> Members

        /// <summary>
        /// Find the current least item of this priority queue.
        /// <exception cref="NoSuchItemException"/> if queue is empty
        /// </summary>
        /// <returns>The least item.</returns>
        [Tested]
        public T FindMin()
        {
            if (_size == 0)
                throw new NoSuchItemException();

            return _heap[0].First;
        }


        /// <summary>
        /// Remove the least item from this  priority queue.
        /// <exception cref="NoSuchItemException"/> if queue is empty
        /// </summary>
        /// <returns>The removed item.</returns>
        [Tested]
        public T DeleteMin()
        {
            IPriorityQueueHandle<T> handle = null;
            return DeleteMin(out handle);
        }


        /// <summary>
        /// Find the current largest item of this priority queue.
        /// <exception cref="NoSuchItemException"/> if queue is empty
        /// </summary>
        /// <returns>The largest item.</returns>
        [Tested]
        public T FindMax()
        {
            if (_size == 0)
                throw new NoSuchItemException("Heap is empty");
            else if (_size == 1)
                return _heap[0].First;
            else
                return _heap[0].Last;
        }


        /// <summary>
        /// Remove the largest item from this  priority queue.
        /// <exception cref="NoSuchItemException"/> if queue is empty
        /// </summary>
        /// <returns>The removed item.</returns>
        [Tested]
        public T DeleteMax()
        {
            IPriorityQueueHandle<T> handle = null;
            return DeleteMax(out handle);
        }


        /// <summary>
        /// The comparer object supplied at creation time for this collection
        /// </summary>
        /// <value>The comparer</value>
        public SCG.IComparer<T> Comparer { get { return _comparer; } }

        #endregion

        #region IExtensible<T> Members

        /// <summary>
        /// If true any call of an updating operation will throw an
        /// <code>ReadOnlyCollectionException</code>
        /// </summary>
        /// <value>True if this collection is read-only.</value>
        public bool IsReadOnly { get { return false; } }

        /// <summary>
        /// 
        /// </summary>
        /// <value>True since this collection has bag semantics</value>
        [Tested]
        public bool AllowsDuplicates { [Tested]get { return true; } }

        /// <summary>
        /// Value is null since this collection has no equality concept for its items. 
        /// </summary>
        /// <value></value>
        public virtual SCG.IEqualityComparer<T> EqualityComparer { get { return _itemEqualityComparer; } }

        /// <summary>
        /// By convention this is true for any collection with set semantics.
        /// </summary>
        /// <value>True if only one representative of a group of equal items 
        /// is kept in the collection together with the total count.</value>
        public virtual bool DuplicatesByCounting { get { return false; } }



        /// <summary>
        /// 
        /// </summary>
        /// <value>The distinguished object to use for locking to synchronize multithreaded access</value>
        [Tested]
        public object SyncRoot { [Tested]get { return _syncRoot; } }


        /// <summary>
        /// Add an item to this priority queue.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>True</returns>
        [Tested]
        public bool Add(T item)
        {
            _stamp++;
            if (add(null, item))
            {
                raiseItemsAdded(item, 1);
                raiseCollectionChanged();
                return true;
            }
            return false;
        }

        private bool add(Handle itemhandle, T item)
        {
            if (_size == 0)
            {
                _size = 1;
                updateFirst(0, item, itemhandle);
                return true;
            }

            if (_size == 2 * _heap.Length)
            {
                Interval[] newheap = new Interval[2 * _heap.Length];

                Array.Copy(_heap, newheap, _heap.Length);
                _heap = newheap;
            }

            if (_size % 2 == 0)
            {
                int i = _size / 2, p = (i + 1) / 2 - 1;
                T tmp = _heap[p].Last;

                if (_comparer.Compare(item, tmp) > 0)
                {
                    updateFirst(i, tmp, _heap[p].LastHandle);
                    updateLast(p, item, itemhandle);
                    bubbleUpMax(p);
                }
                else
                {
                    updateFirst(i, item, itemhandle);

                    if (_comparer.Compare(item, _heap[p].First) < 0)
                        bubbleUpMin(i);
                }
            }
            else
            {
                int i = _size / 2;
                T other = _heap[i].First;

                if (_comparer.Compare(item, other) < 0)
                {
                    updateLast(i, other, _heap[i].FirstHandle);
                    updateFirst(i, item, itemhandle);
                    bubbleUpMin(i);
                }
                else
                {
                    updateLast(i, item, itemhandle);
                    bubbleUpMax(i);
                }
            }
            _size++;

            return true;
        }

        private void updateLast(int cell, T item, Handle handle)
        {
            _heap[cell].Last = item;
            if (handle != null)
                handle.index = 2 * cell + 1;
            _heap[cell].LastHandle = handle;
        }

        private void updateFirst(int cell, T item, Handle handle)
        {
            _heap[cell].First = item;
            if (handle != null)
                handle.index = 2 * cell;
            _heap[cell].FirstHandle = handle;
        }


        /// <summary>
        /// Add the elements from another collection with a more specialized item type 
        /// to this collection. 
        /// </summary>
        /// <typeparam name="U">The type of items to add</typeparam>
        /// <param name="items">The items to add</param>
        [Tested]
        public void AddAll<U>(SCG.IEnumerable<U> items) where U : T
        {
            _stamp++;
            int oldsize = _size;
            foreach (T item in items)
                add(null, item);
            if (_size != oldsize)
            {
                if ((ActiveEvents & EventTypeEnum.Added) != 0)
                    foreach (T item in items)
                        raiseItemsAdded(item, 1);
                raiseCollectionChanged();
            }
        }

        #endregion

        #region ICollection<T> members

        /// <summary>
        /// 
        /// </summary>
        /// <value>True if this collection is empty.</value>
        [Tested]
        public override bool IsEmpty { [Tested]get { return _size == 0; } }

        /// <summary>
        /// 
        /// </summary>
        /// <value>The size of this collection</value>
        [Tested]
        public override int Count { [Tested]get { return _size; } }


        /// <summary>
        /// The value is symbolic indicating the type of asymptotic complexity
        /// in terms of the size of this collection (worst-case or amortized as
        /// relevant).
        /// </summary>
        /// <value>A characterization of the speed of the 
        /// <code>Count</code> property in this collection.</value>
        public override Speed CountSpeed { get { return Speed.Constant; } }

        /// <summary>
        /// Choose some item of this collection. 
        /// </summary>
        /// <exception cref="NoSuchItemException">if collection is empty.</exception>
        /// <returns></returns>
        public override T Choose()
        {
            if (_size == 0)
                throw new NoSuchItemException("Collection is empty");
            return _heap[0].First;
        }


        /// <summary>
        /// Create an enumerator for the collection
        /// <para>Note: the enumerator does *not* enumerate the items in sorted order, 
        /// but in the internal table order.</para>
        /// </summary>
        /// <returns>The enumerator(SIC)</returns>
        [Tested]
        public override SCG.IEnumerator<T> GetEnumerator()
        {
            int mystamp = _stamp;
            for (int i = 0; i < _size; i++)
            {
                if (mystamp != _stamp) throw new CollectionModifiedException();
                yield return i % 2 == 0 ? _heap[i >> 1].First : _heap[i >> 1].Last;
            }
            yield break;
        }


        #endregion

        #region Diagnostics
        private bool check(int i, T min, T max)
        {
            bool retval = true;
            Interval interval = _heap[i];
            T first = interval.First, last = interval.Last;

            if (2 * i + 1 == _size)
            {
                if (_comparer.Compare(min, first) > 0)
                {
                    Console.WriteLine("Cell {0}: parent.first({1}) > first({2})  [size={3}]", i, min, first, _size);
                    retval = false;
                }

                if (_comparer.Compare(first, max) > 0)
                {
                    Console.WriteLine("Cell {0}: first({1}) > parent.last({2})  [size={3}]", i, first, max, _size);
                    retval = false;
                }
                if (interval.FirstHandle != null && interval.FirstHandle.index != 2 * i)
                {
                    Console.WriteLine("Cell {0}: firsthandle.index({1}) != 2*cell({2})  [size={3}]", i, interval.FirstHandle.index, 2 * i, _size);
                    retval = false;
                }

                return retval;
            }
            else
            {
                if (_comparer.Compare(min, first) > 0)
                {
                    Console.WriteLine("Cell {0}: parent.first({1}) > first({2})  [size={3}]", i, min, first, _size);
                    retval = false;
                }

                if (_comparer.Compare(first, last) > 0)
                {
                    Console.WriteLine("Cell {0}: first({1}) > last({2})  [size={3}]", i, first, last, _size);
                    retval = false;
                }

                if (_comparer.Compare(last, max) > 0)
                {
                    Console.WriteLine("Cell {0}: last({1}) > parent.last({2})  [size={3}]", i, last, max, _size);
                    retval = false;
                }
                if (interval.FirstHandle != null && interval.FirstHandle.index != 2 * i)
                {
                    Console.WriteLine("Cell {0}: firsthandle.index({1}) != 2*cell({2})  [size={3}]", i, interval.FirstHandle.index, 2 * i, _size);
                    retval = false;
                }
                if (interval.LastHandle != null && interval.LastHandle.index != 2 * i + 1)
                {
                    Console.WriteLine("Cell {0}: lasthandle.index({1}) != 2*cell+1({2})  [size={3}]", i, interval.LastHandle.index, 2 * i + 1, _size);
                    retval = false;
                }

                int l = 2 * i + 1, r = l + 1;

                if (2 * l < _size)
                    retval = retval && check(l, first, last);

                if (2 * r < _size)
                    retval = retval && check(r, first, last);
            }

            return retval;
        }


        /// <summary>
        /// Check the integrity of the internal data structures of this collection.
        /// Only avaliable in DEBUG builds???
        /// </summary>
        /// <returns>True if check does not fail.</returns>
        [Tested]
        public bool Check()
        {
            if (_size == 0)
                return true;

            if (_size == 1)
                return (object) (_heap[0].First) != null;

            return check(0, _heap[0].First, _heap[0].Last);
        }

        #endregion

        #region IPriorityQueue<T> Members

        [Serializable]
        class Handle : IPriorityQueueHandle<T>
        {
            /// <summary>
            /// To save space, the index is 2*cell for heap[cell].first, and 2*cell+1 for heap[cell].last
            /// </summary>
            internal int index = -1;

            public override string ToString()
            {
                return String.Format("[{0}]", index);
            }

        }

        /// <summary>
        /// Get or set the item corresponding to a handle. 
        /// </summary>
        /// <exception cref="InvalidPriorityQueueHandleException">if the handle is invalid for this queue</exception>
        /// <param name="handle">The reference into the heap</param>
        /// <returns></returns>
        [Tested]
        public T this[IPriorityQueueHandle<T> handle]
        {
            get
            {
                int cell;
                bool isfirst;
                checkHandle(handle, out cell, out isfirst);

                return isfirst ? _heap[cell].First : _heap[cell].Last;
            }
            set
            {
                Replace(handle, value);
            }
        }


        /// <summary>
        /// Check safely if a handle is valid for this queue and if so, report the corresponding queue item.
        /// </summary>
        /// <param name="handle">The handle to check</param>
        /// <param name="item">If the handle is valid this will contain the corresponding item on output.</param>
        /// <returns>True if the handle is valid.</returns>
        public bool Find(IPriorityQueueHandle<T> handle, out T item)
        {
            Handle myhandle = handle as Handle;
            if (myhandle == null)
            {
                item = default(T);
                return false;
            }
            int toremove = myhandle.index;
            int cell = toremove / 2;
            bool isfirst = toremove % 2 == 0;
            {
                if (toremove == -1 || toremove >= _size)
                {
                    item = default(T);
                    return false;
                }
                Handle actualhandle = isfirst ? _heap[cell].FirstHandle : _heap[cell].LastHandle;
                if (actualhandle != myhandle)
                {
                    item = default(T);
                    return false;
                }
            }
            item = isfirst ? _heap[cell].First : _heap[cell].Last;
            return true;
        }


        /// <summary>
        /// Add an item to the priority queue, receiving a 
        /// handle for the item in the queue, 
        /// or reusing an already existing handle.
        /// </summary>
        /// <param name="handle">On output: a handle for the added item. 
        /// On input: null for allocating a new handle, an invalid handle for reuse. 
        /// A handle for reuse must be compatible with this priority queue, 
        /// by being created by a priority queue of the same runtime type, but not 
        /// necessarily the same priority queue object.</param>
        /// <param name="item">The item to add.</param>
        /// <returns>True since item will always be added unless the call throws an exception.</returns>
        [Tested]
        public bool Add(ref IPriorityQueueHandle<T> handle, T item)
        {
            _stamp++;
            Handle myhandle = (Handle) handle;
            if (myhandle == null)
                handle = myhandle = new Handle();
            else
                if (myhandle.index != -1)
                    throw new InvalidPriorityQueueHandleException("Handle not valid for reuse");
            if (add(myhandle, item))
            {
                raiseItemsAdded(item, 1);
                raiseCollectionChanged();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Delete an item with a handle from a priority queue.
        /// </summary>
        /// <exception cref="InvalidPriorityQueueHandleException">if the handle is invalid</exception>
        /// <param name="handle">The handle for the item. The handle will be invalidated, but reusable.</param>
        /// <returns>The deleted item</returns>
        [Tested]
        public T Delete(IPriorityQueueHandle<T> handle)
        {
            _stamp++;
            int cell;
            bool isfirst;
            Handle myhandle = checkHandle(handle, out cell, out isfirst);

            T retval;
            myhandle.index = -1;
            int lastcell = (_size - 1) / 2;

            if (cell == lastcell)
            {
                if (isfirst)
                {
                    retval = _heap[cell].First;
                    if (_size % 2 == 0)
                    {
                        updateFirst(cell, _heap[cell].Last, _heap[cell].LastHandle);
                        _heap[cell].Last = default(T);
                        _heap[cell].LastHandle = null;
                    }
                    else
                    {
                        _heap[cell].First = default(T);
                        _heap[cell].FirstHandle = null;
                    }
                }
                else
                {
                    retval = _heap[cell].Last;
                    _heap[cell].Last = default(T);
                    _heap[cell].LastHandle = null;
                }
                _size--;
            }
            else if (isfirst)
            {
                retval = _heap[cell].First;

                if (_size % 2 == 0)
                {
                    updateFirst(cell, _heap[lastcell].Last, _heap[lastcell].LastHandle);
                    _heap[lastcell].Last = default(T);
                    _heap[lastcell].LastHandle = null;
                }
                else
                {
                    updateFirst(cell, _heap[lastcell].First, _heap[lastcell].FirstHandle);
                    _heap[lastcell].First = default(T);
                    _heap[lastcell].FirstHandle = null;
                }

                _size--;
                if (heapifyMin(cell))
                    bubbleUpMax(cell);
                else
                    bubbleUpMin(cell);
            }
            else
            {
                retval = _heap[cell].Last;

                if (_size % 2 == 0)
                {
                    updateLast(cell, _heap[lastcell].Last, _heap[lastcell].LastHandle);
                    _heap[lastcell].Last = default(T);
                    _heap[lastcell].LastHandle = null;
                }
                else
                {
                    updateLast(cell, _heap[lastcell].First, _heap[lastcell].FirstHandle);
                    _heap[lastcell].First = default(T);
                    _heap[lastcell].FirstHandle = null;
                }

                _size--;
                if (heapifyMax(cell))
                    bubbleUpMin(cell);
                else
                    bubbleUpMax(cell);
            }

            raiseItemsRemoved(retval, 1);
            raiseCollectionChanged();

            return retval;
        }

        private Handle checkHandle(IPriorityQueueHandle<T> handle, out int cell, out bool isfirst)
        {
            Handle myhandle = (Handle) handle;
            int toremove = myhandle.index;
            cell = toremove / 2;
            isfirst = toremove % 2 == 0;
            {
                if (toremove == -1 || toremove >= _size)
                    throw new InvalidPriorityQueueHandleException("Invalid handle, index out of range");
                Handle actualhandle = isfirst ? _heap[cell].FirstHandle : _heap[cell].LastHandle;
                if (actualhandle != myhandle)
                    throw new InvalidPriorityQueueHandleException("Invalid handle, doesn't match queue");
            }
            return myhandle;
        }


        /// <summary>
        /// Replace an item with a handle in a priority queue with a new item. 
        /// Typically used for changing the priority of some queued object.
        /// </summary>
        /// <param name="handle">The handle for the old item</param>
        /// <param name="item">The new item</param>
        /// <returns>The old item</returns>
        [Tested]
        public T Replace(IPriorityQueueHandle<T> handle, T item)
        {
            _stamp++;
            int cell;
            bool isfirst;
            checkHandle(handle, out cell, out isfirst);
            if (_size == 0)
                throw new NoSuchItemException();

            T retval;

            if (isfirst)
            {
                retval = _heap[cell].First;
                _heap[cell].First = item;
                if (_size == 2 * cell + 1) // cell == lastcell
                {
                    int p = (cell + 1) / 2 - 1;
                    if (_comparer.Compare(item, _heap[p].Last) > 0)
                    {
                        Handle thehandle = _heap[cell].FirstHandle;
                        updateFirst(cell, _heap[p].Last, _heap[p].LastHandle);
                        updateLast(p, item, thehandle);
                        bubbleUpMax(p);
                    }
                    else
                        bubbleUpMin(cell);
                }
                else if (heapifyMin(cell))
                    bubbleUpMax(cell);
                else
                    bubbleUpMin(cell);
            }
            else
            {
                retval = _heap[cell].Last;
                _heap[cell].Last = item;
                if (heapifyMax(cell))
                    bubbleUpMin(cell);
                else
                    bubbleUpMax(cell);
            }

            raiseItemsRemoved(retval, 1);
            raiseItemsAdded(item, 1);
            raiseCollectionChanged();

            return retval;
        }

        /// <summary>
        /// Find the current least item of this priority queue.
        /// </summary>
        /// <param name="handle">On return: the handle of the item.</param>
        /// <returns>The least item.</returns>
        public T FindMin(out IPriorityQueueHandle<T> handle)
        {
            if (_size == 0)
                throw new NoSuchItemException();
            handle = _heap[0].FirstHandle;

            return _heap[0].First;
        }

        /// <summary>
        /// Find the current largest item of this priority queue.
        /// </summary>
        /// <param name="handle">On return: the handle of the item.</param>
        /// <returns>The largest item.</returns>
        public T FindMax(out IPriorityQueueHandle<T> handle)
        {
            if (_size == 0)
                throw new NoSuchItemException();
            else if (_size == 1)
            {
                handle = _heap[0].FirstHandle;
                return _heap[0].First;
            }
            else
            {
                handle = _heap[0].LastHandle;
                return _heap[0].Last;
            }
        }

        /// <summary>
        /// Remove the least item from this priority queue.
        /// </summary>
        /// <param name="handle">On return: the handle of the removed item.</param>
        /// <returns>The removed item.</returns>
        public T DeleteMin(out IPriorityQueueHandle<T> handle)
        {
            _stamp++;
            if (_size == 0)
                throw new NoSuchItemException();

            T retval = _heap[0].First;
            Handle myhandle = _heap[0].FirstHandle;
            handle = myhandle;
            if (myhandle != null)
                myhandle.index = -1;

            if (_size == 1)
            {
                _size = 0;
                _heap[0].First = default(T);
                _heap[0].FirstHandle = null;
            }
            else
            {
                int lastcell = (_size - 1) / 2;

                if (_size % 2 == 0)
                {
                    updateFirst(0, _heap[lastcell].Last, _heap[lastcell].LastHandle);
                    _heap[lastcell].Last = default(T);
                    _heap[lastcell].LastHandle = null;
                }
                else
                {
                    updateFirst(0, _heap[lastcell].First, _heap[lastcell].FirstHandle);
                    _heap[lastcell].First = default(T);
                    _heap[lastcell].FirstHandle = null;
                }

                _size--;
                heapifyMin(0);
            }

            raiseItemsRemoved(retval, 1);
            raiseCollectionChanged();
            return retval;

        }

        /// <summary>
        /// Remove the largest item from this priority queue.
        /// </summary>
        /// <param name="handle">On return: the handle of the removed item.</param>
        /// <returns>The removed item.</returns>
        public T DeleteMax(out IPriorityQueueHandle<T> handle)
        {
            _stamp++;
            if (_size == 0)
                throw new NoSuchItemException();

            T retval;
            Handle myhandle;

            if (_size == 1)
            {
                _size = 0;
                retval = _heap[0].First;
                myhandle = _heap[0].FirstHandle;
                if (myhandle != null)
                    myhandle.index = -1;
                _heap[0].First = default(T);
                _heap[0].FirstHandle = null;
            }
            else
            {
                retval = _heap[0].Last;
                myhandle = _heap[0].LastHandle;
                if (myhandle != null)
                    myhandle.index = -1;

                int lastcell = (_size - 1) / 2;

                if (_size % 2 == 0)
                {
                    updateLast(0, _heap[lastcell].Last, _heap[lastcell].LastHandle);
                    _heap[lastcell].Last = default(T);
                    _heap[lastcell].LastHandle = null;
                }
                else
                {
                    updateLast(0, _heap[lastcell].First, _heap[lastcell].FirstHandle);
                    _heap[lastcell].First = default(T);
                    _heap[lastcell].FirstHandle = null;
                }

                _size--;
                heapifyMax(0);
            }
            raiseItemsRemoved(retval, 1);
            raiseCollectionChanged();
            handle = myhandle;
            return retval;
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Make a shallow copy of this IntervalHeap.
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            IntervalHeap<T> clone = new IntervalHeap<T>(_size, _comparer, _itemEqualityComparer);
            clone.AddAll(this);
            return clone;
        }

        #endregion

    }

}
