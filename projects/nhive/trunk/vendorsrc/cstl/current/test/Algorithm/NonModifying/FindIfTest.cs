using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class FindIfTest 
{
    [Test] public void FindIf_IntIterator() 
    {
        int[] array = Constants.TEST_INT_ARRAY;
        ListIterator<int> iter = Algorithm.FindIf(IteratorUtil.Begin(array), IteratorUtil.End(array), IsMinus1);
        Assert.AreEqual(8, iter.Position);
        
        iter = Algorithm.FindIf(IteratorUtil.Begin(array), IteratorUtil.End(array), Never);
        Assert.IsNull(iter);
//        Assert.IsTrue(IteratorUtil.End(array).Equals(iter));
        
        RandomAccessIterator<int> beginR = IteratorUtil.Begin(array);
        RandomAccessIterator<int> endR   = IteratorUtil.End(array);
        RandomAccessIterator<int> iterR  = Algorithm.FindIf(beginR, endR, IsMinus1);
        Assert.AreEqual(8, ((ListIterator<int>)iterR).Position);

        ForwardIterator<int> beginF = IteratorUtil.Begin(array);
        ForwardIterator<int> endF = IteratorUtil.End(array);
        ForwardIterator<int> iterF = Algorithm.FindIf(beginF, endF, IsMinus1);
        Assert.AreEqual(8, ((ListIterator<int>)iterF).Position);

        InputIterator<int> beginI = IteratorUtil.Begin(array);
        InputIterator<int> endI   = IteratorUtil.End(array);
        InputIterator<int> iterI  = Algorithm.FindIf(beginI, endI, IsMinus1);
        Assert.AreEqual(8, ((ListIterator<int>)iterI).Position);
    }

    [Test] public void FindIf_StringIterator() 
    {
        string[] array = Constants.TEST_STRING_ARRAY;
        ListIterator<string> iter = Algorithm.FindIf(IteratorUtil.Begin(array), IteratorUtil.End(array), IsSue);
        Assert.AreEqual(7, iter.Position);

        iter = Algorithm.FindIf(IteratorUtil.Begin(array), IteratorUtil.End(array), Never);
        Assert.IsNull(iter);
//        Assert.IsTrue(IteratorUtil.End(array ).Equals(iter));
    }

    [Test] public void FindIf_IntList() 
    {
        int[] array = Constants.TEST_INT_ARRAY;
        ListIterator<int> iter = Algorithm.FindIf(array, IsMinus1);
        Assert.AreEqual(8, iter.Position);

        iter = Algorithm.FindIf(array, Never);
        Assert.IsNull(iter);
//        Assert.IsTrue(IteratorUtil.End(array).Equals(iter));
    }

    [Test] public void Find_StringList() 
    {
        string[] array = Constants.TEST_STRING_ARRAY;
        ListIterator<string> iter = Algorithm.FindIf(array, IsSue);
        Assert.AreEqual(7, iter.Position);

        iter = Algorithm.FindIf(array, Never);
        Assert.IsNull(iter);
//        Assert.IsTrue(IteratorUtil.End(array).Equals(iter));
    }

    [Test] public void FindIf_IntEnumerator() 
    {
        IEnumerator<int> iter = Algorithm.Find(GetIntEnumerator(), -1);
        Assert.AreEqual(-1, iter.Current);

        iter = Algorithm.Find(GetIntEnumerator(), -217);
        Assert.IsNull(iter);
    }

    [Test] public void FindIf_StringEnumerator() 
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

    static bool IsMinus1(int value)
    {
        return value==-1;
    }
    
    static bool IsSue(string value)
    {
        return value == "Sue";
    }
    
    static bool Never(int value)
    {
        return false;
    }
    
    static bool Never(string value)
    {
        return false;
    }
}

}
