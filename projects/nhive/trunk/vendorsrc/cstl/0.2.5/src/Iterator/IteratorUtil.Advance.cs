//-----------------------------------------------------------------------------
// CSTL
// Copyright (C) 2006 by Harold Howe hhjunk@mchsi.com
// http://sourceforge.net/projects/cstl

using System;
using System.Collections.Generic;

namespace CSTL.Iterator
{

public static partial class IteratorUtil
{
    // TODO: Need some concept of iterator traits. If a generic algorithm is coded
    // to accept an input iterator, and needs to advance it, it would be nice to call
    // the random access version if the iterator is really a random access iterator. 
    // Furthermore, it would be nice if we could dispatch to the correct version
    // without performing a runtime check or cast. Without templates and specialization, this 
    // second requirement is probably not attainable. If we have to perform a cast, is the 
    // cost of the cast greater than the benefits of dispacting to the random access version?
    public static void Advance<T>(InputIterator<T> iter, uint distance)
    {
        while(distance != 0)
        {
            iter.MoveNext();
            --distance;
        }
    }

#if NEVER
    public static void Advance<T>(ForwardIterator<T> iter, uint distance)
    {
        while(distance != 0)
        {
            iter.MoveNext();
            --distance;
        }
    }
#endif

    public static void Advance<T>(BidirectionalIterator<T> iter, int distance)
    {
        while(distance > 0)
        {
            iter.MoveNext();
            --distance;
        }
        
        while(distance < 0)
        {
            iter.MovePrev();
            ++distance;
        }
    }
    
    public static void Advance<T>(RandomAccessIterator<T> iter, int distance)
    {
        iter.OffsetBy(distance);
    }
    
    // TODO: Need some concept of iterator traits. If a generic algorithm is coded
    // to accept an input iterator, and needs to advance it, it would be nice to call
    // the random access version if the iterator is really a random access iterator. 
    // Furthermore, it would be nice if we could dispatch to the correct version
    // without performing a runtime check or cast. Without templates and specialization, this 
    // second requirement is probably not attainable. If we have to perform a cast, is the 
    // cost of the cast greater than the benefits of dispacting to the random access version?
    public static InputIterator<T> AdvanceCopy<T>(InputIterator<T> iter, uint distance)
    {
        iter = IteratorUtil.Clone(iter);
        Advance(iter, distance);
        return iter;
    }

#if NEVER
    public static void Advance<T>(ForwardIterator<T> iter, uint distance)
    {
        while(distance != 0)
        {
            iter.MoveNext();
            --distance;
        }
    }
#endif

    public static BidirectionalIterator<T> AdvanceCopy<T>(BidirectionalIterator<T> iter, int distance)
    {
        iter = IteratorUtil.Clone(iter);
        Advance(iter, distance);
        return iter;
    }
    
    public static RandomAccessIterator<T> AdvanceCopy<T>(RandomAccessIterator<T> iter, int distance)
    {
        iter = IteratorUtil.Clone(iter);
        Advance(iter, distance);
        return iter;
    }    
    
    public static ListIterator<T> AdvanceCopy<T>(ListIterator<T> iter, int distance)
    {
        iter = IteratorUtil.Clone(iter);
        Advance(iter, distance);
        return iter;
    }      
}

}