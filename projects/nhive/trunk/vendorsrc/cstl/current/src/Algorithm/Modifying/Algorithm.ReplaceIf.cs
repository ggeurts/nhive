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

    public static void ReplaceIf<T>(ForwardIterator<T> begin, ForwardIterator<T> end,  T newValue, Functional.UnaryPredicate<T> func )
        where T:IEquatable<T>
    {
        for(begin = IteratorUtil.Clone(begin); !begin.Equals(end); begin.MoveNext())
        {
            if(func(begin.Read()))
                begin.Write(newValue);
        }
    }

    public static void ReplaceIf<T>(IList<T> list, T newValue, Functional.UnaryPredicate<T> func)
        where T:IEquatable<T>
    {
        ReplaceIf(IteratorUtil.Begin(list), IteratorUtil.End(list), newValue, func);
    }
}
}