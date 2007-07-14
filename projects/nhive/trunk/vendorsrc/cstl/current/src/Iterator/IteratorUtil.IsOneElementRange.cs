//-----------------------------------------------------------------------------
// CSTL
// Copyright (C) 2006 by Harold Howe hhjunk@mchsi.com
// http://sourceforge.net/projects/cstl

using System;
using System.Collections.Generic;

namespace CSTL.Iterator
{

public static partial class IteratorUtil
{
    public static bool IsOneElementRange<T>(InputIterator<T> first, InputIterator<T> last)
    {
        first = IteratorUtil.Clone(first);
        first.MoveNext();
        return first.Equals(last);
    }

    public static bool IsOneElementRange<T>(RandomAccessIterator<T> first, RandomAccessIterator<T> last)
    {
        return Distance(first, last)==1;
    }
}

}