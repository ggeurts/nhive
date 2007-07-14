using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;
using CSTL.Utility;

namespace CSTL.Test
{

[TestFixture] 
[Category("Algorithm")]
public class MismatchTest 
{
    int[] array1;
    List<int> array2;
    
    [SetUp]
    public void SetUp()
    {
        array1 = Constants.TEST_INT_ARRAY;
        array2 = new List<int>();
        array2.Add(-100);
        array2.Add(-101);
        array2.AddRange(Constants.TEST_INT_ARRAY);
    }

    [Test] public void Mismatch_ListIterator()
    {
        array1[7]++;
        ListIterator<int> cmpIter = IteratorUtil.Begin(array2);
        cmpIter.MoveNext();
        cmpIter.MoveNext();
        
        Pair<ListIterator<int>, ListIterator<int>> p = Algorithm.Mismatch(IteratorUtil.Begin(array1), IteratorUtil.End(array1), cmpIter);
        Assert.AreEqual(7, p.First.Position);
        Assert.AreEqual(array1[7], p.First.Read());
        Assert.AreEqual(9, p.Second.Position);
        Assert.AreEqual(array2[9], p.Second.Read());
        
        p = Algorithm.Mismatch(IteratorUtil.Begin(array1), IteratorUtil.End(array1), cmpIter, EqualityComparer<int>.Default);
        Assert.AreEqual(7, p.First.Position);
        Assert.AreEqual(array1[7], p.First.Read());
        Assert.AreEqual(9, p.Second.Position);
        Assert.AreEqual(array2[9], p.Second.Read());        
    }
    

    [Test] public void Mismatch_RandomIterator()
    {
        array1[7]++;
        ListIterator<int> cmpIter = IteratorUtil.Begin(array2);
        cmpIter.MoveNext();
        cmpIter.MoveNext();

        RandomAccessIterator<int> beginR = IteratorUtil.Begin(array1);
        RandomAccessIterator<int> endR   = IteratorUtil.End(array1);
        RandomAccessIterator<int> cmpR   = cmpIter;
        Pair<RandomAccessIterator<int>, RandomAccessIterator<int>> pR = Algorithm.Mismatch(beginR, endR, cmpR);
        Assert.AreEqual(7, pR.First.Position);
        Assert.AreEqual(array1[7], pR.First.Read());
        Assert.IsInstanceOfType(typeof(ListIterator<int>), pR.First);
        Assert.AreEqual(9, pR.Second.Position);
        Assert.AreEqual(array2[9], pR.Second.Read());
        Assert.IsInstanceOfType(typeof(ListIterator<int>), pR.Second);
        
        pR = Algorithm.Mismatch(beginR, endR, cmpR, EqualityComparer<int>.Default);
        Assert.AreEqual(7, pR.First.Position);
        Assert.AreEqual(array1[7], pR.First.Read());
        Assert.IsInstanceOfType(typeof(ListIterator<int>), pR.First);
        Assert.AreEqual(9, pR.Second.Position);
        Assert.AreEqual(array2[9], pR.Second.Read());
        Assert.IsInstanceOfType(typeof(ListIterator<int>), pR.Second);        
    }

    [Test] public void Mismatch_ForwardIterator()
    {
        array1[7]++;
        ListIterator<int> cmpIter = IteratorUtil.Begin(array2);
        cmpIter.MoveNext();
        cmpIter.MoveNext();

        ForwardIterator<int> beginF = IteratorUtil.Begin(array1);
        ForwardIterator<int> endF   = IteratorUtil.End(array1);
        ForwardIterator<int> cmpF   = cmpIter;
        Pair<ForwardIterator<int>, ForwardIterator<int>> pF = Algorithm.Mismatch(beginF, endF, cmpF);
        Assert.AreEqual(array1[7], pF.First.Read());
        Assert.IsInstanceOfType(typeof(ListIterator<int>), pF.First);
        Assert.AreEqual(array2[9], pF.Second.Read());
        Assert.IsInstanceOfType(typeof(ListIterator<int>), pF.Second);

        pF = Algorithm.Mismatch(beginF, endF, cmpF, EqualityComparer<int>.Default);
        Assert.AreEqual(array1[7], pF.First.Read());
        Assert.IsInstanceOfType(typeof(ListIterator<int>), pF.First);
        Assert.AreEqual(array2[9], pF.Second.Read());
        Assert.IsInstanceOfType(typeof(ListIterator<int>), pF.Second);
    }

    [Test] public void Mismatch_InputIterator()
    {
        array1[7]++;
        ListIterator<int> cmpIter = IteratorUtil.Begin(array2);
        cmpIter.MoveNext();
        cmpIter.MoveNext();

        InputIterator<int> begin = IteratorUtil.Begin(array1);
        InputIterator<int> end   = IteratorUtil.End(array1);
        InputIterator<int> cmp   = cmpIter;
        Pair<InputIterator<int>, InputIterator<int>> p = Algorithm.Mismatch(begin, end, cmp);
        Assert.AreEqual(array1[7], p.First.Read());
        Assert.IsInstanceOfType(typeof(ListIterator<int>), p.First);
        Assert.AreEqual(array2[9], p.Second.Read());
        Assert.IsInstanceOfType(typeof(ListIterator<int>), p.Second);
        
        p = Algorithm.Mismatch(begin, end, cmp, EqualityComparer<int>.Default);
        Assert.AreEqual(array1[7], p.First.Read());
        Assert.IsInstanceOfType(typeof(ListIterator<int>), p.First);
        Assert.AreEqual(array2[9], p.Second.Read());
        Assert.IsInstanceOfType(typeof(ListIterator<int>), p.Second);
    }
}

}