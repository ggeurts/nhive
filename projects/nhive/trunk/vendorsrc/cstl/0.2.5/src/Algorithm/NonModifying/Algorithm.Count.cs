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

    public static int Count<T>(InputIterator<T> begin, InputIterator<T> end, T value)
        where T: IEquatable<T>
    {
        return Count(IteratorUtil.CreateEnumerator(begin, end), value);
    
#if NEVER
        int count = 0;
        while(!begin.Equals(end))
        {
            if(value.Equals(begin.Read()))
                ++count;
            begin.MoveNext();
        }
        
        return count;
#endif
    }
    
    public static int Count<T>(IEnumerable<T> enumerable, T value)
        where T: IEquatable<T>
    {
        int count = 0;
        foreach(T t  in enumerable)
        {
            if(value.Equals(t))
                ++count;
        }
        
        return count;
    }
    

    public static int Count<T>(InputIterator<T> begin, InputIterator<T> end, T t, IComparer<T> comparer)
    {
        return Count(IteratorUtil.CreateEnumerator(begin, end), t, comparer);
#if NEVER
        int count = 0;
        while(!begin.Equals(end))
        {
            if(comparer.Compare(t, begin.Read())==0)
                ++count;
            begin.MoveNext();
        }
        
        return count;
#endif
    }
    
    public static int Count<T>(IEnumerable<T> enumerable, T value, IComparer<T> comparer)
    {
        int count = 0;
        foreach(T t  in enumerable)
        {
            if(comparer.Compare(value, t)==0)
                ++count;
        }
        
        return count;
    }    
}
}