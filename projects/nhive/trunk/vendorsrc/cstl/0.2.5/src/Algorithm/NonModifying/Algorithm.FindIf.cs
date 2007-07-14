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
    public static InputIterator<T> FindIf<T>(InputIterator<T> begin, InputIterator<T> end, Functional.UnaryPredicate<T> func)
    {
        for(begin=IteratorUtil.Clone(begin); !begin.Equals(end); begin.MoveNext())
        {
            if(func(begin.Read()))
                return begin;
        }
        return null;
    }

    public static ForwardIterator<T> FindIf<T>(ForwardIterator<T> begin, ForwardIterator<T> end, Functional.UnaryPredicate<T> func)
    {
        return (ForwardIterator<T>) FindIf((InputIterator<T>)begin, (InputIterator<T>)end, func);
    }

    public static RandomAccessIterator<T> FindIf<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end, Functional.UnaryPredicate<T> func)
    {
        return (RandomAccessIterator<T>) FindIf((InputIterator<T>)begin, (InputIterator<T>)end, func);
    }

    public static ListIterator<T> FindIf<T>(ListIterator<T> begin, ListIterator<T> end, Functional.UnaryPredicate<T> func)
    {
        return (ListIterator<T>) FindIf((InputIterator<T>)begin, (InputIterator<T>)end, func);
    }

    public static ListIterator<T> FindIf<T>(IList<T> list, Functional.UnaryPredicate<T> func)
    {
        return FindIf(IteratorUtil.Begin(list), IteratorUtil.End(list), func);
    }
    
    public static IEnumerator<T> FindIf<T>(IEnumerable<T> enumerable, Functional.UnaryPredicate<T> func)
    {
        IEnumerator<T> enumerator = enumerable.GetEnumerator();
        while(enumerator.MoveNext())
        {
            T t  = enumerator.Current;
            if(func(t))
                return enumerator;
        }
        return null;
    }
}
}