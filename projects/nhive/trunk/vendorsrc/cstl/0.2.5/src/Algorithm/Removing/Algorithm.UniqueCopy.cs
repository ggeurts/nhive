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
    #region Iterator functions
    public static OutputIterator<T> UniqueCopy<T>(InputIterator<T> begin, InputIterator<T> end, OutputIterator<T> dest)
        where T:IEquatable<T>
    {
        return UniqueCopy(begin, end, dest, EqualityComparer<T>.Default);
    }

    public static ForwardIterator<T> UniqueCopy<T>(InputIterator<T> begin, InputIterator<T> end, ForwardIterator<T> dest)
        where T:IEquatable<T>
    {
        return UniqueCopy(begin, end, (OutputIterator<T>)dest) as ForwardIterator<T>;
    }

    public static RandomAccessIterator<T> UniqueCopy<T>(InputIterator<T> begin, InputIterator<T> end, RandomAccessIterator<T> dest)
        where T:IEquatable<T>
    {
        return UniqueCopy(begin, end, (OutputIterator<T>)dest) as RandomAccessIterator<T>;
    }

    public static ListIterator<T> UniqueCopy<T>(InputIterator<T> begin, InputIterator<T> end, ListIterator<T> dest)
        where T:IEquatable<T>
    {
        return UniqueCopy(begin, end, (OutputIterator<T>)dest) as ListIterator<T>;
    }
    #endregion
    
    #region Iterator + comparer functions
    public static OutputIterator<T> UniqueCopy<T>(InputIterator<T> begin, InputIterator<T> end, OutputIterator<T> dest, IEqualityComparer<T> comparer)
    {
        if(begin.Equals(end))
            return null;

        begin = IteratorUtil.Clone(begin);
        dest  = IteratorUtil.Clone(dest);

        T t = begin.Read();
        dest.Write(t);
        dest.MoveNext();
        begin.MoveNext();
        for(; !begin.Equals(end); begin.MoveNext())
        {
            T next = begin.Read();
            if(!comparer.Equals(t,next))
            {
                t = next;
                dest.Write(next);
                dest.MoveNext();
            }
        }
        return dest;
    }

    public static ForwardIterator<T> UniqueCopy<T>(InputIterator<T> begin, InputIterator<T> end, ForwardIterator<T> dest, IEqualityComparer<T> comparer)
    {
        return UniqueCopy(begin, end, (OutputIterator<T>)dest, comparer) as ForwardIterator<T>;
    }

    public static RandomAccessIterator<T> UniqueCopy<T>(InputIterator<T> begin, InputIterator<T> end, RandomAccessIterator<T> dest, IEqualityComparer<T> comparer)
    {
        return UniqueCopy(begin, end, (OutputIterator<T>)dest, comparer) as RandomAccessIterator<T>;
    }

    public static ListIterator<T> UniqueCopy<T>(InputIterator<T> begin, InputIterator<T> end, ListIterator<T> dest, IEqualityComparer<T> comparer)
    {
        return UniqueCopy(begin, end, (OutputIterator<T>)dest, comparer) as ListIterator<T>;
    }
    #endregion
    
    #region Iterator + predicate functions
    public static OutputIterator<T> UniqueCopy<T>(InputIterator<T> begin, InputIterator<T> end, OutputIterator<T> dest, Functional.BinaryPredicate<T> func)
    {
        if(begin.Equals(end))
            return null;

        begin = IteratorUtil.Clone(begin);
        dest  = IteratorUtil.Clone(dest);
        T t = begin.Read();
        dest.Write(t);
        dest.MoveNext();
        begin.MoveNext();
        for(; !begin.Equals(end); begin.MoveNext())
        {
            T next = begin.Read();
            
            if(!func(t, next))
            {
                t = next;
                dest.Write(next);
                dest.MoveNext();
            }
        }
        return dest;
    }

    public static ForwardIterator<T> UniqueCopy<T>(InputIterator<T> begin, InputIterator<T> end, ForwardIterator<T> dest, Functional.BinaryPredicate<T> func)
    {
        return UniqueCopy(begin, end, (OutputIterator<T>)dest, func) as ForwardIterator<T>;
    }

    public static RandomAccessIterator<T> UniqueCopy<T>(InputIterator<T> begin, InputIterator<T> end, RandomAccessIterator<T> dest, Functional.BinaryPredicate<T> func)
    {
        return UniqueCopy(begin, end, (OutputIterator<T>)dest, func) as RandomAccessIterator<T>;
    }

    public static ListIterator<T> UniqueCopy<T>(InputIterator<T> begin, InputIterator<T> end, ListIterator<T> dest, Functional.BinaryPredicate<T> func)
    {
        return UniqueCopy(begin, end, (OutputIterator<T>)dest, func) as ListIterator<T>;
    }
    #endregion 
    
    
    #region List functions
    public static OutputIterator<T> UniqueCopy<T>(IList<T> source, OutputIterator<T> dest)
        where T:IEquatable<T>
    {
        return UniqueCopy(IteratorUtil.Begin(source), IteratorUtil.End(source), dest);
    }

    public static ForwardIterator<T> UniqueCopy<T>(IList<T> source, ForwardIterator<T> dest)
        where T:IEquatable<T>
    {
        return UniqueCopy(IteratorUtil.Begin(source), IteratorUtil.End(source), (OutputIterator<T>)dest) as ForwardIterator<T>;
    }

    public static RandomAccessIterator<T> UniqueCopy<T>(IList<T> source, RandomAccessIterator<T> dest)
        where T:IEquatable<T>
    {
        return UniqueCopy(IteratorUtil.Begin(source), IteratorUtil.End(source), (OutputIterator<T>)dest) as RandomAccessIterator<T>;
    }

    public static ListIterator<T> UniqueCopy<T>(IList<T> source, ListIterator<T> dest)
        where T:IEquatable<T>
    {
        return UniqueCopy(IteratorUtil.Begin(source), IteratorUtil.End(source), (OutputIterator<T>)dest) as ListIterator<T>;
    }
    #endregion

    #region List + comparer functions
    public static OutputIterator<T> UniqueCopy<T>(IList<T> source, OutputIterator<T> dest, IEqualityComparer<T> comparer)
    {
        return UniqueCopy(IteratorUtil.Begin(source), IteratorUtil.End(source), dest, comparer);
    }

    public static ForwardIterator<T> UniqueCopy<T>(IList<T> source, ForwardIterator<T> dest, IEqualityComparer<T> comparer)
    {
        return UniqueCopy(source, (OutputIterator<T>) dest, comparer) as ForwardIterator<T>;
    }

    public static RandomAccessIterator<T> UniqueCopy<T>(IList<T> source, RandomAccessIterator<T> dest, IEqualityComparer<T> comparer)
    {
        return UniqueCopy(source, (OutputIterator<T>) dest, comparer) as RandomAccessIterator<T>;
    }

    public static ListIterator<T> UniqueCopy<T>(IList<T> source, ListIterator<T> dest, IEqualityComparer<T> comparer)
    {
        return UniqueCopy(source, (OutputIterator<T>) dest, comparer) as ListIterator<T>;
    }
    #endregion

    #region List + predictate functions
    public static OutputIterator<T> UniqueCopy<T>(IList<T> source, OutputIterator<T> dest, Functional.BinaryPredicate<T> func)
    {
        return UniqueCopy(IteratorUtil.Begin(source), IteratorUtil.End(source), dest, func);
    }

    public static ForwardIterator<T> UniqueCopy<T>(IList<T> source, ForwardIterator<T> dest, Functional.BinaryPredicate<T> func)
    {
        return UniqueCopy(source, (OutputIterator<T>)dest, func) as ForwardIterator<T>;
    }

    public static RandomAccessIterator<T> UniqueCopy<T>(IList<T> source, RandomAccessIterator<T> dest, Functional.BinaryPredicate<T> func)
    {
        return UniqueCopy(source, (OutputIterator<T>)dest, func) as RandomAccessIterator<T>;
    }

    public static ListIterator<T> UniqueCopy<T>(IList<T> source, ListIterator<T> dest, Functional.BinaryPredicate<T> func)
    {
        return UniqueCopy(source, (OutputIterator<T>)dest, func) as ListIterator<T>;
    }
    #endregion


    #region List to list funtions
    public static ListIterator<T> UniqueCopy<T>(IList<T> source, IList<T> dest)
        where T:IEquatable<T>
    {
        return UniqueCopy(IteratorUtil.Begin(source), IteratorUtil.End(source), IteratorUtil.Begin(dest));
    }

    public static ListIterator<T> UniqueCopy<T>(IList<T> list, IList<T> dest, IEqualityComparer<T> comparer)
    {
        return UniqueCopy(IteratorUtil.Begin(list), IteratorUtil.End(list), IteratorUtil.Begin(dest), comparer);
    }

    public static ListIterator<T> UniqueCopy<T>(IList<T> source, IList<T> dest, Functional.BinaryPredicate<T> func)
    {
        return UniqueCopy(IteratorUtil.Begin(source), IteratorUtil.End(source), IteratorUtil.Begin(dest), func);
    }
    #endregion
}
}
