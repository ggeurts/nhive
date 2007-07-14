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

    public static void Replace<T>(ForwardIterator<T> begin, ForwardIterator<T> end, T oldValue, T newValue)
        where T:IEquatable<T>
    {
        for(begin = IteratorUtil.Clone(begin); !begin.Equals(end); begin.MoveNext())
        {
            if(oldValue.Equals(begin.Read()))
                begin.Write(newValue);
        }
    }

    public static void Replace<T>(ForwardIterator<T> begin, ForwardIterator<T> end, T oldValue, T newValue, IEqualityComparer<T> comparer)
    {
        for(begin = IteratorUtil.Clone(begin); !begin.Equals(end); begin.MoveNext())
        {
            if(comparer.Equals(oldValue, begin.Read()))
                begin.Write(newValue);
        }
    }

    public static void Replace<T>(IList<T> list, T oldValue, T newValue)
        where T:IEquatable<T>
    {
        Replace(IteratorUtil.Begin(list), IteratorUtil.End(list), oldValue, newValue);
    }

    public static void Replace<T>(IList<T> list, T oldValue, T newValue, IEqualityComparer<T> comparer)
    {
        Replace(IteratorUtil.Begin(list), IteratorUtil.End(list), oldValue, newValue, comparer);
    }
}
}