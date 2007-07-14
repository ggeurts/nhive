using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class PrevPermutationTest 
{
    int[] array;
    ListIterator<int> begin ;
    ListIterator<int> end   ;

    [SetUp] public void SetUp()
    {
        array = new int[]{ 3, 2, 1};
        begin = IteratorUtil.Begin(array);
        end   = IteratorUtil.End(array);
    }
    
    [Test] public void PrevPermutation_Iterator() 
    {
        Assert.IsTrue(Algorithm.PrevPermutation(begin, end));
        Assert.AreEqual(new int[]{3,1,2}, array);
        Assert.IsTrue(Algorithm.PrevPermutation(begin, end));
        Assert.AreEqual(new int[]{2,3,1}, array);
        Assert.IsTrue(Algorithm.PrevPermutation(begin, end));
        Assert.AreEqual(new int[]{2,1,3}, array);
        Assert.IsTrue(Algorithm.PrevPermutation(begin, end));
        Assert.AreEqual(new int[]{1,3, 2}, array);
        Assert.IsTrue(Algorithm.PrevPermutation(begin, end));
        Assert.AreEqual(new int[]{1,2,3}, array);
        Assert.IsFalse(Algorithm.PrevPermutation(begin, end));
        Assert.AreEqual(new int[]{3,2,1}, array);
    }

    [Test] public void PrevPermutation_Comparer() 
    {
        Assert.IsTrue(Algorithm.PrevPermutation(begin, end, Comparer<int>.Default));
        Assert.AreEqual(new int[]{3,1,2}, array);         
        Assert.IsTrue(Algorithm.PrevPermutation(begin, end, Comparer<int>.Default));
        Assert.AreEqual(new int[]{2,3,1}, array);         
        Assert.IsTrue(Algorithm.PrevPermutation(begin, end, Comparer<int>.Default));
        Assert.AreEqual(new int[]{2,1,3}, array);         
        Assert.IsTrue(Algorithm.PrevPermutation(begin, end, Comparer<int>.Default));
        Assert.AreEqual(new int[]{1,3, 2}, array);        
        Assert.IsTrue(Algorithm.PrevPermutation(begin, end, Comparer<int>.Default));
        Assert.AreEqual(new int[]{1,2,3}, array);
        Assert.IsFalse(Algorithm.PrevPermutation(begin, end, Comparer<int>.Default));
        Assert.AreEqual(new int[]{3,2,1}, array);
    }
    
    [Test] public void PrevPermutation_Predicate() 
    {
        Assert.IsTrue(Algorithm.PrevPermutation(begin, end, Functional.LessThan<int>));
        Assert.AreEqual(new int[]{3,1,2}, array);         
        Assert.IsTrue(Algorithm.PrevPermutation(begin, end, Functional.LessThan<int>));
        Assert.AreEqual(new int[]{2,3,1}, array);         
        Assert.IsTrue(Algorithm.PrevPermutation(begin, end, Functional.LessThan<int>));
        Assert.AreEqual(new int[]{2,1,3}, array);         
        Assert.IsTrue(Algorithm.PrevPermutation(begin, end, Functional.LessThan<int>));
        Assert.AreEqual(new int[]{1,3, 2}, array);        
        Assert.IsTrue(Algorithm.PrevPermutation(begin, end, Functional.LessThan<int>));
        Assert.AreEqual(new int[]{1,2,3}, array);
        Assert.IsFalse(Algorithm.PrevPermutation(begin, end, Functional.LessThan<int>));
        Assert.AreEqual(new int[]{3,2,1}, array);
    }
}
}