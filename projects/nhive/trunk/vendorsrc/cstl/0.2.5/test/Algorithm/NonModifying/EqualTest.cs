using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class EqualTest 
{
    [Test] public void Equal_Iterator() 
    {
        int[] range1 = Constants.TEST_INT_ARRAY;
        List<int> range2 = new List<int>(range1);

        Assert.IsTrue(Algorithm.Equal(IteratorUtil.Begin(range1), IteratorUtil.End(range1), IteratorUtil.Begin(range2)));

        --range2[5];
        Assert.IsFalse(Algorithm.Equal(IteratorUtil.Begin(range1), IteratorUtil.End(range1), IteratorUtil.Begin(range2)));
    }

    [Test] public void Equal_IteratorComparer() 
    {
        int[] range1 = Constants.TEST_INT_ARRAY;
        List<int> range2 = new List<int>(range1);

        Assert.IsTrue(Algorithm.Equal(IteratorUtil.Begin(range1), IteratorUtil.End(range1), IteratorUtil.Begin(range2), EqualityComparer<int>.Default));

        --range2[5];
        Assert.IsFalse(Algorithm.Equal(IteratorUtil.Begin(range1), IteratorUtil.End(range1), IteratorUtil.Begin(range2), EqualityComparer<int>.Default));
    }

    [Test] public void Equal_Enumerator() 
    {
        int[] range1 = Constants.TEST_INT_ARRAY;
        List<int> range2 = new List<int>(range1);

        Assert.IsTrue(Algorithm.Equal(range1, range2));

        --range2[5];
        Assert.IsFalse(Algorithm.Equal(range1, range2));
    } 

    [Test] public void Equal_EnumeratorComparer() 
    {
        int[] range1 = Constants.TEST_INT_ARRAY;
        List<int> range2 = new List<int>(range1);

        Assert.IsTrue(Algorithm.Equal(range1, range2, EqualityComparer<int>.Default));

        --range2[5];
        Assert.IsFalse(Algorithm.Equal(range1, range2, EqualityComparer<int>.Default));
    } 
}

}
