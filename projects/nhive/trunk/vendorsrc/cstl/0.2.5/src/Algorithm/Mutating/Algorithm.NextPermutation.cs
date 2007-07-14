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
    public static bool NextPermutation<T>(BidirectionalIterator<T> begin, BidirectionalIterator<T> end)
        where T : IComparable<T>
    {
        return NextPermutation(begin, end, Comparer<T>.Default);
    }
    
    public static bool NextPermutation<T>(BidirectionalIterator<T> begin, BidirectionalIterator<T> end, IComparer<T> comparer)
    {
        if(begin.Equals(end))
            return false;

        if(IteratorUtil.IsOneElementRange(begin, end))
            return false;

        BidirectionalIterator<T> i = IteratorUtil.Clone(end);
        i.MovePrev();

        for(;;)
        {
            BidirectionalIterator<T> ii = IteratorUtil.Clone(i);
            i.MovePrev();

            T i_value = i.Read();
            if(comparer.Compare(i_value, ii.Read())==-1)
            {
                BidirectionalIterator<T> j =  IteratorUtil.Clone(end);
                for(j.MovePrev(); comparer.Compare(i_value,j.Read())!=-1; j.MovePrev())
                    ;
                IteratorUtil.Swap(i, j);
                Reverse(ii, end);
                return true;
            }
            if(i.Equals(begin))
            {
                Reverse(begin, end);
                return false;
            }
        }
    }
    
    public static bool NextPermutation<T>(BidirectionalIterator<T> begin, BidirectionalIterator<T> end, 
                                          Functional.BinaryPredicate<T> func)
    {
        if(begin.Equals(end))
            return false;

        if(IteratorUtil.IsOneElementRange(begin, end))
            return false;

        BidirectionalIterator<T> i = IteratorUtil.Clone(end);
        i.MovePrev();

        for(;;)
        {
            BidirectionalIterator<T> ii = IteratorUtil.Clone(i);
            i.MovePrev();

            T i_value = i.Read();
            if(func(i_value, ii.Read()))
            {
                BidirectionalIterator<T> j =  IteratorUtil.Clone(end);
                for(j.MovePrev(); !func(i_value,j.Read()); j.MovePrev())
                    ;
                IteratorUtil.Swap(i, j);
                Reverse(ii, end);
                return true;
            }
            if(i.Equals(begin))
            {
                Reverse(begin, end);
                return false;
            }
        }
    }
}
}