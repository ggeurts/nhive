using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class StableSortTest : BaseIntTest
{
    [Test] public void StableSort_Iterator() 
    {
        ListIterator<int> begin = IteratorUtil.Begin(src);
        ListIterator<int> end   = IteratorUtil.End(src);
        Algorithm.StableSort(begin, end);
        
        Assert.AreEqual(0, begin.Position);
        Assert.AreEqual(src.Length, end.Position);
        ValidateOutput();
    }
    
    [Test] public void StableSort_Iterator_Comparer() 
    {
        ListIterator<int> begin = IteratorUtil.Begin(src);
        ListIterator<int> end   = IteratorUtil.End(src);
        Algorithm.StableSort(begin, end, Comparer<int>.Default);
        
        Assert.AreEqual(0, begin.Position);
        Assert.AreEqual(src.Length, end.Position);
        ValidateOutput();
    }

#if SORT_WITH_BINARYPREDICATE
    [Test] public void StableSort_Iterator_Predicate() 
    {
        ListIterator<int> begin = IteratorUtil.Begin(src);
        ListIterator<int> end   = IteratorUtil.End(src);
        Algorithm.StableSort(begin, end, LessThan);
        
        Assert.AreEqual(0, begin.Position);
        Assert.AreEqual(src.Length, end.Position);
        ValidateOutput();
    }
#else
    [Test] public void StableSort_Iterator_Predicate() 
    {
        ListIterator<int> begin = IteratorUtil.Begin(src);
        ListIterator<int> end   = IteratorUtil.End(src);
        Algorithm.StableSort(begin, end, Functional.Compare<int>);

        Assert.AreEqual(0, begin.Position);
        Assert.AreEqual(src.Length, end.Position);
        ValidateOutput();
    }
#endif

    [Test] public void StableSort_List() 
    {
        Algorithm.StableSort(src);
        ValidateOutput();
    }
    
    [Test] public void StableSort_List_Comparer() 
    {
        Algorithm.StableSort(src, Comparer<int>.Default);
        ValidateOutput();
    }

#if SORT_WITH_BINARYPREDICATE
    [Test] public void StableSort_List_Predicate() 
    {
        Algorithm.StableSort(src, LessThan);
        ValidateOutput();
    }
#else
    [Test] public void StableSort_List_Predicate() 
    {
        Algorithm.StableSort(src, Functional.Compare);
        ValidateOutput();
    }
#endif

    static bool LessThan(int lhs, int rhs)
    {
        return lhs<rhs;
    }

    public void ValidateOutput()
    {
        int[] array = Constants.TEST_INT_ARRAY;
        Array.Sort(array);
        
        for(int i=0; i<count; ++i)
        {
            Assert.AreEqual(array[i], src[i]);
        }
    }
}
}

