using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL.Iterator;
using CSTL;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class CopyTest_Int : BaseIntTest
{
    [Test] public void Copy_Iterator() 
    {
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        Algorithm.Copy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter);
        
        Assert.AreEqual(dest[0], Int32.MinValue);
        Assert.AreEqual(dest[dest.Length-1], Int32.MinValue);
        for(int i=0; i<count ;++i)
        {
            Assert.AreEqual(dest[i+1], src[i]);
        } 
    }

    [Test] public void Copy_Enumerable() 
    {
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        Algorithm.Copy(src, destIter);
        
        Assert.AreEqual(dest[0], Int32.MinValue);
        Assert.AreEqual(dest[dest.Length-1], Int32.MinValue);
        for(int i=0; i<count;++i)
        {
            Assert.AreEqual(dest[i+1], src[i]);
        } 
    }  
    
    [Test] public void Copy_EnumerableToList() 
    {
        Algorithm.Copy(src, dest);
        for(int i=0; i<count;++i)
        {
            Assert.AreEqual(dest[i], src[i]);
        } 
    }   
    
    
    [Test] public void Copy_IteratorToList() 
    {
        Algorithm.Copy(IteratorUtil.Begin(src), IteratorUtil.End(src), dest);
        for(int i=0; i<count;++i)
        {
            Assert.AreEqual(dest[i], src[i]);
        } 
    }

    [Test] public void Copy_ToList() 
    {
        dest[dest.Length-2] = int.MinValue;
        Algorithm.Copy(src, 2, dest, 1);
        Assert.AreEqual(dest[0], int.MinValue);
        Assert.AreEqual(dest[dest.Length-1], int.MinValue);
        Assert.AreEqual(dest[dest.Length-2], int.MinValue);
        
        for(int i=2; i<src.Length;++i)
        {
            Assert.AreEqual(dest[i-1], src[i]);
        }     
    } 
}

}
