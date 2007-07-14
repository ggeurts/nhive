using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL.Iterator;
using CSTL;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class FillTest_Int
{
    [Test] public void Fill_Iterator() 
    {
        int[] array = new int[10];
        ListIterator<int> begin = IteratorUtil.Begin(array);
        ListIterator<int> end   = IteratorUtil.End(array);
        begin.MoveNext();
        end.MovePrev();        
        Algorithm.Fill(begin, end, 42);
        
        Assert.AreEqual(0, array[0]);
        Assert.AreEqual(0, array[array.Length-1]);
        for(int i=1; i<array.Length-1; ++i)
        {
            Assert.AreEqual(42, array[i]);
        }
    }

    [Test] public void Fill_List() 
    {
        List<int> list = new List<int>();
        for(int i=0; i<10; ++i)
            list.Add(-1);
        
        Algorithm.Fill(list, 42);
        for(int i=0; i<list.Count; ++i)
        {
            Assert.AreEqual(42, list[i]);
        }
    }   
}

}
