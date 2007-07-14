using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL.Iterator;
using CSTL;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class CopyBackwardTest_String : BaseStringTest
{
    [Test] public void CopyBackward_Iterator() 
    {
        ListIterator<string> destIter = IteratorUtil.End(dest);
        destIter.MovePrev();
        
        Algorithm.CopyBackward(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter);
        
        Assert.AreEqual(dest[0], MARKER);
        Assert.AreEqual(dest[dest.Length-1], MARKER);
        for(int i=1; i<dest.Length-1;++i)
        {
            Assert.AreEqual(src[i-1], dest[i]);
        } 
    }

    [Test] public void CopyBackward_List() 
    {
        ListIterator<string> destIter = IteratorUtil.End(dest);
        destIter.MovePrev();
        
        Algorithm.CopyBackward(src, destIter);
        
        Assert.AreEqual(dest[0], MARKER);
        Assert.AreEqual(dest[dest.Length-1], MARKER);
        for(int i=1; i<dest.Length-1;++i)
        {
            Assert.AreEqual(src[i-1], dest[i]);
        } 
    }  
    
    [Test] public void CopyBackward_Enumerable() 
    {
        ListIterator<string> destIter = IteratorUtil.End(dest);
        destIter.MovePrev();
        
        Algorithm.CopyBackward(src as IEnumerable<string>, destIter);
        
        Assert.AreEqual(dest[0], MARKER);
        Assert.AreEqual(dest[dest.Length-1], MARKER);
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

        Assert.AreEqual(dest[0], MARKER);
        Assert.AreEqual(dest[1], MARKER);
        Assert.AreEqual(dest[dest.Length-1], MARKER);
        
        for(int s=count-1, d=dest.Length-2; s>=1; --s, --d)
        {
            Assert.AreEqual(src[s], dest[d]);
        }
    }
}

}
