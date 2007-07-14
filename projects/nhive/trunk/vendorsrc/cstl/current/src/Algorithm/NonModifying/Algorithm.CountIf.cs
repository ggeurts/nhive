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
    public static int CountIf<T>(InputIterator<T> begin, InputIterator<T> end, Functional.UnaryPredicate<T> func)
    {
        return CountIf(IteratorUtil.CreateEnumerator(begin, end), func);

#if NEVER    
        int count = 0;
        while(!begin.Equals(end))
        {
            if(func(begin.Read()))
                ++count;
            begin.MoveNext();
        }
        
        return count;
#endif
    }
    
    public static int CountIf<T>(IEnumerable<T> enumerable, Functional.UnaryPredicate<T> func)
    {
        int count = 0;
        foreach(T t in enumerable)
        {
            if(func(t))
                ++count;
        }
        
        return count;
    }

}
}