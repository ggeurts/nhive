//-----------------------------------------------------------------------------
// CSTL
// Copyright (C) 2006 by Harold Howe hhjunk@mchsi.com
// http://sourceforge.net/projects/cstl

using System;
using System.Collections.Generic;
using CSTL.Iterator;


namespace CSTL
{

public static partial class Algorithm
{
    public delegate int RandomShuffleFunc(int max);

    static public void RandomShuffle<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end)
    {
        RandomShuffle(begin, end, GetRandomNumber);
    }
    
    static public void RandomShuffle<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end, RandomShuffleFunc func)
    {
        if(begin.Equals(end))
            return;

        begin = IteratorUtil.Clone(begin);
        for(RandomAccessIterator<T> iter = IteratorUtil.AdvanceCopy(begin,1); !iter.Equals(end); iter.MoveNext())
        {
            begin.Position = func(iter.Position+1);
            IteratorUtil.Swap(iter, begin);
        }
    }

    static public void RandomShuffle<T>(IList<T> list)
    {
        RandomShuffle(IteratorUtil.Begin(list), IteratorUtil.End(list));
    }
    
    static public void RandomShuffle<T>(IList<T> list, RandomShuffleFunc func)
    {
        RandomShuffle(IteratorUtil.Begin(list), IteratorUtil.End(list), func);
    }


#if DEBUG
    static private readonly Random m_RandomGenerator = new Random(0);
#else
    static private readonly Random m_RandomGenerator = new Random(System.Environment.TickCount);
#endif

    static private int GetRandomNumber(int max)
    {
        lock(m_RandomGenerator)
        return m_RandomGenerator.Next(max);
    }
}
}