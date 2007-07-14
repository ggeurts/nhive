//-----------------------------------------------------------------------------
// CSTL
// Copyright (C) 2006 by Harold Howe hhjunk@mchsi.com
// http://sourceforge.net/projects/cstl

using System;
using System.Collections.Generic;
using CSTL.Iterator;

namespace CSTL
{
public static partial class Functional
{
    // Create an IComparer<T> wrapper for a Comparison<T> functor
    public static ComparisonWrapper<T> Wrap<T>(Comparison<T> func)
    {
        return new ComparisonWrapper<T>(func);
    }
    
    /// <summary>
    /// Wraps a Comparison{T} delegate into an IComparer{T} object. Note that the .NET 2.0 has an internal class called
    /// FunctorObject that does the same thing. The purpose of this class is to allow various algorithms to interact
    /// with code that has followed the Comparison{T} paradigm. Comparison{T} is a .NET delegate definition. CSTL 
    /// algorithms use predicates instead of Comparison{T}. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ComparisonWrapper<T> : IComparer<T>
    {
        public ComparisonWrapper(System.Comparison<T> func)
        {
            m_Func = func;
        }

        public int  Compare(T x, T y)
        {
            return m_Func(x, y);
        }

        private System.Comparison<T> m_Func;
    } 
}
}