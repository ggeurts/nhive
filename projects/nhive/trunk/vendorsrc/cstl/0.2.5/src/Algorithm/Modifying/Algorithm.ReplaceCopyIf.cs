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

    public static void ReplaceCopyIf<T>(InputIterator<T> begin, InputIterator<T> end, OutputIterator<T> dest, Functional.UnaryPredicate<T>func, T newValue )
    {
        for(begin = IteratorUtil.Clone(begin), dest = IteratorUtil.Clone(dest); !begin.Equals(end); begin.MoveNext(), dest.MoveNext())
        {
            if(func(begin.Read()))
                dest.Write(newValue);
        }
    }

    public static void ReplaceCopyIf<T>(IList<T> list, IList<T> dest, Functional.UnaryPredicate<T>func, T newValue)
    {
        ReplaceCopyIf(IteratorUtil.Begin(list), IteratorUtil.End(list), IteratorUtil.Begin(dest), func, newValue);
    }
}
}