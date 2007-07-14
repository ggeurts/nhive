using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL.Iterator;
using CSTL;

namespace CSTL.Test
{

[TestFixture] 
[Category("Algorithm")]
public class UniqueCopyTest_Int : BaseIntTest
{
    #region Iterator tests
    [Test] public void UniqueCopy_ListIterator() 
    {
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        ListIterator<int> result = Algorithm.UniqueCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, result.Position);
        Assert.AreEqual(Int32.MinValue, result.Read());
    }

    [Test] public void UniqueCopy_RandomAccessIterator() 
    {
        RandomAccessIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        RandomAccessIterator<int> result = Algorithm.UniqueCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, result.Position);
        Assert.AreEqual(Int32.MinValue, result.Read());
    }

    [Test] public void UniqueCopy_ForwardIterator() 
    {
        ForwardIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        ForwardIterator<int> result = Algorithm.UniqueCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, IteratorUtil.Distance(IteratorUtil.Begin(dest), result));
        Assert.AreEqual(Int32.MinValue, result.Read());
    }

    [Test] public void UniqueCopy_OutputIterator() 
    {
        OutputIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        OutputIterator<int> result = Algorithm.UniqueCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, (result as ListIterator<int>).Position);
        Assert.AreEqual(Int32.MinValue, (result as ListIterator<int>).Read());
    }
    #endregion

    #region Iterator + comparer tests
    [Test] public void UniqueCopy_Comparer_ListIterator() 
    {
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();

        ListIterator<int> result = Algorithm.UniqueCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter, EqualityComparer<int>.Default);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, result.Position);
        Assert.AreEqual(Int32.MinValue, (result as ListIterator<int>).Read());
    }

    [Test] public void UniqueCopy_Comparer_RandomAccessIterator() 
    {
        RandomAccessIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        RandomAccessIterator<int> result = Algorithm.UniqueCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter, EqualityComparer<int>.Default);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, result.Position);
        Assert.AreEqual(Int32.MinValue, result.Read());
    }

    [Test] public void UniqueCopy_Comparer_ForwardIterator() 
    {
        ForwardIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        ForwardIterator<int> result = Algorithm.UniqueCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter, EqualityComparer<int>.Default);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, IteratorUtil.Distance(IteratorUtil.Begin(dest), result));
        Assert.AreEqual(Int32.MinValue, result.Read());
    }

    [Test] public void UniqueCopy_Comparer_OutputIterator() 
    {
        OutputIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        OutputIterator<int> result = Algorithm.UniqueCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter, EqualityComparer<int>.Default);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, (result as ListIterator<int>).Position);
        Assert.AreEqual(Int32.MinValue, (result as ListIterator<int>).Read());
    }
    #endregion

    #region Iterator + predicate functions
    [Test] public void UniqueCopy_Predicate_ListIterator() 
    {
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        ListIterator<int> result = Algorithm.UniqueCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter, delegate(int lhs, int rhs) {return lhs == rhs;});
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, result.Position);
        Assert.AreEqual(Int32.MinValue, (result as ListIterator<int>).Read());
    }

    [Test] public void UniqueCopy_Predicate_RandomAccessIterator() 
    {
        RandomAccessIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();

        RandomAccessIterator<int> result = Algorithm.UniqueCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter, delegate(int lhs, int rhs) {return lhs == rhs;});
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, result.Position);
        Assert.AreEqual(Int32.MinValue, result.Read());
    }

    [Test] public void UniqueCopy_Predicate_ForwardIterator() 
    {
        ForwardIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        ForwardIterator<int> result = Algorithm.UniqueCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter, delegate(int lhs, int rhs) {return lhs == rhs;});
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, IteratorUtil.Distance(IteratorUtil.Begin(dest), result));
        Assert.AreEqual(Int32.MinValue, result.Read());
    }

    [Test] public void UniqueCopy_Predicate_OutputIterator() 
    {
        OutputIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        OutputIterator<int> result = Algorithm.UniqueCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter, delegate(int lhs, int rhs) {return lhs == rhs;});
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, (result as ListIterator<int>).Position);
        Assert.AreEqual(Int32.MinValue, (result as ListIterator<int>).Read());
    }
    #endregion

    #region List functions
    [Test] public void  UniqueCopy_List_ListIterator()
    {
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();

        ListIterator<int> result = Algorithm.UniqueCopy(src, destIter);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, result.Position);
        Assert.AreEqual(Int32.MinValue, (result as ListIterator<int>).Read());
    }

    [Test] public void UniqueCopy_List_RandomAccessIterator() 
    {
        RandomAccessIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();

        RandomAccessIterator<int> result = Algorithm.UniqueCopy(src, destIter);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, result.Position);
        Assert.AreEqual(Int32.MinValue, result.Read());
    }

    [Test] public void UniqueCopy_List_ForwardIterator() 
    {
        ForwardIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        ForwardIterator<int> result = Algorithm.UniqueCopy(src, destIter);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, IteratorUtil.Distance(IteratorUtil.Begin(dest), result));
        Assert.AreEqual(Int32.MinValue, result.Read());
    }

    [Test] public void UniqueCopy_List_OutputIterator() 
    {
        OutputIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        OutputIterator<int> result = Algorithm.UniqueCopy(src, destIter);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, (result as ListIterator<int>).Position);
        Assert.AreEqual(Int32.MinValue, (result as ListIterator<int>).Read());
    }
    #endregion

    #region List + comparer functions
    [Test] public void  UniqueCopy_List_Comparer_ListIterator() 
    {
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();

        ListIterator<int> result = Algorithm.UniqueCopy(src, destIter, EqualityComparer<int>.Default);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, result.Position);
        Assert.AreEqual(Int32.MinValue, (result as ListIterator<int>).Read());
    }

    [Test] public void  UniqueCopy_List_Comparer_RandomAccessIterator() 
    {
        RandomAccessIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();

        RandomAccessIterator<int> result = Algorithm.UniqueCopy(src, destIter, EqualityComparer<int>.Default);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, result.Position);
        Assert.AreEqual(Int32.MinValue, (result as ListIterator<int>).Read());
    }

    [Test] public void  UniqueCopy_List_Comparer_ForwardIterator() 
    {
        ForwardIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();

        ForwardIterator<int> result = Algorithm.UniqueCopy(src, destIter, EqualityComparer<int>.Default);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, IteratorUtil.Distance(IteratorUtil.Begin(dest), result));
        Assert.AreEqual(Int32.MinValue, (result as ListIterator<int>).Read());
    }

    [Test] public void  UniqueCopy_List_Comparer_OutputIterator() 
    {
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();

        ListIterator<int> result = Algorithm.UniqueCopy(src, destIter, EqualityComparer<int>.Default);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, (result as ListIterator<int>).Position);
        Assert.AreEqual(Int32.MinValue, (result as ListIterator<int>).Read());
    }
    #endregion

    #region List + predicate functions
    [Test] public void  UniqueCopy_List_Predicate_ListIterator() 
    {
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();

        ListIterator<int> result = Algorithm.UniqueCopy(src, destIter, delegate(int lhs, int rhs) {return lhs == rhs;});
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, result.Position);
        Assert.AreEqual(Int32.MinValue, (result as ListIterator<int>).Read());
    }

    [Test] public void  UniqueCopy_List_Predicate_RandomAccessIterator() 
    {
        RandomAccessIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        RandomAccessIterator<int> result = Algorithm.UniqueCopy(src, destIter, delegate(int lhs, int rhs) {return lhs == rhs;});
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, result.Position);
        Assert.AreEqual(Int32.MinValue, (result as ListIterator<int>).Read());
    }

    [Test] public void  UniqueCopy_List_Predicate_ForwardIterator() 
    {
        ForwardIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();

        ForwardIterator<int> result = Algorithm.UniqueCopy(src, destIter, delegate(int lhs, int rhs) {return lhs == rhs;});
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, IteratorUtil.Distance(IteratorUtil.Begin(dest), result));
        Assert.AreEqual(Int32.MinValue, (result as ListIterator<int>).Read());
    }

    [Test] public void  UniqueCopy_List_Predicate_OutputIterator() 
    {
        OutputIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();

        OutputIterator<int> result = Algorithm.UniqueCopy(src, destIter, delegate(int lhs, int rhs) {return lhs == rhs;});
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, (result as ListIterator<int>).Position);
        Assert.AreEqual(Int32.MinValue, (result as ListIterator<int>).Read());
    }
    #endregion
    
    [Test] public void  UniqueCopy_ListToList() 
    {
        ListIterator<int> result = Algorithm.UniqueCopy(src, dest);
        VerifyListToListOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(5, result.Position);
        Assert.AreEqual(Int32.MinValue, (result as ListIterator<int>).Read());

    }

    [Test] public void  UniqueCopy_ListToList_Comparer() 
    {
        ListIterator<int> result = Algorithm.UniqueCopy(src, dest, EqualityComparer<int>.Default);
        VerifyListToListOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(5, result.Position);
        Assert.AreEqual(Int32.MinValue, (result as ListIterator<int>).Read());
    }

    [Test] public void  UniqueCopy_ListToList_Predicate() 
    {
        ListIterator<int> result = Algorithm.UniqueCopy(src, dest, delegate(int lhs, int rhs) {return lhs == rhs;});
        VerifyListToListOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(5, result.Position);
        Assert.AreEqual(Int32.MinValue, (result as ListIterator<int>).Read());
    }
    
    protected override int[] GetValues()
    {
        int[] result = Constants.TEST_BIG_INT_ARRAY;
        Array.Sort(result);
        return result;
    }

    void VerifyOutput()
    {
        Assert.AreEqual(dest[0], Int32.MinValue);
        Assert.AreEqual(dest[1], 1);
        Assert.AreEqual(dest[2], 2);
        Assert.AreEqual(dest[3], 3);
        Assert.AreEqual(dest[4], 4);
        Assert.AreEqual(dest[5], 5);
        for(int i=6; i<dest.Length; ++i)
            Assert.AreEqual(dest[i], int.MinValue);
    }

    void VerifyListToListOutput()
    {
        Assert.AreEqual(dest[0], 1);
        Assert.AreEqual(dest[1], 2);
        Assert.AreEqual(dest[2], 3);
        Assert.AreEqual(dest[3], 4);
        Assert.AreEqual(dest[4], 5);
        for(int i=5; i<dest.Length; ++i)
            Assert.AreEqual(dest[i], int.MinValue);
    }        
}

}
