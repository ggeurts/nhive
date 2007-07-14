using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL.Iterator;
using CSTL;
using CSTL.Collections;

namespace CSTL.Test
{


[TestFixture] 
[Category("Collection")]
public class VectorTest_Int : BaseIntTest
{
    [Test] public void Test() 
    {
        Vector<int> v = new Vector<int>();
        v.PushBack(1);
        v.PushBack(2);
        v.PushBack(3);
        v.PushBack(4);
        v.PushBack(5);
        
        Algorithm.ForEach(v.Begin(), v.End(), Console.WriteLine);
    }
    
    [Test] public void TestConstructors() 
    {
        Vector<int> v = new Vector<int>();
        Assert.AreEqual(0, v.Count);
        Assert.AreEqual(0, v.Capacity);
        Assert.IsTrue(v.Begin().Equals(v.End()));
        Assert.AreEqual(0, IteratorUtil.Distance(v.Begin(), v.End()));

        int[] src = new int[]{1,2,3,4,5};
        v = new Vector<int>(src);
        Assert.AreEqual(src.Length, v.Count);
        Assert.IsFalse(v.Begin().Equals(v.End()));
        Assert.AreEqual(src.Length, IteratorUtil.Distance(v.Begin(), v.End()));
        for(int i=0; i<src.Length; ++i)
        {
            Assert.AreEqual(src[i], v[i]);
        }

        v = new Vector<int>(IteratorUtil.Begin(src), IteratorUtil.End(src));
        Assert.AreEqual(src.Length, v.Count);
        Assert.IsFalse(v.Begin().Equals(v.End()));
        Assert.AreEqual(src.Length, IteratorUtil.Distance(v.Begin(), v.End()));
        for(int i=0; i<src.Length; ++i)
        {
            Assert.AreEqual(src[i], v[i]);
        }

        v = new Vector<int>(v);
        Assert.AreEqual(src.Length, v.Count);
        Assert.IsFalse(v.Begin().Equals(v.End()));
        Assert.AreEqual(src.Length, IteratorUtil.Distance(v.Begin(), v.End()));
        for(int i=0; i<src.Length; ++i)
        {
            Assert.AreEqual(src[i], v[i]);
        }

        v = new Vector<int>(10);
        Assert.AreEqual(10, v.Count);
        Assert.IsFalse(v.Begin().Equals(v.End()));
        Assert.AreEqual(10, IteratorUtil.Distance(v.Begin(), v.End()));
        Algorithm.ForEach(v.Begin(), v.End(), delegate(int x) {
            Assert.AreEqual(0, x);
        });

        v = new Vector<int>(10, 42);
        Assert.AreEqual(10, v.Count);
        Assert.IsFalse(v.Begin().Equals(v.End()));
        Assert.AreEqual(10, IteratorUtil.Distance(v.Begin(), v.End()));
        Algorithm.ForEach(v.Begin(), v.End(), delegate(int x) {
            Assert.AreEqual(42, x);
        });
    }

    [Test] public void TestAssign() 
    {
        Vector<int> src  = new Vector<int>(new int[]{1,2,3,4,5,6,7,8,9,10});
        Vector<int> dest = new Vector<int>(20,50);
        
        dest.Assign(src.Begin(), src.End());
        
        Assert.AreEqual(src.Count, dest.Count);
        for(int i=0; i<dest.Count; ++i)
            Assert.AreEqual(src[i], dest[i]);
            
        dest.Assign(20,42);
        Assert.AreEqual(20, dest.Count);
        for(int i=0; i<dest.Count; ++i)
            Assert.AreEqual(42, dest[i]);
    }

