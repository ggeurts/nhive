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
    public static OutputIterator<T> CopyIf<T>(InputIterator<T> begin, InputIterator<T> end, OutputIterator<T> dest, Functional.UnaryPredicate<T> func)
    {
        for(begin=IteratorUtil.Clone(begin), dest=IteratorUtil.Clone(dest); !begin.Equals(end); begin.MoveNext())
        {
            T t = begin.Read();
            if(func(t))
            {
                dest.Write(t);
                dest.MoveNext(); 
            }
        }
        return dest;
    }

    public static ListIterator<T> CopyIf<T>(InputIterator<T> begin, InputIterator<T> end, ListIterator<T> dest, Functional.UnaryPredicate<T> func)
    {
        return CopyIf(begin, end, (OutputIterator<T>)dest, func) as ListIterator<T>;
    }

    public static RandomAccessIterator<T> CopyIf<T>(InputIterator<T> begin, InputIterator<T> end, RandomAccessIterator<T> dest, Functional.UnaryPredicate<T> func)
    {
        return CopyIf(begin, end, (OutputIterator<T>)dest, func) as RandomAccessIterator<T>;
    }

    public static ForwardIterator<T> CopyIf<T>(InputIterator<T> begin, InputIterator<T> end, ForwardIterator<T> dest, Functional.UnaryPredicate<T> func)
    {
        return CopyIf(begin, end, (OutputIterator<T>)dest, func) as ForwardIterator<T>;
    }

    public static OutputIterator<T> CopyIf<T>(IList<T> source, OutputIterator<T> dest, Functional.UnaryPredicate<T> func)
    {
        return CopyIf(IteratorUtil.Begin(source), IteratorUtil.End(source), dest, func);
    }

    public static ListIterator<T> CopyIf<T>(IList<T> source, ListIterator<T> dest, Functional.UnaryPredicate<T> func)
    {
        return CopyIf(source, (OutputIterator<T>)dest, func) as ListIterator<T>;
    }

    public static RandomAccessIterator<T> CopyIf<T>(IList<T> source, RandomAccessIterator<T> dest, Functional.UnaryPredicate<T> func)
    {
        return CopyIf(source, (OutputIterator<T>)dest, func) as RandomAccessIterator<T>;
    }

    public static ForwardIterator<T> CopyIf<T>(IList<T> source, ForwardIterator<T> dest, Functional.UnaryPredicate<T> func)
    {
        return CopyIf(source, (OutputIterator<T>)dest, func) as ForwardIterator<T>;
    }

    public static ListIterator<T> CopyIf<T>(IList<T> source, IList<T> dest, Functional.UnaryPredicate<T> func)
    {
        return CopyIf(IteratorUtil.Begin(source), IteratorUtil.End(source), IteratorUtil.Begin(dest), func);
    }
}
}
