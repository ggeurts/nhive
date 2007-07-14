using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class ReplaceIfTest_Int : BaseIntTest
{

    static bool Is29(int value)
    {
        return value == 29;
    }

    [Test] public void ReplaceIf_Iterator() 
    {
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        Algorithm.Copy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter); // we assume Algorithm.Copy is good. It has its own unit test.
        
        Algorithm.ReplaceIf(IteratorUtil.Begin(dest), IteratorUtil.End(dest), 42, Is29);
        
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

    [Test] public void ReplaceIf_List() 
    {
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        Algorithm.Copy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter); // we assume Algorithm.Copy is good. It has its own unit test.
        
        Algorithm.ReplaceIf(dest, 42, Is29);
        
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
