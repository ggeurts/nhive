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

    #region Binder helpers that return a UnaryFunction
    public static UnaryFunction<Arg2, Result> Bind1st<Arg1, Arg2, Result>(BinaryFunction<Arg1, Arg2, Result>func, Arg1 arg1)
    {
        return new Binder1st<Arg1, Arg2, Result>(func, arg1).Execute;
    }

    public static UnaryFunction<Arg, Result> Bind1st<Arg, Result>(BinaryFunction<Arg, Arg, Result>func, Arg arg1)
    {
        return new Binder1st<Arg, Arg, Result>(func, arg1).Execute;
    }

    public static UnaryFunction<T, T> Bind1st<T>(BinaryFunction<T, T, T>func, T arg1)
    {
        return new Binder1st<T, T, T>(func, arg1).Execute;
    }

    public static UnaryFunction<Arg1, Result> Bind2nd<Arg1, Arg2, Result>(BinaryFunction<Arg1, Arg2, Result>func, Arg2 arg2)
    {
        return new Binder2nd<Arg1, Arg2, Result>(func, arg2).Execute;
    }

    public static UnaryFunction<Arg, Result> Bind2nd<Arg, Result>(BinaryFunction<Arg, Arg, Result>func, Arg arg2)
    {
        return new Binder2nd<Arg, Arg, Result>(func, arg2).Execute;
    }

    public static UnaryFunction<T, T> Bind2nd<T>(BinaryFunction<T, T, T>func, T arg2)
    {
        return new Binder2nd<T, T, T>(func, arg2).Execute;
    }
    #endregion 

    #region Binder helpers that return predicates
    public static UnaryPredicate<T> Bind1stPred<T>(BinaryPredicate<T>func, T arg1)
    {
        return new Binder1stPred<T>(func, arg1).Execute;
    }

    public static UnaryPredicate<T> Bind2ndPred<T>(BinaryPredicate<T>func, T arg2)
    {
        return new Binder2ndPred<T>(func, arg2).Execute;
    }
    #endregion
    
    #region Negation helpers
    public static UnaryPredicate <Arg> Not1Pred<Arg>(UnaryPredicate<Arg> func)
    {
        return delegate(Arg arg)
        {
            return !func(arg);
        };
    }

    public static BinaryPredicate<Arg> Not2Pred<Arg>(BinaryPredicate<Arg> func)
    {
        return delegate(Arg arg1, Arg arg2)
        {
            return !func(arg1, arg2);
        };
    }

    public static UnaryFunction<Arg, bool> Not1<Arg>(UnaryFunction<Arg, bool> func)
    {
        return delegate(Arg arg)
        {
            return !func(arg);
        };
    }

    public static BinaryFunction<Arg1, Arg2, bool> Not2<Arg1, Arg2>(BinaryFunction<Arg1, Arg2, bool> func)
    {
        return delegate(Arg1 arg1, Arg2 arg2)
        {
            return !func(arg1, arg2);
        };
    }
    #endregion

    public class Binder1st<Arg1, Arg2, Result>
    {
        private Arg1 m_Arg1;
        private BinaryFunction<Arg1, Arg2, Result> m_Function;
        
        public Binder1st(BinaryFunction<Arg1, Arg2, Result> func, Arg1 arg1)
        {
            m_Arg1 = arg1;
            m_Function = func;
        }
        
        public Result Execute(Arg2 arg2)
        {
            return m_Function(m_Arg1, arg2);
        }
    }

    public class Binder1stPred<T>
    {
        private T m_Arg1;
        private BinaryPredicate<T> m_Function;
        
        public Binder1stPred(BinaryPredicate<T> func, T arg1)
        {
            m_Arg1 = arg1;
            m_Function = func;
        }
        
        public bool Execute(T arg2)
        {
            return m_Function(m_Arg1, arg2);
        }
    }

    public class Binder2nd<Arg1, Arg2, Result>
    {
        private Arg2 m_Arg2;
        private BinaryFunction<Arg1, Arg2, Result> m_Function;
        
        public Binder2nd(BinaryFunction<Arg1, Arg2, Result> func, Arg2 arg2)
        {
            m_Arg2 = arg2;
            m_Function = func;
        }
        
        public Result Execute(Arg1 arg1)
        {
            return m_Function(arg1, m_Arg2);
        }
    }

    public class Binder2ndPred<T>
    {
        private T m_Arg2;
        private BinaryPredicate<T> m_Function;
        
        public Binder2ndPred(BinaryPredicate<T> func, T arg2)
        {
            m_Arg2 = arg2;
            m_Function = func;
        }
        
        public bool Execute(T arg1)
        {
            return m_Function(arg1, m_Arg2);
        }
    }
}

}

