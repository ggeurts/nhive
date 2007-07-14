using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL.Iterator;
using CSTL;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class CopyTest_String : BaseStringTest
{
    [Test] public void Copy_Iterator() 
    {
        ListIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        Algorithm.Copy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter);
        
        Assert.AreEqual(dest[0], MARKER);
        Assert.AreEqual(dest[dest.Length-1], MARKER);
        for(int i=0; i<src.Length;++i)
        {
            Assert.AreEqual(dest[i+1], src[i]);
        } 
    }
    
    [Test] public void Copy_Enumerable() 
    {
        ListIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        Algorithm.Copy(src, destIter);
        
        Assert.AreEqual(dest[0], MARKER);
        Assert.AreEqual(dest[dest.Length-1], MARKER);
        for(int i=0; i<src.Length;++i)
        {
            Assert.AreEqual(dest[i+1], src[i]);
        } 
    }
   
}

}