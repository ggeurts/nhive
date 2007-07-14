//-----------------------------------------------------------------------------
// CSTL
// Copyright (C) 2006 by Harold Howe hhjunk@mchsi.com
// http://sourceforge.net/projects/cstl

using System;
using System.Collections.Generic;

namespace CSTL.Iterator
{

public static partial class IteratorUtil
{
    public static void Swap<T>(ForwardIterator<T> lhs, ForwardIterator<T> rhs)
    {
        T temp = lhs.Read();
        lhs.Write(rhs.Read());
        rhs.Write(temp);
    }
}

}