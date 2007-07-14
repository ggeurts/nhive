//-----------------------------------------------------------------------------
// CSTL
// Copyright (C) 2006 by Harold Howe hhjunk@mchsi.com
// http://sourceforge.net/projects/cstl

using System;
using System.Collections.Generic;

namespace CSTL.Iterator
{
public class ListIterator<T> : RandomAccessIterator<T>, IEquatable<ListIterator<T>>, IComparable<ListIterator<T>>
{
    private IList<T> m_List;
    private int m_Position;
    
    public ListIterator(IList<T> list)
    {
        m_List = list;
        m_Position = 0;
    }

    public ListIterator(IList<T> list, int position)
    {
        m_List     = list;
        m_Position = position;
    }
    
    #region RandomAccessIterator<T> Members
    public ListIterator<T> OffsetBy(int offset)
    {
        m_Position += offset;
        return this;
    }

    /// <summary>
    /// Calculate the distance of this iterator from another. DistanceFrom behaves like operator minus,
    /// ie return this - iterator.
    /// </summary>
    /// <param name="iterator"></param>
    /// <returns></returns>
    public int DistanceFrom(RandomAccessIterator<T> iterator)
    {
        return m_Position - iterator.Position;
    }
    
    public int Position
    {
        get
        {
            return m_Position;
        }
        
        set
        {
            m_Position = value;
        }
    }    

    public T this[int index]
    {
        get
        {
            return m_List[m_Position + index];
        }
        set
        {
            m_List[m_Position + index] = value;
        }
    }

    #endregion

    #region BidirectionalIterator<T> Members
    public void MovePrev()
    {
        --Position;
    }
    #endregion

    #region InputIterator<T> Members

    public T Read()
    {
        return m_List[m_Position];
    }

    public void MoveNext()
    {
        ++Position;
    }

    public void Assign(InputIterator<T> iterator)
    {
        ListIterator<T> listIterator = iterator as ListIterator<T>;
        if(listIterator == null)
            throw new ArgumentException("Iterator type mismatch.");
            
        Assign(listIterator);
    }
    #endregion

    #region IEquatable<InputIterator<T>> Members
    public bool Equals(InputIterator<T> rhs)
    {
        ListIterator<T> listIterator = rhs as ListIterator<T>;
        if(listIterator == null)
            return false;
        return Equals(listIterator);
    }
    #endregion

    #region OutputIterator<T> Members
    public void Write(T t)
    {
        m_List[m_Position] = t;
    }
    #endregion

    #region IComparable<RandomAccessIterator<T>> Members

    public int CompareTo(RandomAccessIterator<T> rhs)
    {
        ListIterator<T> listIterator = rhs as ListIterator<T>;
        if(listIterator == null)
            throw new ArgumentException("Iterator type mismatch.");
        
        return CompareTo(listIterator);
    }
    #endregion
    
    
    public void Assign(ListIterator<T> iterator)
    {
        m_List     = iterator.m_List;
        m_Position = iterator.m_Position;
    }
    
    #region IEquitable<ListIterator<T>> Members
    public bool Equals(ListIterator<T> rhs)
    {
        if(object.ReferenceEquals(rhs, null))
            return false;
            
        return object.ReferenceEquals(m_List, rhs.m_List) && (m_Position == rhs.m_Position);
    }
    #endregion

    #region IComparable<ListIterator<T>> Members
    public int CompareTo(ListIterator<T> rhs)
    {
        if(!object.ReferenceEquals(m_List, rhs.m_List))
            throw new ArgumentException("Comparing iterators to different collections.");
            
        return m_Position.CompareTo(rhs.m_Position);
    }
    #endregion

    #region ICloneable Members
    public object Clone()
    {
        return new ListIterator<T>(m_List, m_Position); 
    }
    #endregion
   

    public static ListIterator<T> Begin(IList<T> list)
    {
        return new ListIterator<T>(list, 0);
    }
    
    public static ListIterator<T> End(IList<T> list)
    {
        return new ListIterator<T>(list, list.Count);
    }


    #region RandomAccessIterator<T> Members
    RandomAccessIterator<T> RandomAccessIterator<T>.OffsetBy(int offset)
    {
        return OffsetBy(offset);
    }
    #endregion
}

}