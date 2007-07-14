using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL.Iterator;
using CSTL;

namespace CSTL.Test
{

[TestFixture] 
public class Dummy 
{
    [Test] public void Test() 
    {
        List<int> list = new List<int>(new int[]{1,2,3,4,5,6,7,8,9,10});
    
        list.Insert(0, 42);
    }
}

}
