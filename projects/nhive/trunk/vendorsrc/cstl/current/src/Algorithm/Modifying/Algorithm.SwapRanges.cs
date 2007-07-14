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
    public static void SwapRanges<T>(ForwardIterator<T> begin, ForwardIterator<T> end, ForwardIterator<T> dest)
    {
        for(begin = IteratorUtil.Clone(begin), dest = IteratorUtil.Clone(dest); !begin.Equals(end); begin.MoveNext(), dest.MoveNext())
        {
            IteratorUtil.Swap(begin, dest);
        }
    }
}
}