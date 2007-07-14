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
    // TODO : Consider adding overloads that take a .NET Comparison<T> delegate. In fact, some of the other XXXXIf 
    // algorithms that take a binary predictate should probably have overloads that take a Comparison<T>.

    public static void Sort<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end)
        where T:IComparable<T>
    {
        Sort(begin, end, Comparer<T>.Default);
    }

    public static void Sort<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end, IComparer<T> comparer)
    {
        if(begin.Equals(end))
            return;

        // TODO: This is probably the worst sorting algorithm ever written. I'm lazy, and just want to get the interface
        // and tests for this algorithm in place. Implement a real sorting function later.
        //
        // If we are going to do something this lame, it would probably be better to stuff the items one by one into a 
        // sorted contain, and then copy objects back into [begin,end).
        T[] array = new T[end.Position-begin.Position];
        Copy(begin, end, array);
        Array.Sort(array,comparer);
        Copy(array, begin);
    }

#if NEVER
    // It would be nice to have this routine, but resolution based on delegate type doesn't work very well in C#
    public static void Sort<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end, Comparison<T> func)
    {
        if(begin.Equals(end))
            return;
            
        T[] array = new T[end.Position-begin.Position];
        Copy(begin, end, array);
        Array.Sort(array, func);
        Copy(array, begin);
    }
#endif
    
    public static void Sort<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end, Functional.BinaryPredicate<T> func)
    {

        if(begin.Equals(end))
            return;
            
        T[] array = new T[end.Position-begin.Position];
        Copy(begin, end, array);
        Array.Sort(array, new Functional.BinaryPredicateComparison<T>(func).Compare);
        Copy(array, begin);
    }

    // TODO: The IList based sort methods will be needlessly slow if the list is really List<T> or array<T> because the
    // current sort algorithm creates a complete copy before performing the sort. This copy is not necessary if sorting
    // a full List<T> or array<T>. Despite that, it is still best to implement in terms of the iterator versions because
    // eventually, we will have a real sorting algorithm that doesn't incur this overhead.
    public static void Sort<T>(IList<T> list)
        where T:IComparable<T>
    {
        Sort(list, Comparer<T>.Default);
    }

    public static void Sort<T>(IList<T> list, IComparer<T> comparer)
    {
        Sort(IteratorUtil.Begin(list), IteratorUtil.End(list), comparer);
    }

#if NEVER
    public static void Sort<T>(IList<T> list, Comparison<T> func)
    {
        Sort(IteratorUtil.Begin(list), IteratorUtil.End(list), func);
    }
#endif

    public static void Sort<T>(IList<T> list, Functional.BinaryPredicate<T> func)
    {
        Sort(IteratorUtil.Begin(list), IteratorUtil.End(list), func);
    }
}
}
