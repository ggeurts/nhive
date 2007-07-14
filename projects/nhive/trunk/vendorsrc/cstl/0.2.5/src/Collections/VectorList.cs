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

/// <summary>
/// VectorList adds C++ vector syntax to a built in .NET 2.0 List<T>. Use this when you want to have vector
/// methods in your code, but you need to pass your collection to other methods that expect a List<T>
/// </summary>
/// <remarks>
/// Due to the internal design of the List class, VectorList cannot offer an efficient swap mechanism.
/// The method is available, but it won't be fast. 
/// </remarks>
/// <typeparam name="T"></typeparam>
public class VectorList<T> : List<T>, IEquatable<VectorList<T>>, IComparable<VectorList<T>>
{
    #region Constructors
    public VectorList()
        :base()
    {
    }
    
    public VectorList(IEnumerable<T> enumerable)
        :base(enumerable)
    {
    }
    
    public VectorList(uint count)
        :this(count, default(T))
    {
    }
    
    public VectorList(uint count, T t)
        :base((int)count)
    {
        for(uint i=0 ; i<count; ++i)
            Add(t);
    }
    
    public VectorList(InputIterator<T> begin, InputIterator<T> end)
        :this(begin, end, 0)
    {
    }

    public VectorList(RandomAccessIterator<T> begin, RandomAccessIterator<T> end)
        :this(begin, end, end.DistanceFrom(begin))
    {
    }
    
    protected VectorList(InputIterator<T> begin, InputIterator<T> end, int capacity)
        :base(capacity)
    {
        Algorithm.ForEach(begin, end, Add);
    }
    #endregion

    #region Assign, swap
    public void Assign(InputIterator<T> begin, InputIterator<T> end)
    {
        Clear();
        Algorithm.ForEach(begin, end, Add);
    }
    
    public void Assign(RandomAccessIterator<T> begin, RandomAccessIterator<T> end)
    {
        Clear();
        Capacity = IteratorUtil.Distance(begin, end);
        Algorithm.ForEach(begin, end, Add);
    }
    
    public void Assign(uint count, T elem)
    {
        Clear();
        Capacity = (int)count;
        for(uint i=0; i<count; ++i)
            Add(elem);
    }
    
    public void Swap(VectorList<T> rhs)
    {
        // Note: This implementation is not exception safe. Without access to internals, it may
        //       be impossible to implement this in a correct manner. Fortunately, the only way this 
        //       could fail, I think, is an out of memory situation. 
        VectorList<T> temp = new VectorList<T>(this);
        Clear();
        Capacity = rhs.Count;
        AddRange(rhs);
        rhs.Clear();
        rhs.Capacity = temp.Count;
        rhs.AddRange(temp);
    }
    #endregion

    #region Stack operations
    public void PushBack(T elem)
    {
        Add(elem);
    }
    
    public void PopBack()
    {
        if(!Empty)
            RemoveAt(Count-1);
    }
    #endregion

    #region Insertion
    /// <summary>
    /// Inserts a value before the position pointed to by an iterator
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="value"></param>
    public ListIterator<T> Insert(ListIterator<T> pos, T value)
    {
        int index = pos.Position;
        base.Insert(index, value);
        return null;
    }
    
    /// <summary>
    /// Inserts a range into the collection before a position pointed to by an iterator
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="begin"></param>
    /// <param name="end"></param>
    public void Insert(ListIterator<T> pos, InputIterator<T> begin, InputIterator<T> end)
    {
        int index = pos.Position;
        InsertRange(index, IteratorUtil.CreateEnumerator(begin, end));
    }

    public void Insert(ListIterator<T> pos, RandomAccessIterator<T> begin, RandomAccessIterator<T> end)
    {
        Capacity = Count + IteratorUtil.Distance(begin, end);
        int index = pos.Position;
        InsertRange(index, IteratorUtil.CreateEnumerator(begin, end));
    }
    
    /// <summary>
    /// Inserts count copies of value into the vector in front of pos
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="count"></param>
    /// <param name="value"></param>
    public void Insert(ListIterator<T> pos, uint count, T value)
    {
        Capacity = Count + (int)count;
        int index = pos.Position;
        InsertRange(index, IteratorUtil.CreateEnumerator(count, value));
    }
    #endregion
    
    #region Erase
    /// <summary>
    /// Erases the element at pos, and returns an iterator to the next element.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    ListIterator<T> Erase(ListIterator<T> pos)
    {
        int index = pos.Position;
        base.RemoveAt(index);
        return new ListIterator<T>(this,index);
    }
    
    /// <summary>
    /// Erases a range of elements. Returns an iterator to the next element after the deleted range.
    /// </summary>
    /// <param name="begin"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    ListIterator<T> Erase(ListIterator<T> begin, ListIterator<T> end)
    {
        int index = begin.Position;
        int count = IteratorUtil.Distance(begin, end);
        base.RemoveRange(index, count);
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
    
    public void Resize(uint n)
    {
        // TODO: Finish implementing this.
        throw new NotImplementedException("not implemented yet");
    }

    public void Resize(uint n, T t)
    {
        // TODO: Finish implementing this.
        throw new NotImplementedException("not implemented yet");
    }
 
 
    
    #region IEquatable and equality
    public bool Equals(VectorList<T> rhs)
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
            
        return Equals(obj as VectorList<T>);    
    }    


    public override int GetHashCode()
    {       
        // TODO: implement
        return 1;
    }

	public static bool operator==(VectorList<T> lhs, VectorList<T> rhs)
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
	
	public static bool operator!=(VectorList<T> lhs, VectorList<T> rhs)
	{
	    return !(lhs==rhs);
	}
    #endregion

    #region IComparable<T> and comparison
    public int CompareTo(VectorList<T> rhs)
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
    
        if(obj.GetType() != typeof(VectorList<T>))
            throw new ArgumentException();
            
        return CompareTo((VectorList<T>)obj);
    }    
    #endregion
}


}