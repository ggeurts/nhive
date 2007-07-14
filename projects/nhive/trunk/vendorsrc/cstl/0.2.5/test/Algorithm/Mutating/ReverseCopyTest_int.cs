using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class ReverseCopyTest_Int : BaseIntTest
{
    [Test] public void ReverseCopy_Iterator() 
    {
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();

        Algorithm.ReverseCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter);
        Assert.AreEqual(dest[0], Int32.MinValue);
        Assert.AreEqual(dest[dest.Length-1], Int32.MinValue);
        for(int i=0; i<count ;++i)
        {
            Assert.AreEqual(src[i], dest[dest.Length-2-i]);
        }
    }

    [Test] public void Reverse_List2Iterator() 
    {
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();

        Algorithm.ReverseCopy(src, destIter);
        Assert.AreEqual(dest[0], Int32.MinValue);
        Assert.AreEqual(dest[dest.Length-1], Int32.MinValue);
        for(int i=0; i<count ;++i)
        {
            Assert.AreEqual(src[i], dest[dest.Length-2-i]);
        }
    }
    
    [Test] public void Reverse_Enumerable2Iterator() 
    {
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();

        Algorithm.ReverseCopy(GetEnumerable(), destIter);
        Assert.AreEqual(dest[0], Int32.MinValue);
        Assert.AreEqual(dest[dest.Length-1], Int32.MinValue);
        for(int i=0; i<count ;++i)
        {
            Assert.AreEqual(src[i], dest[dest.Length-2-i]);
        }
    }

    [Test] public void ReverseCopy_Iterator2List() 
    {
        Algorithm.ReverseCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), dest);
        Assert.AreEqual(dest[dest.Length-1], Int32.MinValue);
        Assert.AreEqual(dest[dest.Length-2], Int32.MinValue);
        for(int i=0; i<count ;++i)
        {
            Assert.AreEqual(src[i], dest[count-1-i]);
        }
    }

    [Test] public void ReverseCopy_List2List() 
    {
        Algorithm.ReverseCopy(src, 0, dest, 1);
        Assert.AreEqual(dest[0], Int32.MinValue);
        Assert.AreEqual(dest[dest.Length-1], Int32.MinValue);
        for(int i=0; i<count ;++i)
        {
            Assert.AreEqual(src[i], dest[dest.Length-2-i]);
        }
    }
    
    IEnumerable<int> GetEnumerable()
    {
        foreach(int i in src)
            yield return i;
    }
}

}
