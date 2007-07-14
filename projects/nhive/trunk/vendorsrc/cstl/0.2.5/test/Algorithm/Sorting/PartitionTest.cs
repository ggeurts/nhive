using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class PartitionTest : BaseIntTest
{
    //[Test] public void QuickSort() 
    //{
    //    ListIterator<int> begin  = IteratorUtil.Begin(src);
    //    ListIterator<int> end    = IteratorUtil.End(src);
    //    Algorithm.QuickSort(begin, end, Functional.Compare<int>);
    //}

    //[Test] public void Partition_Iterator() 
    //{
    //    ListIterator<int> begin  = IteratorUtil.Begin(src);
    //    ListIterator<int> end    = IteratorUtil.End(src);
    //    ListIterator<int> result = Algorithm.Partition(begin, end, delegate(int x) {return x<13;});
        
    //    Assert.AreEqual(0, begin.Position);
    //    Assert.AreEqual(src.Length, end.Position);
    //    Assert.IsNotNull(result);
    //    Assert.IsTrue(result.Read() >= 13);
        
    //    while(!begin.Equals(result))
    //    {
    //        Assert.IsTrue(begin.Read() < 13);
    //        begin.MoveNext();
    //    }
        
    //    while(!result.Equals(end))
    //    {
    //        Assert.IsTrue(result.Read() >= 13);
    //        result.MoveNext();
    //    }
    //}
    
    //[Test] public void Partition_RandomAccessIterator() 
    //{
    //    RandomAccessIterator<int> begin  = IteratorUtil.Begin(src);
    //    RandomAccessIterator<int> end    = IteratorUtil.End(src);
    //    RandomAccessIterator<int> result = Algorithm.Partition(begin, end, delegate(int x) {return x<13;});
        
    //    Assert.AreEqual(0, begin.Position);
    //    Assert.AreEqual(src.Length, end.Position);
    //    Assert.IsNotNull(result);
    //    Assert.IsTrue(result.Read() >= 13);
        
    //    while(!begin.Equals(result))
    //    {
    //        Assert.IsTrue(begin.Read() < 13);
    //        begin.MoveNext();
    //    }
        
    //    while(!result.Equals(end))
    //    {
    //        Assert.IsTrue(result.Read() >= 13);
    //        result.MoveNext();
    //    }
    //}

    //[Test] public void Partition_BidirectionalIterator() 
    //{
    //    BidirectionalIterator<int> begin  = IteratorUtil.Begin(src);
    //    BidirectionalIterator<int> end    = IteratorUtil.End(src);
    //    BidirectionalIterator<int> result = Algorithm.Partition(begin, end, delegate(int x) {return x<13;});
        
    //    Assert.IsNotNull(result);
    //    Assert.IsTrue(result.Read() >= 13);
        
    //    while(!begin.Equals(result))
    //    {
    //        Assert.IsTrue(begin.Read() < 13);
    //        begin.MoveNext();
    //    }
        
    //    while(!result.Equals(end))
    //    {
    //        Assert.IsTrue(result.Read() >= 13);
    //        result.MoveNext();
    //    }
    //}

    //[Test] public void Partition_List() 
    //{
    //    ListIterator<int> result = Algorithm.Partition(src, delegate(int x) {return x<13;});
        
    //    Assert.IsNotNull(result);
    //    Assert.IsTrue(result.Read() >= 13);
        
    //    for(int i=0; i<result.Position; ++i)
    //    {
    //        Assert.IsTrue(src[i] < 13);
    //    }
        
    //    for(int i=result.Position; i<src.Length; ++i)
    //    {
    //        Assert.IsTrue(src[i] >= 13);
    //    }
    //}

    //[Test] public void Partition_List_Indexes() 
    //{
    //    src[0] = 100;
    //    src[src.Length-1] = -100;
    //    ListIterator<int> result = Algorithm.Partition(src, 1, src.Length - 2, delegate(int x) {return x<13;});

    //    Assert.IsNotNull(result);
    //    Assert.IsTrue(result.Read() >= 13);

    //    for(int i=1; i<result.Position; ++i)
    //    {
    //        Assert.IsTrue(src[i] < 13);
    //    }
        
    //    for(int i=result.Position; i<src.Length-1; ++i)
    //    {
    //        Assert.IsTrue(src[i] >= 13);
    //    }
        
    //    Assert.AreEqual(100, src[0]);
    //    Assert.AreEqual(-100, src[src.Length-1]);
    //}
    
    //[Test] public void Partition_Pivot() 
    //{
    //    ListIterator<int> begin  = IteratorUtil.Begin(src);
    //    ListIterator<int> end    = IteratorUtil.End(src);
    //    ListIterator<int> pivot  = new ListIterator<int>(src,src.Length-1);
    //    int pivotValue = pivot.Read();
        
    //    ListIterator<int> result = Algorithm.Partition(begin, pivot, end);
    //    Assert.AreEqual(0, begin.Position);
    //    Assert.AreEqual(src.Length, end.Position);
    //    Assert.IsNotNull(result);
    //    Assert.AreEqual(pivotValue, result.Read());
        
    //    while(!begin.Equals(result))
    //    {
    //        Assert.IsTrue(begin.Read() < pivotValue);
    //        begin.MoveNext();
    //    }
        
    //    while(!result.Equals(end))
    //    {
    //        Assert.IsTrue(result.Read() >= pivotValue);
    //        result.MoveNext();
    //    }
    //}
    
    //[Test] public void Partition_Pivot_Comparer() 
    //{
    //    ListIterator<int> begin  = IteratorUtil.Begin(src);
    //    ListIterator<int> end    = IteratorUtil.End(src);
    //    ListIterator<int> pivot  = new ListIterator<int>(src,src.Length-1);
    //    int pivotValue = pivot.Read();
        
    //    ListIterator<int> result = Algorithm.Partition(begin, pivot, end, Comparer<int>.Default);
    //    Assert.AreEqual(0, begin.Position);
    //    Assert.AreEqual(src.Length, end.Position);
    //    Assert.IsNotNull(result);
    //    Assert.AreEqual(pivotValue, result.Read());
        
    //    while(!begin.Equals(result))
    //    {
    //        Assert.IsTrue(begin.Read() < pivotValue);
    //        begin.MoveNext();
    //    }
        
    //    while(!result.Equals(end))
    //    {
    //        Assert.IsTrue(result.Read() >= pivotValue);
    //        result.MoveNext();
    //    }
    //}    

    //[Test] public void Partition_Pivot_Comparison() 
    //{
    //    ListIterator<int> begin  = IteratorUtil.Begin(src);
    //    ListIterator<int> end    = IteratorUtil.End(src);
    //    ListIterator<int> pivot  = new ListIterator<int>(src,src.Length-1);
    //    int pivotValue = pivot.Read();
        
    //    ListIterator<int> result = Algorithm.Partition(begin, pivot, end, Functional.Compare<int>);
    //    Assert.AreEqual(0, begin.Position);
    //    Assert.AreEqual(src.Length, end.Position);
    //    Assert.IsNotNull(result);
    //    Assert.AreEqual(pivotValue, result.Read());
        
    //    while(!begin.Equals(result))
    //    {
    //        Assert.IsTrue(begin.Read() < pivotValue);
    //        begin.MoveNext();
    //    }
        
    //    while(!result.Equals(end))
    //    {
    //        Assert.IsTrue(result.Read() >= pivotValue);
    //        result.MoveNext();
    //    }
    //}      
#if NEVER    
    [Test] public void Sort_Iterator_Comparer() 
    {
        ListIterator<int> begin = IteratorUtil.Begin(src);
        ListIterator<int> end   = IteratorUtil.End(src);
        Algorithm.Sort(begin, end, Comparer<int>.Default);
        
        Assert.AreEqual(0, begin.Position);
        Assert.AreEqual(src.Length, end.Position);
        ValidateOutput();
    }

    [Test] public void Sort_Iterator_Predicate() 
    {
        ListIterator<int> begin = IteratorUtil.Begin(src);
        ListIterator<int> end   = IteratorUtil.End(src);
        Algorithm.Sort(begin, end, LessThan);
        
        Assert.AreEqual(0, begin.Position);
        Assert.AreEqual(src.Length, end.Position);
        ValidateOutput();
    }

    [Test] public void Sort_List() 
    {
        Algorithm.Sort(src);
        ValidateOutput();
    }
    
    [Test] public void Sort_List_Comparer() 
    {
        Algorithm.Sort(src, Comparer<int>.Default);
        ValidateOutput();
    }

    [Test] public void Sort_List_Predicate() 
    {
        Algorithm.Sort(src, LessThan);
        ValidateOutput();
    }
    
    static bool LessThan(int lhs, int rhs)
    {
        return lhs<rhs;
    }
#endif
}
}

