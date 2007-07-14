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

    public static void Reverse<T>(BidirectionalIterator<T> begin, BidirectionalIterator<T> end)
    {
        end = IteratorUtil.Clone(end);
        begin = IteratorUtil.Clone(begin);

        BidirectionalIterator<T> last = IteratorUtil.Clone(end);
        last.MovePrev();
        for(; !begin.Equals(last) && !begin.Equals(end) ; last.MovePrev(), end.MovePrev(), begin.MoveNext())
        {
            IteratorUtil.Swap(begin, last);
        }
    }

    public static void Reverse<T>(IList<T> list)
    {
        Reverse(IteratorUtil.Begin(list), IteratorUtil.End(list));
    }
}
}