//-----------------------------------------------------------------------------
// CSTL
// Copyright (C) 2006 by Harold Howe hhjunk@mchsi.com
// http://sourceforge.net/projects/cstl

using System;
using System.Collections.Generic;
using CSTL.Iterator;


namespace CSTL
{

public static partial class Algorithm
{
    public static ForwardIterator<T> Remove<T>(ForwardIterator<T> begin, ForwardIterator<T> end, T value)
        where T:IEquatable<T>
    {
        ForwardIterator<T> iter = Find(begin, end, value);
        if(iter == null)
            return null;
            
        //ForwardIterator<T> dest = IteratorUtil.Clone(iter);
        return RemoveCopy(iter, end, iter, value);
    }

    public static RandomAccessIterator<T> Remove<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end, T value)
        where T:IEquatable<T>
    {
        return Remove((ForwardIterator<T>)begin, (ForwardIterator<T>)end, value) as RandomAccessIterator<T>;
    }

    public static ListIterator<T> Remove<T>(ListIterator<T> begin, ListIterator<T> end, T value)
        where T:IEquatable<T>
    {
        return Remove((ForwardIterator<T>)begin, (ForwardIterator<T>)end, value) as ListIterator<T>;
    }

    public static ForwardIterator<T> Remove<T>(ForwardIterator<T> begin, ForwardIterator<T> end, T value, IEqualityComparer<T> comparer)
    {
        ForwardIterator<T> iter = Find(begin, end, value, comparer);
        if(iter == null)
            return null;
            
        //ForwardIterator<T> dest = IteratorUtil.Clone(iter);
        return RemoveCopy(iter, end, iter, value, comparer) as ForwardIterator<T>;
    }

    public static RandomAccessIterator<T> Remove<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end, T value, IEqualityComparer<T> comparer)
        where T:IEquatable<T>
    {
        return Remove((ForwardIterator<T>)begin, (ForwardIterator<T>)end, value, comparer) as RandomAccessIterator<T>;
    }

    public static ListIterator<T> Remove<T>(ListIterator<T> begin, ListIterator<T> end, T value, IEqualityComparer<T> comparer)
        where T:IEquatable<T>
    {
        return Remove((ForwardIterator<T>)begin, (ForwardIterator<T>)end, value, comparer) as ListIterator<T>;
    }
}

}
