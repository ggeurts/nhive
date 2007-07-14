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

    public static OutputIterator<T> Copy<T>(InputIterator<T> begin, InputIterator<T> end, OutputIterator<T> target)
    {
        for(begin=IteratorUtil.Clone(begin), target=IteratorUtil.Clone(target); 
            !begin.Equals(end); 
            begin.MoveNext(), target.MoveNext())
        {
            target.Write(begin.Read());
        }
        return target;
    }

    public static OutputIterator<T>  Copy<T>(IEnumerable<T> enumerable, OutputIterator<T> target)
    {
        target = IteratorUtil.Clone(target);
        foreach(T t  in enumerable)
        {
            target.Write(t);
            target.MoveNext();
        }
        return target;
    }

    public static void Copy<T>(IEnumerable<T> enumerable, IList<T> target)
    {
        Copy(enumerable, IteratorUtil.Begin(target));
    }

    public static void Copy<T>(InputIterator<T> begin, InputIterator<T> end, IList<T> target)
    {
        Copy(begin, end, IteratorUtil.Begin(target));
    }

    public static void Copy<T>(IList<T>source, int startIndex, IList<T> dest, int destIndex)
    {
        Copy(IteratorUtil.Begin(source).OffsetBy(startIndex), IteratorUtil.End(source),
             IteratorUtil.Begin(dest).OffsetBy(destIndex));
    }
}
}