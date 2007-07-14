using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class ReplaceTest_Int : BaseIntTest
{
    [Test] public void Replace_Iterator() 
    {
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        Algorithm.Copy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter); // we assume Algorithm.Copy is good. It has its own unit test.
        
        Algorithm.Replace(IteratorUtil.Begin(dest), IteratorUtil.End(dest), 29, 42);
        
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

    [Test] public void Replace_IteratorComparer() 
    {
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        Algorithm.Copy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter); // we assume Algorithm.Copy is good. It has its own unit test.
        
        Algorithm.Replace(IteratorUtil.Begin(dest), IteratorUtil.End(dest), 29, 42, EqualityComparer<int>.Default);
        
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

    [Test] public void Replace_List() 
    {
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        Algorithm.Copy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter); // we assume Algorithm.Copy is good. It has its own unit test.
        
        Algorithm.Replace(dest, 29, 42);
        
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


    [Test] public void Replace_ListComparer() 
    {
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        Algorithm.Copy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter); // we assume Algorithm.Copy is good. It has its own unit test.
        
        Algorithm.Replace(dest, 29, 42, EqualityComparer<int>.Default);
        
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
}

}
