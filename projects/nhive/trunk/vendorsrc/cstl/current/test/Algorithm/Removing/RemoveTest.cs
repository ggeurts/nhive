using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL.Iterator;
using CSTL;

namespace CSTL.Test
{

[TestFixture] 
[Category("Algorithm")]
public class RemoveTest_Int : BaseIntTest
{
    [Test] public void Remove_ListIterator() 
    {
        ListIterator<int> begin = IteratorUtil.Begin(src);
        ListIterator<int> end   = IteratorUtil.End(src);

        ListIterator<int> result = Algorithm.Remove(begin, end, 29);
        Assert.IsNotNull(result);
        Assert.AreEqual(0, begin.Position);
        Assert.AreEqual(src.Length, end.Position);
        Assert.AreEqual(src.Length-4, result.Position);
    }

    [Test] public void Remove_RandomAccessIterator() 
    {
        RandomAccessIterator<int> begin = IteratorUtil.Begin(src);
        RandomAccessIterator<int> end   = IteratorUtil.End(src);

        RandomAccessIterator<int> result = Algorithm.Remove(begin, end, 29);
        Assert.IsNotNull(result);
        Assert.AreEqual(0, begin.Position);
        Assert.AreEqual(src.Length, end.Position);
        Assert.AreEqual(src.Length-4, result.Position);
    }

    [Test] public void Remove_ForwardIterator() 
    {
        ForwardIterator<int> begin = IteratorUtil.Begin(src);
        ForwardIterator<int> end   = IteratorUtil.End(src);

        ForwardIterator<int> result = Algorithm.Remove(begin, end, 29);
        Assert.IsNotNull(result);
        Assert.AreEqual(0, (begin as ListIterator<int>).Position);
        Assert.AreEqual(src.Length, IteratorUtil.Distance(begin, end));
        Assert.AreEqual(src.Length-4, IteratorUtil.Distance(begin, result));
    }

    [Test] public void Remove_Comparer_ListIterator() 
    {
        ListIterator<int> begin = IteratorUtil.Begin(src);
        ListIterator<int> end   = IteratorUtil.End(src);

        ListIterator<int> result = Algorithm.Remove(begin, end, 29, EqualityComparer<int>.Default);
        Assert.IsNotNull(result);
        Assert.AreEqual(0, begin.Position);
        Assert.AreEqual(src.Length, end.Position);
        Assert.AreEqual(src.Length-4, result.Position);
    }

    [Test] public void Remove_Comparer_RandomAccessIterator() 
    {
        RandomAccessIterator<int> begin = IteratorUtil.Begin(src);
        RandomAccessIterator<int> end   = IteratorUtil.End(src);

        RandomAccessIterator<int> result = Algorithm.Remove(begin, end, 29, EqualityComparer<int>.Default);
        Assert.IsNotNull(result);
        Assert.AreEqual(0, begin.Position);
        Assert.AreEqual(src.Length, end.Position);
        Assert.AreEqual(src.Length-4, result.Position);
    }

    [Test] public void Remove_Comparer_ForwardIterator() 
    {
        ForwardIterator<int> begin = IteratorUtil.Begin(src);
        ForwardIterator<int> end   = IteratorUtil.End(src);

        ForwardIterator<int> result = Algorithm.Remove(begin, end, 29, EqualityComparer<int>.Default);
        Assert.IsNotNull(result);
        Assert.AreEqual(0, (begin as ListIterator<int>).Position);
        Assert.AreEqual(src.Length, IteratorUtil.Distance(begin, end));
        Assert.AreEqual(src.Length-4, IteratorUtil.Distance(begin, result));
    }
}

}
