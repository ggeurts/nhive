using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class FindEndTest 
{
    int[] array;
    int[] search;
    
    [SetUp] public void SetUp()
    {
        array  = Constants.TEST_BIG_INT_ARRAY;
        search = new int[]{5,3,3,4};
    }

    [Test] public void FindEnd_Iterator() 
    {
        ForwardIterator<int> result = Algorithm.FindEnd(IteratorUtil.Begin(array), IteratorUtil.End(array), 
                                                        IteratorUtil.Begin(search), IteratorUtil.End(search));
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(typeof(ListIterator<int>), result);
        Assert.AreEqual(array.Length-9, IteratorUtil.Distance(IteratorUtil.Begin(array), result));
        Assert.AreEqual(5, result.Read());
    }

    [Test] public void FindEnd_Comparer() 
    {
        ForwardIterator<int> result = Algorithm.FindEnd(IteratorUtil.Begin(array), IteratorUtil.End(array), 
                                                        IteratorUtil.Begin(search), IteratorUtil.End(search),
                                                        EqualityComparer<int>.Default);
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(typeof(ListIterator<int>), result);
        Assert.AreEqual(array.Length-9, IteratorUtil.Distance(IteratorUtil.Begin(array), result));
        Assert.AreEqual(5, result.Read());
    }
    
    [Test] public void FindEnd_Predicate() 
    {
        ForwardIterator<int> result = Algorithm.FindEnd(IteratorUtil.Begin(array), IteratorUtil.End(array), 
                                                        IteratorUtil.Begin(search), IteratorUtil.End(search),
                                                        Functional.EqualTo<int>);
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(typeof(ListIterator<int>), result);
        Assert.AreEqual(array.Length-9, IteratorUtil.Distance(IteratorUtil.Begin(array), result));
        Assert.AreEqual(5, result.Read());
    }
}
}

