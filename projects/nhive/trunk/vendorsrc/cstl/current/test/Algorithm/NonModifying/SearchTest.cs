using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class SearchTest 
{
    [Test] public void Search_Iterator() 
    {
        int[] array  = Constants.TEST_INT_ARRAY;
        int[] search = new int[3];
        
        Algorithm.CopyN(array, 5, 3, search, 0);

        ForwardIterator<int> result = Algorithm.Search(IteratorUtil.Begin(array), IteratorUtil.End(array), 
                                                       IteratorUtil.Begin(search), IteratorUtil.End(search));
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(typeof(ListIterator<int>), result);
        Assert.AreEqual(5, IteratorUtil.Distance(IteratorUtil.Begin(array), result));
        Assert.AreEqual(array[5], result.Read());
    }

    [Test] public void Search_Comparer() 
    {
        int[] array  = Constants.TEST_INT_ARRAY;
        int[] search = new int[3];
        
        Algorithm.CopyN(array, 5, 3, search, 0);

        ForwardIterator<int> result = Algorithm.Search(IteratorUtil.Begin(array), IteratorUtil.End(array), 
                                                       IteratorUtil.Begin(search), IteratorUtil.End(search),
                                                       EqualityComparer<int>.Default);
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(typeof(ListIterator<int>), result);
        Assert.AreEqual(5, IteratorUtil.Distance(IteratorUtil.Begin(array), result));
        Assert.AreEqual(array[5], result.Read());
    }
    
    [Test] public void Search_Predicate() 
    {
        int[] array  = Constants.TEST_INT_ARRAY;
        int[] search = new int[3];
        
        Algorithm.CopyN(array, 5, 3, search, 0);

        ForwardIterator<int> result = Algorithm.Search(IteratorUtil.Begin(array), IteratorUtil.End(array), 
                                                       IteratorUtil.Begin(search), IteratorUtil.End(search),
                                                       Functional.EqualTo<int>);
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(typeof(ListIterator<int>), result);
        Assert.AreEqual(5, IteratorUtil.Distance(IteratorUtil.Begin(array), result));
        Assert.AreEqual(array[5], result.Read());
    }


}
}

