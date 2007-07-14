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

    public static void CopyN<T>(InputIterator<T> begin, uint count, OutputIterator<T> target)
    {
        for(begin = IteratorUtil.Clone(begin), target = IteratorUtil.Clone(target); count>0; --count)
        {
            target.Write(begin.Read());
            begin.MoveNext();
            target.MoveNext();
        }
    }

    public static void CopyN<T>(IEnumerable<T> enumerable, uint count, OutputIterator<T> target)
    {
        if(count==0)
            return;

        target = IteratorUtil.Clone(target);
        foreach(T t  in enumerable)
        {
            target.Write(t);
            target.MoveNext();
            --count;
            if(count==0)
                return;
        }
    }

    //TODO: review the behavior of this routine. Should we append, or override elements in the target collection?
    // The STL overwrites unless you pass an inserter. Array.Copy does the same. So I guess we should too. 
    // Callers must make sure target can hold all the elements
    public static void CopyN<T>(IEnumerable<T> enumerable, uint count, IList<T> target)
    {
        CopyN(enumerable, count, IteratorUtil.Begin(target));
    }

    public static void CopyN<T>(InputIterator<T> begin, uint count, IList<T> target)
    {
        CopyN(begin, count, IteratorUtil.Begin(target));
    }

    public static void CopyN<T>(IList<T>source, int startIndex, uint count, IList<T> dest, int destIndex)
    {
        CopyN(IteratorUtil.Begin(source).OffsetBy(startIndex), count,
             IteratorUtil.Begin(dest).OffsetBy(destIndex));
    }
}
}