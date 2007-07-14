using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL.Iterator;
using CSTL;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class FillNTest_Int 
{
    [Test] public void FillN_Iterator() 
    {
        int[] array = new int[10];
        ListIterator<int> begin = IteratorUtil.Begin(array);
        ListIterator<int> end   = IteratorUtil.End(array);
        begin.MoveNext();
        end.MovePrev();

        OutputIterator<int> result = Algorithm.FillN(begin, 8, 42);
        Assert.AreEqual(0, array[0]);
        Assert.AreEqual(0, array[array.Length-1]);
        for(int i=1; i<array.Length-1; ++i)
        {
            Assert.AreEqual(42, array[i]);
        }
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(typeof(ListIterator<int>), result);
        Assert.AreEqual(9, (result as ListIterator<int>).Position);
    }

    [Test] public void FillN_List() 
    {
        List<int> list = new List<int>();
        for(int i=0; i<10; ++i)
            list.Add(-1);

        ListIterator<int> result = Algorithm.FillN(list, list.Count-1 , 42);
        Assert.AreEqual(-1, list[list.Count-1]);
        for(int i=0; i<list.Count-1; ++i)
        {
            Assert.AreEqual(42, list[i]);
        }
        Assert.IsNotNull(result);
        Assert.AreEqual(9, result.Position);
    }

    [Test] public void FillN_ListOffset() 
    {
        List<int> list = new List<int>();
        for(int i=0; i<10; ++i)
            list.Add(-1);

        ListIterator<int> result = Algorithm.FillN(list, 1, list.Count-2, 42);
        Assert.AreEqual(-1, list[list.Count-1]);
        Assert.AreEqual(-1, list[0]);
        for(int i=1; i<list.Count-1; ++i)
        {
            Assert.AreEqual(42, list[i]);
        }
        Assert.IsNotNull(result);
        Assert.AreEqual(9, result.Position);
    }
}

}
