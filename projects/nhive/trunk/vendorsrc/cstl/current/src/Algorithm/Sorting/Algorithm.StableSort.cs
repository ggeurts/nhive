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
    // TODO : StableSort should be implemented using a heapsort algorithm. For now, we are implemented in terms of the 
    // Sort algorithm. This is just a stub so I can get the tests in place. I will work out a real implementation later
    // (before version 0.9.0)

    public static void StableSort<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end)
        where T:IComparable<T>
    {
        Sort(begin, end, Comparer<T>.Default);
    }

    public static void StableSort<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end, IComparer<T> comparer)
    {
        Sort(begin, end, comparer);
    }

#if NEVER
    // It would be nice to have this routine, but resolution based on delegate type doesn't work very well in C#
    public static void StableSort<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end, Comparison<T> func)
    {
    }
#endif
    
    public static void StableSort<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end, Functional.BinaryPredicate<T> func)
    {
        Sort(begin, end, func);
    }

    public static void StableSort<T>(IList<T> list)
        where T:IComparable<T>
    {
        Sort(list, Comparer<T>.Default);
    }

    public static void StableSort<T>(IList<T> list, IComparer<T> comparer)
    {
        Sort(IteratorUtil.Begin(list), IteratorUtil.End(list), comparer);
    }

#if NEVER
    public static void StableSort<T>(IList<T> list, Comparison<T> func)
    {
        Sort(IteratorUtil.Begin(list), IteratorUtil.End(list), func);
    }
#endif

    public static void StableSort<T>(IList<T> list, Functional.BinaryPredicate<T> func)
    {
        Sort(IteratorUtil.Begin(list), IteratorUtil.End(list), func);
    }
}
}
