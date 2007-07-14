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
    // TODO: FindEnd matches the C++ name, but wouldn't SearchEnd or SearchLast be more consistent?

    public static ForwardIterator<T> FindEnd<T>(ForwardIterator<T> begin, ForwardIterator<T> end, 
                                                ForwardIterator<T> searchBegin, ForwardIterator<T> searchEnd)
        where T: IEquatable<T>
    {
        return FindEnd(begin, end, searchBegin, searchEnd, EqualityComparer<T>.Default);
    }

    public static ForwardIterator<T> FindEnd<T>(ForwardIterator<T> begin, ForwardIterator<T> end, 
                                                ForwardIterator<T> searchBegin, ForwardIterator<T> searchEnd,
                                                IEqualityComparer<T> comparer)
    {
        if(begin.Equals(end))
            return null;
        
        if(searchBegin.Equals(searchEnd))
            return null;

        ForwardIterator<T> oldResult = null;
        ForwardIterator<T> result = begin;
        while(true)
        {
            result = Search(result, end, searchBegin, searchEnd, comparer);
            if(result == null)
                return oldResult;
            else
            {
                oldResult = IteratorUtil.Clone(result);
                result.MoveNext();
            }
        }
    }

    public static ForwardIterator<T> FindEnd<T>(ForwardIterator<T> begin, ForwardIterator<T> end, 
                                                ForwardIterator<T> searchBegin, ForwardIterator<T> searchEnd,
                                                Functional.BinaryPredicate<T> func)
    {
        if(begin.Equals(end))
            return null;

        if(searchBegin.Equals(searchEnd))
            return null;

        ForwardIterator<T> oldResult = null;
        ForwardIterator<T> result = begin;
        while(true)
        {
            result = Search(result, end, searchBegin, searchEnd, func);
            if(result == null)
                return oldResult;
            else
            {
                oldResult = IteratorUtil.Clone(result);
                result.MoveNext();
            }
        }
    }

}
}