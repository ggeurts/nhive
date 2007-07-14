//-----------------------------------------------------------------------------
// CSTL
// Copyright (C) 2006 by Harold Howe hhjunk@mchsi.com
// http://sourceforge.net/projects/cstl

using System;
using System.Collections.Generic;

using CSTL.Iterator;
using CSTL;

namespace CSTL.Collections
{

public class Vector<T> : IList<T>, IEquatable<Vector<T>>, IComparable<Vector<T>>
{
    const int MIN_ALLOC = 4;
    const int REALLOC_FACTOR = 15; // 1.5 growth factor
    

    #region Constructors
    public Vector()
    {
    }
    
    public Vector(IEnumerable<T> rhs)
    {
        Assign(rhs);
    }

    public Vector(ICollection<T> rhs)
    {
        Alloc(rhs.Count);
        AddRange(rhs);
    }

    public Vector(int count)
        :this(count, default(T))
    {
    }
    
    public Vector(int count, T t)
    {
        if(count>0)
        {
            Alloc(count);
            for(int i=0 ; i<count; ++i)
                Add(t);
        }
    }
    
    public Vector(InputIterator<T> begin, InputIterator<T> end)
        :this(begin, end, 0)
    {
    }

    public Vector(RandomAccessIterator<T> begin, RandomAccessIterator<T> end)
        :this(begin, end, end.DistanceFrom(begin))
    {
    }
    
    protected Vector(InputIterator<T> begin, InputIterator<T> end, int capacity)
    {
        if(capacity > 0)
            Alloc(capacity);
    
        Algorithm.ForEach(begin, end, Add);
    }
    #endregion

    #region Assign, swap
    public void Assign(InputIterator<T> begin, InputIterator<T> end)
    {
        Clear();
        AddRange(begin,end);
    }
    
    public void Assign(RandomAccessIterator<T> begin, RandomAccessIterator<T> end)
    {
        Clear();
        AddRange(begin,end);
    }
    
    public void Assign(int count, T elem)
    {
        Clear();
        CheckReAlloc(count);
        for(int i=0; i<count; ++i)
            Add(elem);
    }

    public void Assign(IEnumerable<T> rhs)
    {
        Clear();
        foreach(T t in rhs)
            Add(t);
    }

    public void Assign(ICollection<T> rhs)
    {
        Clear();
        CheckReAlloc(rhs.Count);
        foreach(T t in rhs)
            Add(t);
    }

    public void Swap(Vector<T> rhs)
    {
        Algorithm.Swap(ref m_Array, ref rhs.m_Array);
        Algorithm.Swap(ref m_Count, ref rhs.m_Count);
    }
    #endregion

    #region Stack operations
    public void PushBack(T elem)
    {
        Add(elem);
    }
    
    public void PopBack()
    {
        if(m_Count>0)
        {
            m_Array[m_Count-1] = default(T);
            --m_Count;
        }
    }
    #endregion

    #region Insertion
    /// <summary>
    /// Inserts a value before the position pointed to by an iterato.r
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="value"></param>
    public ListIterator<T> Insert(ListIterator<T> pos, T value)
    {
        int index = pos.Position;
        Insert(index, value);
        return new ListIterator<T>(this, index);
    }

    public void Insert(ListIterator<T> pos, InputIterator<T> begin, InputIterator<T> end)
    {
        Algorithm.ForEach(begin, end, delegate(T t) {
            Insert(pos,t);
        });
    }

    /// <summary>
    /// Inserts a range into the collection before a position pointed to by an iterator
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="begin"></param>
    /// <param name="end"></param>
    public void Insert(ListIterator<T> pos, ForwardIterator<T> begin, ForwardIterator<T> end)
    {
        int distance = IteratorUtil.Distance(begin, end);
        Insert(pos, begin, end, 0);
    }

    public void Insert(ListIterator<T> pos, RandomAccessIterator<T> begin, RandomAccessIterator<T> end)
    {
        int distance = IteratorUtil.Distance(begin, end);
        Insert(pos, begin, end, distance);
    }
    
    private void Insert(ListIterator<T> pos, InputIterator<T> begin, InputIterator<T> end, int newItems)
    {
        CheckReAlloc(m_Count + newItems);
        
        if(!pos.Equals(End()))
            Shift(pos.Position, newItems);

        Algorithm.Copy(begin, end, new ListIterator<T>(m_Array, pos.Position));
        m_Count += newItems;
    }
    
