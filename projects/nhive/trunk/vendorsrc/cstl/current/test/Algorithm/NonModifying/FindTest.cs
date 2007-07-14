using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class FindTest 
{
    [Test] public void Find_IntIterator() 
    {
        int[] array = Constants.TEST_INT_ARRAY;
        ListIterator<int> iter = Algorithm.Find(IteratorUtil.Begin(array), IteratorUtil.End(array), -1);
        Assert.AreEqual(8, iter.Position);
        
        iter = Algorithm.Find(IteratorUtil.Begin(array), IteratorUtil.End(array), -217);
        Assert.IsNull(iter);
//        Assert.IsTrue(IteratorUtil.End(array).Equals(iter));
    }

    [Test] public void Find_StringIterator() 
    {
        string[] array = Constants.TEST_STRING_ARRAY;
        ListIterator<string> iter = Algorithm.Find(IteratorUtil.Begin(array), IteratorUtil.End(array), "Sue");
        Assert.AreEqual(7, iter.Position);

        iter = Algorithm.Find(IteratorUtil.Begin(array), IteratorUtil.End(array), "python");
        Assert.IsNull(iter);
//        Assert.IsTrue(IteratorUtil.End(array ).Equals(iter));
    }

    [Test] public void Find_IntList() 
    {
        int[] array = Constants.TEST_INT_ARRAY;
        RandomAccessIterator<int> iter = Algorithm.Find(array, -1);
        Assert.AreEqual(8, iter.Position);

        iter = Algorithm.Find(array, -217);
        Assert.IsNull(iter);
//        Assert.IsTrue(IteratorUtil.End(array).Equals(iter));
    }

    [Test] public void Find_StringList() 
    {
        string[] array = Constants.TEST_STRING_ARRAY;
        ListIterator<string> iter = Algorithm.Find(array, "Sue");
        Assert.AreEqual(7, iter.Position);

        iter = Algorithm.Find(array, "python");
        Assert.IsNull(iter);
//        Assert.IsTrue(IteratorUtil.End(array).Equals(iter));
    }

    [Test] public void Find_IntEnumerator() 
    {
        IEnumerator<int> iter = Algorithm.Find(GetIntEnumerator(), -1);
        Assert.AreEqual(-1, iter.Current);

        iter = Algorithm.Find(GetIntEnumerator(), -217);
        Assert.IsNull(iter);
    }

    [Test] public void Find_StringEnumerator() 
    {
        IEnumerator<string> iter = Algorithm.Find(GetStringEnumerator(), "Sue");
        Assert.AreEqual("Sue", iter.Current);

        iter = Algorithm.Find(GetStringEnumerator(), "python");
        Assert.IsNull(iter);
    }
    
    IEnumerable<int> GetIntEnumerator()
    {
        foreach(int i in Constants.TEST_INT_ARRAY)
            yield return i;
    }
    
    IEnumerable<string> GetStringEnumerator()
    {
        foreach(string s in Constants.TEST_STRING_ARRAY)
            yield return s;
    }
}

}
