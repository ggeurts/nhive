using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class CountIfTest 
{
    static bool Is29(int value)
    {
        return value == 29;
    }
    
    static bool IsFrank(string value)
    {
        return value == "Frank";
    }

    [Test] public void CountIf_Iterator() 
    {
        int[] intArray = Constants.TEST_INT_ARRAY;
        string[] stringArray = Constants.TEST_STRING_ARRAY;

        int count = Algorithm.CountIf(IteratorUtil.Begin(intArray), IteratorUtil.End(intArray), Is29);
        Assert.AreEqual(4, count);
        
        count = Algorithm.CountIf(IteratorUtil.Begin(stringArray), IteratorUtil.End(stringArray), IsFrank);
        Assert.AreEqual(3, count);
    }

    [Test] public void CountIf_Enumerator() 
    {
        int count = Algorithm.CountIf(Constants.TEST_INT_ARRAY, Is29);
        Assert.AreEqual(4, count);
        
        count = Algorithm.CountIf(Constants.TEST_STRING_ARRAY, IsFrank);
        Assert.AreEqual(3, count);
    }
}

}
