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
    public static T Accumulate<T>(InputIterator<T> begin, InputIterator<T> end, T initialValue, Functional.BinaryFunction<T,T,T> func)
    {
        T result = initialValue;
        for(begin=IteratorUtil.Clone(begin); !begin.Equals(end);begin.MoveNext())
        {
            result = func(result, begin.Read());
        }
        return result;
    }
    
    public static T Accumulate<T>(IEnumerable<T> enumerable, T initialValue, Functional.BinaryFunction<T, T, T> func)
    {
        T result = initialValue;
        foreach(T t in enumerable)
        {
            result = func(result, t);
        } 
        return result;
    }
}
}

