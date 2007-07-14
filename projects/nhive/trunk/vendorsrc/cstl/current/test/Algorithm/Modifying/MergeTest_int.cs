using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL.Iterator;
using CSTL;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class MergeTest_Int
{
    int[] array1 = {1, 1, 4, 7, 8, 13, 15, 15, 29};
    int[] array2 = {-1, 1, 2, 3, 4, 5, 6, 7, 10, 14, 15, 17, 29, 42};
    int[] expected = {-1, 1, 1, 1, 2, 3, 4, 4, 5, 6, 7, 7, 8, 10, 13, 14, 15,15, 15, 17, 29, 29, 42,};


    static bool LessThan(int lhs, int rhs)
    {
        return lhs < rhs;
    }

    [Test] public void Merge_Default() 
    {
        int[] output   = new int[array1.Length + array2.Length];
        Assert.AreEqual(output.Length, expected.Length, "Bug in test code. expected array not the correct length");

        Algorithm.Merge(IteratorUtil.Begin(array1), IteratorUtil.End(array1), IteratorUtil.Begin(array2), IteratorUtil.End(array2), 
                        IteratorUtil.Begin(output)); 
        for(int i=0; i<output.Length; ++i)
        {
            Assert.AreEqual(expected[i], output[i], string.Format("Failure comparing index {0} : {1} != {2}", i, output[i], expected[i]));
        }
    }

    [Test] public void Merge_Compare() 
    {
        int[] output   = new int[array1.Length + array2.Length];
        Assert.AreEqual(output.Length, expected.Length, "Bug in test code. expected array not the correct length");

        Algorithm.Merge(IteratorUtil.Begin(array1), IteratorUtil.End(array1), IteratorUtil.Begin(array2), IteratorUtil.End(array2), 
                        IteratorUtil.Begin(output), Comparer<int>.Default); 
        for(int i=0; i<output.Length; ++i)
        {
            Assert.AreEqual(expected[i], output[i], string.Format("Failure comparing index {0} : {1} != {2}", i, output[i], expected[i]));
        }
    }

#if SORT_WITH_BINARYPREDICATE
    [Test] public void Merge_Functor() 
    {
        int[] output   = new int[array1.Length + array2.Length];
        Assert.AreEqual(output.Length, expected.Length, "Bug in test code. expected array not the correct length");

        Algorithm.Merge<int>(IteratorUtil.Begin(array1), IteratorUtil.End(array1), IteratorUtil.Begin(array2), IteratorUtil.End(array2), 
                        IteratorUtil.Begin(output), LessThan); 
        for(int i=0; i<output.Length; ++i)
        {
            Assert.AreEqual(expected[i], output[i], string.Format("Failure comparing index {0} : {1} != {2}", i, output[i], expected[i]));
        }
    }
#else
    [Test] public void Merge_Functor() 
    {
        int[] output   = new int[array1.Length + array2.Length];
        Assert.AreEqual(output.Length, expected.Length, "Bug in test code. expected array not the correct length");

        Algorithm.Merge<int>(IteratorUtil.Begin(array1), IteratorUtil.End(array1), IteratorUtil.Begin(array2), IteratorUtil.End(array2), 
                        IteratorUtil.Begin(output), Functional.Compare); 
        for(int i=0; i<output.Length; ++i)
        {
            Assert.AreEqual(expected[i], output[i], string.Format("Failure comparing index {0} : {1} != {2}", i, output[i], expected[i]));
        }
    }

#endif

}

}
