                          using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;
using CSTL.Utility;

namespace CSTL.Test
{

[TestFixture] 
[Category("Algorithm")]
public class AdjacentFindTest 
{
    [Test] public void AdjacentFind_ListIterator()
    {
        int[] array = Constants.TEST_INT_ARRAY;
        
        ListIterator<int> begin = IteratorUtil.Begin(array);
        ListIterator<int> iter = Algorithm.AdjacentFind(begin, IteratorUtil.End(array));
        Assert.AreEqual(6, iter.Position);
        Assert.AreEqual(6, IteratorUtil.Distance(begin, iter));
        Assert.AreEqual(array[6], iter.Read());
        
        iter = Algorithm.AdjacentFind(IteratorUtil.Begin(array), IteratorUtil.End(array), EqualityComparer<int>.Default);
        Assert.AreEqual(6, iter.Position);
        Assert.AreEqual(array[6], iter.Read());

        iter = Algorithm.AdjacentFind(IteratorUtil.Begin(array), IteratorUtil.End(array), AreEqual);
        Assert.AreEqual(6, iter.Position);
        Assert.AreEqual(array[6], iter.Read());
    }

    [Test] public void AdjacentFind_RandomIterator()
    {
        int[] array = Constants.TEST_INT_ARRAY;

        RandomAccessIterator<int> begin = IteratorUtil.Begin(array);
        RandomAccessIterator<int> end   = IteratorUtil.End(array);
        RandomAccessIterator<int> iter  = Algorithm.AdjacentFind(begin, end);
        Assert.AreEqual(6, IteratorUtil.Distance(begin, iter));
        Assert.AreEqual(array[6], iter.Read());

        iter = Algorithm.AdjacentFind(IteratorUtil.Begin(array), IteratorUtil.End(array), EqualityComparer<int>.Default);
        Assert.AreEqual(6, IteratorUtil.Distance(begin, iter));
        Assert.AreEqual(array[6], iter.Read());

        iter = Algorithm.AdjacentFind(IteratorUtil.Begin(array), IteratorUtil.End(array), AreEqual);
        Assert.AreEqual(6, IteratorUtil.Distance(begin, iter));
        Assert.AreEqual(array[6], iter.Read());
    }

    [Test] public void AdjacentFind_ForwardIterator()
    {
        int[] array = Constants.TEST_INT_ARRAY;

        ForwardIterator<int> begin = IteratorUtil.Begin(array);
        ForwardIterator<int> end   = IteratorUtil.End(array);
        ForwardIterator<int> iter  = Algorithm.AdjacentFind(begin, end);
        Assert.AreEqual(6, IteratorUtil.Distance(begin, iter));
        Assert.AreEqual(array[6], iter.Read());

        iter = Algorithm.AdjacentFind(IteratorUtil.Begin(array), IteratorUtil.End(array), EqualityComparer<int>.Default);
        Assert.AreEqual(6, IteratorUtil.Distance(begin, iter));
        Assert.AreEqual(array[6], iter.Read());

        iter = Algorithm.AdjacentFind(IteratorUtil.Begin(array), IteratorUtil.End(array), AreEqual);
        Assert.AreEqual(6, IteratorUtil.Distance(begin, iter));
        Assert.AreEqual(array[6], iter.Read());
    }
    
    public bool AreEqual(int lhs, int rhs)
    {
        return lhs == rhs;
    }
}

}