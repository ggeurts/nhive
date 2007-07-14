//-----------------------------------------------------------------------------
// CSTL
// Copyright (C) 2006 by Harold Howe hhjunk@mchsi.com
// http://sourceforge.net/projects/cstl

using System;
using System.Collections.Generic;

namespace CSTL.Iterator
{


public interface InputIterator<T> : IEquatable<InputIterator<T>>, ICloneable
{
    T Read();
    void MoveNext();
//    bool Equals(InputIterator<T> iterator);
    void Assign(InputIterator<T> iterator);
}

public interface OutputIterator<T> : ICloneable
{
    void Write(T t);
    void MoveNext();
    void Assign(InputIterator<T> iterator);
}

public interface ForwardIterator<T> : InputIterator<T>, OutputIterator<T>
{
    new void MoveNext(); 
    new void Assign(InputIterator<T> iterator);
}

public interface BidirectionalIterator<T>: ForwardIterator<T>
{
    void MovePrev();
}

public interface RandomAccessIterator<T> : BidirectionalIterator<T>, IComparable<RandomAccessIterator<T>>
{
    RandomAccessIterator<T> OffsetBy(int offset);
    int DistanceFrom(RandomAccessIterator<T> iterator);
    
    int Position
    {
        get;set;
    }
    
//    int CompareTo(RandomAccessIterator<T> iterator);
    
    T this[int index]
    {
        get ; set;
    }
}

public static partial class IteratorUtil
{
    public static ListIterator<T> Begin<T>(IList<T> list)
    {
        return new ListIterator<T>(list, 0);
    }
    
    public static ListIterator<T> End<T>(IList<T> list)
    {
        return new ListIterator<T>(list, list.Count);
    }

    public static BackInsertIterator<T> BackInserter<T>(ICollection<T> collection)
    {
        return new BackInsertIterator<T>(collection);
    }
    
    public static InsertIterator<T> FrontInserter<T>(IList<T> list)
    {
        return new InsertIterator<T>(list,0);
    }

    public static InsertIterator<T> Inserter<T>(IList<T> list, int pos)
    {
        return new InsertIterator<T>(list,pos);
    }
    
    public static I Clone<I>(I i)
        where I : ICloneable
    {
        return (I)i.Clone();
    }
}

}