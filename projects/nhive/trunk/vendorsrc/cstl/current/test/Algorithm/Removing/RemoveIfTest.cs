using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL.Iterator;
using CSTL;

namespace CSTL.Test
{

[TestFixture] 
[Category("Algorithm")]
public class RemoveIfTest_Int : BaseIntTest
{
    static bool Is29(int value)
    {
        return value == 29;
    }
    
    [Test] public void RemoveIf_ListIterator() 
    {
        ListIterator<int> begin = IteratorUtil.Begin(src);
        ListIterator<int> end   = IteratorUtil.End(src);

        ListIterator<int> result = Algorithm.RemoveIf(begin, end, Is29);
        Assert.IsNotNull(result);
        Assert.AreEqual(0, begin.Position);
        Assert.AreEqual(src.Length, end.Position);
        Assert.AreEqual(src.Length-4, result.Position);
    }

    [Test] public void RemoveIf_RandomAccessIterator() 
    {
        RandomAccessIterator<int> begin = IteratorUtil.Begin(src);
        RandomAccessIterator<int> end   = IteratorUtil.End(src);

        RandomAccessIterator<int> result = Algorithm.RemoveIf(begin, end, Is29);
        Assert.IsNotNull(result);
        Assert.AreEqual(0, begin.Position);
        Assert.AreEqual(src.Length, end.Position);
        Assert.AreEqual(src.Length-4, result.Position);
    }

    [Test] public void RemoveIf_ForwardIterator() 
    {
        ForwardIterator<int> begin = IteratorUtil.Begin(src);
        ForwardIterator<int> end   = IteratorUtil.End(src);

        ForwardIterator<int> result = Algorithm.RemoveIf(begin, end, Is29);
        Assert.IsNotNull(result);
        Assert.AreEqual(0, (begin as ListIterator<int>).Position);
        Assert.AreEqual(src.Length, IteratorUtil.Distance(begin, end));
        Assert.AreEqual(src.Length-4, IteratorUtil.Distance(begin, result));
    }
}

}
