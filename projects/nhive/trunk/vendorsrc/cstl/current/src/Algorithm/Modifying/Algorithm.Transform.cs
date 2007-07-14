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
    public static OutputIterator<O> Transform<I,O>(InputIterator<I> begin, InputIterator<I> end, OutputIterator<O> dest, Functional.UnaryFunction<I,O> func)
    {
        for(begin = IteratorUtil.Clone(begin), dest = IteratorUtil.Clone(dest); !begin.Equals(end); begin.MoveNext(), dest.MoveNext())
        {
            dest.Write(func(begin.Read()));
        }
        return dest;
    }

    public static OutputIterator<O> Transform<I,O>(IEnumerable<I> enumerable, OutputIterator<O> dest, Functional.UnaryFunction<I,O> func)
    {
        IEnumerator<I> enumerator = enumerable.GetEnumerator();
        for(dest = IteratorUtil.Clone(dest); enumerator.MoveNext(); dest.MoveNext())
        {
            dest.Write(func(enumerator.Current));
        }
        return dest;
    }

    public static OutputIterator<O> Transform<I,O>(InputIterator<I> begin1, InputIterator<I> end1, InputIterator<I> begin2,
                                                   OutputIterator<O> dest, Functional.BinaryFunction<I, I, O> func)
    {
        begin1 = IteratorUtil.Clone(begin1);
        begin2 = IteratorUtil.Clone(begin2);
        dest = IteratorUtil.Clone(dest);
        for(; !begin1.Equals(end1); begin1.MoveNext(), begin2.MoveNext(), dest.MoveNext())
        {
            dest.Write(func(begin1.Read(), begin2.Read()));
        }
        return dest;
    }
    
}
}