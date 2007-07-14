using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class RotateTest
{
    [Test] public void Rotate_Iterator() 
    {
        int[] array = new int[12];
        array[0] = Int32.MinValue;
        array[array.Length-1] = Int32.MinValue;
        for(int i=1; i<=10; ++i)
            array[i] = i;

        ListIterator<int> begin = new ListIterator<int>(array, 1);
        ListIterator<int> end   = IteratorUtil.End(array);
        end.MovePrev();
        ListIterator<int> newBeginIter = new ListIterator<int>(array, 4);

        Algorithm.Rotate(begin, newBeginIter, end);
        Assert.AreEqual(Int32.MinValue,array[0]);
        Assert.AreEqual(Int32.MinValue,array[array.Length-1]);
        for(int i=1, c=4; i<8; ++i,++c)
        {
            Assert.AreEqual(c, array[i]);
        }
        
        for(int i=8, c=1; i<11; ++i, ++c)
        {
            Assert.AreEqual(c, array[i]);
        }
    }

    [Test] public void RotateCopy_Iterator() 
    {
        int[] src = new int[10];
        int[] dest = new int[12];
        dest[0] = Int32.MinValue;
        dest[dest.Length-1] = Int32.MinValue;
        for(int i=0; i<src.Length; ++i)
            src[i] = i+1;

        ListIterator<int> middle = new ListIterator<int>(src, 3);
        Algorithm.RotateCopy(IteratorUtil.Begin(src), middle, IteratorUtil.End(src), new ListIterator<int>(dest, 1));

        Assert.AreEqual(Int32.MinValue,dest[0]);
        Assert.AreEqual(Int32.MinValue,dest[dest.Length-1]);
        for(int i=1, c=4; i<8; ++i,++c)
        {
            Assert.AreEqual(c, dest[i]);
        }
        
        for(int i=8, c=1; i<11; ++i, ++c)
        {
            Assert.AreEqual(c, dest[i]);
        }
    }
/*
    [Test] public void Reverse_List() 
    {
        List<int> src2 = new List<int>(src);
        src2.Reverse();
        
        Algorithm.Reverse(src2);
        
        for(int i=0; i<count ;++i)
        {
            Assert.AreEqual(src[i], src2[i]);
        }
    }
 * */
}

}
