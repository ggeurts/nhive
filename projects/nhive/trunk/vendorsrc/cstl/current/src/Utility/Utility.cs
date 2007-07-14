//-----------------------------------------------------------------------------
// CSTL
// Copyright (C) 2006 by Harold Howe hhjunk@mchsi.com
// http://sourceforge.net/projects/cstl

using System;
using System.Collections.Generic;

namespace CSTL.Utility
{

public struct Pair<T,U>
{
    public Pair(T first, U second)
    {
        First  = first;
        Second = second;
    }
    
    public T First;
    public U Second;
    
    public Type FirstType  { get { return typeof(T);}}
    public Type SecondType { get { return typeof(U);}}
}

}

namespace CSTL
{
public static partial class Algorithm
{
    public static void Swap<T>(ref T lhs, ref T rhs)
    {
        T temp = lhs;
        lhs = rhs;
        rhs = temp;
    }
}
}

