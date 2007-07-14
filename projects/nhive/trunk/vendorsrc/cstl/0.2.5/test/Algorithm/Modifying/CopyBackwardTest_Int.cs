using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL.Iterator;
using CSTL;

namespace CSTL.Test
{

[TestFixture] 
[Category("Algorithm")]
public class CopyBackwardTest_Int : BaseIntTest
{
    [Test] public void CopyBackward_Iterator() 
    {
        ListIterator<int> destIter = IteratorUtil.End(dest);
        destIter.MovePrev();
        
        Algorithm.CopyBackward(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter);
        
        Assert.AreEqual(dest[0], Int32.MinValue);
        Assert.AreEqual(dest[dest.Length-1], Int32.MinValue);
        for(int i=1; i<dest.Length-1;++i)
        {
            Assert.AreEqual(src[i-1], dest[i]);
        } 
    }

    [Test] public void CopyBackward_List() 
    {
        ListIterator<int> destIter = IteratorUtil.End(dest);
        destIter.MovePrev();
        
        Algorithm.CopyBackward(src, destIter);
        
        Assert.AreEqual(dest[0], Int32.MinValue);
        Assert.AreEqual(dest[dest.Length-1], Int32.MinValue);
        for(int i=1; i<dest.Length-1;++i)
        {
            Assert.AreEqual(src[i-1], dest[i]);
        } 
    }  
    
    [Test] public void CopyBackward_Enumerable() 
    {
        ListIterator<int> destIter = IteratorUtil.End(dest);
        destIter.MovePrev();
        
        Algorithm.CopyBackward(src as IEnumerable<int>, destIter);
        
        Assert.AreEqual(dest[0], Int32.MinValue);
        Assert.AreEqual(dest[dest.Length-1], Int32.MinValue);
        for(int i=1; i<dest.Length-1;++i)
        {
            Assert.AreEqual(src[i-1], dest[i]);
        }
    }

    [Test] public void CopyBackword_ListToList() 
    {
        Algorithm.CopyBackward(src, dest);
        for(int s=src.Length-1,d=dest.Length-1; s>=0; --s, --d)
        {
            Assert.AreEqual(src[s], dest[d]);
        }
    }

    [Test] public void CopyBackward_IteratorToList() 
    {
        Algorithm.CopyBackward(IteratorUtil.Begin(src), IteratorUtil.End(src), dest);
        for(int s=src.Length-1,d=dest.Length-1; s>=0; --s, --d)
        {
            Assert.AreEqual(src[s], dest[d]);
        }
    }

    [Test] public void CopyBackward_ListToList_Offsets() 
    {
        Algorithm.CopyBackward(src, 1, dest, dest.Length-1);

        Assert.AreEqual(dest[0], int.MinValue);
        Assert.AreEqual(dest[1], int.MinValue);
        Assert.AreEqual(dest[dest.Length-1], int.MinValue);
//        Assert.AreEqual(dest[dest.Length-2], int.MinValue);
        
        for(int s=count-1, d=dest.Length-2; s>=1; --s, --d)
        {
            Assert.AreEqual(src[s], dest[d]);
        }
    }
}

}
