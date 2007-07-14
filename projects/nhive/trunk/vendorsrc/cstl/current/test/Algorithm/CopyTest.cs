using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL.Iterator;
using CSTL;

namespace CSTL.Test
{


[TestFixture] 
public class CopyTest
{
    [Test] public void Copy_IteratorInt() 
    {
        // test using simple int arrays
        int[] src  = Constants.TEST_INT_ARRAY;
        int[] dest = new int[src.Length+2];
        dest[0] = Int32.MinValue; 
        dest[dest.Length-1] = Int32.MinValue;
        
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        Algorithm.Copy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter);
        
        Assert.AreEqual(dest[0], Int32.MinValue);
        Assert.AreEqual(dest[dest.Length-1], Int32.MinValue);
        for(int i=0; i<src.Length;++i)
        {
            Assert.AreEqual(dest[i+1], src[i]);
        } 
    }

    [Test] public void Copy_IteratorString() 
    {
        // test using simple int arrays
        string[] src  = Constants.TEST_STRING_ARRAY;
        string[] dest = new string[src.Length+2];
        dest[0] = "XXXX"; 
        dest[dest.Length-1] = "XXXX";
        
        ListIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        Algorithm.Copy(IteratorUtil.Begin(src), IteratorUtil.End(src), destIter);
        
        Assert.AreEqual(dest[0], "XXXX");
        Assert.AreEqual(dest[dest.Length-1], "XXXX");
        for(int i=0; i<src.Length;++i)
        {
            Assert.AreEqual(dest[i+1], src[i]);
        } 
    }
    
    [Test] public void Copy_EnumerableInt() 
    {
        // test using simple int arrays
        int[] src  = Constants.TEST_INT_ARRAY;
        int[] dest = new int[src.Length+2];
        dest[0] = Int32.MinValue; 
        dest[dest.Length-1] = Int32.MinValue;
        
        ListIterator<int> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        Algorithm.Copy(src, destIter);
        
        Assert.AreEqual(dest[0], Int32.MinValue);
        Assert.AreEqual(dest[dest.Length-1], Int32.MinValue);
        for(int i=0; i<src.Length;++i)
        {
            Assert.AreEqual(dest[i+1], src[i]);
        } 
    }  
    
    [Test] public void Copy_EnumerableString() 
    {
        // test using simple int arrays
        string[] src  = Constants.TEST_STRING_ARRAY;
        string[] dest = new string[src.Length+2];
        dest[0] = "XXXX"; 
        dest[dest.Length-1] = "XXXX";
        
        ListIterator<string> destIter = IteratorUtil.Begin(dest);
        destIter.MoveNext();
        
        Algorithm.Copy(src, destIter);
        
        Assert.AreEqual(dest[0], "XXXX");
        Assert.AreEqual(dest[dest.Length-1], "XXXX");
        for(int i=0; i<src.Length;++i)
        {
            Assert.AreEqual(dest[i+1], src[i]);
        } 
    }
    
    [Test] public void Copy_EnumerableIntToList() 
    {
        // test using simple int arrays
        int[] src  = Constants.TEST_INT_ARRAY;
        int[] dest = new int[src.Length];
        
        Algorithm.Copy(src, dest);
        for(int i=0; i<src.Length;++i)
        {
            Assert.AreEqual(dest[i], src[i]);
        } 
    }   
    
    
    [Test] public void Copy_IteratorIntToList() 
    {
        // test using simple int arrays
        int[] src  = Constants.TEST_INT_ARRAY;
        int[] dest = new int[src.Length+2];

        Algorithm.Copy(IteratorUtil.Begin(src), IteratorUtil.End(src), dest);
        for(int i=0; i<src.Length;++i)
        {
            Assert.AreEqual(dest[i], src[i]);
        } 
    }

    [Test] public void Copy_ToList() 
    {
        // test using simple int arrays
        int[] src  = Constants.TEST_INT_ARRAY;
        int[] dest = new int[src.Length+2];
        dest[0] = int.MinValue;
        dest[dest.Length-1] = int.MinValue;

        Algorithm.Copy(src, 2, dest, 1);
        Assert.AreEqual(dest[0], int.MinValue);
        Assert.AreEqual(dest[dest.Length-1], int.MinValue);
        
        for(int i=2; i<src.Length;++i)
        {
            Assert.AreEqual(dest[i-1], src[i]);
        }     
    } 
}

}