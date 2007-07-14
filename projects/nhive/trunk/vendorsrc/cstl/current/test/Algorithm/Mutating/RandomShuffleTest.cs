using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{


[TestFixture] 
[Category("Algorithm")]
public class RandomShuffleTest : BaseIntTest
{
    [Test] public void RandomShuffle_Iterator() 
    {
        // Not much of a test. 
        Algorithm.RandomShuffle(IteratorUtil.Begin(src), IteratorUtil.End(src));
        Algorithm.RandomShuffle(IteratorUtil.Begin(src), IteratorUtil.End(src));
    }

    [Test] public void RandomShuffle_Iterator_Func() 
    {
        Random rand = new Random(System.Environment.TickCount);
        Algorithm.RandomShuffle(IteratorUtil.Begin(src), IteratorUtil.End(src), delegate(int max) { return rand.Next(max);});
        Algorithm.RandomShuffle(IteratorUtil.Begin(src), IteratorUtil.End(src), delegate(int max) { return rand.Next(max);});
    }

    [Test] public void RandomShuffle_List() 
    {
        Algorithm.RandomShuffle(src);
        Algorithm.RandomShuffle(src);
    }

    [Test] public void RandomShuffle_List_Func() 
    {
        Random rand = new Random(System.Environment.TickCount);
        Algorithm.RandomShuffle(src, delegate(int max) { return rand.Next(max);});
        Algorithm.RandomShuffle(src, delegate(int max) { return rand.Next(max);});
    }


    protected override int[] GetValues()
    {
        return new int[]{1,2,3,4,5,6,7,8,9,10};
    }
}

}