    /// <summary>
    /// Inserts count copies of value into the vector in front of pos
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="count"></param>
    /// <param name="value"></param>
    public void Insert(ListIterator<T> pos, int count, T value)
    {
        CheckReAlloc(m_Count + count);
        if(!pos.Equals(End()))
            Shift(pos.Position, (int)count);
        Algorithm.FillN(new ListIterator<T>(m_Array,pos.Position), (int)count,value);
        m_Count += count;
    }
    #endregion
    
    #region Range Addition
    public void AddRange(InputIterator<T> begin, InputIterator<T> end)
    {
        Algorithm.ForEach(begin, end, Add);
    }

    public void AddRange(RandomAccessIterator<T> begin, RandomAccessIterator<T> end)
    {
        CheckReAlloc(m_Count + IteratorUtil.Distance(begin, end));
        Algorithm.ForEach(begin, end, Add);
    }

    public void AddRange(IEnumerable<T> enumerable)
    {
        foreach(T t in enumerable)
            Add(t);
    }

    public void AddRange(ICollection<T> collection)
    {
        CheckReAlloc(m_Count + collection.Count);
        foreach(T t in collection)
            Add(t);
    }    
    #endregion
    
    #region Erase
    /// <summary>
    /// Erases the element at pos, and returns an iterator to the next element.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public ListIterator<T> Erase(ListIterator<T> pos)
    {
        int index = pos.Position;
        RemoveAt(index);
        return new ListIterator<T>(this,index);
    }
    
    /// <summary>
    /// Erases a range of elements. Returns an iterator to the next element after the deleted range.
    /// </summary>
    /// <param name="begin"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public ListIterator<T> Erase(ListIterator<T> begin, ListIterator<T> end)
    {
        int index = begin.Position;
        int count = IteratorUtil.Distance(begin, end);
        Shift(index + count, -count);
        m_Count -= count;
        return new ListIterator<T>(this,index);
    }
    #endregion

    #region Iterator methods
    public ListIterator<T> Begin()
    {
        return new ListIterator<T>(this,0);
    }
    
    public ListIterator<T> End()
    {
        return new ListIterator<T>(this, Count);
    }
    #endregion
    
    public bool Empty
    {
        get {return Count == 0;}
    }
    
    public T Front
    {
        get {return this[0];}
        set {this[0] = value;}
    }
    
    public T Back
    {
        get {return this[Count-1];}
        set {this[Count-1] = value;}
    }
    
    public void Resize(int n)
    {
        // TODO: Finish implementing this.
        throw new NotImplementedException("not implemented yet");
    }

    public void Resize(int n, T t)
    {
        // TODO: Finish implementing this.
        throw new NotImplementedException("not implemented yet");
    }


    #region Capacity
    public int Capacity
    {
        get
        {
            if(m_Array == null)
                return 0;
                
            return m_Array.Length;
        }
        set
        {
            if ((m_Array == null) || (value != m_Array.Length))
            {
                if(value < m_Count)
                    return; // ignore request to set capacity below count;
                else if(value == 0)
                    m_Array = null;
                else if(value > 0 )
                    Alloc(value);
            }
        }
    }
    #endregion

    #region IList<T> Members

    public int IndexOf(T item)
    {
        ListIterator<T> iter = Algorithm.Find(Begin(), End(), item, EqualityComparer<T>.Default);
        if(iter == null)
            return -1;

        return iter.Position;
    }

    public void Insert(int index, T item)
    {
        RangeCheckLower(index);
        CheckReAlloc();

        // Shift all the items between index and the end of the container right one spot. If they are inserting at the
        // end, then don't bother.
        if(index != m_Count)
            Shift(index, 1);
        m_Array[index] =  item;
        ++m_Count;
    }

    public void RemoveAt(int index)
    {
        RangeCheck(index);

        if(index != m_Count - 1)
            Shift(index+1, -1);

        m_Array[m_Count -1] = default(T);
        --m_Count;
    }

    public T this[int index]
    {
        get
        {
            RangeCheck(index);
            return m_Array[index];
        }
        set
        {
            RangeCheck(index);
            m_Array[index] = value;
        }
    }

    #endregion

    #region ICollection<T> Members

    public void Add(T item)
    {
        CheckReAlloc();
        
        m_Array[m_Count] = item;
        ++m_Count;
    }

