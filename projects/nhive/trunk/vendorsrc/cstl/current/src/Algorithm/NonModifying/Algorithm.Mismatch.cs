//-----------------------------------------------------------------------------
// CSTL
// Copyright (C) 2006 by Harold Howe hhjunk@mchsi.com
// http://sourceforge.net/projects/cstl

using System;
using System.Collections.Generic;
using CSTL.Iterator;
using CSTL.Utility;


namespace CSTL
{

public static partial class Algorithm
{
    public static Pair<InputIterator<T>, InputIterator<T>>
        Mismatch<T>(InputIterator<T> begin, InputIterator<T> end, InputIterator<T> cmpBegin)
        where T: IEquatable<T>
    {
        return Mismatch(begin, end, cmpBegin, EqualityComparer<T>.Default);
    }

    public static Pair<ForwardIterator<T>, ForwardIterator<T>>
        Mismatch<T>(ForwardIterator<T> begin, ForwardIterator<T> end, ForwardIterator<T> cmpBegin)
        where T: IEquatable<T>
    {
        Pair<InputIterator<T>, InputIterator<T>> result = Mismatch((InputIterator<T>)begin, (InputIterator<T>)end, (InputIterator<T>)cmpBegin);
        return new Pair<ForwardIterator<T>,ForwardIterator<T>>((ForwardIterator<T>) result.First, (ForwardIterator<T>)result.Second);
    }

    public static Pair<RandomAccessIterator<T>, RandomAccessIterator<T>>
        Mismatch<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end, RandomAccessIterator<T> cmpBegin)
        where T: IEquatable<T>
    {
        Pair<InputIterator<T>, InputIterator<T>> result = Mismatch((InputIterator<T>)begin, (InputIterator<T>)end, (InputIterator<T>)cmpBegin);
        return new Pair<RandomAccessIterator<T>,RandomAccessIterator<T>>((RandomAccessIterator<T>) result.First, (RandomAccessIterator<T>)result.Second);
    }

    public static Pair<ListIterator<T>, ListIterator<T>>
        Mismatch<T>(ListIterator<T> begin, ListIterator<T> end, ListIterator<T> cmpBegin)
        where T: IEquatable<T>
    {
        Pair<InputIterator<T>, InputIterator<T>> result = Mismatch((InputIterator<T>)begin, (InputIterator<T>)end, (InputIterator<T>)cmpBegin);
        return new Pair<ListIterator<T>,ListIterator<T>>((ListIterator<T>) result.First, (ListIterator<T>)result.Second);
    }

    public static Utility.Pair<InputIterator<T>, InputIterator<T>>
        Mismatch<T>(InputIterator<T> begin, InputIterator<T> end,  InputIterator<T> cmpBegin, IEqualityComparer<T> comparer)
    {
        for(begin=IteratorUtil.Clone(begin), cmpBegin=IteratorUtil.Clone(cmpBegin); !begin.Equals(end); begin.MoveNext(), cmpBegin.MoveNext())
        {
            T t1 = begin.Read();
            T t2 = cmpBegin.Read();
            if(!comparer.Equals(t1,t2))
                return new CSTL.Utility.Pair<InputIterator<T>,InputIterator<T>>(IteratorUtil.Clone(begin), IteratorUtil.Clone(cmpBegin));
        }
        
        return new CSTL.Utility.Pair<InputIterator<T>,InputIterator<T>>(null, null);//begin, cmpBegin);
    }

    public static Pair<ForwardIterator<T>, ForwardIterator<T>>
        Mismatch<T>(ForwardIterator<T> begin, ForwardIterator<T> end, ForwardIterator<T> cmpBegin, IEqualityComparer<T> comparer)
    {
        Pair<InputIterator<T>, InputIterator<T>> result = Mismatch((InputIterator<T>)begin, (InputIterator<T>)end, (InputIterator<T>)cmpBegin, comparer);
        return new Pair<ForwardIterator<T>,ForwardIterator<T>>((ForwardIterator<T>) result.First, (ForwardIterator<T>)result.Second);
    }

    public static Pair<RandomAccessIterator<T>, RandomAccessIterator<T>>
        Mismatch<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end, RandomAccessIterator<T> cmpBegin, IEqualityComparer<T> comparer)
    {
        Pair<InputIterator<T>, InputIterator<T>> result = Mismatch((InputIterator<T>)begin, (InputIterator<T>)end, (InputIterator<T>)cmpBegin, comparer);
        return new Pair<RandomAccessIterator<T>,RandomAccessIterator<T>>((RandomAccessIterator<T>) result.First, (RandomAccessIterator<T>)result.Second);
    }

    public static Pair<ListIterator<T>, ListIterator<T>>
        Mismatch<T>(ListIterator<T> begin, ListIterator<T> end, ListIterator<T> cmpBegin, IEqualityComparer<T> comparer)
    {
        Pair<InputIterator<T>, InputIterator<T>> result = Mismatch((InputIterator<T>)begin, (InputIterator<T>)end, (InputIterator<T>)cmpBegin, comparer);
        return new Pair<ListIterator<T>,ListIterator<T>>((ListIterator<T>) result.First, (ListIterator<T>)result.Second);
    }
}
}