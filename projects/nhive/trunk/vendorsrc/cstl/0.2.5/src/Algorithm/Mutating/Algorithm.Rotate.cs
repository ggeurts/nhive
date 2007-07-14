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
    public static void Rotate<T>(ForwardIterator<T> begin, ForwardIterator<T> middle, ForwardIterator<T> end)
    {
        if(begin.Equals(middle) || middle.Equals(end) || begin.Equals(end))
            return;

//        begin  = IteratorUtil.Clone(begin);
//        middle = IteratorUtil.Clone(middle);
        ForwardIterator<T> left  = IteratorUtil.Clone(begin);
        ForwardIterator<T> right = IteratorUtil.Clone(middle);
        while(true)
        {
            IteratorUtil.Swap(left, right);
            left.MoveNext();
            right.MoveNext();
            if(left.Equals(middle))
            {
                if(right.Equals(end))
                    return;
                else
                    middle = IteratorUtil.Clone(right);
            }
            else if(right.Equals(end))
            {
                right = IteratorUtil.Clone(middle);
            }
        }
    }
}
}