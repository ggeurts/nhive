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
    public static void Merge<T>(InputIterator<T> begin1, InputIterator<T> end1, 
                                InputIterator<T> begin2, InputIterator<T> end2, OutputIterator<T> dest)
        where T:IComparable<T>
    {
        Merge(begin1, end1, begin2, end2, dest, Comparer<T>.Default);
    }
    
    public static void Merge<T>(InputIterator<T> begin1, InputIterator<T> end1, 
                                InputIterator<T> begin2, InputIterator<T> end2, OutputIterator<T> dest, IComparer<T>comp)
    {
        T t1, t2;
        begin1 = IteratorUtil.Clone(begin1); 
        begin2 = IteratorUtil.Clone(begin2); 
        dest = IteratorUtil.Clone(dest);
        for(; !begin1.Equals(end1) && !begin2.Equals(end2); dest.MoveNext())
        {
            t1  = begin1.Read();
            t2  = begin2.Read();
            int compare = comp.Compare(t1,t2);
            
            if(compare == -1)
            {
                dest.Write(t1);
                begin1.MoveNext();
            }
            else
            {
                dest.Write(t2);
                begin2.MoveNext();
            }
        }
        
        Copy(begin1, end1, dest);
        Copy(begin2, end2, dest);
    }

#if SORT_WITH_BINARYPREDICATE
    public static void Merge<T>(InputIterator<T> begin1, InputIterator<T> end1, 
                                InputIterator<T> begin2, InputIterator<T> end2, OutputIterator<T> dest, Functional.BinaryPredicate<T> func)
    {
        T t1, t2;
        begin1 = IteratorUtil.Clone(begin1); 
        begin2 = IteratorUtil.Clone(begin2); 
        dest = IteratorUtil.Clone(dest);
        for(; !begin1.Equals(end1) && !begin2.Equals(end2); dest.MoveNext())
        {
            t1  = begin1.Read();
            t2  = begin2.Read();
            bool less = func(t1,t2);
            
            if(less)
            {
                dest.Write(t1);
                begin1.MoveNext();
            }
            else
            {
                dest.Write(t2);
                begin2.MoveNext();
            }
        }
        
        Copy(begin1, end1, dest);
        Copy(begin2, end2, dest);
    }
#else
    public static void Merge<T>(InputIterator<T> begin1, InputIterator<T> end1, 
                                InputIterator<T> begin2, InputIterator<T> end2, OutputIterator<T> dest, Comparison<T>comp)
    {
        Merge(begin1, end1, begin2, end2, dest, Functional.Wrap(comp));
    }
#endif

}
}