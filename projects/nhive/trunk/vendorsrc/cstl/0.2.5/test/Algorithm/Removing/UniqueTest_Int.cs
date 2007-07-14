using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL.Iterator;
using CSTL;

namespace CSTL.Test
{

[TestFixture] 
[Category("Algorithm")]
public class UniqueTest_Int : BaseIntTest
{
    [Test] public void Unique_Iterator() 
    {
        ListIterator<int> result = Algorithm.Unique(IteratorUtil.Begin(src), IteratorUtil.End(src));
        VerifyOutput();

        Assert.IsNotNull(result);
        Assert.AreEqual(5, result.Position);
    }

    [Test] public void Unique_Iterator_Comparer() 
    {
        ListIterator<int> result = Algorithm.Unique(IteratorUtil.Begin(src), IteratorUtil.End(src), EqualityComparer<int>.Default);
        VerifyOutput();

        Assert.IsNotNull(result);
        Assert.AreEqual(5, result.Position);
    }

    [Test] public void Unique_Iterator_Predicate() 
    {
        ListIterator<int> result = Algorithm.Unique(IteratorUtil.Begin(src), IteratorUtil.End(src), delegate(int lhs, int rhs) {return lhs == rhs;});
        VerifyOutput();

        Assert.IsNotNull(result);
        Assert.AreEqual(5, result.Position);
    }

    [Test] public void Unique_ForwardIterator() 
    {
        ForwardIterator<int> begin  = IteratorUtil.Begin(src);
        ForwardIterator<int> end    = IteratorUtil.End(src);
        ForwardIterator<int> result = Algorithm.Unique(begin, end);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(5, IteratorUtil.Distance(begin, result));
    }

    [Test] public void Unique_ForwardIterator_Comparer() 
    {
        ForwardIterator<int> begin  = IteratorUtil.Begin(src);
        ForwardIterator<int> end    = IteratorUtil.End(src);
        ForwardIterator<int> result = Algorithm.Unique(begin, end, EqualityComparer<int>.Default);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(5, IteratorUtil.Distance(begin, result));
    }

    [Test] public void Unique_ForwardIterator_Predicate()
    {
        ForwardIterator<int> begin  = IteratorUtil.Begin(src);
        ForwardIterator<int> end    = IteratorUtil.End(src);
        ForwardIterator<int> result = Algorithm.Unique(begin, end, delegate(int lhs, int rhs) {return lhs == rhs;});
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(5, IteratorUtil.Distance(begin, result));
    }

    [Test] public void Unique_RandomAccessIterator() 
    {
        RandomAccessIterator<int> begin  = IteratorUtil.Begin(src);
        RandomAccessIterator<int> end    = IteratorUtil.End(src);
        RandomAccessIterator<int> result = Algorithm.Unique(begin, end);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(5, IteratorUtil.Distance(begin, result));
    }

    [Test] public void Unique_RandomAccessIterator_Comparer() 
    {
        RandomAccessIterator<int> begin  = IteratorUtil.Begin(src);
        RandomAccessIterator<int> end    = IteratorUtil.End(src);
        RandomAccessIterator<int> result = Algorithm.Unique(begin, end, EqualityComparer<int>.Default);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(5, IteratorUtil.Distance(begin, result));
    }

    [Test] public void Unique_RandomAccessIterator_Predicate()
    {
        RandomAccessIterator<int> begin  = IteratorUtil.Begin(src);
        RandomAccessIterator<int> end    = IteratorUtil.End(src);
        RandomAccessIterator<int> result = Algorithm.Unique(begin, end, delegate(int lhs, int rhs) {return lhs == rhs;});
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(5, IteratorUtil.Distance(begin, result));
    }
    
    [Test] public void  Unique_List() 
    {
        Algorithm.Unique(src);
        VerifyOutput();
    }

    [Test] public void  Unique_List_Comparer() 
    {
        Algorithm.Unique(src, EqualityComparer<int>.Default);
        VerifyOutput();
    }

    [Test] public void  Unique_List_Predicate() 
    {
        Algorithm.Unique(src, delegate(int lhs, int rhs) {return lhs == rhs;});
        VerifyOutput();
    }

    protected override int[] GetValues()
    {
        int[] result = Constants.TEST_BIG_INT_ARRAY;
        Array.Sort(result);
        return result;
    }

    void VerifyOutput()
    {
        Assert.AreEqual(src[0], 1);
        Assert.AreEqual(src[1], 2);
        Assert.AreEqual(src[2], 3);
        Assert.AreEqual(src[3], 4);
        Assert.AreEqual(src[4], 5);
    }
}

}
