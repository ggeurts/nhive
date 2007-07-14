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
    public static ForwardIterator<T> SearchN<T>(ForwardIterator<T> begin, ForwardIterator<T> end, 
                                                int count, T value)
        where T: IEquatable<T>
    {
        return SearchN(begin, end, count, value, EqualityComparer<T>.Default);
    }

    public static ForwardIterator<T> SearchN<T>(ForwardIterator<T> begin, ForwardIterator<T> end, 
                                                int count, T value, IEqualityComparer<T> comparer)
    {
        if(begin.Equals(end) || (count <= 0))
            return null;
            
        if(count == 1)
            return Find(begin, end, value, comparer);

        begin = IteratorUtil.Clone(begin);
        while(!begin.Equals(end))
        {
            begin = Find(begin,end, value, comparer);
            if(begin == null)
                return null;

            ForwardIterator<T> iter = IteratorUtil.Clone(begin);
            iter.MoveNext();
            if(iter.Equals(end))
                return null;

            int matchCount = count-1;
            while(comparer.Equals(iter.Read(), value))
            {
                --matchCount;
                if(matchCount==0)
                    return begin;

                iter.MoveNext();
                if(iter.Equals(end))
                    return null;
            }
            
            begin.MoveNext();
        }
        return null;
    }
    
    public static ForwardIterator<T> SearchN<T>(ForwardIterator<T> begin, ForwardIterator<T> end, 
                                                int count, Functional.UnaryPredicate<T> func)
    {
        if(begin.Equals(end) || (count <= 0))
            return null;
            
        if(count == 1)
            return FindIf(begin, end, func);

        begin = IteratorUtil.Clone(begin);
        while(!begin.Equals(end))
        {
            begin = FindIf(begin, end, func);
            if(begin == null)
                return null;
                
            ForwardIterator<T> iter = IteratorUtil.Clone(begin);
            iter.MoveNext();
            if(iter.Equals(end))
                return null;

            int matchCount = count-1;
            while(func(iter.Read()))
            {
                --matchCount;
                if(matchCount==0)
                    return begin;

                iter.MoveNext();
                if(iter.Equals(end))
                    return null;
            }
            
            begin.MoveNext();
        }
        return null;
    }
}
}