    [Test] public void TestSwap() 
    {
        Vector<int> src  = new Vector<int>(new int[]{1,2,3,4,5,6,7,8,9,10});
        Vector<int> dest = new Vector<int>(2000,42);
        
        dest.Swap(src);
        
        Assert.AreEqual(2000, src.Count);
        Assert.AreEqual(10, dest.Count);
        
        for(int i=1; i<=10; ++i)
            Assert.AreEqual(i, dest[i-1]);
            
        for(int i=0; i<2000; ++i)
            Assert.AreEqual(42, src[i]);
    }

    [Test] public void TestStackOps() 
    {
        Vector<int> v = new Vector<int>();
        v.PushBack(1);
        Assert.AreEqual(1, v.Count);
        Assert.AreEqual(1,v[0]);

        v.PushBack(2);
        Assert.AreEqual(2, v.Count);
        Assert.AreEqual(1,v[0]);
        Assert.AreEqual(2,v[1]);
        
        v.PopBack();
        Assert.AreEqual(1, v.Count);
        Assert.AreEqual(1,v[0]);

        v.PushBack(42);
        Assert.AreEqual(2, v.Count);
        Assert.AreEqual(1,v[0]);
        Assert.AreEqual(42,v[1]);

        v.PopBack();
        Assert.AreEqual(1, v.Count);
        Assert.AreEqual(1,v[0]);

        v.PopBack();
        Assert.AreEqual(0, v.Count);
        
        v.PopBack(); // test underflow
        Assert.AreEqual(0, v.Count);
    }
    
    [Test] public void TestInsert() 
    {
        int[] array = new int[]{0,1,2,3,4,5,6};
        Vector<int> v = new Vector<int>(array);

        // test insertion in the middle
        ListIterator<int> pos   = new ListIterator<int>(v, 4);
        ListIterator<int> result = v.Insert(pos, 29);
        Assert.AreEqual(array.Length+1, v.Count);
        Assert.AreEqual(3, v[3]);
        Assert.AreEqual(29, v[4]);
        Assert.AreEqual(4, v[5]);
        Assert.AreEqual(29, result.Read());

        v = new Vector<int>(array);
        v.Insert(4, 29);
        Assert.AreEqual(array.Length+1, v.Count);
        Assert.AreEqual(3, v[3]);
        Assert.AreEqual(29, v[4]);
        Assert.AreEqual(4, v[5]);

        // test insertion at the beginning
        v = new Vector<int>(array);
        result = v.Insert(v.Begin(), 29);
        Assert.AreEqual(array.Length+1, v.Count);
        Assert.AreEqual(29, v[0]);
        Assert.AreEqual(0, v[1]);
        Assert.AreEqual(1, v[2]);
        Assert.AreEqual(29, result.Read());

        v = new Vector<int>(array);
        v.Insert(0, 29);
        Assert.AreEqual(29, v[0]);
        Assert.AreEqual(0, v[1]);
        Assert.AreEqual(1, v[2]);

        // test insertion at the end
        v = new Vector<int>(array);
        result = v.Insert(v.End(), 29);
        Assert.AreEqual(array.Length+1, v.Count);
        Assert.AreEqual(5, v[5]);
        Assert.AreEqual(6, v[6]);
        Assert.AreEqual(29, v[7]);
        Assert.AreEqual(29, result.Read());

        v = new Vector<int>(array);
        v.Insert(v.Count, 29);
        Assert.AreEqual(5, v[5]);
        Assert.AreEqual(6, v[6]);
        Assert.AreEqual(29, v[7]);
    }


