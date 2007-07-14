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
    public static ForwardIterator<T> RemoveIf<T>(ForwardIterator<T> begin, ForwardIterator<T> end, Functional.UnaryPredicate<T> func)
    {
        ForwardIterator<T> iter = FindIf(begin, end, func);
        if(iter == null)
            return null;

        return RemoveCopyIf(iter, end, iter, func) as ForwardIterator<T>;
    }

    public static RandomAccessIterator<T> RemoveIf<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end, Functional.UnaryPredicate<T> func)
    {
        return RemoveIf((ForwardIterator<T>)begin, (ForwardIterator<T>)end, func) as RandomAccessIterator<T>;
    }

    public static ListIterator<T> RemoveIf<T>(ListIterator<T> begin, ListIterator<T> end, Functional.UnaryPredicate<T> func)
    {
        return RemoveIf((ForwardIterator<T>)begin, (ForwardIterator<T>)end, func) as ListIterator<T>;
    }
}
}
