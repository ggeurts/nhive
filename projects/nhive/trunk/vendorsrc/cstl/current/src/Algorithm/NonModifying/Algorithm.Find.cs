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
    public static I Find<I,T>(I begin, I end, T value)
        where T: IEquatable<T>
        where I: class, InputIterator<T> 
    {
        for(begin=IteratorUtil.Clone(begin); !begin.Equals(end); begin.MoveNext())
        {
            if(value.Equals(begin.Read()))
                return begin;
        }
        return null;
    }
#if NEVER
    public static InputIterator<T> Find<T>(InputIterator<T> begin, InputIterator<T> end, T value)
        where T: IEquatable<T>
    {
        for(; !begin.Equals(end); begin.MoveNext())
        {
            if(value.Equals(begin.Read()))
                return begin;
        }
        return null;
    }
#endif

    public static ListIterator<T> Find<T>(IList<T> list, T value)
        where T: IEquatable<T>
    {
        return Find(IteratorUtil.Begin(list), IteratorUtil.End(list), value);
    }

    public static I Find<I,T>(I begin, I end, T t, IEqualityComparer<T> comparer)
        where I: class, InputIterator<T> 
    {
        for(begin=IteratorUtil.Clone(begin); !begin.Equals(end); begin.MoveNext())
        {
            if(comparer.Equals(t, begin.Read()))
                return begin;
        }
        
        return null;
    }

    public static ListIterator<T> Find<T>(IList<T> list, T value, IEqualityComparer<T> comparer)
    {
        return Find(IteratorUtil.Begin(list), IteratorUtil.End(list), value, comparer);
    }

    // I am not real fond of these next two methods. It is desirable to have a find that can work 
    // on any enumerable object. However, returning an IEnumerator doesn't seem right. Maybe we 
    // should return an InputIterator that wraps IEnumerator? Unfortunately, enumerators cannot be
    // compared, so that would result in a broken InputIterator implementation.
    public static IEnumerator<T> Find<T>(IEnumerable<T> enumerable, T value)
        where T: IEquatable<T>
    {
        IEnumerator<T> enumerator = enumerable.GetEnumerator();
        while(enumerator.MoveNext())
        {
            T t  = enumerator.Current;
            if(value.Equals(t))
                return enumerator;
        }
        return null;
    } 
    
    public static IEnumerator<T> Find<T>(IEnumerable<T> enumerable, T value, IEqualityComparer<T> comparer)
        where T: IComparable<T>
    {
        IEnumerator<T> enumerator = enumerable.GetEnumerator();
        while(enumerator.MoveNext())
        {
            T t  = enumerator.Current;
            if(comparer.Equals(value, t))
                return enumerator;
        }
        
        return null;
    }
}
}