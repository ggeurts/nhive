using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class CountTest 
{
    [Test] public void Count_Iterator() 
    {
        int[] intArray = Constants.TEST_INT_ARRAY;
        string[] stringArray = Constants.TEST_STRING_ARRAY;

        int count = Algorithm.Count(IteratorUtil.Begin(intArray), IteratorUtil.End(intArray), 29);
        Assert.AreEqual(4, count);
        
        count = Algorithm.Count(IteratorUtil.Begin(stringArray), IteratorUtil.End(stringArray), "Frank");
        Assert.AreEqual(3, count);
    }

    [Test] public void Count_Enumerator() 
    {
        int count = Algorithm.Count(Constants.TEST_INT_ARRAY, 29);
        Assert.AreEqual(4, count);
        
        count = Algorithm.Count(Constants.TEST_STRING_ARRAY, "Frank");
        Assert.AreEqual(3, count);
    }
}

}
