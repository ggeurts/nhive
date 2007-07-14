                                     using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class SearchNTest 
{
    [Test] public void SearchN_Iterator() 
    {
        int[] array  = Constants.TEST_BIG_INT_ARRAY;
        ForwardIterator<int> result = Algorithm.SearchN(IteratorUtil.Begin(array), IteratorUtil.End(array), 3,1);
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(typeof(ListIterator<int>), result);
        Assert.AreEqual(5, IteratorUtil.Distance(IteratorUtil.Begin(array), result));
        Assert.AreEqual(array[5], result.Read());
    }

    [Test] public void SearchN_Comparer() 
    {
        int[] array  = Constants.TEST_BIG_INT_ARRAY;
        ForwardIterator<int> result = Algorithm.SearchN(IteratorUtil.Begin(array), IteratorUtil.End(array), 
                                                        3,1, EqualityComparer<int>.Default);
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(typeof(ListIterator<int>), result);
        Assert.AreEqual(5, IteratorUtil.Distance(IteratorUtil.Begin(array), result));
        Assert.AreEqual(array[5], result.Read());
    }
    
    [Test] public void Search_Predicate() 
    {
        int[] array  = Constants.TEST_BIG_INT_ARRAY;
        ForwardIterator<int> result = Algorithm.SearchN(IteratorUtil.Begin(array), IteratorUtil.End(array), 
                                                        3, Functional.Bind2ndPred(Functional.EqualTo<int>, 1));
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(typeof(ListIterator<int>), result);
        Assert.AreEqual(5, IteratorUtil.Distance(IteratorUtil.Begin(array), result));
        Assert.AreEqual(array[5], result.Read());
    }


}
}

