using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{

[TestFixture] 
[Category("Functional")]
public class BinaryPredicateTest
{

    [Test]public void EqualTo()
    {
        int x = 29, y=-17;
        string s1 = "hello", s2="world";
        object o1,o2;

        Assert.IsFalse(Functional.EqualTo(x,y));
        Assert.IsTrue (Functional.NotEqualTo(x,y));
        o1 = x;
        o2 = y;

        Assert.IsFalse(Functional.EqualTo(o1,o2));
        Assert.IsTrue (Functional.NotEqualTo(o1,o2));
        o2 = 29;
        Assert.IsTrue(Functional.EqualTo(o1,o2));
        Assert.IsFalse(Functional.NotEqualTo(o1,o2));
        Assert.IsTrue(Functional.EqualTo(x,o2));
        Assert.IsFalse(Functional.NotEqualTo(x,o2));
        o2=null;
        Assert.IsFalse(Functional.EqualTo(o1,o2));
        Assert.IsFalse(Functional.EqualTo(o2,o1));
        Assert.IsTrue(Functional.NotEqualTo(o1,o2));
        Assert.IsTrue(Functional.NotEqualTo(o2,o1));

        Assert.IsFalse(Functional.EqualTo(s1, s2));
        Assert.IsTrue(Functional.NotEqualTo(s1, s2));
        s2 = "hello";
        Assert.IsTrue(Functional.EqualTo(s1, s2));
        Assert.IsFalse(Functional.NotEqualTo(s1, s2));
        s2 = null;
        Assert.IsFalse(Functional.EqualTo(s1, s2));
        Assert.IsFalse(Functional.EqualTo(s2, s1));
        Assert.IsTrue(Functional.NotEqualTo(s1, s2));
        Assert.IsTrue(Functional.NotEqualTo(s2, s1));

        s1 = null;
        Assert.IsTrue(Functional.EqualTo(s1, s2));
        Assert.IsTrue(Functional.EqualTo(s2, s1));
        Assert.IsFalse(Functional.NotEqualTo(s1, s2));
        Assert.IsFalse(Functional.NotEqualTo(s2, s1));
    }

    [Test]public void Comparisons()
    {
        int x = 29, y=-17;
        string s1 = "hello", s2="world";

        Assert.IsTrue (Functional.LessThan(y,x));
        Assert.IsFalse(Functional.LessThan(x,y));
        Assert.IsTrue (Functional.GreaterThan(x,y));
        Assert.IsFalse(Functional.GreaterThan(y,x));
        Assert.IsTrue (Functional.LessThanOrEqual(y,x));
        Assert.IsFalse(Functional.LessThanOrEqual(x,y));
        Assert.IsTrue (Functional.GreaterThanOrEqual(x,y));
        Assert.IsFalse(Functional.GreaterThanOrEqual(y,x));

        y = 29;
        Assert.IsFalse(Functional.LessThan(y,x));
        Assert.IsFalse(Functional.LessThan(x,y));
        Assert.IsFalse(Functional.GreaterThan(x,y));
        Assert.IsFalse(Functional.GreaterThan(y,x));
        Assert.IsTrue (Functional.LessThanOrEqual(y,x));
        Assert.IsTrue (Functional.LessThanOrEqual(x,y));
        Assert.IsTrue (Functional.GreaterThanOrEqual(x,y));
        Assert.IsTrue (Functional.GreaterThanOrEqual(y,x));

        Assert.IsTrue (Functional.LessThan(s1,s2));
        Assert.IsFalse(Functional.LessThan(s2,s1));
        Assert.IsTrue (Functional.GreaterThan(s2,s1));
        Assert.IsFalse(Functional.GreaterThan(s1,s2));
        Assert.IsTrue (Functional.LessThanOrEqual(s1,s2));
        Assert.IsFalse(Functional.LessThanOrEqual(s2,s1));
        Assert.IsTrue (Functional.GreaterThanOrEqual(s2,s1));
        Assert.IsFalse(Functional.GreaterThanOrEqual(s1,s2));
        
        s2 = "hello";
        Assert.IsFalse(Functional.LessThan(s1,s2));
        Assert.IsFalse(Functional.LessThan(s2,s1));
        Assert.IsFalse(Functional.GreaterThan(s2,s1));
        Assert.IsFalse(Functional.GreaterThan(s1,s2));
        Assert.IsTrue (Functional.LessThanOrEqual(s1,s2));
        Assert.IsTrue (Functional.LessThanOrEqual(s2,s1));
        Assert.IsTrue (Functional.GreaterThanOrEqual(s2,s1));
        Assert.IsTrue(Functional.GreaterThanOrEqual(s1,s2));

        s1 = null;
        Assert.IsTrue (Functional.LessThan(s1,s2));
        Assert.IsFalse(Functional.LessThan(s2,s1));
        Assert.IsTrue (Functional.GreaterThan(s2,s1));
        Assert.IsFalse(Functional.GreaterThan(s1,s2));
        Assert.IsTrue (Functional.LessThanOrEqual(s1,s2));
        Assert.IsFalse(Functional.LessThanOrEqual(s2,s1));
        Assert.IsTrue (Functional.GreaterThanOrEqual(s2,s1));
        Assert.IsFalse(Functional.GreaterThanOrEqual(s1,s2));


        s2 = null;
        Assert.IsFalse(Functional.LessThan(s1,s2));
        Assert.IsFalse(Functional.LessThan(s2,s1));
        Assert.IsFalse(Functional.GreaterThan(s2,s1));
        Assert.IsFalse(Functional.GreaterThan(s1,s2));
        Assert.IsTrue (Functional.LessThanOrEqual(s1,s2));
        Assert.IsTrue (Functional.LessThanOrEqual(s2,s1));
        Assert.IsTrue (Functional.GreaterThanOrEqual(s2,s1));
        Assert.IsTrue (Functional.GreaterThanOrEqual(s1,s2));
    }
}

}
