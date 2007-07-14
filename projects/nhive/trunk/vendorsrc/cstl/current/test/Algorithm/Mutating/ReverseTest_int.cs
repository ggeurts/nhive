using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class ReverseTest_Int : BaseIntTest
{
    [Test] public void Reverse_Iterator() 
    {
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        Algorithm.Copy(src, destIter);
        
        ListIterator<int> endIter = IteratorUtil.End(dest);
        endIter.MovePrev();
        
        Algorithm.Reverse(destIter, endIter);
        
        Assert.AreEqual(dest[0], Int32.MinValue);
        Assert.AreEqual(dest[dest.Length-1], Int32.MinValue);
        for(int i=0; i<count ;++i)
        {
            Assert.AreEqual(src[i], dest[dest.Length-2-i]);
        }
    }

    [Test] public void Reverse_List() 
    {
        List<int> src2 = new List<int>(src);
        src2.Reverse();
        
        Algorithm.Reverse(src2);
        
        for(int i=0; i<count ;++i)
        {
            Assert.AreEqual(src[i], src2[i]);
        }
    }
}

}
