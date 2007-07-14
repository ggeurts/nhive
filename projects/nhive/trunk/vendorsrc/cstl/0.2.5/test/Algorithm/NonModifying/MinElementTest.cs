using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{

[TestFixture] 
[Category("Algorithm")]
public class MinElementTest 
{
    [Test] public void MinElement_IntIterator() 
    {
        int[] array = Constants.TEST_INT_ARRAY;
        ListIterator<int> iter = Algorithm.MinElement(IteratorUtil.Begin(array), IteratorUtil.End(array));
        Assert.AreEqual(-17, iter.Read());
        Assert.AreEqual(4, iter.Position);

        RandomAccessIterator<int> beginR = IteratorUtil.Begin(array);
        RandomAccessIterator<int> endR   = IteratorUtil.End(array);
        RandomAccessIterator<int> iterR  = Algorithm.MinElement(beginR, endR);
        Assert.AreEqual(4, ((ListIterator<int>)iterR).Position);

        ForwardIterator<int> beginF = IteratorUtil.Begin(array);
        ForwardIterator<int> endF = IteratorUtil.End(array);
        ForwardIterator<int> iterF = Algorithm.MinElement(beginF, endF);
        Assert.AreEqual(4, ((ListIterator<int>)iterF).Position);
    }    

    [Test] public void MinElement_StringIterator() 
    {
        string[] array = Constants.TEST_STRING_ARRAY;
        ListIterator<string> iter = Algorithm.MinElement(IteratorUtil.Begin(array), IteratorUtil.End(array));
        Assert.AreEqual("Bob", iter.Read());
        Assert.AreEqual(9, iter.Position);
    }  
        
    [Test] public void MinElement_IntIteratorComparer() 
    {
        int[] array = Constants.TEST_INT_ARRAY;
        ListIterator<int> iter = Algorithm.MinElement(IteratorUtil.Begin(array), IteratorUtil.End(array), Comparer<int>.Default);
        Assert.AreEqual(-17, iter.Read());
        Assert.AreEqual(4, iter.Position);
    }  
    
    [Test] public void MinElement_StringIteratorComparer() 
    {
        string[] array = Constants.TEST_STRING_ARRAY;
        ListIterator<string> iter = Algorithm.MinElement(IteratorUtil.Begin(array), IteratorUtil.End(array),Comparer<string>.Default);
        Assert.AreEqual(null, iter.Read());
        Assert.AreEqual(3, iter.Position);
    }

#if SORT_WITH_BINARYPREDICATE
    [Test] public void MinElement_IntIteratorPredicate() 
    {
        int[] array = Constants.TEST_INT_ARRAY;
        ListIterator<int> iter = Algorithm.MinElement(IteratorUtil.Begin(array), IteratorUtil.End(array), LessThanInt);
        Assert.AreEqual(-17, iter.Read());
        Assert.AreEqual(4, iter.Position);

        RandomAccessIterator<int> beginR = IteratorUtil.Begin(array);
        RandomAccessIterator<int> endR   = IteratorUtil.End(array);
        RandomAccessIterator<int> iterR  = Algorithm.MinElement(beginR, endR, LessThanInt);
        Assert.AreEqual(4, ((ListIterator<int>)iterR).Position);

        ForwardIterator<int> beginF = IteratorUtil.Begin(array);
        ForwardIterator<int> endF = IteratorUtil.End(array);
        ForwardIterator<int> iterF = Algorithm.MinElement(beginF, endF, LessThanInt);
        Assert.AreEqual(4, ((ListIterator<int>)iterF).Position);
    }
#else
    [Test] public void MaxElement_IntIteratorPredicate() 
    {
        int[] array = Constants.TEST_INT_ARRAY;
        ListIterator<int> iter = Algorithm.MinElement(IteratorUtil.Begin(array), IteratorUtil.End(array), Functional.Compare);
        Assert.AreEqual(-17, iter.Read());
        Assert.AreEqual(4, iter.Position);

        RandomAccessIterator<int> beginR = IteratorUtil.Begin(array);
        RandomAccessIterator<int> endR   = IteratorUtil.End(array);
        RandomAccessIterator<int> iterR  = Algorithm.MinElement(beginR, endR, Functional.Compare);
        Assert.AreEqual(4, ((ListIterator<int>)iterR).Position);

        ForwardIterator<int> beginF = IteratorUtil.Begin(array);
        ForwardIterator<int> endF = IteratorUtil.End(array);
        ForwardIterator<int> iterF = Algorithm.MinElement(beginF, endF, Functional.Compare);
        Assert.AreEqual(4, ((ListIterator<int>)iterF).Position);
    }
#endif

    [Test] public void MinElement_IntList()
    {
        int[] array = Constants.TEST_INT_ARRAY;
        ListIterator<int> iter = Algorithm.MinElement(array);
        Assert.AreEqual(-17, iter.Read());
        Assert.AreEqual(4, iter.Position);
    }

    [Test] public void MinElement_IntListComparer() 
    {
        int[] array = Constants.TEST_INT_ARRAY;
        ListIterator<int> iter = Algorithm.MinElement(array, Comparer<int>.Default);
        Assert.AreEqual(-17, iter.Read());
        Assert.AreEqual(4, iter.Position);
    }

#if SORT_WITH_BINARYPREDICATE
    [Test] public void MinElement_IntListPredicate() 
    {
        int[] array = Constants.TEST_INT_ARRAY;
        ListIterator<int> iter = Algorithm.MinElement(array, LessThanInt);
        Assert.AreEqual(-17, iter.Read());
        Assert.AreEqual(4, iter.Position);
    }
#else
    [Test] public void MaxElement_IntListPredicate() 
    {
        int[] array = Constants.TEST_INT_ARRAY;
        ListIterator<int> iter = Algorithm.MinElement(array, Functional.Compare);
        Assert.AreEqual(-17, iter.Read());
        Assert.AreEqual(4, iter.Position);
    }
#endif

    [Test] public void MinElement_IntEnumerator()
    {
        int value = Algorithm.MinElement(GetIntEnumerator());
        Assert.AreEqual(-17, value);
    }

    [Test] public void MinElement_IntEnumeratorComparer()
    {
        int value = Algorithm.MinElement(GetIntEnumerator(), Comparer<int>.Default);
        Assert.AreEqual(-17, value);
    }

#if SORT_WITH_BINARYPREDICATE
    [Test] public void MinElement_IntEnumeratorPredicate()
    {
        int value = Algorithm.MinElement(GetIntEnumerator(), LessThanInt);
        Assert.AreEqual(-17, value);
    }
#else
    [Test] public void MaxElement_IntEnumeratorPredicate()
    {
        int value = Algorithm.MinElement(GetIntEnumerator(), Functional.Compare);
        Assert.AreEqual(-17, value);
    }
#endif
    IEnumerable<int> GetIntEnumerator()
    {
        foreach(int i in Constants.TEST_INT_ARRAY)
            yield return i;
    }

    bool LessThanInt(int lhs, int rhs)
    {
        return lhs < rhs;
    }
}

}
