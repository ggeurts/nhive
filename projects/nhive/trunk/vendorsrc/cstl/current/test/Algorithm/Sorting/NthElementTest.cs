using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class NthElementTest : BaseIntTest
{
    [Test] public void NthElement_Iterator() 
    {
        int[] copy = new int[src.Length];
        Algorithm.Copy(src, copy);
        Array.Sort(copy);
    
        ListIterator<int> begin  = IteratorUtil.Begin(src);
        ListIterator<int> end    = IteratorUtil.End(src);
        ListIterator<int> nth  = IteratorUtil.AdvanceCopy(begin,4);
        
        Algorithm.NthElement(begin, nth, end);
        Assert.AreEqual(0, begin.Position);
        Assert.AreEqual(src.Length, end.Position);
        Assert.AreEqual(4, nth.Position);
        
        Assert.AreEqual(copy[4], src[4]);
        
        for(int i=0; i<4; ++i)
        {
            Assert.IsTrue(src[i] < src[4]);
        }
        
        for(int i=5; i<src.Length; ++i)
        {
            Assert.IsTrue(src[i]>=src[4]);
        }
    }

    [Test] public void NthElement_Iterator_Comparer() 
    {
        int[] copy = new int[src.Length];
        Algorithm.Copy(src, copy);
        Array.Sort(copy);
    
        ListIterator<int> begin  = IteratorUtil.Begin(src);
        ListIterator<int> end    = IteratorUtil.End(src);
        ListIterator<int> nth  = IteratorUtil.AdvanceCopy(begin,4);
        int pivotValue = nth.Read();
        
        Algorithm.NthElement(begin, nth, end, Comparer<int>.Default);
        Assert.AreEqual(0, begin.Position);
        Assert.AreEqual(src.Length, end.Position);

        Assert.AreEqual(copy[4], src[4]);

        for(int i=0; i<4; ++i)
        {
            Assert.IsTrue(src[i] < src[4]);
        }
        
        for(int i=5; i<src.Length; ++i)
        {
            Assert.IsTrue(src[i]>=src[4]);
        }
    }

    [Test] public void NthElement_Iterator_Comparison() 
    {
        int[] copy = new int[src.Length];
        Algorithm.Copy(src, copy);
        Array.Sort(copy);
    
        ListIterator<int> begin  = IteratorUtil.Begin(src);
        ListIterator<int> end    = IteratorUtil.End(src);
        ListIterator<int> nth  = IteratorUtil.AdvanceCopy(begin,4);
        int pivotValue = nth.Read();
        
        Algorithm.NthElement(begin, nth, end, Functional.Compare);
        Assert.AreEqual(0, begin.Position);
        Assert.AreEqual(src.Length, end.Position);

        Assert.AreEqual(copy[4], src[4]);

        for(int i=0; i<4; ++i)
        {
            Assert.IsTrue(src[i] < src[4]);
        }
        
        for(int i=5; i<src.Length; ++i)
        {
            Assert.IsTrue(src[i]>=src[4]);
        }
    }


    [Test] public void NthElement_List() 
    {
        int[] copy = new int[src.Length];
        Algorithm.Copy(src, copy);
        Array.Sort(copy);
        
        Algorithm.NthElement(src, 4);
        Assert.AreEqual(copy[4], src[4]);

        for(int i=0; i<4; ++i)
        {
            Assert.IsTrue(src[i] < src[4]);
        }

        for(int i=5; i<src.Length; ++i)
        {
            Assert.IsTrue(src[i]>=src[4]);
        }
    }

    [Test] public void NthElement_List_Comparer() 
    {
        int[] copy = new int[src.Length];
        Algorithm.Copy(src, copy);
        Array.Sort(copy);
        
        Algorithm.NthElement(src, 4, Comparer<int>.Default);
        Assert.AreEqual(copy[4], src[4]);

        for(int i=0; i<4; ++i)
        {
            Assert.IsTrue(src[i] < src[4]);
        }

        for(int i=5; i<src.Length; ++i)
        {
            Assert.IsTrue(src[i]>=src[4]);
        }
    }

    [Test] public void NthElement_List_Comparison() 
    {
        int[] copy = new int[src.Length];
        Algorithm.Copy(src, copy);
        Array.Sort(copy);
        
        Algorithm.NthElement(src, 4, Functional.Compare);
        Assert.AreEqual(copy[4], src[4]);

        for(int i=0; i<4; ++i)
        {
            Assert.IsTrue(src[i] < src[4]);
        }

        for(int i=5; i<src.Length; ++i)
        {
            Assert.IsTrue(src[i]>=src[4]);
        }
    }
}
}
