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
    public static ForwardIterator<T> Unique<T>(ForwardIterator<T> begin, ForwardIterator<T> end)
        where T:IEquatable<T>
    {
        return Unique(begin, end, EqualityComparer<T>.Default);
    }

    public static RandomAccessIterator<T> Unique<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end)
        where T:IEquatable<T>
    {
        return Unique((ForwardIterator<T>)begin, (ForwardIterator<T>)end) as RandomAccessIterator<T>;
    }

    public static ListIterator<T> Unique<T>(ListIterator<T> begin, ListIterator<T> end)
        where T:IEquatable<T>
    {
        return Unique((ForwardIterator<T>)begin, (ForwardIterator<T>)end) as ListIterator<T>;
    }
    
    public static ForwardIterator<T> Unique<T>(ForwardIterator<T> begin, ForwardIterator<T> end, IEqualityComparer<T> comp)
    {
        if(begin.Equals(end))
            return null;
            
        ForwardIterator<T> firstMatch = AdjacentFind(begin, end, comp) as ForwardIterator<T>;
        if(firstMatch != null)
        {
            return UniqueCopy(firstMatch, end, firstMatch, comp) as ForwardIterator<T>; 
        }
        return null;
    }

    public static RandomAccessIterator<T> Unique<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end, IEqualityComparer<T> comp)
    {
        return Unique((ForwardIterator<T>)begin, (ForwardIterator<T>) end, comp) as RandomAccessIterator<T>;
    }

    public static ListIterator<T> Unique<T>(ListIterator<T> begin, ListIterator<T> end, IEqualityComparer<T> comp)
    {
        return Unique((ForwardIterator<T>)begin, (ForwardIterator<T>) end, comp) as ListIterator<T>;
    }

    public static ForwardIterator<T> Unique<T>(ForwardIterator<T> begin, ForwardIterator<T> end, Functional.BinaryPredicate<T> op)
    {
        if(begin.Equals(end))
            return null;
            
        ForwardIterator<T> firstMatch = AdjacentFind(begin, end, op) as ForwardIterator<T>;
        if(firstMatch != null)
        {
            return UniqueCopy(firstMatch, end, firstMatch, op) as ForwardIterator<T>; 
        }
        return null;
    }

    public static RandomAccessIterator<T> Unique<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end, Functional.BinaryPredicate<T> op)
    {
        return Unique((ForwardIterator<T>)begin, (ForwardIterator<T>)end, op) as RandomAccessIterator<T>;
    }

    public static ListIterator<T> Unique<T>(ListIterator<T> begin, ListIterator<T> end, Functional.BinaryPredicate<T> op)
    {
        return Unique((ForwardIterator<T>)begin, (ForwardIterator<T>)end, op) as ListIterator<T>;
    }


    public static ListIterator<T> Unique<T>(IList<T> list)
        where T:IEquatable<T>
    {
        return Unique(IteratorUtil.Begin(list), IteratorUtil.End(list));
    }

    public static ListIterator<T> Unique<T>(IList<T> list,  IEqualityComparer<T> comp)
    {
        return Unique(IteratorUtil.Begin(list), IteratorUtil.End(list), comp);
    }

    public static ListIterator<T> Unique<T>(IList<T> list, Functional.BinaryPredicate<T> op)
    {
        return Unique(IteratorUtil.Begin(list), IteratorUtil.End(list), op);
    }
}
}