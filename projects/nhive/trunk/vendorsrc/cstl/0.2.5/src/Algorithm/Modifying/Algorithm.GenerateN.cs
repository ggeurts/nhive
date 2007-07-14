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
    public static void GenerateN<T>(OutputIterator<T> begin, int count, Functional.GenFunction<T> func)
    {
        for(begin = IteratorUtil.Clone(begin); count>0; --count, begin.MoveNext())
        {
            begin.Write(func());
        }
    }
    
    public static void GenerateN<T>(IList<T> list, int count, Functional.GenFunction<T> func)
    {
        GenerateN(IteratorUtil.Begin(list), count, func);
    }
}
}