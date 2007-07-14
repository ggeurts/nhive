using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL.Iterator;
using CSTL;

namespace CSTL.Test
{

[TestFixture] 
[Category("Algorithm")]
public class RemoveCopyTest_Int : BaseIntTest
{
    [Test] public void RemoveCopy_ListIterator() 
    {
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        ListIterator<int> result = Algorithm.RemoveCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter, 29);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(src.Length-3, result.Position);
        Assert.AreEqual(Int32.MinValue, result.Read());
    }

    [Test] public void RemoveCopy_RandomAccessIterator() 
    {
        RandomAccessIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        RandomAccessIterator<int> result = Algorithm.RemoveCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter, 29);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(src.Length-3, result.Position);
        Assert.AreEqual(Int32.MinValue, result.Read());
    }

    [Test] public void RemoveCopy_ForwardIterator() 
    {
        ForwardIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        ForwardIterator<int> result = Algorithm.RemoveCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter, 29);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(src.Length-3, IteratorUtil.Distance(IteratorUtil.Begin(dest), result));
        Assert.AreEqual(Int32.MinValue, result.Read());
    }

    [Test] public void RemoveCopy_OutputIterator() 
    {
        OutputIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        OutputIterator<int> result = Algorithm.RemoveCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter, 29);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(src.Length-3, (result as ListIterator<int>).Position);
    }

    [Test] public void RemoveCopy_Comparer_ListIterator() 
    {
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        ListIterator<int> result = Algorithm.RemoveCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter, 29, EqualityComparer<int>.Default);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(src.Length-3, result.Position);
        Assert.AreEqual(Int32.MinValue, result.Read());
    }

    [Test] public void RemoveCopy_Comparer_RandomAccessIterator() 
    {
        RandomAccessIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        RandomAccessIterator<int> result = Algorithm.RemoveCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter, 29, EqualityComparer<int>.Default);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(src.Length-3, result.Position);
        Assert.AreEqual(Int32.MinValue, result.Read());
    }

    [Test] public void RemoveCopy_Comparer_ForwardIterator() 
    {
        ForwardIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        ForwardIterator<int> result = Algorithm.RemoveCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter, 29, EqualityComparer<int>.Default);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(src.Length-3, IteratorUtil.Distance(IteratorUtil.Begin(dest), result));
        Assert.AreEqual(Int32.MinValue, result.Read());
    }

    [Test] public void RemoveCopy_Comparer_OutputIterator() 
    {
        OutputIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        OutputIterator<int> result = Algorithm.RemoveCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter, 29, EqualityComparer<int>.Default);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(src.Length-3, (result as ListIterator<int>).Position);
    }

    [Test] public void RemoveCopy_List_ListIterator() 
    {
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        ListIterator<int> result = Algorithm.RemoveCopy(src, destIter, 29);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(src.Length-3, result.Position);
        Assert.AreEqual(Int32.MinValue, result.Read());
    }

    [Test] public void RemoveCopy_List_RandomAccessIterator() 
    {
        RandomAccessIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        RandomAccessIterator<int> result = Algorithm.RemoveCopy(src, destIter, 29);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(src.Length-3, result.Position);
        Assert.AreEqual(Int32.MinValue, result.Read());
    }

    [Test] public void RemoveCopy_List_ForwardIterator() 
    {
        ForwardIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        ForwardIterator<int> result = Algorithm.RemoveCopy(src, destIter, 29);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(src.Length-3, IteratorUtil.Distance(IteratorUtil.Begin(dest), result));
        Assert.AreEqual(Int32.MinValue, result.Read());
    }

    [Test] public void RemoveCopy_List_OutputIterator() 
    {
        OutputIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        OutputIterator<int> result = Algorithm.RemoveCopy(src, destIter, 29);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(src.Length-3, (result as ListIterator<int>).Position);
    }



    [Test] public void RemoveCopy_List_ListIterator_Comparer() 
    {
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        ListIterator<int> result = Algorithm.RemoveCopy(src, destIter, 29, EqualityComparer<int>.Default);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(src.Length-3, result.Position);
        Assert.AreEqual(Int32.MinValue, result.Read());
    }

    [Test] public void RemoveCopy_List_RandomAccessIterator_Comparer() 
    {
        RandomAccessIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        RandomAccessIterator<int> result = Algorithm.RemoveCopy(src, destIter, 29, EqualityComparer<int>.Default);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(src.Length-3, result.Position);
        Assert.AreEqual(Int32.MinValue, result.Read());
    }

    [Test] public void RemoveCopy_List_ForwardIterator_Comparer() 
    {
        ForwardIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        ForwardIterator<int> result = Algorithm.RemoveCopy(src, destIter, 29, EqualityComparer<int>.Default);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(src.Length-3, IteratorUtil.Distance(IteratorUtil.Begin(dest), result));
        Assert.AreEqual(Int32.MinValue, result.Read());
    }

    [Test] public void RemoveCopy_List_OutputIterator_Comparer() 
    {
        OutputIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        OutputIterator<int> result = Algorithm.RemoveCopy(src, destIter, 29, EqualityComparer<int>.Default);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(src.Length-3, (result as ListIterator<int>).Position);
    }

    [Test] public void RemoveCopy_List2List()
    {
        ListIterator<int> result = Algorithm.RemoveCopy(src, dest, 29);
        VerifyListOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(src.Length-4, result.Position);
        Assert.AreEqual(Int32.MinValue, result.Read());
    }

    [Test] public void RemoveCopy_List2List_Comparer()
    {
        ListIterator<int> result = Algorithm.RemoveCopy(src, dest, 29, EqualityComparer<int>.Default);
        VerifyListOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(src.Length-4, result.Position);
        Assert.AreEqual(Int32.MinValue, result.Read());
    }
    
    private void VerifyOutput()
    {
        Assert.AreEqual(dest[0], Int32.MinValue);
        Assert.AreEqual(dest[1], 5);
        Assert.AreEqual(dest[2], 2);
        Assert.AreEqual(dest[3], 34);
        Assert.AreEqual(dest[4], -17);
        Assert.AreEqual(dest[5], 33);
        Assert.AreEqual(dest[6], -1);
        Assert.AreEqual(dest[7], 100);
        Assert.AreEqual(dest[8], 12);
        for(int i=9; i<dest.Length; ++i)
            Assert.AreEqual(dest[i], int.MinValue);
    }

    private void VerifyListOutput()
    {
        Assert.AreEqual(dest[0], 5);
        Assert.AreEqual(dest[1], 2);
        Assert.AreEqual(dest[2], 34);
        Assert.AreEqual(dest[3], -17);
        Assert.AreEqual(dest[4], 33);
        Assert.AreEqual(dest[5], -1);
        Assert.AreEqual(dest[6], 100);
        Assert.AreEqual(dest[7], 12);
        for(int i=8; i<dest.Length; ++i)
            Assert.AreEqual(dest[i], int.MinValue);
    }

}
}
