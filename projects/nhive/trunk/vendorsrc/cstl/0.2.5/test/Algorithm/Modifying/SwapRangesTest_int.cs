using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class SwapRangesTest_Int : BaseIntTest
{
    [Test] public void SwapRanges_Iterator() 
    {
        int[] src2 = new int[src.Length];
        for(int i=0; i<count; ++i)
            src2[i] = -src[i];
        
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        Algorithm.Copy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter); // we assume Algorithm.Copy is good. It has its own unit test.
        
        Algorithm.SwapRanges(IteratorUtil.Begin(src2), IteratorUtil.End(src2), destIter);
        
        Assert.AreEqual(dest[0], Int32.MinValue);
        Assert.AreEqual(dest[dest.Length-1], Int32.MinValue);
        for(int i=0; i<count ;++i)
        {
            Assert.AreEqual(-src[i], dest[i+1]);
            Assert.AreEqual(src[i],  src2[i]);
        }
    }
}

}
