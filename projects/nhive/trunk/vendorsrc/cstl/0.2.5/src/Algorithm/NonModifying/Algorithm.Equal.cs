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

    public static bool Equal<T>(InputIterator<T> begin, InputIterator<T> end, InputIterator<T> cmpBegin)
        where T: IEquatable<T>
    {
        return Equal(begin, end, cmpBegin, EqualityComparer<T>.Default);
#if NEVER
        for(; !begin.Equals(end); begin.MoveNext(), cmpBegin.MoveNext())
        {
            T t1 = begin.Read();
            T t2 = cmpBegin.Read();
            if(!t1.Equals(t2))
                return false;
        }
        
        return true;
#endif
    }
    
    public static bool Equal<T>(InputIterator<T> begin, InputIterator<T> end,  InputIterator<T> cmpBegin, IEqualityComparer<T> comparer)
    {
        begin = IteratorUtil.Clone(begin);
        cmpBegin = IteratorUtil.Clone(cmpBegin);
        
        for(; !begin.Equals(end); begin.MoveNext(), cmpBegin.MoveNext())
        {
            T t1 = begin.Read();
            T t2 = cmpBegin.Read();
            if(!comparer.Equals(t1,t2))
                return false;
        }
        
        return true;    
    }
    
#if NEVER
// Not sure why I coded these. The STL doesn't have an analgous version of these    
    public static bool Equal<T>(InputIterator<T> begin, InputIterator<T> end, InputIterator<T> cmpBegin, InputIterator<T>cmpEnd)
        where T: IEquatable<T>
    {
        return Equal(begin, end, cmpBegin, cmpEnd, new Functional.EqualComparer<T>());
#if NEVER
        for(; !begin.Equals(end); begin.MoveNext(), cmpBegin.MoveNext())
        {
            T t1 = begin.Read();
            T t2 = cmpBegin.Read();
            if(!t1.Equals(t2))
                return false;
        }
        
        return true;
#endif
    }
    
    public static bool Equal<T>(InputIterator<T> begin, InputIterator<T> end,  InputIterator<T> cmpBegin, InputIterator<T> cmpEnd, IComparer<T> comparer)
    {
        for(; !begin.Equals(end) && !cmpBegin.Equals(cmpEnd); begin.MoveNext(), cmpBegin.MoveNext())
        {
            T t1 = begin.Read();
            T t2 = cmpBegin.Read();
            if(comparer.Compare(t1,t2)!=0)
                return false;
        }
        
        if(begin.Equals(end) && cmpBegin.Equals(cmpEnd))
            return true;
        else
            return false;
    }

    public static bool Equal<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end, RandomAccessIterator<T> cmpBegin, RandomAccessIterator<T>cmpEnd)
        where T: IEquatable<T>
    {
        return Equal(begin, end, cmpBegin, cmpEnd, new Functional.EqualComparer<T>());
    }
    
    public static bool Equal<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end,  RandomAccessIterator<T> cmpBegin, RandomAccessIterator<T> cmpEnd, IComparer<T> comparer)
    {
        if(IteratorUtil.Distance(begin, end) != IteratorUtil.Distance(cmpBegin, cmpEnd))
            return false;
            
        for(; !begin.Equals(end) && !cmpBegin.Equals(cmpEnd); begin.MoveNext(), cmpBegin.MoveNext())
        {
            T t1 = begin.Read();
            T t2 = cmpBegin.Read();
            if(comparer.Compare(t1,t2)!=0)
                return false;
        }
        
        return true;
    }
#endif
    
    public static bool Equal<T>(IEnumerable<T> enumerable1, IEnumerable<T> enumerable2)
        where T: IEquatable<T>
    {
        return Equal(enumerable1, enumerable2, EqualityComparer<T>.Default);
    }

    public static bool Equal<T>(IEnumerable<T> enumerable1, IEnumerable<T> enumerable2, IEqualityComparer<T> comparer)
    {
        IEnumerator<T> e1 = enumerable1.GetEnumerator();
        IEnumerator<T> e2 = enumerable2.GetEnumerator();
        
        bool b1,b2;
        for(b1=e1.MoveNext(), b2=e2.MoveNext(); b1 && b2; b1=e1.MoveNext(), b2=e2.MoveNext())
        {
            T t1 = e1.Current;
            T t2 = e2.Current;
            if(!comparer.Equals(t1,t2))
                return false;
        }
        
        // Ensure enumerators finished at the same time.
        if(b1==b2)
            return true;

        return false;
    }
}
}