    public void Clear()
    {
        // Note: follow C++ approach of not adjusting capacity when Clear is called. 
        // TODO: Should this call Erase(begin(), end()), as in C++?
        if(m_Array != null) 
        {
            // Zero the array. This will allow a GC of objects in the array if we were the only remaining reference
            Array.Clear(m_Array, 0, m_Array.Length);
        }
        m_Count = 0;
    }

    public bool Contains(T item)
    {
        return Algorithm.Find(Begin(), End(), item, EqualityComparer<T>.Default) != null;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        Algorithm.Copy(this, 0, array, arrayIndex);
    }

    public int Count
    {
        get { return m_Count; }
    }

    public bool IsReadOnly
    {
        get { return false; }
    }

    public bool Remove(T item)
    {
        ListIterator<T> iter = Algorithm.Find(Begin(), End(), item, EqualityComparer<T>.Default);
        if(iter != null)
            Erase(iter);
            
       return false; // TODO fix
    }

    #endregion

    #region IEnumerable<T> Members

    public IEnumerator<T> GetEnumerator()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    #endregion

    #region IEnumerable Members

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    #endregion

    
    #region IEquatable and equality
    public bool Equals(Vector<T> rhs)
    {
        if(object.ReferenceEquals(rhs, null))   // Equals must return false if rhs==null
            return false;

        // TODO: implement
        return false;
    }
    
    public override bool Equals(object obj)
    {
        if(object.ReferenceEquals(this,obj))     // if Object.ReferenceEquals returns true,
            return true;                         // then both instances point to same object
            
        return Equals(obj as Vector<T>);    
    }    


    public override int GetHashCode()
    {       
        // TODO: implement
        return 1;
    }

    public static bool operator==(Vector<T> lhs, Vector<T> rhs)
    {
        if(object.ReferenceEquals(lhs, null))   
        {
            if(object.ReferenceEquals(rhs, null))
                return true;
            else
                return false;
        }
    
        return lhs.Equals(rhs);
    }
    
    public static bool operator!=(Vector<T> lhs, Vector<T> rhs)
    {
        return !(lhs==rhs);
    }
    #endregion

    #region IComparable<T> and comparison
    public int CompareTo(Vector<T> rhs)
    {
        if(rhs==null)
            return 1;
        
        return 1;
    }
    
    public int CompareTo(Object obj)
    {
        if(object.ReferenceEquals(this,obj))     // if Object.Equals returns true,
            return 0;     // then both instances point to same object
        if(obj==null)
            return 1;
    
        if(obj.GetType() != typeof(Vector<T>))
            throw new ArgumentException();
            
        return CompareTo((Vector<T>)obj);
    }    
    #endregion
    
    
    private void Alloc(int size)
    {
        if((m_Array == null) || (m_Array.Length != size))
        {
            T[] temp = new T[size];
            
            if(m_Array != null)
                Algorithm.Copy(m_Array, temp);
            m_Array = temp;
        }
    }

    private void ReAlloc(int newCount)
    {
        int newSize  = Math.Max(MIN_ALLOC, m_Count * REALLOC_FACTOR / 10);
        Alloc(Math.Max(newSize, newCount));
    }

    private void RangeCheck(int index)
    {
        if((index < 0) || (index >= m_Count))
            throw new IndexOutOfRangeException();
    }

    private void RangeCheckLower(int index)
    {
        if(index < 0)
            throw new IndexOutOfRangeException();
    }
    
    private void CheckReAlloc()
    {
        CheckReAlloc(m_Count + 1);
    }

    private void CheckReAlloc(int newCount)
    {
        if((m_Array == null) || (newCount > m_Array.Length))
        {
            ReAlloc(newCount);
        }
    }

    private void Shift(int index, int distance)
    {
        // shift elements starting at pos to the right. distance determines how far
        ListIterator<T> srcBegin = new ListIterator<T>(m_Array,index);
        ListIterator<T> srcEnd   = new ListIterator<T>  (m_Array,m_Count);
        if(distance > 0)
        {
            // if shifting right, we must use CopyBackward. 
            ListIterator<T> targetEnd = new ListIterator<T>(m_Array, m_Count + distance);
            Algorithm.CopyBackward(srcBegin, srcEnd, targetEnd);
        }
        else
        {
            // If shifting left, copy using the standard Copy algorithm
            ListIterator<T> target = new ListIterator<T>(m_Array, index+distance);
            Algorithm.Copy(srcBegin, srcEnd, target);
        
        }
    }

    private T[] m_Array;
    private int m_Count;

}


}