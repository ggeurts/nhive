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
    public static bool LexCompare<T>(InputIterator<T> begin1, InputIterator<T> end1, 
                                     InputIterator<T> begin2, InputIterator<T> end2)
        where T: IComparable<T>
    {
        return LexCompare(begin1, end1, begin2, end2, Comparer<T>.Default);
    }

    public static bool LexCompare<T>(InputIterator<T> begin1, InputIterator<T> end1, 
                                     InputIterator<T> begin2, InputIterator<T> end2,
                                     Comparer<T> comparer)
    {
        begin1 = IteratorUtil.Clone(begin1);
        //end1   = IteratorUtil.Clone(end1);
        begin2 = IteratorUtil.Clone(begin2);
        //end2   = IteratorUtil.Clone(end2);

        for(;; begin1.MoveNext(), begin2.MoveNext())
        {
            if(begin2.Equals(end2))
                return false;
            if(begin1.Equals(end1))
                return true;
            
            int compare = comparer.Compare(begin1.Read(), begin2.Read());
            if(compare == -1)
                return true;
            if(compare == 1)
                return false;
        }
    }

}
}