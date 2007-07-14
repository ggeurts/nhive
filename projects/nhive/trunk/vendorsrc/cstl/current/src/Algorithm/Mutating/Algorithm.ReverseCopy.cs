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

    public static void ReverseCopy<T>(BidirectionalIterator<T> begin, BidirectionalIterator<T> end, OutputIterator<T> dest)
    {
        end = IteratorUtil.Clone(end);
        dest = IteratorUtil.Clone(dest);

        while(!begin.Equals(end))
        {
            end.MovePrev();
            dest.Write(end.Read());
            dest.MoveNext();
        }
    }

    public static void ReverseCopy<T>(IList<T> source, OutputIterator<T> target)
    {
        ReverseCopy(IteratorUtil.Begin(source), IteratorUtil.End(source), target);
    }

    /// <summary>
    /// Reverses elements from a source enumerable to an output iterator in reverse order
    /// </summary>
    /// <remarks>
    /// Because .NET does not allow use to traverse an enumerator in reverse, we have to create a complete, temporary
    /// copy of the input collection. This is terribly inefficient. Beware when using this routine with large collections.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerable"></param>
    /// <param name="target"></param>
    public static void ReverseCopy<T>(IEnumerable<T> enumerable, OutputIterator<T> target)
    {
        ReverseCopy(new List<T>(enumerable), target);
    }

    public static void ReverseCopy<T>(IList<T> source, IList<T> target)
    {
        ReverseCopy(IteratorUtil.Begin(source), IteratorUtil.End(source), IteratorUtil.Begin(target));
    }
    
    public static void ReverseCopy<T>(BidirectionalIterator<T> begin, BidirectionalIterator<T> end, IList<T> target)
    {
        ReverseCopy(begin, end, IteratorUtil.Begin(target));
    }

    public static void ReverseCopy<T>(IList<T>source, int startIndex, IList<T> dest, int destIndex)
    {
        ReverseCopy(IteratorUtil.Begin(source).OffsetBy(startIndex), IteratorUtil.End(source),
             IteratorUtil.Begin(dest).OffsetBy(destIndex));
    }    
}
}