using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL.Iterator;
using CSTL;

namespace CSTL.Test
{

public class BaseIntTest
{
    protected int [] src;
    protected int [] dest;
    protected int    count;
    
    
    [SetUp]
    public void Init()
    {
        src     = GetValues();
        count   = src.Length;
        dest    = new int[count+2];
        FillDest();
    }
    
    protected virtual int[] GetValues()
    {   
        return Constants.TEST_INT_ARRAY;
    }
    
    protected virtual void FillDest()
    {
        for(int i=0; i<dest.Length; ++i)
            dest[i] = int.MinValue;
    }
}

}
