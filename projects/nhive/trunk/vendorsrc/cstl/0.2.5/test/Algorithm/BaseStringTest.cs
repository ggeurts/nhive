using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CSTL.Test
{

public class BaseStringTest
{
    protected const string MARKER = "XXXX";
    protected string [] src;
    protected string [] dest;
    protected int    count;
    
    
    [SetUp]
    public void Init()
    {
        src  = GetValues();
        count = src.Length;
        dest = new string[count+2];
        FillDest();
    }
    
    protected virtual string[] GetValues()
    {   
        return Constants.TEST_STRING_ARRAY;
    }
    
    protected virtual void FillDest()
    {
        for(int i=0; i<dest.Length; ++i)
            dest[i] = MARKER;
    }

    
}

}
