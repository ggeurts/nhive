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

    public static void ReplaceCopy<T>(InputIterator<T> begin, InputIterator<T> end, OutputIterator<T> dest, T oldValue, T newValue)
        where T:IEquatable<T>
    {
        ReplaceCopy(begin, end, dest, oldValue, newValue, EqualityComparer<T>.Default);
    }

    public static void ReplaceCopy<T>(InputIterator<T> begin, InputIterator<T> end, OutputIterator<T> dest, T oldValue, T newValue, IEqualityComparer<T> comparer)
    {
        for(begin = IteratorUtil.Clone(begin), dest = IteratorUtil.Clone(dest); !begin.Equals(end); begin.MoveNext(),dest.MoveNext())
        {
            T srcValue = begin.Read();
            if(comparer.Equals(oldValue, srcValue))
                dest.Write(newValue);
            else
                dest.Write(srcValue);
        }
    }

    public static void ReplaceCopy<T>(IList<T> list, OutputIterator<T> dest, T oldValue, T newValue)
        where T:IEquatable<T>
    {
        ReplaceCopy(IteratorUtil.Begin(list), IteratorUtil.End(list), dest, oldValue, newValue);
    }

    public static void ReplaceCopy<T>(IList<T> list, OutputIterator<T> dest, T oldValue, T newValue, IEqualityComparer<T> comparer)
        where T:IEquatable<T>
    {
        ReplaceCopy(IteratorUtil.Begin(list), IteratorUtil.End(list), dest, oldValue, newValue, comparer);
    }

    // TODO: consider adding list versions that can start at a given src index, and copy to a given end index.
    // also
    
    public static void ReplaceCopy<T>(IList<T> list, IList<T> dest, T oldValue, T newValue)
        where T:IEquatable<T>
    {
        ReplaceCopy(IteratorUtil.Begin(list), IteratorUtil.End(list), IteratorUtil.Begin(dest), oldValue, newValue);
    }

    public static void ReplaceCopy<T>(IList<T> list, IList<T> dest,T oldValue, T newValue, IEqualityComparer<T> comparer)
    {
        ReplaceCopy(IteratorUtil.Begin(list), IteratorUtil.End(list), IteratorUtil.Begin(dest), oldValue, newValue, comparer);
    }
}
}