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
    
    public static IEnumerable<T> CreateEnumerator<T>(InputIterator<T> begin, InputIterator<T> end)
    {
        begin = IteratorUtil.Clone(begin);
        while(!begin.Equals(end))
        {
            yield return begin.Read();
            begin.MoveNext();
        }
    }
    
    public static IEnumerable<T> CreateEnumerator<T>(uint count, T t)
    {
        for(uint i = 0; i < count; ++i)
        {
            yield return t;
        }
    }
}
}
