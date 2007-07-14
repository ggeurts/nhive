using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class TransformTest_Int : BaseIntTest
{
    [Test] public void Transform_Unary() 
    {
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        Algorithm.Transform(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter, DoubleIt);
        Assert.AreEqual(dest[0], Int32.MinValue);
        Assert.AreEqual(dest[dest.Length-1], Int32.MinValue);
        for(int i=0; i<count ;++i)
        {
            Assert.AreEqual(src[i]*2, dest[i+1]);
        }
    }
    
    [Test] public void Transform_Binary() 
    {
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        Algorithm.Transform(IteratorUtil.Begin(src), IteratorUtil.End(src), IteratorUtil.Begin(src), destIter, AddEm);
        Assert.AreEqual(dest[0], Int32.MinValue);
        Assert.AreEqual(dest[dest.Length-1], Int32.MinValue);
        for(int i=0; i<count ;++i)
        {
            Assert.AreEqual(src[i]*2, dest[i+1]);
        }
    }

    int DoubleIt(int value)
    {
        return value * 2;
    }
    
    int AddEm(int lhs, int rhs)
    {
        return lhs + rhs;
    }
}

}
