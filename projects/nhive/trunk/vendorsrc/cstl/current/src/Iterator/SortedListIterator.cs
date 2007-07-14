using System;
using System.Collections.Generic;

namespace STL.Iterator
{
/// <summary>
/// Provides an iterator wrapper around the .NET SortedList<T> class
/// </summary>
/// <typeparam name="T"></typeparam>
public class SortedListIterator<T> : RandomAccessIterator<T>, IEquatable<SortedListIterator<T>>, IComparable<SortedListIterator<T>>
{
    private SortedList<T> m_List;
    private int m_Position = 0;
    
    public SortedListIterator(SortedList<T> list)
    {
        m_List = list;
        m_Position = 0;
    }

    public SortedListIterator(SortedList<T> list, int position)
    {
        m_List     = list;
        m_Position = position;
    }
    
    #region RandomAccessIterator<T> Members
    public RandomAccessIterator<T> OffsetBy(int offset)
    {
        m_Position += offset;
        return this;
    }

    public int DistanceFrom(RandomAccessIterator<T> iterator)
    {
        return iterator.Position - m_Position;
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
            return m_List.GetByIndex(m_Position + index);
        }
        set
        {
            //m_List[m_Position + index] = value;
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
        return m_List.GetByIndex(m_Position);
    }

    public void MoveNext()
    {
        ++Position;
    }

    public void Assign(InputIterator<T> iterator)
    {
        SortedListIterator<T> listIterator = iterator as SortedListIterator<T>;
        if(listIterator == null)
            throw new ArgumentException("Iterator type mismatch.");
            
        Assign(listIterator);
    }
    #endregion

    #region IEquatable<InputIterator<T>> Members
    public bool Equals(InputIterator<T> rhs)
    {
        SortedListIterator<T> listIterator = rhs as SortedListIterator<T>;
        if(listIterator == null)
            return false;
        return Equals(listIterator);
    }
    #endregion

    #region OutputIterator<T> Members
    public void Write(T t)
    {
        
    }
    #endregion

    #region IComparable<RandomAccessIterator<T>> Members
    public int CompareTo(RandomAccessIterator<T> rhs)
    {
        SortedListIterator<T> listIterator = rhs as SortedListIterator<T>;
        if(listIterator == null)
            throw new ArgumentException("Iterator type mismatch.");
        
        return CompareTo(listIterator);
    }
    #endregion
    
    
    public void Assign(SortedListIterator<T> iterator)
    {
        m_List     = iterator.m_List;
        m_Position = iterator.m_Position;
    }
    
    #region IEquitable<SortedListIterator<T>> Members
    public bool Equals(SortedListIterator<T> rhs)
    {
        return object.ReferenceEquals(m_List, rhs.m_List) && (m_Position == rhs.m_Position);
    }
    #endregion

    #region IComparable<SortedListIterator<T>> Members
    public int CompareTo(SortedListIterator<T> rhs)
    {
        if(!object.ReferenceEquals(m_List, rhs.m_List))
            throw new ArgumentException("Comparing iterators to different collections.");
            
        return m_Position.CompareTo(rhs.m_Position);
    }
    #endregion

    #region ICloneable Members
    public object Clone()
    {
        return new SortedListIterator<T>(m_List, m_Position); 
    }
    #endregion
   

    public static SortedListIterator<T> Begin(IList<T> list)
    {
        return new SortedListIterator<T>(list, 0);
    }
    
    public static SortedListIterator<T> End(IList<T> list)
    {
        return new SortedListIterator<T>(list, list.Count);
    }

}

}