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
    /// A bag collection based on a hash table of (item,count) pairs. 
    /// </summary>
    [Serializable]
    public class HashBag<T> : CollectionBase<T>, ICollection<T>
    {
        #region Fields
        HashSet<Multiplicity<T>> dict;
        #endregion

        #region Events

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public override EventTypeEnum ListenableEvents { get { return EventTypeEnum.Basic; } }

        #endregion

        #region Constructors
        /// <summary>
        /// Create a hash bag with the deafult item equalityComparer.
        /// </summary>
        public HashBag() : this(EqualityComparer<T>.Default) { }

        /// <summary>
        /// Create a hash bag with an external item equalityComparer.
        /// </summary>
        /// <param name="itemequalityComparer">The external item equalityComparer.</param>
        public HashBag(SCG.IEqualityComparer<T> itemequalityComparer)
            : base(itemequalityComparer)
        {
            dict = new HashSet<Multiplicity<T>>(new MultiplicityEqualityComparer<T>(itemequalityComparer));
        }

        /// <summary>
        /// Create a hash bag with external item equalityComparer, prescribed initial table size and default fill threshold (66%)
        /// </summary>
        /// <param name="capacity">Initial table size (rounded to power of 2, at least 16)</param>
        /// <param name="itemequalityComparer">The external item equalityComparer</param>
        public HashBag(int capacity, SCG.IEqualityComparer<T> itemequalityComparer)
            : base(itemequalityComparer)
        {
            dict = new HashSet<Multiplicity<T>>(capacity, new MultiplicityEqualityComparer<T>(itemequalityComparer));
        }


        /// <summary>
        /// Create a hash bag with external item equalityComparer, prescribed initial table size and fill threshold.
        /// </summary>
        /// <param name="capacity">Initial table size (rounded to power of 2, at least 16)</param>
        /// <param name="fill">Fill threshold (valid range 10% to 90%)</param>
        /// <param name="itemequalityComparer">The external item equalityComparer</param>
        public HashBag(int capacity, double fill, SCG.IEqualityComparer<T> itemequalityComparer)
            : base(itemequalityComparer)
        {
            dict = new HashSet<Multiplicity<T>>(capacity, fill, new MultiplicityEqualityComparer<T>(itemequalityComparer));
        }

        #endregion

        #region IEditableCollection<T> Members

        /// <summary>
        /// The complexity of the Contains operation
        /// </summary>
        /// <value>Always returns Speed.Constant</value>
        [Tested]
        public virtual Speed ContainsSpeed { [Tested]get { return Speed.Constant; } }

        /// <summary>
        /// Check if an item is in the bag 
        /// </summary>
        /// <param name="item">The item to look for</param>
        /// <returns>True if bag contains item</returns>
        [Tested]
        public virtual bool Contains(T item)
        { return dict.Contains(new Multiplicity<T>(item, 0)); }


        /// <summary>
        /// Check if an item (collection equal to a given one) is in the bag and
        /// if so report the actual item object found.
        /// </summary>
        /// <param name="item">On entry, the item to look for.
        /// On exit the item found, if any</param>
        /// <returns>True if bag contains item</returns>
        [Tested]
        public virtual bool Find(ref T item)
        {
            Multiplicity<T> p = new Multiplicity<T>(item, 0);

            if (dict.Find(ref p))
            {
                item = p.Value;
                return true;
            }

            return false;
        }


        /// <summary>
        /// Check if an item (collection equal to a given one) is in the bag and
        /// if so replace the item object in the bag with the supplied one.
        /// </summary>
        /// <param name="item">The item object to update with</param>
        /// <returns>True if item was found (and updated)</returns>
        [Tested]
        public virtual bool Update(T item)
        { T olditem = default(T); return Update(item, out olditem); }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="olditem"></param>
        /// <returns></returns>
        public virtual bool Update(T item, out T olditem)
        {
            Multiplicity<T> p = new Multiplicity<T>(item, 0);

            updatecheck();

            //Note: we cannot just do dict.Update: we have to lookup the count before we 
            //know what to update with. There is of course a way around if we use the 
            //implementation of hashset -which we do not want to do.
            //The hashbag is moreover mainly a proof of concept
            if (dict.Find(ref p))
            {
                olditem = p.Value;
                p.Value = item;
                dict.Update(p);
                if (ActiveEvents != 0)
                    raiseForUpdate(item, olditem, p.Count);
                return true;
            }

            olditem = default(T);
            return false;
        }


        /// <summary>
        /// Check if an item (collection equal to a given one) is in the bag.
        /// If found, report the actual item object in the bag,
        /// else add the supplied one.
        /// </summary>
        /// <param name="item">On entry, the item to look for or add.
        /// On exit the actual object found, if any.</param>
        /// <returns>True if item was found</returns>
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
        /// Check if an item (collection equal to a supplied one) is in the bag and
        /// if so replace the item object in the set with the supplied one; else
        /// add the supplied one.
        /// </summary>
        /// <param name="item">The item to look for and update or add</param>
        /// <returns>True if item was updated</returns>
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
            return false;
        }

        /// <summary>
        /// Remove one copy af an item from the bag
        /// </summary>
        /// <param name="item">The item to remove</param>
        /// <returns>True if item was (found and) removed </returns>
        [Tested]
        public virtual bool Remove(T item)
        {
            Multiplicity<T> p = new Multiplicity<T>(item, 0);

            updatecheck();
            if (dict.Find(ref p))
            {
                Size--;
                if (p.Count == 1)
                    dict.Remove(p);
                else
                {
                    p.Count--;
                    dict.Update(p);
                }
                if (ActiveEvents != 0)
                    raiseForRemove(p.Value);
                return true;
            }

            return false;
        }


        /// <summary>
        /// Remove one copy of an item from the bag, reporting the actual matching item object.
        /// </summary>
        /// <param name="item">The value to remove.</param>
        /// <param name="removeditem">The removed value.</param>
        /// <returns>True if item was found.</returns>
        [Tested]
        public virtual bool Remove(T item, out T removeditem)
        {
            updatecheck();
            Multiplicity<T> p = new Multiplicity<T>(item, 0);
            if (dict.Find(ref p))
            {
                removeditem = p.Value;
                Size--;
                if (p.Count == 1)
                    dict.Remove(p);
                else
                {
                    p.Count--;
                    dict.Update(p);
                }
                if (ActiveEvents != 0)
                    raiseForRemove(removeditem);

                return true;
            }

            removeditem = default(T);
            return false;
        }

        /// <summary>
        /// Remove all items in a supplied collection from this bag, counting multiplicities.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="items">The items to remove.</param>
        [Tested]
        public virtual void RemoveAll<U>(SCG.IEnumerable<U> items) where U : T
        {
#warning Improve if items is a counting bag
            updatecheck();
            bool mustRaise = (ActiveEvents & (EventTypeEnum.Changed | EventTypeEnum.Removed)) != 0;
            RaiseForRemoveAllHandler raiseHandler = mustRaise ? new RaiseForRemoveAllHandler(this) : null;
            foreach (U item in items)
            {
                Multiplicity<T> p = new Multiplicity<T>(item, 0);
                if (dict.Find(ref p))
                {
                    Size--;
                    if (p.Count == 1)
                        dict.Remove(p);
                    else
                    {
                        p.Count--;
                        dict.Update(p);
                    }
                    if (mustRaise)
                        raiseHandler.Remove(p.Value);
                }
            }
            if (mustRaise)
                raiseHandler.Raise();
        }

        /// <summary>
        /// Remove all items from the bag, resetting internal table to initial size.
        /// </summary>
        [Tested]
        public virtual void Clear()
        {
            updatecheck();
            if (Size == 0)
                return;
            dict.Clear();
            int oldsize = Size;
            Size = 0;
            if ((ActiveEvents & EventTypeEnum.Cleared) != 0)
                raiseCollectionCleared(true, oldsize);
            if ((ActiveEvents & EventTypeEnum.Changed) != 0)
                raiseCollectionChanged();
        }


        /// <summary>
        /// Remove all items *not* in a supplied collection from this bag,
        /// counting multiplicities.
        /// </summary>
        /// <typeparam name="U"></typeparam>
        /// <param name="items">The items to retain</param>
        [Tested]
        public virtual void RetainAll<U>(SCG.IEnumerable<U> items) where U : T
        {
            updatecheck();

            HashBag<T> res = new HashBag<T>(ItemEqualityComparer);

            foreach (U item in items)
            {
                Multiplicity<T> p = new Multiplicity<T>(item);
                if (dict.Find(ref p))
                {
                    Multiplicity<T> q = p;
                    if (res.dict.Find(ref q))
                    {
                        if (q.Count < p.Count)
                        {
                            q.Count++;
                            res.dict.Update(q);
                            res.Size++;
                        }
                    }
                    else
                    {
                        q.Count = 1;
                        res.dict.Add(q);
                        res.Size++;
                    }
                }
            }

            if (Size == res.Size)
                return;

            CircularQueue<T> wasRemoved = null;
            if ((ActiveEvents & EventTypeEnum.Removed) != 0)
            {
                wasRemoved = new CircularQueue<T>();
                foreach (Multiplicity<T> p in dict)
                {
                    int removed = p.Count - res.ContainsCount(p.Value);
                    if (removed > 0)
#warning We could send bag events here easily using a CircularQueue of (should?)
                        for (int i = 0; i < removed; i++)
                            wasRemoved.Enqueue(p.Value);
                }
            }
            dict = res.dict;
            Size = res.Size;

            if ((ActiveEvents & EventTypeEnum.Removed) != 0)
                raiseForRemoveAll(wasRemoved);
            else if ((ActiveEvents & EventTypeEnum.Changed) != 0)
                raiseCollectionChanged();
        }

        /// <summary>
        /// Check if all items in a supplied collection is in this bag
        /// (counting multiplicities). 
        /// </summary>
        /// <param name="items">The items to look for.</param>
        /// <typeparam name="U"></typeparam>
        /// <returns>True if all items are found.</returns>
        [Tested]
        public virtual bool ContainsAll<U>(SCG.IEnumerable<U> items) where U : T
        {
            HashBag<T> res = new HashBag<T>(ItemEqualityComparer);

            foreach (T item in items)
                if (res.ContainsCount(item) < ContainsCount(item))
                    res.Add(item);
                else
                    return false;

            return true;
        }


        /// <summary>
        /// Create an array containing all items in this bag (in enumeration order).
        /// </summary>
        /// <returns>The array</returns>
        [Tested]
        public override T[] ToArray()
        {
            T[] res = new T[Size];
            int ind = 0;

            foreach (Multiplicity<T> p in dict)
                for (int i = 0; i < p.Count; i++)
                    res[ind++] = p.Value;

            return res;
        }


        /// <summary>
        /// Count the number of times an item is in this set.
        /// </summary>
        /// <param name="item">The item to look for.</param>
        /// <returns>The count</returns>
        [Tested]
        public virtual int ContainsCount(T item)
        {
            Multiplicity<T> p = new Multiplicity<T>(item, 0);

            if (dict.Find(ref p))
                return p.Count;

            return 0;
        }

        public virtual ICollectionValue<T> UniqueItems() 
        { 
            return new DropMultiplicity<T>(dict); 
        }

        public virtual ICollectionValue<Multiplicity<T>> ItemMultiplicities()
        {
            return new GuardedCollectionValue<Multiplicity<T>>(dict);
        }

        /// <summary>
        /// Remove all copies of item from this set.
        /// </summary>
        /// <param name="item">The item to remove</param>
        [Tested]
        public virtual void RemoveAllCopies(T item)
        {
            updatecheck();

            Multiplicity<T> p = new Multiplicity<T>(item, 0);

            if (dict.Find(ref p))
            {
                Size -= p.Count;
                dict.Remove(p);
                if ((ActiveEvents & EventTypeEnum.Removed) != 0)
                    raiseItemsRemoved(p.Value, p.Count);
                if ((ActiveEvents & EventTypeEnum.Changed) != 0)
                    raiseCollectionChanged();
            }
        }

        #endregion

        #region ICollection<T> Members


        /// <summary>
        /// Copy the items of this bag to part of an array.
        /// <exception cref="ArgumentOutOfRangeException"/> if i is negative.
        /// <exception cref="ArgumentException"/> if the array does not have room for the items.
        /// </summary>
        /// <param name="array">The array to copy to</param>
        /// <param name="index">The starting index.</param>
        [Tested]
        public override void CopyTo(T[] array, int index)
        {
            if (index < 0 || index >= array.Length || index + Count > array.Length)
                throw new ArgumentOutOfRangeException();

            foreach (Multiplicity<T> p in dict)
                for (int j = 0; j < p.Count; j++)
                    array[index++] = p.Value;
        }

        #endregion

        #region IExtensible<T> Members

        /// <summary>
        /// Report if this is a set collection.
        /// </summary>
        /// <value>Always true</value>
        [Tested]
        public virtual bool AllowsDuplicates { [Tested] get { return true; } }

        /// <summary>
        /// By convention this is true for any collection with set semantics.
        /// </summary>
        /// <value>True if only one representative of a group of equal items 
        /// is kept in the collection together with the total count.</value>
        public virtual bool DuplicatesByCounting { get { return true; } }

        /// <summary>
        /// Add an item to this bag.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>Always true</returns>
        [Tested]
        public virtual bool Add(T item)
        {
            updatecheck();
            add(ref item);
            if (ActiveEvents != 0)
                raiseForAdd(item);
            return true;
        }

        private void add(ref T item)
        {
            Multiplicity<T> p = new Multiplicity<T>(item, 1);
            if (dict.Find(ref p))
            {
                p.Count++;
                dict.Update(p);
                item = p.Value;
            }
            else
                dict.Add(p);
            Size++;
        }

        /// <summary>
        /// Add the elements from another collection with a more specialized item type 
        /// to this collection. 
        /// </summary>
        /// <typeparam name="U">The type of items to add</typeparam>
        /// <param name="items">The items to add</param>
        public virtual void AddAll<U>(SCG.IEnumerable<U> items) where U : T
        {
            updatecheck();
#warning We could easily raise bag events
            bool mustRaiseAdded = (ActiveEvents & EventTypeEnum.Added) != 0;
            CircularQueue<T> wasAdded = mustRaiseAdded ? new CircularQueue<T>() : null;
            bool wasChanged = false;
            foreach (T item in items)
            {
                T jtem = item;
                add(ref jtem);
                wasChanged = true;
                if (mustRaiseAdded)
                    wasAdded.Enqueue(jtem);
            }
            if (!wasChanged)
                return;
            if (mustRaiseAdded)
                foreach (T item in wasAdded)
                    raiseItemsAdded(item, 1);
            if ((ActiveEvents & EventTypeEnum.Changed) != 0)
                raiseCollectionChanged();
        }

        #endregion

        #region IEnumerable<T> Members


        /// <summary>
        /// Choose some item of this collection. 
        /// </summary>
        /// <exception cref="NoSuchItemException">if collection is empty.</exception>
        /// <returns></returns>
        public override T Choose()
        {
            return dict.Choose().Value;
        }

        /// <summary>
        /// Create an enumerator for this bag.
        /// </summary>
        /// <returns>The enumerator</returns>
        [Tested]
        public override SCG.IEnumerator<T> GetEnumerator()
        {
            int left;
            int mystamp = Stamp;

            foreach (Multiplicity<T> p in dict)
            {
                left = p.Count;
                while (left > 0)
                {
                    if (mystamp != Stamp)
                        throw new CollectionModifiedException();

                    left--;
                    yield return p.Value;
                }
            }
        }
        #endregion

        #region ICloneable Members

        /// <summary>
        /// Make a shallow copy of this HashBag.
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            //TODO: make sure this 
            HashBag<T> clone = new HashBag<T>(dict.Count > 0 ? dict.Count : 1, ItemEqualityComparer);
            //TODO: make sure this really adds in the counting bag way!
            clone.AddAll(this);
            return clone;
        }

        #endregion


        #region Diagnostics
        /// <summary>
        /// Test internal structure of data (invariants)
        /// </summary>
        /// <returns>True if pass</returns>
        [Tested]
        public virtual bool Check()
        {
            bool retval = dict.Check();
            int count = 0;

            foreach (Multiplicity<T> p in dict)
                count += p.Count;

            if (count != Size)
            {
                Console.WriteLine("count({0}) != size({1})", count, Size);
                retval = false;
            }

            return retval;
        }
        #endregion
    }
}
