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
    public static int Distance<T>(InputIterator<T> first, InputIterator<T> last)
    {
        first = IteratorUtil.Clone(first);
        int result = 0;
        while(!first.Equals(last))
        {
            first.MoveNext();
            ++result;
        }
        return result;
    }

    public static int Distance<T>(RandomAccessIterator<T> first, RandomAccessIterator<T> last)
    {
        return last.DistanceFrom(first);
    }
}

}