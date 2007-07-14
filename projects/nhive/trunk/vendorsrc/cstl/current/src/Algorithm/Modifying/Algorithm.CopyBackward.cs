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
    /// <summary>
    /// Copies elements from [begin,end) to [target-(end-begin), target), starting with *(target-1) = *(end-1),
    /// and progressing backwards towards begin. Note that although elements are copied in reverse order, the order
    /// of elements is not changed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="begin"></param>
    /// <param name="end"></param>
    /// <param name="target"></param>
    public static void CopyBackward<T>(BidirectionalIterator<T> begin, BidirectionalIterator<T> end, BidirectionalIterator<T> targetEnd)
    {
        targetEnd = IteratorUtil.Clone(targetEnd);
        end    = IteratorUtil.Clone(end);
        while(!begin.Equals(end))
        {
            end.MovePrev();
            targetEnd.MovePrev();
            targetEnd.Write(end.Read());
        }
    }

    /// <summary>
    /// Copies the items from a source IList to an output iterator in reverse order. 
    /// </summary>
    /// <remarks>Note the subtle difference between CopyBackward and ReverseCopy. </remarks>
    /// <remarks>
    /// This method is a little different than its Copy counterpart. Copy takes an IEnumerable<T>, which is very loose. We require
    /// an IList<T>. The reason is because .NET does not provide a way to iterate an enumerable collection in reverse. We do provide 
    /// a workaround (see the next method), but it is very ineffecient."/>
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="target"></param>
    public static void CopyBackward<T>(IList<T> list, BidirectionalIterator<T> targetEnd)
    {
        CopyBackward(IteratorUtil.Begin(list), IteratorUtil.End(list), targetEnd);
    }

    /// <summary>
    /// Copies elements from a source enumerable to an output iterator in reverse order
    /// </summary>
    /// <remarks>
    /// Because .NET does not allow use to traverse an enumerator in reverse, we have to create a complete, temporary
    /// copy of the input collection. This is terribly inefficient. Beware when using this routine with large collections.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerable"></param>
    /// <param name="target"></param>
    public static void CopyBackward<T>(IEnumerable<T> enumerable, BidirectionalIterator<T> targetEnd)
    {
        CopyBackward(new List<T>(enumerable), targetEnd);
    }

    public static void CopyBackward<T>(IList<T>  source, IList<T> targetEnd)
    {
        CopyBackward(source, IteratorUtil.End(targetEnd));
    }

    public static void CopyBackward<T>(BidirectionalIterator<T> begin, BidirectionalIterator<T> end, IList<T> targetEnd)
    {
        CopyBackward(begin, end, IteratorUtil.End(targetEnd));
    }

    public static void CopyBackward<T>(IList<T>source, int startIndex, IList<T> dest, int destIndex)
    {
        CopyBackward(IteratorUtil.Begin(source).OffsetBy(startIndex), IteratorUtil.End(source),
             IteratorUtil.Begin(dest).OffsetBy(destIndex));
    }
}
}