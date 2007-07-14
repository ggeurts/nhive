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

    public static void Fill<T>(ForwardIterator<T> begin, ForwardIterator<T> end, T value)
    {
        for(begin = IteratorUtil.Clone(begin); !begin.Equals(end); begin.MoveNext())
        {
            begin.Write(value);
        }
    }
    
    public static void Fill<T>(IList<T> list, T value)
    {
        Fill(IteratorUtil.Begin(list), IteratorUtil.End(list), value);
    }
}
}