    [Test] public void TestRangeInsert() 
    {
        int[] array = new int[]{0,1,2,3,4,5,6};
        Vector<int> v = new Vector<int>(array);
        int[] splice = new int[]{-1,-2,-3};

        // range insert in the middle
        ListIterator<int> pos   = new ListIterator<int>(v, 4);
        v.Insert(pos, IteratorUtil.Begin(splice), IteratorUtil.End(splice));
        Assert.AreEqual(array.Length+splice.Length, v.Count);
        Assert.AreEqual(3, v[3]);
        Assert.AreEqual(-1, v[4]);
        Assert.AreEqual(-2, v[5]);
        Assert.AreEqual(-3, v[6]);
        Assert.AreEqual(4, v[7]);

        v = new Vector<int>(array);
        pos = new ListIterator<int>(v, 4);
        v.Insert(pos, 3, 42);
        Assert.AreEqual(array.Length+splice.Length, v.Count);
        Assert.AreEqual(3, v[3]);
        Assert.AreEqual(42, v[4]);
        Assert.AreEqual(42, v[5]);
        Assert.AreEqual(42, v[6]);
        Assert.AreEqual(4, v[7]);

        // range insert at the beginning
        v = new Vector<int>(array);
        v.Insert(v.Begin(), IteratorUtil.Begin(splice), IteratorUtil.End(splice));
        Assert.AreEqual(array.Length+splice.Length, v.Count);
        Assert.AreEqual(-1, v[0]);
        Assert.AreEqual(-2, v[1]);
        Assert.AreEqual(-3, v[2]);
        Assert.AreEqual(0, v[3]);
        Assert.AreEqual(1, v[4]);

        v = new Vector<int>(array);
        v.Insert(v.Begin(), 3, 42);
        Assert.AreEqual(array.Length+splice.Length, v.Count);
        Assert.AreEqual(42, v[0]);
        Assert.AreEqual(42, v[1]);
        Assert.AreEqual(42, v[2]);
        Assert.AreEqual(0, v[3]);
        Assert.AreEqual(1, v[4]);

        // range insert at the end
        v = new Vector<int>(array);
        v.Insert(v.End(), IteratorUtil.Begin(splice), IteratorUtil.End(splice));
        Assert.AreEqual(array.Length+splice.Length, v.Count);
        Assert.AreEqual(5, v[5]);
        Assert.AreEqual(6, v[6]);
        Assert.AreEqual(-1, v[7]);
        Assert.AreEqual(-2, v[8]);
        Assert.AreEqual(-3, v[9]);

        v = new Vector<int>(array);
        v.Insert(v.End(), 3, 42);
        Assert.AreEqual(array.Length+splice.Length, v.Count);
        Assert.AreEqual(5, v[5]);
        Assert.AreEqual(6, v[6]);
        Assert.AreEqual(42, v[7]);
        Assert.AreEqual(42, v[8]);
        Assert.AreEqual(42, v[9]);
    }


    [Test] public void TestAddRange() 
    {
        int[] array = new int[]{0,1,2,3,4,5,6};
        Vector<int> v = new Vector<int>(array);
        int[] splice = new int[]{-1,-2,-3};
        
        v.AddRange(splice);
        Assert.AreEqual(array.Length+splice.Length, v.Count);
        Assert.AreEqual(5, v[5]);
        Assert.AreEqual(6, v[6]);
        Assert.AreEqual(-1, v[7]);
        Assert.AreEqual(-2, v[8]);
        Assert.AreEqual(-3, v[9]);
        
        v = new Vector<int>(array);
        v.AddRange(IteratorUtil.Begin(splice), IteratorUtil.End(splice));
        Assert.AreEqual(array.Length+splice.Length, v.Count);
        Assert.AreEqual(5, v[5]);
        Assert.AreEqual(6, v[6]);
        Assert.AreEqual(-1, v[7]);
        Assert.AreEqual(-2, v[8]);
        Assert.AreEqual(-3, v[9]);
    }
    
    [Test] public void TestRemoveAt()
    {
        int[] array = new int[]{0,1,2,3,4,5,6};
        Vector<int> v = new Vector<int>(array);
        
        v.RemoveAt(4);
        Assert.AreEqual(array.Length-1, v.Count);
        Assert.AreEqual(3, v[3]);
        Assert.AreEqual(5, v[4]);
        
        v.Assign(array);
        v.RemoveAt(0);
        Assert.AreEqual(array.Length-1, v.Count);
        Assert.AreEqual(1, v[0]);
        Assert.AreEqual(2, v[1]);

        v.Assign(array);
        v.RemoveAt(v.Count-1);
        Assert.AreEqual(array.Length-1, v.Count);
        Assert.AreEqual(4, v[4]);
        Assert.AreEqual(5, v[5]);
    }

