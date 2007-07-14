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
    public static void RotateCopy<T>(ForwardIterator<T> begin, ForwardIterator<T> middle,
                                     ForwardIterator<T> end, OutputIterator<T> dest)
    {
        Copy(begin, middle , Copy(middle, end, dest));

#if NEVER
        dest = IteratorUtil.Clone(dest);
        ForwardIterator<T> src  = IteratorUtil.Clone(newBegin);
        newBegin = IteratorUtil.Clone(newBegin);
        
        for(; !src.Equals(end); dest.MoveNext(), src.MoveNext())
        {
            dest.Write(src.Read());
        }
        
        src = IteratorUtil.Clone(begin);
        for(; !src.Equals(newBegin); dest.MoveNext(), src.MoveNext())
        {
            dest.Write(src.Read());
        }
#endif
    }
}
}