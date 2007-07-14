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
    public static OutputIterator<T> RemoveCopy<T>(InputIterator<T> begin, InputIterator<T> end, OutputIterator<T> dest, T value)
        where T:IEquatable<T>
    {
        for(begin=IteratorUtil.Clone(begin), dest=IteratorUtil.Clone(dest); !begin.Equals(end); begin.MoveNext())
        {
            T t = begin.Read();
            if(!value.Equals(t))
            {
                dest.Write(t);
                dest.MoveNext(); 
            }
        }
        return dest;
    }

    public static ForwardIterator<T> RemoveCopy<T>(InputIterator<T> begin, InputIterator<T> end, ForwardIterator<T> dest, T value)
        where T:IEquatable<T>
    {
        return RemoveCopy(begin, end, (OutputIterator<T>) dest, value) as ForwardIterator<T>;
    }

    public static RandomAccessIterator<T> RemoveCopy<T>(InputIterator<T> begin, InputIterator<T> end, RandomAccessIterator<T> dest, T value)
        where T:IEquatable<T>
    {
        return RemoveCopy(begin, end, (OutputIterator<T>) dest, value) as RandomAccessIterator<T>;
    }

    public static ListIterator<T> RemoveCopy<T>(InputIterator<T> begin, InputIterator<T> end, ListIterator<T> dest, T value)
        where T:IEquatable<T>
    {
        return RemoveCopy(begin, end, (OutputIterator<T>) dest, value) as ListIterator<T>;
    }
    #endregion
    
    #region Iterator + comparer functions
    public static OutputIterator<T> RemoveCopy<T>(InputIterator<T> begin, InputIterator<T> end, OutputIterator<T> dest, T value, IEqualityComparer<T> comparer)
    {
        for(begin=IteratorUtil.Clone(begin), dest=IteratorUtil.Clone(dest); !begin.Equals(end); begin.MoveNext())
        {
            T t = begin.Read();
            if(!comparer.Equals(value, t))
            {
                dest.Write(t);
                dest.MoveNext(); 
            }
        }
        return dest;
    }

    public static ListIterator<T> RemoveCopy<T>(InputIterator<T> begin, InputIterator<T> end, ListIterator<T> dest, T value, IEqualityComparer<T> comparer)
    {
        return RemoveCopy(begin,end, (OutputIterator<T>)dest, value, comparer) as ListIterator<T>;
    }

    public static RandomAccessIterator<T> RemoveCopy<T>(InputIterator<T> begin, InputIterator<T> end, RandomAccessIterator<T> dest, T value, IEqualityComparer<T> comparer)
    {
        return RemoveCopy(begin,end, (OutputIterator<T>)dest, value, comparer) as RandomAccessIterator<T>;
    }

    public static ForwardIterator<T> RemoveCopy<T>(InputIterator<T> begin, InputIterator<T> end, ForwardIterator<T> dest, T value, IEqualityComparer<T> comparer)
    {
        return RemoveCopy(begin,end, (OutputIterator<T>)dest, value, comparer) as ForwardIterator<T>;
    }
    #endregion

    #region List to iterator methods
    public static OutputIterator<T> RemoveCopy<T>(IList<T> source, OutputIterator<T> dest, T value)
        where T:IEquatable<T>
    {
        return RemoveCopy(IteratorUtil.Begin(source), IteratorUtil.End(source), dest, value);
    }

    public static ListIterator<T> RemoveCopy<T>(IList<T> source, ListIterator<T> dest, T value)
        where T:IEquatable<T>
    {
        return RemoveCopy(source, (OutputIterator<T>)dest, value) as ListIterator<T>;
    }

    public static RandomAccessIterator<T> RemoveCopy<T>(IList<T> source, RandomAccessIterator<T> dest, T value)
        where T:IEquatable<T>
    {
        return RemoveCopy(source, (OutputIterator<T>)dest, value) as RandomAccessIterator<T>;
    }

    public static ForwardIterator<T> RemoveCopy<T>(IList<T> source, ForwardIterator<T> dest, T value)
        where T:IEquatable<T>
    {
        return RemoveCopy(source, (OutputIterator<T>)dest, value) as ForwardIterator<T>;
    }

    #endregion

    #region List to iterator + comparer methods
    public static OutputIterator<T> RemoveCopy<T>(IList<T> source, OutputIterator<T> dest, T value, IEqualityComparer<T> comparer)
    {
        return RemoveCopy(IteratorUtil.Begin(source), IteratorUtil.End(source), dest, value, comparer);
    }

    public static ListIterator<T> RemoveCopy<T>(IList<T> source, ListIterator<T> dest, T value, IEqualityComparer<T> comparer)
    {
        return RemoveCopy(source, (OutputIterator<T>)dest, value, comparer) as ListIterator<T>;
    }

    public static RandomAccessIterator<T> RemoveCopy<T>(IList<T> source, RandomAccessIterator<T> dest, T value, IEqualityComparer<T> comparer)
    {
        return RemoveCopy(source, (OutputIterator<T>)dest, value, comparer) as RandomAccessIterator<T>;
    }

    public static ForwardIterator<T> RemoveCopy<T>(IList<T> source, ForwardIterator<T> dest, T value, IEqualityComparer<T> comparer)
    {
        return RemoveCopy(source, (OutputIterator<T>)dest, value, comparer) as ForwardIterator<T>;
    }
    #endregion

    public static ListIterator<T> RemoveCopy<T>(IList<T> source, IList<T> dest, T value)
        where T:IEquatable<T>
    {
        return RemoveCopy(IteratorUtil.Begin(source), IteratorUtil.End(source), IteratorUtil.Begin(dest), value);
    }

    public static ListIterator<T> RemoveCopy<T>(IList<T> list, IList<T> dest,T value, IEqualityComparer<T> comparer)
    {
        return RemoveCopy(IteratorUtil.Begin(list), IteratorUtil.End(list), IteratorUtil.Begin(dest), value, comparer);
    }
}
}
