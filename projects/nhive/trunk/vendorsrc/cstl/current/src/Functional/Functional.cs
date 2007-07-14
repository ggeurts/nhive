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
    // TODO: .NET has some built in delegates
    //    Action<T> = UnaryVoidFunction
    //    Comparison<T> = Binary predicate that returns int, like Compare
    //    Converter<T,U> = can't really see a use for it
    //    Predicate<T>   = UnaryPredicate<T>
    // Consider using the .NET types when possible.

    #region Functional delegates
    public delegate T GenFunction<T>();
    public delegate void UnaryVoidFunction<T>(T t);
    public delegate void BinaryVoidFunction<T>(T lhs, T rhs);
    public delegate Result UnaryFunction<Arg, Result>(Arg arg);
    public delegate Result BinaryFunction<Arg1, Arg2, Result>(Arg1 lhs, Arg2 rhs);

    public delegate bool UnaryPredicate<T>(T t);
    public delegate bool BinaryPredicate<T>(T lhs, T rhs);
    #endregion
    
    
    #region Predefined unary functions
    static public int     Negate(int     value) { return -value; }
    static public Int16   Negate(Int16   value) { return (Int16)(-value); }
    static public Int64   Negate(Int64   value) { return -value; }
    static public double  Negate(double  value) { return -value; }
    static public float   Negate(float   value) { return -value; }
    static public decimal Negate(decimal value) { return -value; }
    #endregion

    #region Predefined binary functions
    static public int     Plus(int     lhs, int     rhs) { return lhs+rhs; }
    static public Int16   Plus(Int16   lhs, Int16   rhs) { return (Int16)(lhs+rhs); }
    static public Int64   Plus(Int64   lhs, Int64   rhs) { return lhs+rhs; }
    static public double  Plus(double  lhs, double  rhs) { return lhs+rhs; }
    static public float   Plus(float   lhs, float   rhs) { return lhs+rhs; }
    static public decimal Plus(decimal lhs, decimal rhs) { return lhs+rhs; }
    static public string  Plus(string  lhs, string  rhs) { return lhs+rhs; }

    static public int     Minus(int     lhs, int     rhs) { return lhs-rhs; }
    static public Int16   Minus(Int16   lhs, Int16   rhs) { return (Int16)(lhs-rhs); }
    static public Int64   Minus(Int64   lhs, Int64   rhs) { return lhs-rhs; }
    static public double  Minus(double  lhs, double  rhs) { return lhs-rhs; }
    static public float   Minus(float   lhs, float   rhs) { return lhs-rhs; }
    static public decimal Minus(decimal lhs, decimal rhs) { return lhs-rhs; }

    static public int     Multiply(int     lhs, int     rhs) { return lhs*rhs; }
    static public Int16   Multiply(Int16   lhs, Int16   rhs) { return (Int16)(lhs*rhs); }
    static public Int64   Multiply(Int64   lhs, Int64   rhs) { return lhs*rhs; }
    static public double  Multiply(double  lhs, double  rhs) { return lhs*rhs; }
    static public float   Multiply(float   lhs, float   rhs) { return lhs*rhs; }
    static public decimal Multiply(decimal lhs, decimal rhs) { return lhs*rhs; }

    static public int     Divide(int     lhs, int     rhs) { return lhs/rhs; }
    static public Int16   Divide(Int16   lhs, Int16   rhs) { return (Int16)(lhs/rhs); }
    static public Int64   Divide(Int64   lhs, Int64   rhs) { return lhs/rhs; }
    static public double  Divide(double  lhs, double  rhs) { return lhs/rhs; }
    static public float   Divide(float   lhs, float   rhs) { return lhs/rhs; }
    static public decimal Divide(decimal lhs, decimal rhs) { return lhs/rhs; }

    static public int     Mod(int     lhs, int     rhs) { return lhs%rhs; }
    static public Int16   Mod(Int16   lhs, Int16   rhs) { return (Int16)(lhs%rhs); }
    static public Int64   Mod(Int64   lhs, Int64   rhs) { return lhs%rhs; }
    static public double  Mod(double  lhs, double  rhs) { return lhs%rhs; }
    static public float   Mod(float   lhs, float   rhs) { return lhs%rhs; }
    static public decimal Mod(decimal lhs, decimal rhs) { return lhs%rhs; }

    static public int     And(int     lhs, int     rhs) { return lhs & rhs; }
    static public Int16   And(Int16   lhs, Int16   rhs) { return (Int16)(lhs & rhs); }
    static public Int64   And(Int64   lhs, Int64   rhs) { return lhs & rhs; }

    static public int     Or(int     lhs, int     rhs) { return lhs | rhs; }
    static public Int16   Or(Int16   lhs, Int16   rhs) { return (Int16)(lhs | rhs); }
    static public Int64   Or(Int64   lhs, Int64   rhs) { return lhs | rhs; }

    static public int     Xor(int     lhs, int     rhs) { return lhs ^ rhs; }
    static public Int16   Xor(Int16   lhs, Int16   rhs) { return (Int16)(lhs ^ rhs); }
    static public Int64   Xor(Int64   lhs, Int64   rhs) { return lhs ^ rhs; }
    #endregion


    #region Predefined binary predicates
    static public bool EqualTo<T>(T lhs, T rhs )
        where T : IEquatable<T>
    {
        if(object.ReferenceEquals(lhs, null))
            return object.ReferenceEquals(rhs, null);
        
        return lhs.Equals(rhs);
    }
    
    static public bool NotEqualTo<T>(T lhs, T rhs)
        where T : IEquatable<T>
    {
        return !EqualTo(lhs, rhs);
    }

    static public bool EqualTo(object lhs, object rhs )
    {
        if(object.ReferenceEquals(lhs, null))
            return object.ReferenceEquals(rhs, null);
        
        return lhs.Equals(rhs);
    }
    
    static public bool NotEqualTo(object lhs, object rhs)
    {
        return !EqualTo(lhs, rhs);
    }

    static public bool Equivalent<T>(T lhs, T rhs )
        where T : IComparable<T>
    {
        if(object.ReferenceEquals(lhs, null))
            return object.ReferenceEquals(rhs, null);
        
        return lhs.CompareTo(rhs)==0;
    }
    
    static public bool LessThan<T>(T lhs, T rhs)
        where T : IComparable<T>
    {
        int compare = Comparer<T>.Default.Compare(lhs, rhs);
        return compare == -1;
    }

    static public bool GreaterThan<T>(T lhs, T rhs)
        where T : IComparable<T>
    {
        int compare = Comparer<T>.Default.Compare(lhs, rhs);
        return compare == 1;
    }

    static public bool LessThanOrEqual<T>(T lhs, T rhs)
        where T : IComparable<T>
    {
        int compare = Comparer<T>.Default.Compare(lhs, rhs);
        return (compare  == -1) || (compare ==0);
    }

    static public bool GreaterThanOrEqual<T>(T lhs, T rhs)
        where T : IComparable<T>
    {
        int compare = Comparer<T>.Default.Compare(lhs, rhs);
        return (compare  == 1) || (compare ==0);
    }

