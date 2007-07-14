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
    public static InputIterator<T> AdjacentFind<T>(InputIterator<T> begin, InputIterator<T> end)
        where T: IEquatable<T>
    {
        return AdjacentFind(begin, end, EqualityComparer<T>.Default);
    }

    public static ForwardIterator<T> AdjacentFind<T>(ForwardIterator<T> begin, ForwardIterator<T> end)
        where T: IEquatable<T>
    {
        return (ForwardIterator<T>)AdjacentFind((InputIterator<T>)begin, (InputIterator<T>)end);
    }

    public static RandomAccessIterator<T> AdjacentFind<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end)
        where T: IEquatable<T>
    {
        return (RandomAccessIterator<T>)AdjacentFind((InputIterator<T>)begin, (InputIterator<T>)end);
    }

    public static ListIterator<T> AdjacentFind<T>(ListIterator<T> begin, ListIterator<T> end)
        where T: IEquatable<T>
    {
        return (ListIterator<T>)AdjacentFind((InputIterator<T>)begin, (InputIterator<T>)end);
    }

    public static InputIterator<T> AdjacentFind<T>(InputIterator<T> begin, InputIterator<T> end, IEqualityComparer<T> comparer)
    {
        if(begin.Equals(end))
            return null;
        begin = IteratorUtil.Clone(begin);
        InputIterator<T> next = IteratorUtil.Clone(begin);
        next.MoveNext();
        T t1 = begin.Read();
        T t2;
        
        for(; !next.Equals(end); begin.MoveNext(), next.MoveNext(), t1=t2)
        {
            t2 = next.Read();
            if(comparer.Equals(t1, t2))
                return IteratorUtil.Clone(begin);
        }
        return null;
    }

    public static ForwardIterator<T> AdjacentFind<T>(ForwardIterator<T> begin, ForwardIterator<T> end, IEqualityComparer<T> comparer)
    {
        return (ForwardIterator<T>)AdjacentFind((InputIterator<T>)begin, (InputIterator<T>)end, comparer);
    }

    public static RandomAccessIterator<T> AdjacentFind<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end,  IEqualityComparer<T> comparer)
    {
        return (RandomAccessIterator<T>)AdjacentFind((InputIterator<T>)begin, (InputIterator<T>)end, comparer);
    }

    public static ListIterator<T> AdjacentFind<T>(ListIterator<T> begin, ListIterator<T> end,  IEqualityComparer<T> comparer)
    {
        return (ListIterator<T>)AdjacentFind((InputIterator<T>)begin, (InputIterator<T>)end, comparer);
    }

    public static InputIterator<T> AdjacentFind<T>(InputIterator<T> begin, InputIterator<T> end, Functional.BinaryPredicate<T> op)
    {
        if(begin.Equals(end))
            return null;
        
        begin = IteratorUtil.Clone(begin);
        InputIterator<T> next = IteratorUtil.Clone(begin);
        next.MoveNext();
        T t1 = begin.Read();
        T t2;
        
        for(; !next.Equals(end); begin.MoveNext(), next.MoveNext(), t1=t2)
        {
            t2 = next.Read();
            if(op(t1,t2))
                return IteratorUtil.Clone(begin);
        }
        return null;
    }

    public static ForwardIterator<T> AdjacentFind<T>(ForwardIterator<T> begin, ForwardIterator<T> end, Functional.BinaryPredicate<T> op)
    {
        return (ForwardIterator<T>)AdjacentFind((InputIterator<T>)begin, (InputIterator<T>)end, op);
    }

    public static RandomAccessIterator<T> AdjacentFind<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end, Functional.BinaryPredicate<T> op)
    {
        return (RandomAccessIterator<T>)AdjacentFind((InputIterator<T>)begin, (InputIterator<T>)end, op);
    }

    public static ListIterator<T> AdjacentFind<T>(ListIterator<T> begin, ListIterator<T> end,  Functional.BinaryPredicate<T> op)
    {
        return (ListIterator<T>)AdjacentFind((InputIterator<T>)begin, (InputIterator<T>)end, op);
    }
}
}