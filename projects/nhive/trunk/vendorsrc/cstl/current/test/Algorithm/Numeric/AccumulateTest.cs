using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL.Iterator;
using CSTL;

namespace CSTL.Test
{

[TestFixture] 
[Category("Algorithm")]
public class AccumulateTest_Int 
{
    static int Add(int lhs, int rhs)
    {
        return lhs + rhs;
    }
    
    [Test] public void Accumulate_Iterator()
    {
        int[] array = Constants.TEST_INT_ARRAY;
        int sum = Algorithm.Accumulate(IteratorUtil.Begin(array), IteratorUtil.End(array), 0, Add);
        
        int expectedTotal = 0;
        foreach (int value in array)
            expectedTotal +=  value;

         Assert.AreEqual(expectedTotal, sum);
    }

    [Test] public void Accumulate_Enumerator()
    {
        int[] array = Constants.TEST_INT_ARRAY;
        int sum = Algorithm.Accumulate(array, 0, Add);
        
        int expectedTotal = 0;
        foreach (int value in array)
            expectedTotal +=  value;

         Assert.AreEqual(expectedTotal, sum);
    }
}

}