using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class ReplaceCopyTest_Int : BaseIntTest
{
    [Test] public void ReplaceCopy_Iterator() 
    {
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        Algorithm.ReplaceCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter, 29, 42);
        
        Assert.AreEqual(dest[0], Int32.MinValue);
        Assert.AreEqual(dest[dest.Length-1], Int32.MinValue);
        for(int i=0; i<count ;++i)
        {
            if(src[i] == 29)
                Assert.AreEqual(42, dest[i+1]);
            else
                Assert.AreEqual(src[i], dest[i+1]);
        }
    }

    [Test] public void ReplaceCopy_Comparer() 
    {
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        Algorithm.ReplaceCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter, 29, 42, EqualityComparer<int>.Default);
        
        Assert.AreEqual(dest[0], Int32.MinValue);
        Assert.AreEqual(dest[dest.Length-1], Int32.MinValue);
        for(int i=0; i<count ;++i)
        {
            if(src[i] == 29)
                Assert.AreEqual(42, dest[i+1]);
            else
                Assert.AreEqual(src[i], dest[i+1]);
        }
    }

    [Test] public void ReplaceCopy_List() 
    {
        Algorithm.ReplaceCopy(src, dest, 29, 42);
        
        Assert.AreEqual(dest[dest.Length-1], Int32.MinValue);
        Assert.AreEqual(dest[dest.Length-2], Int32.MinValue);
        for(int i=0; i<count ;++i)
        {
            if(src[i] == 29)
                Assert.AreEqual(42, dest[i]);
            else
                Assert.AreEqual(src[i], dest[i]);
        }
    }

    [Test] public void ReplaceCopy_ListComparer() 
    {
        Algorithm.ReplaceCopy(src, dest, 29, 42, EqualityComparer<int>.Default);

        Assert.AreEqual(dest[dest.Length-1], Int32.MinValue);
        Assert.AreEqual(dest[dest.Length-2], Int32.MinValue);
        for(int i=0; i<count ;++i)
        {
            if(src[i] == 29)
                Assert.AreEqual(42, dest[i]);
            else
                Assert.AreEqual(src[i], dest[i]);
        }
    }

    [Test] public void ReplaceCopy_List2Iterator() 
    {
        Algorithm.ReplaceCopy(src, IteratorUtil.Begin(dest), 29, 42);
        
        Assert.AreEqual(dest[dest.Length-1], Int32.MinValue);
        Assert.AreEqual(dest[dest.Length-2], Int32.MinValue);
        for(int i=0; i<count ;++i)
        {
            if(src[i] == 29)
                Assert.AreEqual(42, dest[i]);
            else
                Assert.AreEqual(src[i], dest[i]);
        }
    }

    [Test] public void ReplaceCopy_List2IteratorComparer() 
    {
        Algorithm.ReplaceCopy(src, IteratorUtil.Begin(dest), 29, 42, EqualityComparer<int>.Default);

        Assert.AreEqual(dest[dest.Length-1], Int32.MinValue);
        Assert.AreEqual(dest[dest.Length-2], Int32.MinValue);
        for(int i=0; i<count ;++i)
        {
            if(src[i] == 29)
                Assert.AreEqual(42, dest[i]);
            else
                Assert.AreEqual(src[i], dest[i]);
        }
    }
}

}
