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
    #region Iterator methods
    public static ForwardIterator<T> MinElement<T>(ForwardIterator<T> begin, ForwardIterator<T> end)
        where T: IComparable<T>
    {
        if(begin.Equals(end))
            return null;

        begin = IteratorUtil.Clone(begin);
        ForwardIterator<T> minIter = IteratorUtil.Clone(begin);
        T min = minIter.Read();
        begin.MoveNext();
        for(; !begin.Equals(end); begin.MoveNext())
        {
            T temp = begin.Read();
            if(object.ReferenceEquals(temp,null))
                continue;

            int compare = temp.CompareTo(min);
            if(compare == -1)
            {
                min = temp;
                minIter = IteratorUtil.Clone(begin);
            }
        }

        return minIter;
    }

    public static RandomAccessIterator<T> MinElement<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end)
        where T: IComparable<T>
    {
        return (RandomAccessIterator<T>) MinElement((ForwardIterator<T>)begin, (ForwardIterator<T>)end);
    }

    public static ListIterator<T> MinElement<T>(ListIterator<T> begin, ListIterator<T> end)
        where T: IComparable<T>
    {
        return (ListIterator<T>) MinElement((ForwardIterator<T>)begin, (ForwardIterator<T>)end);
    }

    public static I MinElement<I,T>(I begin, I end, IComparer<T> comparer)
        where I: class, ForwardIterator<T>
    {
        if(begin.Equals(end))
            return null;

        begin = IteratorUtil.Clone(begin);
        I minIter = IteratorUtil.Clone(begin);
        T min = minIter.Read();
        begin.MoveNext();
        for(; !begin.Equals(end); begin.MoveNext())
        {
            T temp = begin.Read();
            int compare = comparer.Compare(temp, min);
            if(compare == -1)
            {
                min = temp;
                minIter = IteratorUtil.Clone(begin);
            }
        }

        return minIter;
    }

#if SORT_WITH_BINARYPREDICATE
    public static ForwardIterator<T> MinElement<T>(ForwardIterator<T> begin, ForwardIterator<T> end, Functional.BinaryPredicate<T> func)
    {
        if(begin.Equals(end))
            return null;

        begin = IteratorUtil.Clone(begin);
        ForwardIterator<T> minIter = IteratorUtil.Clone(begin);
        T min = minIter.Read();
        begin.MoveNext();
        for(; !begin.Equals(end); begin.MoveNext())
        {
            T temp = begin.Read();
            bool less = func(temp, min);
            if(less)
            {
                min = temp;
                minIter = IteratorUtil.Clone(begin);
            }
        }

        return minIter;
    }

    public static RandomAccessIterator<T> MinElement<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end, Functional.BinaryPredicate<T> func)
    {
        return (RandomAccessIterator<T>) MinElement((ForwardIterator<T>)begin, (ForwardIterator<T>)end, func);
    }

    public static ListIterator<T> MinElement<T>(ListIterator<T> begin, ListIterator<T> end, Functional.BinaryPredicate<T> func)
    {
        return (ListIterator<T>) MinElement((ForwardIterator<T>)begin, (ForwardIterator<T>)end, func);
    }

#else
    public static ForwardIterator<T> MinElement<T>(ForwardIterator<T> begin, ForwardIterator<T> end, Comparison<T> comp)
    {
        return MinElement(begin, end, Functional.Wrap(comp));
    }

    public static RandomAccessIterator<T> MinElement<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end, Comparison<T> comp)
    {
        return (RandomAccessIterator<T>) MinElement((ForwardIterator<T>)begin, (ForwardIterator<T>)end, comp);
    }

    public static ListIterator<T> MinElement<T>(ListIterator<T> begin, ListIterator<T> end, Comparison<T> comp)
    {
        return (ListIterator<T>) MinElement((ForwardIterator<T>)begin, (ForwardIterator<T>)end, comp);
    }
#endif   


    #endregion

    #region List methods
    public static ListIterator<T> MinElement<T>(IList<T> list)
        where T: IComparable<T>
    {
        return MinElement(IteratorUtil.Begin(list), IteratorUtil.End(list));
    }

    public static ListIterator<T> MinElement<T>(IList<T> list, IComparer<T> comparer)
    {
        return MinElement(IteratorUtil.Begin(list), IteratorUtil.End(list), comparer);
    }

#if SORT_WITH_BINARYPREDICATE
    public static ListIterator<T> MinElement<T>(IList<T> list, Functional.BinaryPredicate<T> func)
    {
        return MinElement(IteratorUtil.Begin(list), IteratorUtil.End(list), func);
    }
#else
    public static ListIterator<T> MinElement<T>(IList<T> list, Comparison<T> func)
    {
        return MinElement(IteratorUtil.Begin(list), IteratorUtil.End(list), func);
    }
#endif    
    #endregion 

    #region Enumerable methods. 
    public static T MinElement<T>(IEnumerable<T> enumerable)
        where T: IComparable<T>
    {
        IEnumerator<T> enumerator = enumerable.GetEnumerator();
        if(!enumerator.MoveNext())
            return default(T);
           
        T min = enumerator.Current;
        while(enumerator.MoveNext())
        {
            T temp = enumerator.Current;
            int compare = temp.CompareTo(min);
            if(compare == -1)
            {
                min = temp;
            }
        }

        return min;
    }

    public static T MinElement<T>(IEnumerable<T> enumerable, IComparer<T> comparer)
    {
        IEnumerator<T> enumerator = enumerable.GetEnumerator();
        if(!enumerator.MoveNext())
            return default(T);
           
        T min = enumerator.Current;
        while(enumerator.MoveNext())
        {
            T temp = enumerator.Current;
            int compare = comparer.Compare(temp, min);
            if(compare == -1)
                min = temp;
        }
        
        return min;    
    }
    
#if SORT_WITH_BINARYPREDICATE
    public static T MinElement<T>(IEnumerable<T> enumerable, Functional.BinaryPredicate<T> func)
    {
        IEnumerator<T> enumerator = enumerable.GetEnumerator();
        if(!enumerator.MoveNext())
            return default(T);
           
        T min = enumerator.Current;
        while(enumerator.MoveNext())
        {
            T temp = enumerator.Current;
            bool less = func(temp, min);
            if(less)
                min = temp;
        }
        
        return min;    
    }
#else
    public static T MinElement<T>(IEnumerable<T> enumerable, Comparison<T> func)
    {
        return MinElement(enumerable, Functional.Wrap(func));
    }
#endif    
    #endregion
}
}

