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

#define HASHINDEX

using System;
using System.Diagnostics;
using SCG = System.Collections.Generic;

namespace C5
{
    /// <summary>
    /// A list collection based on a plain dynamic array data structure.
    /// Expansion of the internal array is performed by doubling on demand. 
    /// The internal array is only shrinked by the Clear method. 
    ///
    /// <i>When the FIFO property is set to false this class works fine as a stack of T.
    /// When the FIFO property is set to true the class will function as a (FIFO) queue
    /// but very inefficiently, use a LinkedList (<see cref="T:C5.LinkedList`1"/>) instead.</i>
    /// </summary>
    [Serializable]
    public class ArrayList<T> : ArrayBase<T>, IList<T>, SCG.IList<T> //, System.Runtime.Serialization.ISerializable
#if HASHINDEX
#else
        , IStack<T>, IQueue<T>
#endif
    {
        #region Fields

        /// <summary>
        /// Has this list or view not been invalidated by some operation (by someone calling Dispose())
        /// </summary>
        private bool _isValid = true;

        //TODO: wonder if we should save some memory on none-view situations by 
        //      putting these three fields into a single ref field?
        /// <summary>
        /// The underlying list if we are a view, null else.
        /// </summary>
        private ArrayList<T> _innerList;
        private WeakViewList<ArrayList<T>> _views;
        private WeakViewList<ArrayList<T>>.Node _myWeakReference;

        /// <summary>
        /// The size of the underlying list.
        /// </summary>
        int InnerSize
        {
            get { return (_innerList ?? this).Size; }
        }

        /// <summary>
        /// The underlying field of the FIFO property
        /// </summary>
        bool _isFifo = false;

#if HASHINDEX
        private HashSet<Multiplicity<T>> _itemIndex;
#endif
        #endregion

        #region Events

        public override EventTypeEnum ListenableEvents { get { return _innerList == null ? EventTypeEnum.All : EventTypeEnum.None; } }

        /*
            /// <summary>
            /// 
            /// </summary>
            /// <value></value>
            public override event CollectionChangedHandler<T> CollectionChanged
            {
              add
              {
                if (underlying == null)
                  base.CollectionChanged += value;
                else
                  throw new UnlistenableEventException("Can't listen to a view");
              }
              remove
              {
                if (underlying == null)
                  base.CollectionChanged -= value;
                else
                  throw new UnlistenableEventException("Can't listen to a view");
              }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <value></value>
            public override event CollectionClearedHandler<T> CollectionCleared
            {
              add
              {
                if (underlying == null)
                  base.CollectionCleared += value;
                else
                  throw new UnlistenableEventException("Can't listen to a view");
              }
              remove
              {
                if (underlying == null)
                  base.CollectionCleared -= value;
                else
                  throw new UnlistenableEventException("Can't listen to a view");
              }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <value></value>
            public override event ItemsAddedHandler<T> ItemsAdded
            {
              add
              {
                if (underlying == null)
                  base.ItemsAdded += value;
                else
                  throw new UnlistenableEventException("Can't listen to a view");
              }
              remove
              {
                if (underlying == null)
                  base.ItemsAdded -= value;
                else
                  throw new UnlistenableEventException("Can't listen to a view");
              }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <value></value>
            public override event ItemInsertedHandler<T> ItemInserted
            {
              add
              {
                if (underlying == null)
                  base.ItemInserted += value;
                else
                  throw new UnlistenableEventException("Can't listen to a view");
              }
              remove
              {
                if (underlying == null)
                  base.ItemInserted -= value;
                else
                  throw new UnlistenableEventException("Can't listen to a view");
              }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <value></value>
            public override event ItemsRemovedHandler<T> ItemsRemoved
            {
              add
              {
                if (underlying == null)
                  base.ItemsRemoved += value;
                else
                  throw new UnlistenableEventException("Can't listen to a view");
              }
              remove
              {
                if (underlying == null)
                  base.ItemsRemoved -= value;
                else
                  throw new UnlistenableEventException("Can't listen to a view");
              }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <value></value>
            public override event ItemRemovedAtHandler<T> ItemRemovedAt
            {
              add
              {
                if (underlying == null)
                  base.ItemRemovedAt += value;
                else
                  throw new UnlistenableEventException("Can't listen to a view");
              }
              remove
              {
                if (underlying == null)
                  base.ItemRemovedAt -= value;
                else
                  throw new UnlistenableEventException("Can't listen to a view");
              }
            }

              */

        #endregion

        #region Util

        private bool Equals(T i1, T i2) { return ItemEqualityComparer.Equals(i1, i2); }

        /// <summary>
        /// Increment or decrement the private size fields
        /// </summary>
        /// <param name="delta">Increment (with sign)</param>
        void addtosize(int delta)
        {
            Size += delta;
            if (_innerList != null)
                _innerList.Size += delta;
        }

        #region Array handling
        /// <summary>
        /// Double the size of the internal array.
        /// </summary>
        protected override void expand()
        { expand(2 * array.Length, InnerSize); }


        /// <summary>
        /// Expand the internal array, resetting the index of the first unused element.
        /// </summary>
        /// <param name="newcapacity">The new capacity (will be rouded upwards to a power of 2).</param>
        /// <param name="newsize">The new count of </param>
        protected override void expand(int newcapacity, int newsize)
        {
            base.expand(newcapacity, newsize);
            if (_innerList != null)
                _innerList.array = array;
        }

        #endregion

        #region Checks
        /// <summary>
        /// Check if it is valid to perform updates and increment stamp if so.
        /// </summary>
        /// <exception cref="ViewDisposedException"> If check fails by this list being a disposed view.</exception>
        /// <exception cref="ReadOnlyCollectionException"> If check fails by this being a read only list.</exception>
        protected override void updatecheck()
        {
            validitycheck();
            base.updatecheck();
            if (_innerList != null)
                _innerList.Stamp++;
        }


        /// <summary>
        /// Check if we are a view that the underlying list has only been updated through us.
        /// <para>This method should be called from enumerators etc to guard against 
        /// modification of the base collection.</para>
        /// </summary>
        /// <exception cref="ViewDisposedException"> if check fails.</exception>
        void validitycheck()
        {
            if (!_isValid)
                throw new ViewDisposedException();
        }


        /// <summary>
        /// Check that the list has not been updated since a particular time.
        /// <para>To be used by enumerators and range </para>
        /// </summary>
        /// <exception cref="ViewDisposedException"> If check fails by this list being a disposed view.</exception>
        /// <exception cref="CollectionModifiedException">If the list *has* beeen updated since that  time..</exception>
        /// <param name="stamp">The stamp indicating the time.</param>
        protected override void modifycheck(int stamp)
        {
            validitycheck();
            if (this.Stamp != stamp)
                throw new CollectionModifiedException();
        }

        #endregion

        #region Searching

        /// <summary>
        /// Internal version of IndexOf without modification checks.
        /// </summary>
        /// <param name="item">Item to look for</param>
        /// <returns>The index of first occurrence</returns>
        int indexOf(T item)
        {
#if HASHINDEX
            Multiplicity<T> p = new Multiplicity<T>(item);
            if (_itemIndex.Find(ref p) && p.Count >= offset && p.Count < offset + Size)
                return p.Count - offset;
#else
            for (int i = 0; i < Size; i++)
                if (Equals(item, array[offset + i]))
                    return i;
#endif
            return ~Size;
        }

        /// <summary>
        /// Internal version of LastIndexOf without modification checks.
        /// </summary>
        /// <param name="item">Item to look for</param>
        /// <returns>The index of last occurrence</returns>
        int lastIndexOf(T item)
        {
#if HASHINDEX
            return indexOf(item);
#else
            for (int i = Size - 1; i >= 0; i--)
                if (Equals(item, array[offset + i]))
                    return i;
            return ~Size;
#endif
        }
        #endregion

        #region Inserting

#if HASHINDEX
        /// <summary>
        /// Internal version of Insert with no modification checks.
        /// </summary>
        /// <exception cref="DuplicateNotAllowedException"> if item already in list.</exception>
        /// <param name="i">Index to insert at</param>
        /// <param name="item">Item to insert</param>
#else
        /// <summary>
        /// Internal version of Insert with no modification checks.
        /// </summary>
        /// <param name="i">Index to insert at</param>
        /// <param name="item">Item to insert</param>
#endif
        protected override void insert(int i, T item)
        {
#if HASHINDEX
            Multiplicity<T> p = new Multiplicity<T>(item, offset + i);
            if (_itemIndex.FindOrAdd(ref p))
                throw new DuplicateNotAllowedException("Item already in indexed list: " + item);
#endif
            baseinsert(i, item);
#if HASHINDEX
            reindex(i + offset + 1);
#endif
        }

        private void baseinsert(int i, T item)
        {
            if (InnerSize == array.Length)
                expand();
            i += offset;
            if (i < InnerSize)
                Array.Copy(array, i, array, i + 1, InnerSize - i);
            array[i] = item;
            addtosize(1);
            fixViewsAfterInsert(1, i);
        }
        #endregion

        #region Removing

        /// <summary>
        /// Internal version of RemoveAt with no modification checks.
        /// </summary>
        /// <param name="i">Index to remove at</param>
        /// <returns>The removed item</returns>
        T removeAt(int i)
        {
            i += offset;
            fixViewsBeforeSingleRemove(i);
            T retval = array[i];
            addtosize(-1);
            if (InnerSize > i)
                Array.Copy(array, i + 1, array, i, InnerSize - i);
            array[InnerSize] = default(T);
#if HASHINDEX
            _itemIndex.Remove(new Multiplicity<T>(retval));
            reindex(i);
#endif
            return retval;
        }
        #endregion

        #region Indexing

#if HASHINDEX
        private void reindex(int start) { reindex(start, InnerSize); }

        private void reindex(int start, int end)
        {
            for (int j = start; j < end; j++)
                _itemIndex.UpdateOrAdd(new Multiplicity<T>(array[j], j));
        }
#endif
        #endregion

        #region fixView utilities

        /// <summary>
        /// 
        /// </summary>
        /// <param name="added">The actual number of inserted nodes</param>
        /// <param name="realInsertionIndex"></param>
        void fixViewsAfterInsert(int added, int realInsertionIndex)
        {
            if (_views != null)
                foreach (ArrayList<T> view in _views)
                {
                    if (view != this)
                    {
                        if (view.offset < realInsertionIndex && view.offset + view.Size > realInsertionIndex)
                            view.Size += added;
                        if (view.offset > realInsertionIndex || (view.offset == realInsertionIndex && view.Size > 0))
                            view.offset += added;
                    }
                }
        }

        void fixViewsBeforeSingleRemove(int realRemovalIndex)
        {
            if (_views != null)
                foreach (ArrayList<T> view in _views)
                {
                    if (view != this)
                    {
                        if (view.offset <= realRemovalIndex && view.offset + view.Size > realRemovalIndex)
                            view.Size--;
                        if (view.offset > realRemovalIndex)
                            view.offset--;
                    }
                }
        }

        /// <summary>
        /// Fix offsets and sizes of other views before removing an interval from this 
        /// </summary>
        /// <param name="start">the start of the interval relative to the array/underlying</param>
        /// <param name="count"></param>
        void fixViewsBeforeRemove(int start, int count)
        {
            int clearend = start + count - 1;
            if (_views != null)
                foreach (ArrayList<T> view in _views)
                {
                    if (view == this)
                        continue;
                    int viewoffset = view.offset, viewend = viewoffset + view.Size - 1;
                    if (start < viewoffset)
                    {
                        if (clearend < viewoffset)
                            view.offset = viewoffset - count;
                        else
                        {
                            view.offset = start;
                            view.Size = clearend < viewend ? viewend - clearend : 0;
                        }
                    }
                    else if (start <= viewend)
                        view.Size = clearend <= viewend ? view.Size - count : start - viewoffset;
                }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherOffset"></param>
        /// <param name="otherSize"></param>
        /// <returns>The position of View(otherOffset, otherSize) wrt. this view</returns>
        MutualViewPosition viewPosition(int otherOffset, int otherSize)
        {
            int end = offset + Size, otherEnd = otherOffset + otherSize;
            if (otherOffset >= end || otherEnd <= offset)
                return MutualViewPosition.NonOverlapping;
            if (Size == 0 || (otherOffset <= offset && end <= otherEnd))
                return MutualViewPosition.Contains;
            if (otherSize == 0 || (offset <= otherOffset && otherEnd <= end))
                return MutualViewPosition.ContainedIn;
            return MutualViewPosition.Overlapping;
        }

        //TODO: make version that fits the new, more forgiving rules for disposing
        void disposeOverlappingViews(bool reverse)
        {
            if (_views != null)
                foreach (ArrayList<T> view in _views)
                {
                    if (view != this)
                    {
                        switch (viewPosition(view.offset, view.Size))
                        {
                            case MutualViewPosition.ContainedIn:
                                if (reverse)
                                    view.offset = 2 * offset + Size - view.Size - view.offset;
                                else
                                    view.Dispose();
                                break;
                            case MutualViewPosition.Overlapping:
                                view.Dispose();
                                break;
                            case MutualViewPosition.Contains:
                            case MutualViewPosition.NonOverlapping:
                                break;
                        }
                    }
                }
        }
        #endregion

        #endregion

        #region Position, PositionComparer and ViewHandler nested types
        class PositionComparer : SCG.IComparer<Position>
        {
            public int Compare(Position a, Position b)
            {
                return a.index.CompareTo(b.index);
            }
        }
        /// <summary>
        /// During RemoveAll, we need to cache the original endpoint indices of views (??? also for HashedArrayList?)
        /// </summary>
        struct Position
        {
            public readonly ArrayList<T> view;
            public readonly int index;
            public Position(ArrayList<T> view, bool left)
            {
                this.view = view;
                index = left ? view.offset : view.offset + view.Size - 1;
            }
            public Position(int index) { this.index = index; view = null; }
        }

        /// <summary>
        /// Handle the update of (other) views during a multi-remove operation.
        /// </summary>
        struct ViewHandler
        {
            ArrayList<Position> leftEnds;
            ArrayList<Position> rightEnds;
            int leftEndIndex, rightEndIndex;
            internal readonly int viewCount;
            internal ViewHandler(ArrayList<T> list)
            {
                leftEndIndex = rightEndIndex = viewCount = 0;
                leftEnds = rightEnds = null;
                if (list._views != null)
                    foreach (ArrayList<T> v in list._views)
                        if (v != list)
                        {
                            if (leftEnds == null)
                            {
                                leftEnds = new ArrayList<Position>();
                                rightEnds = new ArrayList<Position>();
                            }
                            leftEnds.Add(new Position(v, true));
                            rightEnds.Add(new Position(v, false));
                        }
                if (leftEnds == null)
                    return;
                viewCount = leftEnds.Count;
                leftEnds.Sort(new PositionComparer());
                rightEnds.Sort(new PositionComparer());
            }
            /// <summary>
            /// This is to be called with realindex pointing to the first node to be removed after a (stretch of) node that was not removed
            /// </summary>
            /// <param name="removed"></param>
            /// <param name="realindex"></param>
            internal void skipEndpoints(int removed, int realindex)
            {
                if (viewCount > 0)
                {
                    Position endpoint;
                    while (leftEndIndex < viewCount && (endpoint = leftEnds[leftEndIndex]).index <= realindex)
                    {
                        ArrayList<T> view = endpoint.view;
                        view.offset = view.offset - removed;
                        view.Size += removed;
                        leftEndIndex++;
                    }
                    while (rightEndIndex < viewCount && (endpoint = rightEnds[rightEndIndex]).index < realindex)
                    {
                        endpoint.view.Size -= removed;
                        rightEndIndex++;
                    }
                }
            }
            internal void updateViewSizesAndCounts(int removed, int realindex)
            {
                if (viewCount > 0)
                {
                    Position endpoint;
                    while (leftEndIndex < viewCount && (endpoint = leftEnds[leftEndIndex]).index <= realindex)
                    {
                        ArrayList<T> view = endpoint.view;
                        view.offset = view.Offset - removed;
                        view.Size += removed;
                        leftEndIndex++;
                    }
                    while (rightEndIndex < viewCount && (endpoint = rightEnds[rightEndIndex]).index < realindex)
                    {
                        endpoint.view.Size -= removed;
                        rightEndIndex++;
                    }
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Create an array list with default item equalityComparer and initial capacity 8 items.
        /// </summary>
        public ArrayList() : this(8) { }


        /// <summary>
        /// Create an array list with external item equalityComparer and initial capacity 8 items.
        /// </summary>
        /// <param name="itemEqualityComparer">The external item equalityComparer</param>
        public ArrayList(SCG.IEqualityComparer<T> itemEqualityComparer) : this(8, itemEqualityComparer) { }


        /// <summary>
        /// Create an array list with default item equalityComparer and prescribed initial capacity.
        /// </summary>
        /// <param name="capacity">The prescribed capacity</param>
        public ArrayList(int capacity) : this(capacity, EqualityComparer<T>.Default) { }


        /// <summary>
        /// Create an array list with external item equalityComparer and prescribed initial capacity.
        /// </summary>
        /// <param name="capacity">The prescribed capacity</param>
        /// <param name="itemEqualityComparer">The external item equalityComparer</param>
        public ArrayList(int capacity, SCG.IEqualityComparer<T> itemEqualityComparer)
            : base(capacity, itemEqualityComparer)
        {
#if HASHINDEX
            _itemIndex = new HashSet<Multiplicity<T>>(new MultiplicityEqualityComparer<T>(itemEqualityComparer));
#endif
        }

        #endregion

        #region IList<T> Members

        /// <summary>
        /// </summary>
        /// <exception cref="NoSuchItemException"> if this list is empty.</exception>
        /// <value>The first item in this list.</value>
        [Tested]
        public virtual T First
        {
            [Tested]
            get
            {
                validitycheck();
                if (Size == 0)
                    throw new NoSuchItemException();

                return array[offset];
            }
        }


        /// <summary>
        /// </summary>
        /// <exception cref="NoSuchItemException"> if this list is empty.</exception>
        /// <value>The last item in this list.</value>
        [Tested]
        public virtual T Last
        {
            [Tested]
            get
            {
                validitycheck();
                if (Size == 0)
                    throw new NoSuchItemException();

                return array[offset + Size - 1];
            }
        }


        /// <summary>
        /// Since <code>Add(T item)</code> always add at the end of the list,
        /// this describes if list has FIFO or LIFO semantics.
        /// </summary>
        /// <value>True if the <code>Remove()</code> operation removes from the
        /// start of the list, false if it removes from the end. The default for a new array list is false.</value>
        [Tested]
        public virtual bool FIFO
        {
            [Tested]
            get { validitycheck(); return _isFifo; }
            [Tested]
            set { updatecheck(); _isFifo = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsFixedSize
        {
            get { validitycheck(); return false; }
        }


#if HASHINDEX
        /// <summary>
        /// On this list, this indexer is read/write.
        /// </summary>
        /// <exception cref="IndexOutOfRangeException"> if index is negative or
        /// &gt;= the size of the collection.</exception>
        /// <exception cref="DuplicateNotAllowedException"> By the get operation
        /// if the item already is present somewhere else in the list.</exception>
        /// <value>The index'th item of this list.</value>
        /// <param name="index">The index of the item to fetch or store.</param>
#else
        /// <summary>
        /// On this list, this indexer is read/write.
        /// </summary>
        /// <exception cref="IndexOutOfRangeException"> if index is negative or
        /// &gt;= the size of the collection.</exception>
        /// <value>The index'th item of this list.</value>
        /// <param name="index">The index of the item to fetch or store.</param>
#endif
        [Tested]
        public virtual T this[int index]
        {
            [Tested]
            get
            {
                validitycheck();
                if (index < 0 || index >= Size)
                    throw new IndexOutOfRangeException();

                return array[offset + index];
            }
            [Tested]
            set
            {
                updatecheck();
                if (index < 0 || index >= Size)
                    throw new IndexOutOfRangeException();
                index += offset;
                T item = array[index];
#if HASHINDEX
                Multiplicity<T> p = new Multiplicity<T>(value, index);
                if (ItemEqualityComparer.Equals(value, item))
                {
                    array[index] = value;
                    _itemIndex.Update(p);
                }
                else if (!_itemIndex.FindOrAdd(ref p))
                {
                    _itemIndex.Remove(new Multiplicity<T>(item));
                    array[index] = value;
                }
                else
                {
                    throw new DuplicateNotAllowedException("Item already in indexed list");
                }
#else
                array[index] = value;
#endif
                (_innerList ?? this).raiseForSetThis(index, value, item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public virtual Speed IndexingSpeed { get { return Speed.Constant; } }

#if HASHINDEX
        /// <summary>
        /// Insert an item at a specific index location in this list. 
        ///</summary>
        /// <exception cref="IndexOutOfRangeException"> if index is negative or
        /// &gt; the size of the collection. </exception>
        /// <exception cref="DuplicateNotAllowedException"> 
        /// If the item is already present in the list.</exception>
        /// <param name="index">The index at which to insert.</param>
        /// <param name="item">The item to insert.</param>
#else
        /// <summary>
        /// Insert an item at a specific index location in this list. 
        ///</summary>
        /// <exception cref="IndexOutOfRangeException"> if i is negative or
        /// &gt; the size of the collection. </exception>
        /// <param name="index">The index at which to insert.</param>
        /// <param name="item">The item to insert.</param>
#endif
        [Tested]
        public virtual void Insert(int index, T item)
        {
            updatecheck();
            if (index < 0 || index > Size)
                throw new IndexOutOfRangeException();

            insert(index, item);
            (_innerList ?? this).raiseForInsert(index + offset, item);
        }

        /// <summary>
        /// Insert an item at the end of a compatible view, used as a pointer.
        /// <para>The <code>pointer</code> must be a view on the same list as
        /// <code>this</code> and the endpoitn of <code>pointer</code> must be
        /// a valid insertion point of <code>this</code></para>
        /// </summary>
        /// <exception cref="IncompatibleViewException">If <code>pointer</code> 
        /// is not a view on or the same list as <code>this</code></exception>
        /// <exception cref="IndexOutOfRangeException"><b>??????</b> if the endpoint of 
        ///  <code>pointer</code> is not inside <code>this</code></exception>
        /// <exception cref="DuplicateNotAllowedException"> if the list has
        /// <code>AllowsDuplicates==false</code> and the item is 
        /// already in the list.</exception>
        /// <param name="pointer"></param>
        /// <param name="item"></param>
        public void Insert(IList<T> pointer, T item)
        {
            if ((pointer == null) || ((pointer.Underlying ?? pointer) != (_innerList ?? this)))
                throw new IncompatibleViewException();
            Insert(pointer.Offset + pointer.Count - Offset, item);
        }

#if HASHINDEX
        /// <summary>
        /// Insert into this list all items from an enumerable collection starting 
        /// at a particular index.
        /// </summary>
        /// <exception cref="IndexOutOfRangeException"> if index is negative or
        /// &gt; the size of the collection.</exception>
        /// <exception cref="DuplicateNotAllowedException"> If <code>items</code> 
        /// contains duplicates or some item already  present in the list.</exception>
        /// <param name="index">Index to start inserting at</param>
        /// <param name="items">Items to insert</param>
#else
        /// <summary>
        /// Insert into this list all items from an enumerable collection starting 
        /// at a particular index.
        /// </summary>
        /// <exception cref="IndexOutOfRangeException"> if index is negative or
        /// &gt; the size of the collection.</exception>
        /// <param name="index">Index to start inserting at</param>
        /// <param name="items">Items to insert</param>
        /// <typeparam name="U"></typeparam>
#endif
        [Tested]
        public virtual void InsertAll<U>(int index, SCG.IEnumerable<U> items) where U : T
        {
            updatecheck();
            if (index < 0 || index > Size)
                throw new IndexOutOfRangeException();
            index += offset;
            int toadd = EnumerableBase<U>.countItems(items);
            if (toadd == 0)
                return;
            if (toadd + InnerSize > array.Length)
                expand(toadd + InnerSize, InnerSize);
            if (InnerSize > index)
                Array.Copy(array, index, array, index + toadd, InnerSize - index);
            int i = index;
            try
            {

                foreach (T item in items)
                {
#if HASHINDEX
                    Multiplicity<T> p = new Multiplicity<T>(item, i);
                    if (_itemIndex.FindOrAdd(ref p))
                        throw new DuplicateNotAllowedException("Item already in indexed list");
#endif
                    array[i++] = item;
                }
            }
            finally
            {
                int added = i - index;
                if (added < toadd)
                {
                    Array.Copy(array, index + toadd, array, i, InnerSize - index);
                    Array.Clear(array, InnerSize + added, toadd - added);
                }
                if (added > 0)
                {
                    addtosize(added);
#if HASHINDEX
                    reindex(i);
#endif
                    fixViewsAfterInsert(added, index);
                    (_innerList ?? this).raiseForInsertAll(index, added);
                }
            }
        }
        private void raiseForInsertAll(int index, int added)
        {
            if (ActiveEvents != 0)
            {
                if ((ActiveEvents & (EventTypeEnum.Added | EventTypeEnum.Inserted)) != 0)
                    for (int j = index; j < index + added; j++)
                    {
                        raiseItemInserted(array[j], j);
                        raiseItemsAdded(array[j], 1);
                    }
                raiseCollectionChanged();
            }
        }

#if HASHINDEX
        /// <summary>
        /// Insert an item at the front of this list;
        /// </summary>
        /// <exception cref="DuplicateNotAllowedException">If the item is already in the list</exception>
        /// <param name="item">The item to insert.</param>
#else
        /// <summary>
        /// Insert an item at the front of this list;
        /// </summary>
        /// <param name="item">The item to insert.</param>
#endif
        [Tested]
        public virtual void InsertFirst(T item)
        {
            updatecheck();
            insert(0, item);
            (_innerList ?? this).raiseForInsert(offset, item);
        }


#if HASHINDEX
        /// <summary>
        /// Insert an item at the back of this list.
        /// </summary>
        /// <exception cref="DuplicateNotAllowedException">If the item is already in the list</exception>
        /// <param name="item">The item to insert.</param>
#else
        /// <summary>
        /// Insert an item at the back of this list.
        /// </summary>
        /// <param name="item">The item to insert.</param>
#endif
        [Tested]
        public virtual void InsertLast(T item)
        {
            updatecheck();
            insert(Size, item);
            (_innerList ?? this).raiseForInsert(Size - 1 + offset, item);
        }

        //NOTE: if the filter throws an exception, no result will be returned.
        /// <summary>
        /// Create a new list consisting of the items of this list satisfying a 
        /// certain predicate.
        /// <para>The new list will be of type HashedArrayList</para>
        /// </summary>
        /// <param name="filter">The filter delegate defining the predicate.</param>
        /// <returns>The new list.</returns>
        [Tested]
        public virtual IList<T> FindAll(Fun<T, bool> filter)
        {
            validitycheck();
            int stamp = this.Stamp;
            ArrayList<T> res = new ArrayList<T>(ItemEqualityComparer);
            int j = 0, rescap = res.array.Length;
            for (int i = 0; i < Size; i++)
            {
                T a = array[offset + i];
                bool found = filter(a);
                modifycheck(stamp);
                if (found)
                {
                    if (j == rescap) res.expand(rescap = 2 * rescap, j);
                    res.array[j++] = a;
                }
            }
            res.Size = j;
#if HASHINDEX
            res.reindex(0);
#endif
            return res;
        }


#if HASHINDEX
        /// <summary>
        /// Create a new list consisting of the results of mapping all items of this
        /// list. The new list will use the default item equalityComparer for the item type V.
        /// <para>The new list will be of type HashedArrayList</para>
        /// </summary>
        /// <exception cref="DuplicateNotAllowedException">If <code>mapper</code>
        /// creates duplicates</exception>
        /// <typeparam name="V">The type of items of the new list</typeparam>
        /// <param name="mapper">The delegate defining the map.</param>
        /// <returns>The new list.</returns>
#else
        /// <summary>
        /// Create a new list consisting of the results of mapping all items of this
        /// list. The new list will use the default item equalityComparer for the item type V.
        /// <para>The new list will be of type ArrayList</para>
        /// </summary>
        /// <typeparam name="V">The type of items of the new list</typeparam>
        /// <param name="mapper">The delegate defining the map.</param>
        /// <returns>The new list.</returns>
#endif
        [Tested]
        public virtual IList<V> Map<V>(Fun<T, V> mapper)
        {
            validitycheck();

            ArrayList<V> res = new ArrayList<V>(Size);

            return map<V>(mapper, res);
        }

#if HASHINDEX
        /// <summary>
        /// Create a new list consisting of the results of mapping all items of this
        /// list. The new list will use a specified item equalityComparer for the item type.
        /// <para>The new list will be of type HashedArrayList</para>
        /// </summary>
        /// <exception cref="DuplicateNotAllowedException">If <code>mapper</code>
        /// creates duplicates</exception>
        /// <typeparam name="V">The type of items of the new list</typeparam>
        /// <param name="mapper">The delegate defining the map.</param>
        /// <param name="itemEqualityComparer">The item equalityComparer to use for the new list</param>
        /// <returns>The new list.</returns>
#else
        /// <summary>
        /// Create a new list consisting of the results of mapping all items of this
        /// list. The new list will use a specified item equalityComparer for the item type.
        /// <para>The new list will be of type ArrayList</para>
        /// </summary>
        /// <typeparam name="V">The type of items of the new list</typeparam>
        /// <param name="mapper">The delegate defining the map.</param>
        /// <param name="itemEqualityComparer">The item equalityComparer to use for the new list</param>
        /// <returns>The new list.</returns>
#endif
        public virtual IList<V> Map<V>(Fun<T, V> mapper, SCG.IEqualityComparer<V> itemEqualityComparer)
        {
            validitycheck();

            ArrayList<V> res = new ArrayList<V>(Size, itemEqualityComparer);
            return map<V>(mapper, res);
        }

        private IList<V> map<V>(Fun<T, V> mapper, ArrayList<V> res)
        {
            int stamp = this.Stamp;
            if (Size > 0)
                for (int i = 0; i < Size; i++)
                {
                    V mappeditem = mapper(array[offset + i]);
                    modifycheck(stamp);
#if HASHINDEX
                    Multiplicity<V> p = new Multiplicity<V>(mappeditem, i);
                    if (res._itemIndex.FindOrAdd(ref p))
                        throw new ArgumentException("Mapped item already in indexed list");
#endif
                    res.array[i] = mappeditem;
                }
            res.Size = Size;
            return res;
        }

        /// <summary>
        /// Remove one item from the list: from the front if <code>FIFO</code>
        /// is true, else from the back.
        /// </summary>
        /// <exception cref="NoSuchItemException"> if this list is empty.</exception>
        /// <returns>The removed item.</returns>
        [Tested]
        public virtual T Remove()
        {
            updatecheck();
            if (Size == 0)
                throw new NoSuchItemException("List is empty");

            T item = removeAt(_isFifo ? 0 : Size - 1);
            (_innerList ?? this).raiseForRemove(item);
            return item;
        }

        /// <summary>
        /// Remove one item from the fromnt of the list.
        /// </summary>
        /// <exception cref="NoSuchItemException"> if this list is empty.</exception>
        /// <returns>The removed item.</returns>
        [Tested]
        public virtual T RemoveFirst()
        {
            updatecheck();
            if (Size == 0)
                throw new NoSuchItemException("List is empty");

            T item = removeAt(0);
            (_innerList ?? this).raiseForRemoveAt(offset, item);
            return item;
        }


        /// <summary>
        /// Remove one item from the back of the list.
        /// </summary>
        /// <exception cref="NoSuchItemException"> if this list is empty.</exception>
        /// <returns>The removed item.</returns>
        [Tested]
        public virtual T RemoveLast()
        {
            updatecheck();
            if (Size == 0)
                throw new NoSuchItemException("List is empty");

            T item = removeAt(Size - 1);
            (_innerList ?? this).raiseForRemoveAt(Size + offset, item);
            return item;
        }

        /// <summary>
        /// Create a list view on this list. 
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"> if the start or count is negative
        /// or the range does not fit within list.</exception>
        /// <param name="start">The index in this list of the start of the view.</param>
        /// <param name="count">The size of the view.</param>
        /// <returns>The new list view.</returns>
        [Tested]
        public virtual IList<T> View(int start, int count)
        {
            validitycheck();
            checkRange(start, count);
            if (_views == null)
                _views = new WeakViewList<ArrayList<T>>();
            ArrayList<T> retval = (ArrayList<T>) MemberwiseClone();


            retval._innerList = _innerList != null ? _innerList : this;
            retval.offset = start + offset;
            retval.Size = count;
            retval._myWeakReference = _views.Add(retval);
            return retval;
        }

        /// <summary>
        /// Create a list view on this list containing the (first) occurrence of a particular item.
        /// <para>Returns <code>null</code> if the item is not in this list.</para>
        /// </summary>
        /// <param name="item">The item to find.</param>
        /// <returns>The new list view.</returns>
        [Tested]
        public virtual IList<T> ViewOf(T item)
        {
            int index = indexOf(item);
            if (index < 0)
                return null;
            return View(index, 1);
        }


        /// <summary>
        /// Create a list view on this list containing the last occurrence of a particular item. 
        /// <para>Returns <code>null</code> if the item is not in this list.</para>
        /// </summary>
        /// <param name="item">The item to find.</param>
        /// <returns>The new list view.</returns>
        [Tested]
        public virtual IList<T> LastViewOf(T item)
        {
            int index = lastIndexOf(item);
            if (index < 0)
                return null;
            return View(index, 1);
        }

        /// <summary>
        /// Null if this list is not a view.
        /// </summary>
        /// <value>Underlying list for view.</value>
        [Tested]
        public virtual IList<T> Underlying { [Tested]get { return _innerList; } }


        /// <summary>
        /// </summary>
        /// <value>Offset for this list view or 0 for an underlying list.</value>
        [Tested]
        public virtual int Offset { [Tested]get { return offset; } }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public virtual bool IsValid { get { return _isValid; } }

        /// <summary>
        /// Slide this list view along the underlying list.
        /// </summary>
        /// <exception cref="NotAViewException"> if this list is not a view.</exception>
        /// <exception cref="ArgumentOutOfRangeException"> if the operation
        /// would bring either end of the view outside the underlying list.</exception>
        /// <param name="offset">The signed amount to slide: positive to slide
        /// towards the end.</param>
        [Tested]
        public virtual IList<T> Slide(int offset)
        {
            if (!TrySlide(offset, Size))
                throw new ArgumentOutOfRangeException();
            return this;
        }


        /// <summary>
        /// Slide this list view along the underlying list, changing its size.
        /// </summary>
        /// <exception cref="NotAViewException"> if this list is not a view.</exception>
        /// <exception cref="ArgumentOutOfRangeException"> if the operation
        /// would bring either end of the view outside the underlying list.</exception>
        /// <param name="offset">The signed amount to slide: positive to slide
        /// towards the end.</param>
        /// <param name="size">The new size of the view.</param>
        [Tested]
        public virtual IList<T> Slide(int offset, int size)
        {
            if (!TrySlide(offset, size))
                throw new ArgumentOutOfRangeException();
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotAViewException"> if this list is not a view.</exception>
        /// <param name="offset"></param>
        /// <returns></returns>
        [Tested]
        public virtual bool TrySlide(int offset)
        {
            return TrySlide(offset, Size);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotAViewException"> if this list is not a view.</exception>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [Tested]
        public virtual bool TrySlide(int offset, int size)
        {
            updatecheck();
            if (_innerList == null)
                throw new NotAViewException("Not a view");

            int newoffset = this.offset + offset;
            int newsize = size;

            if (newoffset < 0 || newsize < 0 || newoffset + newsize > InnerSize)
                return false;

            this.offset = newoffset;
            this.Size = newsize;
            return true;
        }

        /// <summary>
        /// 
        /// <para>Returns null if <code>otherView</code> is strictly to the left of this view</para>
        /// </summary>
        /// <param name="otherView"></param>
        /// <exception cref="IncompatibleViewException">If otherView does not have the same underlying list as this</exception>
        /// <returns></returns>
        public virtual IList<T> Span(IList<T> otherView)
        {
            if ((otherView == null) || ((otherView.Underlying ?? otherView) != (_innerList ?? this)))
                throw new IncompatibleViewException();
            if (otherView.Offset + otherView.Count - Offset < 0)
                return null;
            return (_innerList ?? this).View(Offset, otherView.Offset + otherView.Count - Offset);
        }

        /// <summary>
        /// Reverst the list so the items are in the opposite sequence order.
        /// </summary>
        [Tested]
        public virtual void Reverse()
        {
            updatecheck();
            if (Size == 0)
                return;
            for (int i = 0, length = Size / 2, end = offset + Size - 1; i < length; i++)
            {
                T swap = array[offset + i];

                array[offset + i] = array[end - i];
                array[end - i] = swap;
            }
#if HASHINDEX
            reindex(offset, offset + Size);
#endif
            //TODO: be more forgiving wrt. disposing
            disposeOverlappingViews(true);
            (_innerList ?? this).raiseCollectionChanged();
        }

        /// <summary>
        /// Check if this list is sorted according to the default sorting order
        /// for the item type T, as defined by the <see cref="T:C5.Comparer`1"/> class 
        /// </summary>
        /// <exception cref="NotComparableException">if T is not comparable</exception>
        /// <returns>True if the list is sorted, else false.</returns>
        [Tested]
        public bool IsSorted() { return IsSorted(Comparer<T>.Default); }

        /// <summary>
        /// Check if this list is sorted according to a specific sorting order.
        /// </summary>
        /// <param name="c">The comparer defining the sorting order.</param>
        /// <returns>True if the list is sorted, else false.</returns>
        [Tested]
        public virtual bool IsSorted(SCG.IComparer<T> c)
        {
            validitycheck();
            for (int i = offset + 1, end = offset + Size; i < end; i++)
                if (c.Compare(array[i - 1], array[i]) > 0)
                    return false;

            return true;
        }

        /// <summary>
        /// Sort the items of the list according to the default sorting order
        /// for the item type T, as defined by the Comparer[T] class 
        /// (<see cref="T:C5.Comparer`1"/>).
        /// </summary>
        /// <exception cref="InvalidOperationException">if T is not comparable</exception>
        public virtual void Sort()
        {
            Sort(Comparer<T>.Default);
        }


        /// <summary>
        /// Sort the items of the list according to a specific sorting order.
        /// </summary>
        /// <param name="comparer">The comparer defining the sorting order.</param>
        [Tested]
        public virtual void Sort(SCG.IComparer<T> comparer)
        {
            updatecheck();
            if (Size == 0)
                return;
            Sorting.IntroSort<T>(array, offset, Size, comparer);
            disposeOverlappingViews(false);
#if HASHINDEX
            reindex(offset, offset + Size);
#endif
            (_innerList ?? this).raiseCollectionChanged();
        }


        /// <summary>
        /// Randomly shuffle the items of this list. 
        /// </summary>
        public virtual void Shuffle() { Shuffle(new C5Random()); }


        /// <summary>
        /// Shuffle the items of this list according to a specific random source.
        /// </summary>
        /// <param name="rnd">The random source.</param>
        public virtual void Shuffle(Random rnd)
        {
            updatecheck();
            if (Size == 0)
                return;
            for (int i = offset, top = offset + Size, end = top - 1; i < end; i++)
            {
                int j = rnd.Next(i, top);
                if (j != i)
                {
                    T tmp = array[i];
                    array[i] = array[j];
                    array[j] = tmp;
                }
            }
            disposeOverlappingViews(false);
#if HASHINDEX
            reindex(offset, offset + Size);
#endif
            (_innerList ?? this).raiseCollectionChanged();
        }
        #endregion

        #region IIndexed<T> Members

        /// <summary>
        /// Search for an item in the list going forwrds from the start.
        /// </summary>
        /// <param name="item">Item to search for.</param>
        /// <returns>Index of item from start.</returns>
        [Tested]
        public virtual int IndexOf(T item) { validitycheck(); return indexOf(item); }


        /// <summary>
        /// Search for an item in the list going backwords from the end.
        /// </summary>
        /// <param name="item">Item to search for.</param>
        /// <returns>Index of item from the end.</returns>
        [Tested]
        public virtual int LastIndexOf(T item) { validitycheck(); return lastIndexOf(item); }


        /// <summary>
        /// Remove the item at a specific position of the list.
        /// </summary>
        /// <exception cref="IndexOutOfRangeException"> if index is negative or
        /// &gt;= the size of the collection.</exception>
        /// <param name="index">The index of the item to remove.</param>
        /// <returns>The removed item.</returns>
        [Tested]
        public virtual T RemoveAt(int index)
        {
            updatecheck();
            if (index < 0 || index >= Size)
                throw new IndexOutOfRangeException("Index out of range for sequenced collection");

            T item = removeAt(index);
            (_innerList ?? this).raiseForRemoveAt(offset + index, item);
            return item;
        }


        /// <summary>
        /// Remove all items in an index interval.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">If <code>start</code>
        /// and <code>count</code> does not describe a valid interval in the list</exception> 
        /// <param name="start">The index of the first item to remove.</param>
        /// <param name="count">The number of items to remove.</param>
        [Tested]
        public virtual void RemoveInterval(int start, int count)
        {
            updatecheck();
            if (count == 0)
                return;
            checkRange(start, count);
            start += offset;
            fixViewsBeforeRemove(start, count);
#if HASHINDEX
            Multiplicity<T> p = new Multiplicity<T>();
            for (int i = start, end = start + count; i < end; i++)
            {
                p.Value = array[i];
                _itemIndex.Remove(p);
            }
#endif
            Array.Copy(array, start + count, array, start, InnerSize - start - count);
            addtosize(-count);
            Array.Clear(array, InnerSize, count);
#if HASHINDEX
            reindex(start);
#endif
            (_innerList ?? this).raiseForRemoveInterval(start, count);
        }
        void raiseForRemoveInterval(int start, int count)
        {
            if (ActiveEvents != 0)
            {
                raiseCollectionCleared(Size == 0, count, start);
                raiseCollectionChanged();
            }
        }
        #endregion

        #region ICollection<T> Members

        /// <summary>
        /// The value is symbolic indicating the type of asymptotic complexity
        /// in terms of the size of this collection (worst-case or amortized as
        /// relevant).
        /// </summary>
        /// <value>Speed.Linear</value>
        [Tested]
        public virtual Speed ContainsSpeed
        {
            [Tested]
            get
            {
#if HASHINDEX
                return Speed.Constant;
#else
                return Speed.Linear;
#endif
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Tested]
        public override int GetUnsequencedHashCode()
        { validitycheck(); return base.GetUnsequencedHashCode(); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="that"></param>
        /// <returns></returns>
        [Tested]
        public override bool UnsequencedEquals(ICollection<T> that)
        { validitycheck(); return base.UnsequencedEquals(that); }

        /// <summary>
        /// Check if this collection contains (an item equivalent to according to the
        /// itemequalityComparer) a particular value.
        /// </summary>
        /// <param name="item">The value to check for.</param>
        /// <returns>True if the items is in this collection.</returns>
        [Tested]
        public virtual bool Contains(T item)
        { validitycheck(); return indexOf(item) >= 0; }


        /// <summary>
        /// Check if this collection contains an item equivalent according to the
        /// itemequalityComparer to a particular value. If so, return in the ref argument (a
        /// binary copy of) the actual value found.
        /// </summary>
        /// <param name="item">The value to look for.</param>
        /// <returns>True if the items is in this collection.</returns>
        [Tested]
        public virtual bool Find(ref T item)
        {
            validitycheck();

            int i;

            if ((i = indexOf(item)) >= 0)
            {
                item = array[offset + i];
                return true;
            }

            return false;
        }


        /// <summary>
        /// Check if this collection contains an item equivalent according to the
        /// itemequalityComparer to a particular value. If so, update the item in the collection 
        /// to with a binary copy of the supplied value. This will only update the first 
        /// mathching item.
        /// </summary>
        /// <param name="item">Value to update.</param>
        /// <returns>True if the item was found and hence updated.</returns>
        [Tested]
        public virtual bool Update(T item)
        {
            T olditem;
            return Update(item, out olditem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="olditem"></param>
        /// <returns></returns>
        public virtual bool Update(T item, out T olditem)
        {
            updatecheck();
            int i;

            if ((i = indexOf(item)) >= 0)
            {
                olditem = array[offset + i];
                array[offset + i] = item;
#if HASHINDEX
                _itemIndex.Update(new Multiplicity<T>(item, offset + i));
#endif
                (_innerList ?? this).raiseForUpdate(item, olditem);
                return true;
            }

            olditem = default(T);
            return false;
        }

        /// <summary>
        /// Check if this collection contains an item equivalent according to the
        /// itemequalityComparer to a particular value. If so, return in the ref argument (a
        /// binary copy of) the actual value found. Else, add the item to the collection.
        /// </summary>
        /// <param name="item">The value to look for.</param>
        /// <returns>True if the item was found (hence not added).</returns>
        [Tested]
        public virtual bool FindOrAdd(ref T item)
        {
            updatecheck();
            if (Find(ref item))
                return true;

            Add(item);
            return false;
        }


        /// <summary>
        /// Check if this collection contains an item equivalent according to the
        /// itemequalityComparer to a particular value. If so, update the item in the collection 
        /// to with a binary copy of the supplied value. This will only update the first 
        /// mathching item.
        /// </summary>
        /// <param name="item">Value to update.</param>
        /// <returns>True if the item was found and hence updated.</returns>
        [Tested]
        public virtual bool UpdateOrAdd(T item)
        {
            updatecheck();
            if (Update(item))
                return true;

            Add(item);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="olditem"></param>
        /// <returns></returns>
        public virtual bool UpdateOrAdd(T item, out T olditem)
        {
            updatecheck();
            if (Update(item, out olditem))
                return true;

            Add(item);
            olditem = default(T);
            return false;
        }

        /// <summary>
        /// Remove a particular item from this list. The item will be searched 
        /// for from the end of the list if <code>FIFO == false</code> (the default), 
        /// else from the start.
        /// </summary>
        /// <param name="item">The value to remove.</param>
        /// <returns>True if the item was found (and removed).</returns>
        [Tested]
        public virtual bool Remove(T item)
        {
            updatecheck();

            int i = _isFifo ? indexOf(item) : lastIndexOf(item);

            if (i < 0)
                return false;

            T removeditem = removeAt(i);
            (_innerList ?? this).raiseForRemove(removeditem);
            return true;
        }


        /// <summary>
        /// Remove the first copy of a particular item from this collection if found.
        /// If an item was removed, report a binary copy of the actual item removed in 
        /// the argument. The item will be searched 
        /// for from the end of the list if <code>FIFO == false</code> (the default), 
        /// else from the start.
        /// </summary>
        /// <param name="item">The value to remove.</param>
        /// <param name="removeditem">The removed value.</param>
        /// <returns>True if the item was found (and removed).</returns>
        [Tested]
        public virtual bool Remove(T item, out T removeditem)
        {
            updatecheck();

            int i = _isFifo ? indexOf(item) : lastIndexOf(item);

            if (i < 0)
            {
                removeditem = default(T);
                return false;
            }

            removeditem = removeAt(i);
            (_innerList ?? this).raiseForRemove(removeditem);
            return true;
        }


        //TODO: remove from end or according to FIFO?
        /// <summary>
        /// Remove all items in another collection from this one, taking multiplicities into account.
        /// Matching items will be removed from the front. Current implementation is not optimal.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="items">The items to remove.</param>
        [Tested]
        public virtual void RemoveAll<U>(SCG.IEnumerable<U> items) where U : T
        {
            updatecheck();
            if (Size == 0)
                return;
            //TODO: reactivate the old code for small sizes
            HashBag<T> toremove = new HashBag<T>(ItemEqualityComparer);
            toremove.AddAll(items);
            if (toremove.Count == 0)
                return;
            RaiseForRemoveAllHandler raiseHandler = new RaiseForRemoveAllHandler(_innerList ?? this);
            bool mustFire = raiseHandler.MustFire;
            ViewHandler viewHandler = new ViewHandler(this);
            int j = offset;
            int removed = 0;
            int i = offset, end = offset + Size;
#if HASHINDEX
            Multiplicity<T> p = new Multiplicity<T>();
#endif
            while (i < end)
            {
                T item;
                //pass by a stretch of nodes
                while (i < end && !toremove.Contains(item = array[i]))
                {
#if HASHINDEX
                    if (j < i)
                    {
                        p.Value = item;
                        p.Count = j;
                        _itemIndex.Update(p);
                    }
#endif
                    array[j] = item;
                    i++; j++;
                }
                viewHandler.skipEndpoints(removed, i);
                //Remove a stretch of nodes
                while (i < end && toremove.Remove(item = array[i]))
                {
#if HASHINDEX
                    p.Value = item;
                    _itemIndex.Remove(p);
#endif
                    if (mustFire)
                        raiseHandler.Remove(item);
                    removed++;
                    i++;
                    viewHandler.updateViewSizesAndCounts(removed, i);
                }
            }
            if (removed == 0)
                return;
            viewHandler.updateViewSizesAndCounts(removed, InnerSize);
            Array.Copy(array, offset + Size, array, j, InnerSize - offset - Size);
            addtosize(-removed);
            Array.Clear(array, InnerSize, removed);
#if HASHINDEX
            reindex(j);
#endif
            if (mustFire)
                raiseHandler.Raise();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        void RemoveAll(Fun<T, bool> predicate)
        {
            updatecheck();
            if (Size == 0)
                return;
            RaiseForRemoveAllHandler raiseHandler = new RaiseForRemoveAllHandler(_innerList ?? this);
            bool mustFire = raiseHandler.MustFire;
            ViewHandler viewHandler = new ViewHandler(this);
            int j = offset;
            int removed = 0;
            int i = offset, end = offset + Size;
#if HASHINDEX
            Multiplicity<T> p = new Multiplicity<T>();
#endif
            while (i < end)
            {
                T item;
                //pass by a stretch of nodes
                while (i < end && !predicate(item = array[i]))
                {
                    updatecheck();
#if HASHINDEX
                    if (j < i)
                    {
                        p.Value = item;
                        p.Count = j;
                        _itemIndex.Update(p);
                    }
#endif
                    //if (j<i)
                    array[j] = item;
                    i++; j++;
                }
                updatecheck();
                viewHandler.skipEndpoints(removed, i);
                //Remove a stretch of nodes
                while (i < end && predicate(item = array[i]))
                {
                    updatecheck();
#if HASHINDEX
                    p.Value = item;
                    _itemIndex.Remove(p);
#endif
                    if (mustFire)
                        raiseHandler.Remove(item);
                    removed++;
                    i++;
                    viewHandler.updateViewSizesAndCounts(removed, i);
                }
                updatecheck();
            }
            if (removed == 0)
                return;
            viewHandler.updateViewSizesAndCounts(removed, InnerSize);
            Array.Copy(array, offset + Size, array, j, InnerSize - offset - Size);
            addtosize(-removed);
            Array.Clear(array, InnerSize, removed);
#if HASHINDEX
            reindex(j);
#endif
            if (mustFire)
                raiseHandler.Raise();
        }

        /// <summary>
        /// Remove all items from this collection, resetting internal array size.
        /// </summary>
        [Tested]
        public override void Clear()
        {
            if (_innerList == null)
            {
                updatecheck();
                if (Size == 0)
                    return;
                int oldsize = Size;
                fixViewsBeforeRemove(0, Size);
#if HASHINDEX
                _itemIndex.Clear();
#endif
                array = new T[8];
                Size = 0;
                (_innerList ?? this).raiseForRemoveInterval(offset, oldsize);
            }
            else
                RemoveInterval(0, Size);
        }

        /// <summary>
        /// Remove all items not in some other collection from this one, taking multiplicities into account.
        /// Items are retained front first.  
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="items">The items to retain.</param>
        [Tested]
        public virtual void RetainAll<U>(SCG.IEnumerable<U> items) where U : T
        {
            updatecheck();
            if (Size == 0)
                return;
            HashBag<T> toretain = new HashBag<T>(ItemEqualityComparer);
            toretain.AddAll(items);
            if (toretain.Count == 0)
            {
                Clear();
                return;
            }
            RaiseForRemoveAllHandler raiseHandler = new RaiseForRemoveAllHandler(_innerList ?? this);
            bool mustFire = raiseHandler.MustFire;
            ViewHandler viewHandler = new ViewHandler(this);
            int j = offset;
            int removed = 0;
            int i = offset, end = offset + Size;
#if HASHINDEX
            Multiplicity<T> p = new Multiplicity<T>();
#endif
            while (i < end)
            {
                T item;
                //pass by a stretch of nodes
                while (i < end && toretain.Remove(item = array[i]))
                {
#if HASHINDEX
                    if (j < i)
                    {
                        p.Value = item;
                        p.Count = j;
                        _itemIndex.Update(p);
                    }
#endif
                    //if (j<i)
                    array[j] = item;
                    i++; j++;
                }
                viewHandler.skipEndpoints(removed, i);
                //Remove a stretch of nodes
                while (i < end && !toretain.Contains(item = array[i]))
                {
#if HASHINDEX
                    p.Value = item;
                    _itemIndex.Remove(p);
#endif
                    if (mustFire)
                        raiseHandler.Remove(item);
                    removed++;
                    i++;
                    viewHandler.updateViewSizesAndCounts(removed, i);
                }
            }
            if (removed == 0)
                return;
            viewHandler.updateViewSizesAndCounts(removed, InnerSize);
            Array.Copy(array, offset + Size, array, j, InnerSize - offset - Size);
            addtosize(-removed);
            Array.Clear(array, InnerSize, removed);
#if HASHINDEX
            reindex(j);
#endif
            raiseHandler.Raise();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        void RetainAll(Fun<T, bool> predicate)
        {
            updatecheck();
            if (Size == 0)
                return;
            RaiseForRemoveAllHandler raiseHandler = new RaiseForRemoveAllHandler(_innerList ?? this);
            bool mustFire = raiseHandler.MustFire;
            ViewHandler viewHandler = new ViewHandler(this);
            int j = offset;
            int removed = 0;
            int i = offset, end = offset + Size;
#if HASHINDEX
            Multiplicity<T> p = new Multiplicity<T>();
#endif
            while (i < end)
            {
                T item;
                //pass by a stretch of nodes
                while (i < end && predicate(item = array[i]))
                {
                    updatecheck();
#if HASHINDEX
                    if (j < i)
                    {
                        p.Value = item;
                        p.Count = j;
                        _itemIndex.Update(p);
                    }
#endif
                    //if (j<i)
                    array[j] = item;
                    i++; j++;
                }
                updatecheck();
                viewHandler.skipEndpoints(removed, i);
                //Remove a stretch of nodes
                while (i < end && !predicate(item = array[i]))
                {
                    updatecheck();
#if HASHINDEX
                    p.Value = item;
                    _itemIndex.Remove(p);
#endif
                    if (mustFire)
                        raiseHandler.Remove(item);
                    removed++;
                    i++;
                    viewHandler.updateViewSizesAndCounts(removed, i);
                }
                updatecheck();
            }
            if (removed == 0)
                return;
            viewHandler.updateViewSizesAndCounts(removed, InnerSize);
            Array.Copy(array, offset + Size, array, j, InnerSize - offset - Size);
            addtosize(-removed);
            Array.Clear(array, InnerSize, removed);
#if HASHINDEX
            reindex(j);
#endif
            raiseHandler.Raise();
        }


        /// <summary>
        /// Check if this collection contains all the values in another collection,
        /// taking multiplicities into account.
        /// Current implementation is not optimal.
        /// </summary>
        /// <param name="items">The </param>
        /// <typeparam name="U"></typeparam>
        /// <returns>True if all values in <code>items</code>is in this collection.</returns>
        [Tested]
        public virtual bool ContainsAll<U>(SCG.IEnumerable<U> items) where U : T
        {
            validitycheck();
#if HASHINDEX
            foreach (T item in items)
                if (indexOf(item) < 0)
                    return false;

            return true;
#else
            //TODO: use aux hash bag to obtain linear time procedure
            HashBag<T> tomatch = new HashBag<T>(ItemEqualityComparer);
            tomatch.AddAll(items);
            if (tomatch.Count == 0)
                return true;
            for (int i = offset, end = offset + Size; i < end; i++)
            {
                tomatch.Remove(array[i]);
                if (tomatch.Count == 0)
                    return true;
            }
            return false;
#endif
        }


        /// <summary>
        /// Count the number of items of the collection equal to a particular value.
        /// Returns 0 if and only if the value is not in the collection.
        /// </summary>
        /// <param name="item">The value to count.</param>
        /// <returns>The number of copies found.</returns>
        [Tested]
        public virtual int ContainsCount(T item)
        {
            validitycheck();
#if HASHINDEX
            return indexOf(item) >= 0 ? 1 : 0;
#else
            int count = 0;
            for (int i = 0; i < Size; i++)
                if (Equals(item, array[offset + i]))
                    count++;
            return count;
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ICollectionValue<T> UniqueItems()
        {
#if HASHINDEX
            return this;
#else
            HashBag<T> hashbag = new HashBag<T>(ItemEqualityComparer);
            hashbag.AddAll(this);
            return hashbag.UniqueItems();
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual ICollectionValue<Multiplicity<T>> ItemMultiplicities()
        {
#if HASHINDEX
            return new MultiplicityOne<T>(this);
#else
            HashBag<T> hashbag = new HashBag<T>(ItemEqualityComparer);
            hashbag.AddAll(this);
            return hashbag.ItemMultiplicities();
#endif
        }





        /// <summary>
        /// Remove all items equal to a given one.
        /// </summary>
        /// <param name="item">The value to remove.</param>
        [Tested]
        public virtual void RemoveAllCopies(T item)
        {
#if HASHINDEX
            Remove(item);
#else
            updatecheck();
            if (Size == 0)
                return;
            RaiseForRemoveAllHandler raiseHandler = new RaiseForRemoveAllHandler(_innerList ?? this);
            bool mustFire = raiseHandler.MustFire;
            ViewHandler viewHandler = new ViewHandler(this);
            int j = offset;
            int removed = 0;
            int i = offset, end = offset + Size;
            while (i < end)
            {
                //pass by a stretch of nodes
                while (i < end && !Equals(item, array[i]))
                    array[j++] = array[i++];
                viewHandler.skipEndpoints(removed, i);
                //Remove a stretch of nodes
                while (i < end && Equals(item, array[i]))
                {
                    if (mustFire)
                        raiseHandler.Remove(array[i]);
                    removed++;
                    i++;
                    viewHandler.updateViewSizesAndCounts(removed, i);
                }
            }
            if (removed == 0)
                return;
            viewHandler.updateViewSizesAndCounts(removed, InnerSize);
            Array.Copy(array, offset + Size, array, j, InnerSize - offset - Size);
            addtosize(-removed);
            Array.Clear(array, InnerSize, removed);
            raiseHandler.Raise();
#endif
        }


        //TODO: check views
        /// <summary>
        /// Check the integrity of the internal data structures of this array list.
        /// </summary>
        /// <returns>True if check does not fail.</returns>
        [Tested]
        public override bool Check()
        {
            bool retval = true;

            if (InnerSize > array.Length)
            {
                Console.WriteLine("InnerSize ({0}) > array.Length ({1})", Size, array.Length);
                return false;
            }

            if (offset + Size > InnerSize)
            {
                Console.WriteLine("offset({0})+size({1}) > InnerSize ({2})", offset, Size, InnerSize);
                return false;
            }

            if (offset < 0)
            {
                Console.WriteLine("offset({0}) < 0", offset);
                return false;
            }

            for (int i = 0; i < InnerSize; i++)
            {
                if ((object) (array[i]) == null)
                {
                    Console.WriteLine("Bad element: null at (base)index {0}", i);
                    retval = false;
                }
            }

            for (int i = InnerSize, length = array.Length; i < length; i++)
            {
                if (!Equals(array[i], default(T)))
                {
                    Console.WriteLine("Bad element: != default(T) at (base)index {0}", i);
                    retval = false;
                }
            }

#if HASHINDEX
            if (InnerSize != _itemIndex.Count)
            {
                Console.WriteLine("size ({0})!= index.Count ({1})", Size, _itemIndex.Count);
                retval = false;
            }

            for (int i = 0; i < InnerSize; i++)
            {
                Multiplicity<T> p = new Multiplicity<T>(array[i]);

                if (!_itemIndex.Find(ref p))
                {
                    Console.WriteLine("Item {1} at {0} not in hashindex", i, array[i]);
                    retval = false;
                }

                if (p.Count != i)
                {
                    Console.WriteLine("Item {1} at {0} has hashindex {2}", i, array[i], p.Value);
                    retval = false;
                }
            }
#endif
            return retval;
        }

        #endregion

        #region IExtensible<T> Members

        /// <summary>
        /// 
        /// </summary>
        /// <value>True, indicating array list has bag semantics.</value>
        [Tested]
        public virtual bool AllowsDuplicates
        {
            [Tested]
            get
            {
#if HASHINDEX
                return false;
#else
                return true;
#endif
            }
        }

        /// <summary>
        /// By convention this is true for any collection with set semantics.
        /// </summary>
        /// <value>True if only one representative of a group of equal items 
        /// is kept in the collection together with the total count.</value>
        public virtual bool DuplicatesByCounting
        {
            get
            {
#if HASHINDEX
                return true;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// Add an item to end of this list.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>True</returns>
        [Tested]
        public virtual bool Add(T item)
        {
            updatecheck();
#if HASHINDEX
            Multiplicity<T> p = new Multiplicity<T>(item, Size + offset);
            if (_itemIndex.FindOrAdd(ref p))
                return false;
#endif
            baseinsert(Size, item);
#if HASHINDEX
            reindex(Size + offset);
#endif
            (_innerList ?? this).raiseForAdd(item);
            return true;
        }


        /// <summary>
        /// Add the elements from another collection to this collection.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="items"></param>
        [Tested]
        public virtual void AddAll<U>(SCG.IEnumerable<U> items) where U : T
        {
            updatecheck();
            int toadd = EnumerableBase<U>.countItems(items);
            if (toadd == 0)
                return;

            if (toadd + InnerSize > array.Length)
                expand(toadd + InnerSize, InnerSize);

            int i = Size + offset;
            if (InnerSize > i)
                Array.Copy(array, i, array, i + toadd, InnerSize - i);
            try
            {
                foreach (T item in items)
                {
#if HASHINDEX
                    Multiplicity<T> p = new Multiplicity<T>(item, i);
                    if (_itemIndex.FindOrAdd(ref p))
                        continue;
#endif
                    array[i++] = item;
                }
            }
            finally
            {
                int added = i - Size - offset;
                if (added < toadd)
                {
                    Array.Copy(array, Size + offset + toadd, array, i, InnerSize - Size - offset);
                    Array.Clear(array, InnerSize + added, toadd - added);
                }
                if (added > 0)
                {
                    addtosize(added);
#if HASHINDEX
                    reindex(i);
#endif
                    fixViewsAfterInsert(added, i - added);
                    (_innerList ?? this).raiseForAddAll(i - added, added);
                }
            }
        }
        private void raiseForAddAll(int start, int added)
        {
            if ((ActiveEvents & EventTypeEnum.Added) != 0)
                for (int i = start, end = start + added; i < end; i++)
                    raiseItemsAdded(array[i], 1);
            raiseCollectionChanged();
        }

        #endregion

        #region IDirectedEnumerable<T> Members

        /// <summary>
        /// Create a collection containing the same items as this collection, but
        /// whose enumerator will enumerate the items backwards. The new collection
        /// will become invalid if the original is modified. Method typicaly used as in
        /// <code>foreach (T x in coll.Backwards()) {...}</code>
        /// </summary>
        /// <returns>The backwards collection.</returns>
        [Tested]
        IDirectedEnumerable<T> IDirectedEnumerable<T>.Backwards() { return Backwards(); }

        #endregion

        #region ICollectionValue<T> Members
        /// <summary>
        /// 
        /// </summary>
        /// <value>The number of items in this collection</value>
        [Tested]
        public override int Count { [Tested]get { validitycheck(); return Size; } }
        #endregion

        #region IEnumerable<T> Members
        //TODO: make tests of all calls on a disposed view throws the right exception! (Which should be C5.InvalidListViewException)
        /// <summary>
        /// Create an enumerator for the collection
        /// </summary>
        /// <returns>The enumerator</returns>
        [Tested]
        public override SCG.IEnumerator<T> GetEnumerator()
        {
            validitycheck();
            return base.GetEnumerator();
        }
        #endregion

#if HASHINDEX
#else
        #region IStack<T> Members

        /// <summary>
        /// Push an item to the top of the stack.
        /// </summary>
        /// <param name="item">The item</param>
        [Tested]
        public virtual void Push(T item)
        {
            InsertLast(item);
        }

        /// <summary>
        /// Pop the item at the top of the stack from the stack.
        /// </summary>
        /// <returns>The popped item.</returns>
        [Tested]
        public virtual T Pop()
        {
            return RemoveLast();
        }

        #endregion

        #region IQueue<T> Members

        /// <summary>
        /// Enqueue an item at the back of the queue. 
        /// </summary>
        /// <param name="item">The item</param>
        [Tested]
        public virtual void Enqueue(T item)
        {
            InsertLast(item);
        }

        /// <summary>
        /// Dequeue an item from the front of the queue.
        /// </summary>
        /// <returns>The item</returns>
        [Tested]
        public virtual T Dequeue()
        {
            return RemoveFirst();
        }

        #endregion
#endif
        #region IDisposable Members

        /// <summary>
        /// Invalidate this list. If a view, just invalidate the view. 
        /// If not a view, invalidate the list and all views on it.
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(false);
        }

        void Dispose(bool disposingUnderlying)
        {
            if (_isValid)
            {
                if (_innerList != null)
                {
                    _isValid = false;
                    if (!disposingUnderlying && _views != null)
                        _views.Remove(_myWeakReference);
                    _innerList = null;
                    _views = null;
                    _myWeakReference = null;
                }
                else
                {
                    //isValid = false;
                    if (_views != null)
                        foreach (ArrayList<T> view in _views)
                            view.Dispose(true);
                    Clear();
                }
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Make a shallow copy of this ArrayList.
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            ArrayList<T> clone = new ArrayList<T>(Size, ItemEqualityComparer);
            clone.AddAll(this);
            return clone;
        }

        #endregion

        #region System.Collections.Generic.IList<T> Members

        void System.Collections.Generic.IList<T>.RemoveAt(int index)
        {
            RemoveAt(index);
        }

        void System.Collections.Generic.ICollection<T>.Add(T item)
        {
            Add(item);
        }

        #endregion
    }
}
