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
    public static ForwardIterator<T> MaxElement<T>(ForwardIterator<T> begin, ForwardIterator<T> end)
        where T: IComparable<T>
    {
        if(begin.Equals(end))
            return null;

        begin = IteratorUtil.Clone(begin);
        ForwardIterator<T> maxIter = IteratorUtil.Clone(begin);
        T max = maxIter.Read();
        begin.MoveNext();
        for(; !begin.Equals(end); begin.MoveNext())
        {
            T temp = begin.Read();
            if(object.ReferenceEquals(temp,null))
                continue;

            int compare = temp.CompareTo(max);
            if(compare == 1)
            {
                max = temp;
                maxIter = IteratorUtil.Clone(begin);
            }
        }

        return maxIter;
    }

    public static RandomAccessIterator<T> MaxElement<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end)
        where T: IComparable<T>
    {
        return (RandomAccessIterator<T>) MaxElement((ForwardIterator<T>)begin, (ForwardIterator<T>)end);
    }

    public static ListIterator<T> MaxElement<T>(ListIterator<T> begin, ListIterator<T> end)
        where T: IComparable<T>
    {
        return (ListIterator<T>) MaxElement((ForwardIterator<T>)begin, (ForwardIterator<T>)end);
    }

    public static I MaxElement<I,T>(I begin, I end,  IComparer<T> comparer)
        where I: class, ForwardIterator<T>    
    {
        if(begin.Equals(end))
            return null;

        begin = IteratorUtil.Clone(begin);
        I maxIter = IteratorUtil.Clone(begin);
        T max = maxIter.Read();
        begin.MoveNext();
        for(; !begin.Equals(end); begin.MoveNext())
        {
            T temp = begin.Read();
            int compare = comparer.Compare(temp, max);
            if(compare == 1)
            {
                max = temp;
                maxIter = IteratorUtil.Clone(begin);
            }
        }
        
        return maxIter;
    }
    
#if SORT_WITH_BINARYPREDICATE
    public static ForwardIterator<T> MaxElement<T>(ForwardIterator<T> begin, ForwardIterator<T> end, Functional.BinaryPredicate<T> func)
    {
        if(begin.Equals(end))
            return null;
            
        begin = IteratorUtil.Clone(begin);
        ForwardIterator<T> maxIter = IteratorUtil.Clone(begin);
        T max = maxIter.Read();
        begin.MoveNext();
        for(; !begin.Equals(end); begin.MoveNext())
        {
            T temp = begin.Read();
            bool more = func(temp, max);
            if(more)
            {
                max = temp;
                maxIter = IteratorUtil.Clone(begin);
            }
        }
        
        return maxIter;
    }

    public static RandomAccessIterator<T> MaxElement<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end, Functional.BinaryPredicate<T> func)
    {
        return (RandomAccessIterator<T>) MaxElement((ForwardIterator<T>)begin, (ForwardIterator<T>)end, func);
    }

    public static ListIterator<T> MaxElement<T>(ListIterator<T> begin, ListIterator<T> end, Functional.BinaryPredicate<T> func)
    {
        return (ListIterator<T>) MaxElement((ForwardIterator<T>)begin, (ForwardIterator<T>)end, func);
    }

#else
    public static ForwardIterator<T> MaxElement<T>(ForwardIterator<T> begin, ForwardIterator<T> end, Comparison<T> comp)
    {
        return MaxElement(begin, end, Functional.Wrap(comp));
    }

    public static RandomAccessIterator<T> MaxElement<T>(RandomAccessIterator<T> begin, RandomAccessIterator<T> end, Comparison<T> comp)
    {
        return (RandomAccessIterator<T>) MaxElement((ForwardIterator<T>)begin, (ForwardIterator<T>)end, comp);
    }

    public static ListIterator<T> MaxElement<T>(ListIterator<T> begin, ListIterator<T> end, Comparison<T> comp)
    {
        return (ListIterator<T>) MaxElement((ForwardIterator<T>)begin, (ForwardIterator<T>)end, comp);
    }
#endif   


    #endregion

    #region List methods
    public static ListIterator<T> MaxElement<T>(IList<T> list)
        where T: IComparable<T>
    {
        return MaxElement(IteratorUtil.Begin(list), IteratorUtil.End(list));
    }

    public static ListIterator<T> MaxElement<T>(IList<T> list, IComparer<T> comparer)
    {
        return MaxElement(IteratorUtil.Begin(list), IteratorUtil.End(list), comparer);
    }

#if SORT_WITH_BINARYPREDICATE
    public static ListIterator<T> MaxElement<T>(IList<T> list, Functional.BinaryPredicate<T> func)
    {
        return MaxElement(IteratorUtil.Begin(list), IteratorUtil.End(list), func);
    }
#else
    public static ListIterator<T> MaxElement<T>(IList<T> list, Comparison<T> func)
    {
        return MaxElement(IteratorUtil.Begin(list), IteratorUtil.End(list), func);
    }
#endif    
    #endregion 

    #region Enumerable methods. 
    public static T MaxElement<T>(IEnumerable<T> enumerable)
        where T: IComparable<T>
    {
        IEnumerator<T> enumerator = enumerable.GetEnumerator();
        if(!enumerator.MoveNext())
            return default(T);
           
        T max = enumerator.Current;
        while(enumerator.MoveNext())
        {
            T temp = enumerator.Current;
            int compare = temp.CompareTo(max);
            if(compare == 1)
            {
                max = temp;
            }
        }

        return max;
    }

    public static T MaxElement<T>(IEnumerable<T> enumerable, IComparer<T> comparer)
    {
        IEnumerator<T> enumerator = enumerable.GetEnumerator();
        if(!enumerator.MoveNext())
            return default(T);
           
        T max = enumerator.Current;
        while(enumerator.MoveNext())
        {
            T temp = enumerator.Current;
            int compare = comparer.Compare(temp, max);
            if(compare == 1)
                max = temp;
        }

        return max;
    }
    
#if SORT_WITH_BINARYPREDICATE
    public static T MaxElement<T>(IEnumerable<T> enumerable, Functional.BinaryPredicate<T> func)
    {
        IEnumerator<T> enumerator = enumerable.GetEnumerator();
        if(!enumerator.MoveNext())
            return default(T);
           
        T max = enumerator.Current;
        while(enumerator.MoveNext())
        {
            T temp = enumerator.Current;
            // TODO: review this and compare with comments from Josuttis
            bool more = func(temp, max);
            if(more)
                max = temp;
        }

        return max; 
    }
#else
    public static T MaxElement<T>(IEnumerable<T> enumerable, Comparison<T> func)
    {
        return MaxElement(enumerable, Functional.Wrap(func));
    }
#endif    

    #endregion
}
}

