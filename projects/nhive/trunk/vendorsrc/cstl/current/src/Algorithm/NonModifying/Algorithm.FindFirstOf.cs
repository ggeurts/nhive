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
    public static ForwardIterator<T> FindFirstOf<T>(ForwardIterator<T> begin, ForwardIterator<T>end, 
                                                    ForwardIterator<T> searchBegin, ForwardIterator<T> searchEnd)
        where T: IEquatable<T>
    {
        return FindFirstOf(begin, end, searchBegin, searchEnd, EqualityComparer<T>.Default);
    }

    public static ForwardIterator<T> FindFirstOf<T>(ForwardIterator<T> begin, ForwardIterator<T>end, 
                                                    ForwardIterator<T> searchBegin, ForwardIterator<T> searchEnd,
                                                    IEqualityComparer<T> comparer)
    {
        begin = IteratorUtil.Clone(begin);
        for(; !begin.Equals(end); begin.MoveNext())
        {
            for(ForwardIterator<T> searchIter = IteratorUtil.Clone(searchBegin); 
                !searchIter.Equals(searchEnd); searchIter.MoveNext())
            {
                if(comparer.Equals(begin.Read(), searchIter.Read()))
                    return begin;
            }
        }
        return null;
    }

    public static ForwardIterator<T> FindFirstOf<T>(ForwardIterator<T> begin, ForwardIterator<T>end, 
                                                    ForwardIterator<T> searchBegin, ForwardIterator<T> searchEnd,
                                                    Functional.BinaryPredicate<T> func)
    {
        begin = IteratorUtil.Clone(begin);
        for(; !begin.Equals(end); begin.MoveNext())
        {
            for(ForwardIterator<T> searchIter = IteratorUtil.Clone(searchBegin); 
                !searchIter.Equals(searchEnd); searchIter.MoveNext())
            {
                if(func(begin.Read(), searchIter.Read()))
                    return begin;
            }
        }
        return null;
    }

}
}