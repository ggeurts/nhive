using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class SortTest : BaseIntTest
{
    [Test] public void Sort_Iterator() 
    {
        ListIterator<int> begin = IteratorUtil.Begin(src);
        ListIterator<int> end   = IteratorUtil.End(src);
        Algorithm.Sort(begin, end);
        
        Assert.AreEqual(0, begin.Position);
        Assert.AreEqual(src.Length, end.Position);
        ValidateOutput();
    }
    
    [Test] public void Sort_Iterator_Comparer() 
    {
        ListIterator<int> begin = IteratorUtil.Begin(src);
        ListIterator<int> end   = IteratorUtil.End(src);
        Algorithm.Sort(begin, end, Comparer<int>.Default);
        
        Assert.AreEqual(0, begin.Position);
        Assert.AreEqual(src.Length, end.Position);
        ValidateOutput();
    }

#if SORT_WITH_BINARYPREDICATE
    [Test] public void Sort_Iterator_Predicate() 
    {
        ListIterator<int> begin = IteratorUtil.Begin(src);
        ListIterator<int> end   = IteratorUtil.End(src);
        Algorithm.Sort(begin, end, LessThan);
        
        Assert.AreEqual(0, begin.Position);
        Assert.AreEqual(src.Length, end.Position);
        ValidateOutput();
    }
#else
    [Test] public void Sort_Iterator_Predicate() 
    {
        ListIterator<int> begin = IteratorUtil.Begin(src);
        ListIterator<int> end   = IteratorUtil.End(src);
        Algorithm.Sort(begin, end, Functional.Compare);
        
        Assert.AreEqual(0, begin.Position);
        Assert.AreEqual(src.Length, end.Position);
        ValidateOutput();
    }
#endif

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

#if SORT_WITH_BINARYPREDICATE
    [Test] public void Sort_List_Predicate() 
    {
        Algorithm.Sort(src, LessThan);
        ValidateOutput();
    }
#else
    [Test] public void Sort_List_Predicate() 
    {
        Algorithm.Sort(src, Functional.Compare);
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

