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

    public static void Generate<T>(ForwardIterator<T> begin, ForwardIterator<T> end, Functional.GenFunction<T> func)
    {
        for(begin = IteratorUtil.Clone(begin); !begin.Equals(end); begin.MoveNext())
        {
            begin.Write(func());
        }
    }
    
    public static void Generate<T>(IList<T> list, Functional.GenFunction<T> func)
    {
        Generate(IteratorUtil.Begin(list), IteratorUtil.End(list), func);
    }
}
}