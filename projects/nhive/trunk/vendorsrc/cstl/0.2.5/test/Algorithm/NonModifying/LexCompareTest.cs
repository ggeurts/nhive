using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class LexCompareTest 
{
    int[] array1;
    int[] array2;
    int[] array3;

    ListIterator<int> begin1 ;
    ListIterator<int> end1   ;
    ListIterator<int> begin2 ;
    ListIterator<int> end2   ;
    ListIterator<int> begin3 ;
    ListIterator<int> end3   ;

    [SetUp] public void SetUp()
    {
        array1 = new int[]{ 1, 2, 3, 4, 5};
        array2 = new int[]{ 1, 2, 3, 4, 5};
        array3 = new int[]{ 1, 2, 3, 4, 5, 1};
        begin1 = IteratorUtil.Begin(array1);
        end1   = IteratorUtil.End(array1);
        begin2 = IteratorUtil.Begin(array2);
        end2   = IteratorUtil.End(array2);
        begin3 = IteratorUtil.Begin(array3);
        end3   = IteratorUtil.End(array3);
        
    }
    
    [Test] public void LexCompare_Iterator() 
    {
        Assert.IsFalse(Algorithm.LexCompare(begin1, end1, begin1, end1));
        Assert.IsFalse(Algorithm.LexCompare(begin1, end1, begin2, end2));
        Assert.IsFalse(Algorithm.LexCompare(begin2, end2, begin1, end1));
        
        Assert.IsTrue (Algorithm.LexCompare(begin1, end1, begin3, end3));
        Assert.IsFalse(Algorithm.LexCompare(begin3, end3, begin1, end1));
        
        array2[0] = -array2[0];
        Assert.IsFalse(Algorithm.LexCompare(begin1, end1, begin2, end2));
        Assert.IsTrue (Algorithm.LexCompare(begin2, end2, begin1, end1));
        array2[0] = -array2[0];
        array2[2] = -array2[2];
        Assert.IsFalse(Algorithm.LexCompare(begin1, end1, begin2, end2));
        Assert.IsTrue (Algorithm.LexCompare(begin2, end2, begin1, end1));
        array2[2] = -array2[2];
        array2[4] = -array2[4];
        Assert.IsFalse(Algorithm.LexCompare(begin1, end1, begin2, end2));
        Assert.IsTrue (Algorithm.LexCompare(begin2, end2, begin1, end1));
    }

    [Test] public void LexCompare_Comparer() 
    {
        Assert.IsFalse(Algorithm.LexCompare(begin1, end1, begin1, end1, Comparer<int>.Default));
        Assert.IsFalse(Algorithm.LexCompare(begin1, end1, begin2, end2, Comparer<int>.Default));
        Assert.IsFalse(Algorithm.LexCompare(begin2, end2, begin1, end1, Comparer<int>.Default));
        
        Assert.IsTrue (Algorithm.LexCompare(begin1, end1, begin3, end3, Comparer<int>.Default));
        Assert.IsFalse(Algorithm.LexCompare(begin3, end3, begin1, end1, Comparer<int>.Default));
        
        array2[0] = -array2[0];
        Assert.IsFalse(Algorithm.LexCompare(begin1, end1, begin2, end2, Comparer<int>.Default));
        Assert.IsTrue (Algorithm.LexCompare(begin2, end2, begin1, end1, Comparer<int>.Default));
        array2[0] = -array2[0];
        array2[2] = -array2[2];
        Assert.IsFalse(Algorithm.LexCompare(begin1, end1, begin2, end2, Comparer<int>.Default));
        Assert.IsTrue (Algorithm.LexCompare(begin2, end2, begin1, end1, Comparer<int>.Default));
        array2[2] = -array2[2];
        array2[4] = -array2[4];
        Assert.IsFalse(Algorithm.LexCompare(begin1, end1, begin2, end2, Comparer<int>.Default));
        Assert.IsTrue (Algorithm.LexCompare(begin2, end2, begin1, end1, Comparer<int>.Default));
    }
}
}