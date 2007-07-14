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
    public static void ForEach<T>(InputIterator<T> begin, InputIterator<T> end, Functional.UnaryVoidFunction<T> func)
    {
        for(begin=IteratorUtil.Clone(begin); !begin.Equals(end); begin.MoveNext())
        {
            func(begin.Read());
        }
    }

    public static void ForEach<T>(IEnumerable<T> enumerable, Functional.UnaryVoidFunction<T> func)
    {
        foreach(T t in enumerable)
            func(t);
    }
}
}