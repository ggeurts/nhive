using System;
using System.Collections.Generic;
using NUnit.Framework;

using CSTL;
using CSTL.Iterator;

namespace CSTL.Test
{

[TestFixture] 
[Category("Functional")]
public class BindTest
{
    object lastRhs;
    object lastLhs;

    public int AddEm(int lhs, int rhs)
    {
        lastRhs = rhs;
        lastLhs = lhs;
        return lhs+rhs;
    }
    
    public double Exp(int lhs, int power)
    {
        lastRhs = power;
        lastLhs = lhs;
        return Math.Pow(lhs, power);
    }
    
    public double Mux(int lhs, string rhs)
    {
        lastRhs = rhs;
        lastLhs = lhs;
        
        return Math.Pow(lhs, Convert.ToInt32(rhs));
    }

    [Test]public void Bind_SameTypes()
    {
        Functional.UnaryFunction<int, int> func1 = Functional.Bind1st(AddEm, 10);
        int result = func1(12);
        Assert.AreEqual(22, result);
        Assert.AreEqual(10, lastLhs);
        Assert.AreEqual(12, lastRhs);
        
        Functional.UnaryFunction<int, int> func2 = Functional.Bind2nd(AddEm, 10);
        result = func2(12);
        Assert.AreEqual(22, result);
        Assert.AreEqual(12, lastLhs);
        Assert.AreEqual(10, lastRhs);
    }

    [Test]public void Bind_TwoTypes()
    {
        Functional.UnaryFunction<int, double> func1 = Functional.Bind1st<int, double>(Exp, 3);
        double result = func1(2);
        Assert.AreEqual(9, result);
        Assert.AreEqual(3, lastLhs);
        Assert.AreEqual(2, lastRhs);
        
        Functional.UnaryFunction<int, double> func2 = Functional.Bind2nd<int, double>(Exp, 3);
        result = func2(2);
        Assert.AreEqual(8, result);
        Assert.AreEqual(2, lastLhs);
        Assert.AreEqual(3, lastRhs);
    }

    [Test]public void Bind_ThreeTypes()
    {
        Functional.UnaryFunction<string, double> func1 = Functional.Bind1st<int, string, double>(Mux, 3);
        double result = func1("2");
        Assert.AreEqual(9, result);
        Assert.AreEqual(3, lastLhs);
        Assert.AreEqual("2", lastRhs);
        
        Functional.UnaryFunction<int, double> func2 = Functional.Bind2nd<int, string, double>(Mux, "3");
        result = func2(2);
        Assert.AreEqual(8, result);
        Assert.AreEqual(2, lastLhs);
        Assert.AreEqual("3", lastRhs);
    }

    [Test]public void Bind_Predicate()
    {
        Functional.UnaryPredicate<int> func1 = Functional.Bind1stPred(Functional.LessThan<int>, 29);
        Assert.IsFalse(func1(12)); // 29 < 12 == false;
        Assert.IsTrue (func1(30)); // 29 < 30 == true;
        Assert.IsFalse(func1(29)); // 29 < 29 == false;

        Functional.UnaryPredicate<int> func2 = Functional.Bind2ndPred(Functional.LessThan<int>, 29);
        Assert.IsTrue (func2(12)); // 12 < 29 == true;
        Assert.IsFalse(func2(30)); // 30 < 12 == false;
        Assert.IsFalse(func2(29)); // 29 < 29 == false;
    }
    
    [Test]public void Not_Function()
    {
        Functional.UnaryFunction<int, bool> func1 = Functional.Not1(Functional.Bind1st<int, bool>(Functional.LessThan<int>, 29));
        Assert.IsTrue(func1(12));  // !(29 < 12) == false;
        Assert.IsFalse(func1(30)); // !(29 < 30) == true;
        Assert.IsTrue(func1(29));  // !(29 < 29) == false;
        
        Functional.BinaryFunction<int, int, bool> func2 = Functional.Not2<int, int>(Functional.LessThan<int>);
        Assert.IsFalse(func2(12, 29)); // !(29 < 12) == false;
        Assert.IsTrue(func2(30, 29));  // !(29 < 30) == true;
        Assert.IsTrue(func2(29, 29));  // !(29 < 29) == false;
    }

    [Test]public void Not_Predicate()
    {
        Functional.UnaryPredicate<int> func1 = Functional.Not1Pred(Functional.Bind1stPred(Functional.LessThan<int>, 29));
        Assert.IsTrue(func1(12));  // !(29 < 12) == false;
        Assert.IsFalse(func1(30)); // !(29 < 30) == true;
        Assert.IsTrue(func1(29));  // !(29 < 29) == false;
        
        Functional.BinaryPredicate<int> func2 = Functional.Not2Pred<int>(Functional.LessThan<int>);
        Assert.IsFalse(func2(12, 29)); // !(29 < 12) == false;
        Assert.IsTrue(func2(30, 29));  // !(29 < 30) == true;
        Assert.IsTrue(func2(29, 29));  // !(29 < 29) == false;
    }

}

}
