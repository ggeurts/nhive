using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL.Iterator;
using CSTL;

namespace CSTL.Test
{

[TestFixture] 
[Category("Algorithm")]
public class UniqueCopyTest_String: BaseStringTest
{
    #region Iterator tests
    [Test] public void UniqueCopy_Iterator() 
    {
        ListIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        ListIterator<string> result = Algorithm.UniqueCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, result.Position);
        Assert.AreEqual(MARKER, result.Read());
    }

    [Test] public void UniqueCopy_RandomAccessIterator() 
    {
        RandomAccessIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        RandomAccessIterator<string> result = Algorithm.UniqueCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, result.Position);
        Assert.AreEqual(MARKER, result.Read());
    }

    [Test] public void UniqueCopy_ForwardIterator() 
    {
        ForwardIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        ForwardIterator<string> result = Algorithm.UniqueCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, IteratorUtil.Distance(IteratorUtil.Begin(dest), result));
        Assert.AreEqual(MARKER, result.Read());
    }

    [Test] public void UniqueCopy_OutputIterator() 
    {
        OutputIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        OutputIterator<string> result = Algorithm.UniqueCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, (result as ListIterator<string>).Position);
        Assert.AreEqual(MARKER, (result as ListIterator<string>).Read());
    }
    #endregion

    #region Iterator + comparer tests
    [Test] public void UniqueCopy_Comparer_ListIterator() 
    {
        ListIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();

        ListIterator<string> result = Algorithm.UniqueCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter, EqualityComparer<string>.Default);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, result.Position);
        Assert.AreEqual(MARKER, (result as ListIterator<string>).Read());
    }

    [Test] public void UniqueCopy_Comparer_RandomAccessIterator() 
    {
        RandomAccessIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        RandomAccessIterator<string> result = Algorithm.UniqueCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter, EqualityComparer<string>.Default);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, result.Position);
        Assert.AreEqual(MARKER, result.Read());
    }

    [Test] public void UniqueCopy_Comparer_ForwardIterator() 
    {
        ForwardIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        ForwardIterator<string> result = Algorithm.UniqueCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter, EqualityComparer<string>.Default);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, IteratorUtil.Distance(IteratorUtil.Begin(dest), result));
        Assert.AreEqual(MARKER, result.Read());
    }

    [Test] public void UniqueCopy_Comparer_OutputIterator() 
    {
        OutputIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        OutputIterator<string> result = Algorithm.UniqueCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter, EqualityComparer<string>.Default);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, (result as ListIterator<string>).Position);
        Assert.AreEqual(MARKER, (result as ListIterator<string>).Read());
    }
    #endregion

    #region Iterator + predicate functions
    [Test] public void UniqueCopy_Predicate_ListIterator() 
    {
        ListIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        ListIterator<string> result = Algorithm.UniqueCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter, delegate(string lhs, string rhs) {return lhs == rhs;});
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, result.Position);
        Assert.AreEqual(MARKER, (result as ListIterator<string>).Read());
    }

    [Test] public void UniqueCopy_Predicate_RandomAccessIterator() 
    {
        RandomAccessIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();

        RandomAccessIterator<string> result = Algorithm.UniqueCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter, delegate(string lhs, string rhs) {return lhs == rhs;});
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, result.Position);
        Assert.AreEqual(MARKER, result.Read());
    }

    [Test] public void UniqueCopy_Predicate_ForwardIterator() 
    {
        ForwardIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        ForwardIterator<string> result = Algorithm.UniqueCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter, delegate(string lhs, string rhs) {return lhs == rhs;});
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, IteratorUtil.Distance(IteratorUtil.Begin(dest), result));
        Assert.AreEqual(MARKER, result.Read());
    }

    [Test] public void UniqueCopy_Predicate_OutputIterator() 
    {
        OutputIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        OutputIterator<string> result = Algorithm.UniqueCopy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter, delegate(string lhs, string rhs) {return lhs == rhs;});
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, (result as ListIterator<string>).Position);
        Assert.AreEqual(MARKER, (result as ListIterator<string>).Read());
    }
    #endregion

    #region List functions
    [Test] public void  UniqueCopy_List_ListIterator()
    {
        ListIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();

        ListIterator<string> result = Algorithm.UniqueCopy(src, destIter);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, result.Position);
        Assert.AreEqual(MARKER, (result as ListIterator<string>).Read());
    }

    [Test] public void UniqueCopy_List_RandomAccessIterator() 
    {
        RandomAccessIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();

        RandomAccessIterator<string> result = Algorithm.UniqueCopy(src, destIter);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, result.Position);
        Assert.AreEqual(MARKER, result.Read());
    }

    [Test] public void UniqueCopy_List_ForwardIterator() 
    {
        ForwardIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        ForwardIterator<string> result = Algorithm.UniqueCopy(src, destIter);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, IteratorUtil.Distance(IteratorUtil.Begin(dest), result));
        Assert.AreEqual(MARKER, result.Read());
    }

    [Test] public void UniqueCopy_List_OutputIterator() 
    {
        OutputIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        OutputIterator<string> result = Algorithm.UniqueCopy(src, destIter);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, (result as ListIterator<string>).Position);
        Assert.AreEqual(MARKER, (result as ListIterator<string>).Read());
    }
    #endregion

    #region List + comparer functions
    [Test] public void  UniqueCopy_List_Comparer_ListIterator() 
    {
        ListIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();

        ListIterator<string> result = Algorithm.UniqueCopy(src, destIter, EqualityComparer<string>.Default);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, result.Position);
        Assert.AreEqual(MARKER, (result as ListIterator<string>).Read());
    }

    [Test] public void  UniqueCopy_List_Comparer_RandomAccessIterator() 
    {
        RandomAccessIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();

        RandomAccessIterator<string> result = Algorithm.UniqueCopy(src, destIter, EqualityComparer<string>.Default);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, result.Position);
        Assert.AreEqual(MARKER, (result as ListIterator<string>).Read());
    }

    [Test] public void  UniqueCopy_List_Comparer_ForwardIterator() 
    {
        ForwardIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();

        ForwardIterator<string> result = Algorithm.UniqueCopy(src, destIter, EqualityComparer<string>.Default);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, IteratorUtil.Distance(IteratorUtil.Begin(dest), result));
        Assert.AreEqual(MARKER, (result as ListIterator<string>).Read());
    }

    [Test] public void  UniqueCopy_List_Comparer_OutputIterator() 
    {
        ListIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();

        ListIterator<string> result = Algorithm.UniqueCopy(src, destIter, EqualityComparer<string>.Default);
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, (result as ListIterator<string>).Position);
        Assert.AreEqual(MARKER, (result as ListIterator<string>).Read());
    }
    #endregion

    #region List + predicate functions
    [Test] public void  UniqueCopy_List_Predicate_ListIterator() 
    {
        ListIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();

        ListIterator<string> result = Algorithm.UniqueCopy(src, destIter, delegate(string lhs, string rhs) {return lhs == rhs;});
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, result.Position);
        Assert.AreEqual(MARKER, (result as ListIterator<string>).Read());
    }

    [Test] public void  UniqueCopy_List_Predicate_RandomAccessIterator() 
    {
        RandomAccessIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        RandomAccessIterator<string> result = Algorithm.UniqueCopy(src, destIter, delegate(string lhs, string rhs) {return lhs == rhs;});
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, result.Position);
        Assert.AreEqual(MARKER, (result as ListIterator<string>).Read());
    }

    [Test] public void  UniqueCopy_List_Predicate_ForwardIterator() 
    {
        ForwardIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();

        ForwardIterator<string> result = Algorithm.UniqueCopy(src, destIter, delegate(string lhs, string rhs) {return lhs == rhs;});
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, IteratorUtil.Distance(IteratorUtil.Begin(dest), result));
        Assert.AreEqual(MARKER, (result as ListIterator<string>).Read());
    }

    [Test] public void  UniqueCopy_List_Predicate_OutputIterator() 
    {
        OutputIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();

        OutputIterator<string> result = Algorithm.UniqueCopy(src, destIter, delegate(string lhs, string rhs) {return lhs == rhs;});
        VerifyOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(6, (result as ListIterator<string>).Position);
        Assert.AreEqual(MARKER, (result as ListIterator<string>).Read());
    }
    #endregion
    
    [Test] public void  UniqueCopy_ListToList() 
    {
        ListIterator<string> result = Algorithm.UniqueCopy(src, dest);
        VerifyListToListOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(5, result.Position);
        Assert.AreEqual(MARKER, (result as ListIterator<string>).Read());

    }

    [Test] public void  UniqueCopy_ListToList_Comparer() 
    {
        ListIterator<string> result = Algorithm.UniqueCopy(src, dest, EqualityComparer<string>.Default);
        VerifyListToListOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(5, result.Position);
        Assert.AreEqual(MARKER, (result as ListIterator<string>).Read());
    }

    [Test] public void  UniqueCopy_ListToList_Predicate() 
    {
        ListIterator<string> result = Algorithm.UniqueCopy(src, dest, delegate(string lhs, string rhs) {return lhs == rhs;});
        VerifyListToListOutput();
        Assert.IsNotNull(result);
        Assert.AreEqual(5, result.Position);
        Assert.AreEqual(MARKER, (result as ListIterator<string>).Read());
    }
    
    protected override string[] GetValues()
    {
        string[] result = Constants.TEST_BIG_STRING_ARRAY;
        Array.Sort(result);
        return result;
    }

    void VerifyOutput()
    {
        Assert.AreEqual(MARKER, dest[0]);
        Assert.AreEqual("1"   , dest[1]);
        Assert.AreEqual("2"   , dest[2]);
        Assert.AreEqual("3"   , dest[3]);
        Assert.AreEqual("4"   , dest[4]);
        Assert.AreEqual("5"   , dest[5]);
        for(int i=6; i<dest.Length; ++i)
            Assert.AreEqual(MARKER, dest[i]);
    }

    void VerifyListToListOutput()
    {
        Assert.AreEqual("1", dest[0]);
        Assert.AreEqual("2", dest[1]);
        Assert.AreEqual("3", dest[2]);
        Assert.AreEqual("4", dest[3]);
        Assert.AreEqual("5", dest[4]);
        for(int i=5; i<dest.Length; ++i)
            Assert.AreEqual(MARKER, dest[i]);
    }
}

}
