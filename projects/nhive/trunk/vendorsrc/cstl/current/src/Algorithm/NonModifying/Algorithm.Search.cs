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
    public static ForwardIterator<T> Search<T>(ForwardIterator<T> begin, ForwardIterator<T> end, 
                                               ForwardIterator<T> searchBegin, ForwardIterator<T> searchEnd)
        where T: IEquatable<T>
    {
        return Search(begin, end, searchBegin, searchEnd, EqualityComparer<T>.Default);
#if NEVER
        if(begin.Equals(end))
            return null;
        
        if(searchBegin.Equals(searchEnd))
            return null;
            
        begin = IteratorUtil.Clone(begin);
        searchBegin = IteratorUtil.Clone(searchBegin);
        
        if(IteratorUtil.IsOneElementRange(searchBegin, searchEnd))
        {
            return Find(begin, end, searchBegin.Read());
        }
        
        T firstSearchElement = searchBegin.Read();
        while(!begin.Equals(end))
        {
            begin = Find(begin,end, firstSearchElement);
            if(begin == null)
                return null;
                
            ForwardIterator<T> iter = IteratorUtil.Clone(begin);
            iter.MoveNext();
            if(iter.Equals(end))
                return null;
                
            ForwardIterator<T> searchIter = IteratorUtil.Clone(searchBegin);
            searchIter.MoveNext();
            
            while(iter.Read().Equals(searchIter.Read()))
            {
                searchIter.MoveNext();
                if(searchIter.Equals(searchEnd))
                {
                    return begin;
                }
                
                iter.MoveNext();
                if(iter.Equals(end))
                    return null;
            }
            
            begin.MoveNext();
        }
        return null;
#endif
    }

    public static ForwardIterator<T> Search<T>(ForwardIterator<T> begin, ForwardIterator<T> end, 
                                               ForwardIterator<T> searchBegin, ForwardIterator<T> searchEnd,
                                               IEqualityComparer<T> comparer)
    {
        if(begin.Equals(end))
            return null;
        
        if(searchBegin.Equals(searchEnd))
            return null;
            
        begin = IteratorUtil.Clone(begin);
        searchBegin = IteratorUtil.Clone(searchBegin);
        
        if(IteratorUtil.IsOneElementRange(searchBegin, searchEnd))
        {
            return Find(begin, end, searchBegin.Read(), comparer);
        }
        
        T firstSearchElement = searchBegin.Read();
        while(!begin.Equals(end))
        {
            begin = Find(begin,end, firstSearchElement, comparer);
            if(begin == null)
                return null;
                
            ForwardIterator<T> iter = IteratorUtil.Clone(begin);
            iter.MoveNext();
            if(iter.Equals(end))
                return null;
                
            ForwardIterator<T> searchIter = IteratorUtil.Clone(searchBegin);
            searchIter.MoveNext();
            
            while(comparer.Equals(iter.Read(), searchIter.Read()))
            {
                searchIter.MoveNext();
                if(searchIter.Equals(searchEnd))
                {
                    return begin;
                }
                
                iter.MoveNext();
                if(iter.Equals(end))
                    return null;
            }
            
            begin.MoveNext();
        }
        return null;
    }
    
    public static ForwardIterator<T> Search<T>(ForwardIterator<T> begin, ForwardIterator<T> end, 
                                               ForwardIterator<T> searchBegin, ForwardIterator<T> searchEnd,
                                               Functional.BinaryPredicate<T> func)
    {
        if(begin.Equals(end))
            return null;

        if(searchBegin.Equals(searchEnd))
            return null;

        begin = IteratorUtil.Clone(begin);
        searchBegin = IteratorUtil.Clone(searchBegin);
        T firstSearchElement = searchBegin.Read();

        if(IteratorUtil.IsOneElementRange(searchBegin, searchEnd))
        {
            return FindIf(begin, end, Functional.Bind2ndPred(func, firstSearchElement));
        }

        while(!begin.Equals(end))
        {
            begin = FindIf(begin, end, Functional.Bind2ndPred(func, firstSearchElement));
            if(begin == null)
                return null;
                
            ForwardIterator<T> iter = IteratorUtil.Clone(begin);
            iter.MoveNext();
            if(iter.Equals(end))
                return null;
                
            ForwardIterator<T> searchIter = IteratorUtil.Clone(searchBegin);
            searchIter.MoveNext();
            
            while(func(iter.Read(), searchIter.Read()))
            {
                searchIter.MoveNext();
                if(searchIter.Equals(searchEnd))
                {
                    return begin;
                }
                
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