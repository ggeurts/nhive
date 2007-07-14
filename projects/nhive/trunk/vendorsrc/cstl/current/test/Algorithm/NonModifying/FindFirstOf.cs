           using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class FindFirstOfTest 
{
    int[] array;
    int[] search;
    
    [SetUp] public void SetUp()
    {
        array  = Constants.TEST_INT_ARRAY;
        search = new int[]{111,42,-1,33, 15};
    }

    [Test] public void FindFirstOf_Iterator() 
    {
        ForwardIterator<int> result = Algorithm.FindFirstOf(IteratorUtil.Begin(array), IteratorUtil.End(array), 
                                                            IteratorUtil.Begin(search), IteratorUtil.End(search));
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(typeof(ListIterator<int>), result);
        Assert.AreEqual(5, IteratorUtil.Distance(IteratorUtil.Begin(array), result));
        Assert.AreEqual(33, result.Read());
    }

    [Test] public void FindFirstOf_Comparer() 
    {
        ForwardIterator<int> result = Algorithm.FindFirstOf(IteratorUtil.Begin(array), IteratorUtil.End(array), 
                                                            IteratorUtil.Begin(search), IteratorUtil.End(search),
                                                            EqualityComparer<int>.Default);
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(typeof(ListIterator<int>), result);
        Assert.AreEqual(5, IteratorUtil.Distance(IteratorUtil.Begin(array), result));
        Assert.AreEqual(33, result.Read());
    }
    
    [Test] public void FindEnd_Predicate() 
    {
        ForwardIterator<int> result = Algorithm.FindFirstOf(IteratorUtil.Begin(array), IteratorUtil.End(array), 
                                                            IteratorUtil.Begin(search), IteratorUtil.End(search),
                                                            Functional.EqualTo<int>);
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(typeof(ListIterator<int>), result);
        Assert.AreEqual(5, IteratorUtil.Distance(IteratorUtil.Begin(array), result));
        Assert.AreEqual(33, result.Read());
    }
}
}

