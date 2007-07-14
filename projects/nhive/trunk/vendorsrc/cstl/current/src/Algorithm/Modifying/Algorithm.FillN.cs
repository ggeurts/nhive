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
    public static OutputIterator<T> FillN<T>(OutputIterator<T> begin, int count, T value)
    {
        for(begin = IteratorUtil.Clone(begin); count>0; --count, begin.MoveNext())
        {
            begin.Write(value);
        }
        return begin;
    }

    public static ListIterator<T> FillN<T>(IList<T> list, int count, T value)
    {
        return (ListIterator<T>) FillN(IteratorUtil.Begin(list), count, value);
    }

    public static ListIterator<T> FillN<T>(IList<T> list, int startIndex, int count, T value)
    {
        ListIterator<T> begin = IteratorUtil.Begin(list);
        begin.OffsetBy(startIndex);
        return (ListIterator<T>) FillN(begin, count, value);
    }
}
}