    [Test] public void TestErase()
    {
        int[] array = new int[]{0,1,2,3,4,5,6};
        Vector<int> v = new Vector<int>(array);
        ListIterator<int> result;
        
        result = v.Erase(v.Begin().OffsetBy(4));
        Assert.AreEqual(array.Length-1, v.Count);
        Assert.AreEqual(3, v[3]);
        Assert.AreEqual(5, v[4]);
        Assert.AreEqual(4, result.Position);
        Assert.AreEqual(5, result.Read());
        
        v.Assign(array);
        result = v.Erase(v.Begin());
        Assert.AreEqual(array.Length-1, v.Count);
        Assert.AreEqual(1, v[0]);
        Assert.AreEqual(2, v[1]);
        Assert.AreEqual(0, result.Position);
        Assert.AreEqual(1, result.Read());

        v.Assign(array);
        result = v.Erase(v.End().OffsetBy(-1));
        Assert.AreEqual(array.Length-1, v.Count);
        Assert.AreEqual(4, v[4]);
        Assert.AreEqual(5, v[5]);
        Assert.AreEqual(array.Length-1, result.Position);
        Assert.IsTrue(result.Equals(v.End()));
    }

    [Test] public void TestRangeErase()
    {
        int[] array = new int[]{0,1,2,3,4,5,6};
        Vector<int> v = new Vector<int>(array);
        ListIterator<int> result;

        result = v.Erase(v.Begin().OffsetBy(2), v.Begin().OffsetBy(4));
        Assert.AreEqual(array.Length-2, v.Count);
        Assert.AreEqual(0, v[0]);
        Assert.AreEqual(1, v[1]);
        Assert.AreEqual(4, v[2]);
        Assert.AreEqual(5, v[3]);
        Assert.AreEqual(6, v[4]);
        Assert.AreEqual(2, result.Position);

        v.Assign(array);
        result = v.Erase(v.Begin(), v.Begin().OffsetBy(2));
        Assert.AreEqual(array.Length-2, v.Count);
        Assert.AreEqual(2, v[0]);
        Assert.AreEqual(3, v[1]);
        Assert.AreEqual(4, v[2]);
        Assert.AreEqual(5, v[3]);
        Assert.AreEqual(6, v[4]);
        Assert.AreEqual(0, result.Position);

        v.Assign(array);
        result = v.Erase(v.Begin().OffsetBy(5), v.End());
        Assert.AreEqual(array.Length-2, v.Count);
        Assert.AreEqual(0, v[0]);
        Assert.AreEqual(1, v[1]);
        Assert.AreEqual(2, v[2]);
        Assert.AreEqual(3, v[3]);
        Assert.AreEqual(4, v[4]);
        Assert.AreEqual(array.Length-2, result.Position);
    }
    
    [Test] public void TestIteratorMethods()
    {
        int[] array = new int[]{-100,1,2,3,4,5,6,100};
        Vector<int> v = new Vector<int>(array);
        
        Assert.AreEqual(0, v.Begin().Position);
        Assert.AreEqual(-100, v.Begin().Read());
        Assert.AreEqual(array.Length, v.End().Position);
        
        ListIterator<int> last = v.End().OffsetBy(-1);
        Assert.AreEqual(array.Length-1, last.Position);
        Assert.AreEqual(100, last.Read());
    }

    [Test] public void TestFrontBack()
    {
        int[] array = new int[]{-100,1,2,3,4,5,6,100};
        Vector<int> v = new Vector<int>(array);
        
        Assert.AreEqual(-100, v.Front);
        Assert.AreEqual(100, v.Back);
    }    
}

}