#if NEVER
    static public bool logical_and<T>   (T lhs, T rhs );  // returns lhs && rhs;
    static public bool logical_or<T>    (T lhs, T rhs );  // returns lhs || rhs;    
#endif
    #endregion


    #region Predefined binary comparison functions
    static public int Compare<T>(T lhs, T rhs)
    {
        return Comparer<T>.Default.Compare(lhs, rhs);
    }

    static public int ReverseCompare<T>(T lhs, T rhs)
    {
        return -Comparer<T>.Default.Compare(lhs, rhs);
    }

    // These Compare overloads help prevent boxing when comparing on built in value types
    static public int Compare(int lhs, int rhs)
    {
        if(lhs < rhs)
            return -1;
        if(lhs > rhs)
            return 1;
        return 0;
    }

    static public int Compare(Int16 lhs, Int16 rhs)
    {
        if(lhs < rhs)
            return -1;
        if(lhs > rhs)
            return 1;
        return 0;
    }

    static public int Compare(Int64 lhs, Int64 rhs)
    {
        if(lhs < rhs)
            return -1;
        if(lhs > rhs)
            return 1;
        return 0;
    }

    static public int Compare(double lhs, double rhs)
    {
        if(lhs < rhs)
            return -1;
        if(lhs > rhs)
            return 1;
        return 0;
    }

    static public int Compare(float lhs, float rhs)
    {
        if(lhs < rhs)
            return -1;
        if(lhs > rhs)
            return 1;
        return 0;
    }

    static public int Compare(decimal lhs, decimal rhs)
    {
        if(lhs < rhs)
            return -1;
        if(lhs > rhs)
            return 1;
        return 0;
    }

    #endregion


    /// <summary>
    ///  This class is used internally to foster code reuse between algorithms that compare objects via
    /// IEquatable, and sister algorithms that compare by IComparer. This class allows the equatable routines
    /// to be implemented in terms of the comparer routines, so long as the algorithm is only looking for
    /// equality, and not ranking. When EqualComparer returns 0, two objects are the same. When it returns
    /// non-zero, they don't. 
    /// </summary>
    /// <remarks>
    /// Non-zero results from this comparer do not generate a meaningful sort order. This class is marked
    /// as internal because it doesn't fulfull the expected contract of ICompararer<T>"/>
    /// </remarks>
    /// 
    /// <typeparam name="T"></typeparam>
#if NEVER
    internal class EqualComparer<T> : IComparer<T>
        where T : IEquatable<T>
    {

        #region IComparer<T> Members
        public int Compare(T x, T y)
        {
            if(object.ReferenceEquals(x,null))
                return -1;
                
            if(x.Equals(y))
                return 0;
                
            return 1;
        }
        #endregion
    }
#endif

    internal class BinaryPredicateComparison<T>
    {
        public BinaryPredicateComparison(BinaryPredicate<T> func)
        {
            m_Func = func;
        }
        
        public int Compare(T lhs, T rhs)
        {
            if(m_Func(lhs,rhs))
                return -1;
            else if(m_Func(rhs, lhs))
                return 1;
            else
                return 0;
        }
        
        BinaryPredicate<T> m_Func;
    }
}

}

