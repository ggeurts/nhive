using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{

[TestFixture] 
[Category("Algorithm")]
public class MaxElementTest 
{
    [Test] public void MaxElement_IntIterator() 
    {
        int[] array = Constants.TEST_INT_ARRAY;
        ListIterator<int> iter = Algorithm.MaxElement(IteratorUtil.Begin(array), IteratorUtil.End(array));
        Assert.AreEqual(100, iter.Read());
        Assert.AreEqual(10, iter.Position);

        RandomAccessIterator<int> beginR = IteratorUtil.Begin(array);
        RandomAccessIterator<int> endR   = IteratorUtil.End(array);
        RandomAccessIterator<int> iterR  = Algorithm.MaxElement(beginR, endR);
        Assert.AreEqual(10, ((ListIterator<int>)iterR).Position);

        ForwardIterator<int> beginF = IteratorUtil.Begin(array);
        ForwardIterator<int> endF = IteratorUtil.End(array);
        ForwardIterator<int> iterF = Algorithm.MaxElement(beginF, endF);
        Assert.AreEqual(10, ((ListIterator<int>)iterF).Position);
    }

    [Test] public void MaxElement_StringIterator() 
    {
        string[] array = Constants.TEST_STRING_ARRAY;
        ListIterator<string> iter = Algorithm.MaxElement(IteratorUtil.Begin(array), IteratorUtil.End(array));
        Assert.AreEqual("world", iter.Read());
        Assert.AreEqual(1, iter.Position);
    }

    [Test] public void MaxElement_IntIteratorComparer() 
    {
        int[] array = Constants.TEST_INT_ARRAY;
        ListIterator<int> iter = Algorithm.MaxElement(IteratorUtil.Begin(array), IteratorUtil.End(array), Comparer<int>.Default);
        Assert.AreEqual(100, iter.Read());
        Assert.AreEqual(10, iter.Position);
    }

    [Test] public void MaxElement_StringIteratorComparer() 
    {
        string[] array = Constants.TEST_STRING_ARRAY;
        ListIterator<string> iter = Algorithm.MaxElement(IteratorUtil.Begin(array), IteratorUtil.End(array),Comparer<string>.Default);
        Assert.AreEqual("world", iter.Read());
        Assert.AreEqual(1, iter.Position);
    }

#if SORT_WITH_BINARYPREDICATE
    [Test] public void MaxElement_IntIteratorPredicate() 
    {
        int[] array = Constants.TEST_INT_ARRAY;
        ListIterator<int> iter = Algorithm.MaxElement(IteratorUtil.Begin(array), IteratorUtil.End(array), GreaterThanInt);
        Assert.AreEqual(100, iter.Read());
        Assert.AreEqual(10, iter.Position);

        RandomAccessIterator<int> beginR = IteratorUtil.Begin(array);
        RandomAccessIterator<int> endR   = IteratorUtil.End(array);
        RandomAccessIterator<int> iterR  = Algorithm.MaxElement(beginR, endR, GreaterThanInt);
        Assert.AreEqual(10, ((ListIterator<int>)iterR).Position);

        ForwardIterator<int> beginF = IteratorUtil.Begin(array);
        ForwardIterator<int> endF = IteratorUtil.End(array);
        ForwardIterator<int> iterF = Algorithm.MaxElement(beginF, endF, GreaterThanInt);
        Assert.AreEqual(10, ((ListIterator<int>)iterF).Position);
    }
#else
    [Test] public void MaxElement_IntIteratorPredicate() 
    {
        int[] array = Constants.TEST_INT_ARRAY;
        ListIterator<int> iter = Algorithm.MaxElement(IteratorUtil.Begin(array), IteratorUtil.End(array), Functional.Compare);
        Assert.AreEqual(100, iter.Read());
        Assert.AreEqual(10, iter.Position);

        RandomAccessIterator<int> beginR = IteratorUtil.Begin(array);
        RandomAccessIterator<int> endR   = IteratorUtil.End(array);
        RandomAccessIterator<int> iterR  = Algorithm.MaxElement(beginR, endR, Functional.Compare);
        Assert.AreEqual(10, ((ListIterator<int>)iterR).Position);

        ForwardIterator<int> beginF = IteratorUtil.Begin(array);
        ForwardIterator<int> endF = IteratorUtil.End(array);
        ForwardIterator<int> iterF = Algorithm.MaxElement(beginF, endF, Functional.Compare);
        Assert.AreEqual(10, ((ListIterator<int>)iterF).Position);
    }
#endif

    [Test] public void MaxElement_IntList()
    {
        int[] array = Constants.TEST_INT_ARRAY;
        ListIterator<int> iter = Algorithm.MaxElement(array);
        Assert.AreEqual(100, iter.Read());
        Assert.AreEqual(10, iter.Position);
    }

    [Test] public void MaxElement_IntListComparer() 
    {
        int[] array = Constants.TEST_INT_ARRAY;
        ListIterator<int> iter = Algorithm.MaxElement(array, Comparer<int>.Default);
        Assert.AreEqual(100, iter.Read());
        Assert.AreEqual(10, iter.Position);
    }

#if SORT_WITH_BINARYPREDICATE
    [Test] public void MaxElement_IntListPredicate() 
    {
        int[] array = Constants.TEST_INT_ARRAY;
        ListIterator<int> iter = Algorithm.MaxElement(array, GreaterThanInt);
        Assert.AreEqual(100, iter.Read());
        Assert.AreEqual(10, iter.Position);
    }
#else
    [Test] public void MaxElement_IntListPredicate() 
    {
        int[] array = Constants.TEST_INT_ARRAY;
        ListIterator<int> iter = Algorithm.MaxElement(array, Functional.Compare);
        Assert.AreEqual(100, iter.Read());
        Assert.AreEqual(10, iter.Position);
    }
#endif

    [Test] public void MaxElement_IntEnumerator()
    {
        int value = Algorithm.MaxElement(GetIntEnumerator());
        Assert.AreEqual(100, value);
    }

    [Test] public void MaxElement_IntEnumeratorComparer()
    {
        int value = Algorithm.MaxElement(GetIntEnumerator(), Comparer<int>.Default);
        Assert.AreEqual(100, value);
    }

#if SORT_WITH_BINARYPREDICATE
    [Test] public void MaxElement_IntEnumeratorPredicate()
    {
        int value = Algorithm.MaxElement(GetIntEnumerator(), GreaterThanInt);
        Assert.AreEqual(100, value);
    }
#else
    [Test] public void MaxElement_IntEnumeratorPredicate()
    {
        int value = Algorithm.MaxElement(GetIntEnumerator(), Functional.Compare);
        Assert.AreEqual(100, value);
    }
#endif
    IEnumerable<int> GetIntEnumerator()
    {
        foreach(int i in Constants.TEST_INT_ARRAY)
            yield return i;
    }

    bool GreaterThanInt(int lhs, int rhs)
    {
        return lhs > rhs;
    }
}

}
