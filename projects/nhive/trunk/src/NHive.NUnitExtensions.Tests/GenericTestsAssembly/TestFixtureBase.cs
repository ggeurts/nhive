namespace GenericTestsAssembly
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using NUnit.Framework;
    using NHive.NUnitExtensions.Framework;

    public class FixtureBase
    {
        [SetUp]
        public void Setup()
        {
            GenericTestCounters.SetUp.Increment();
        }

        [TearDown]
        public void TearDown()
        {
            GenericTestCounters.TearDown.Increment();
        }

        [Test]
        public void Success()
        {
            GenericTestCounters.Test.Increment();
        }

        [Test]
        public void AnotherSuccess()
        {
            GenericTestCounters.Test.Increment();
        }
    }

    [TestFixture]
    [GenericFixture(typeof(int))]
    public class NonGenericFixture : FixtureBase
    { }

    public class GenericFixtureWithoutAttribute<T> : FixtureBase
    { }

    [TestFixture]
    [GenericFixture(typeof(int))]
    [GenericFixture(typeof(string))]
    public class EmptyGenericFixture<T>
    { }

    [TestFixture]
    [GenericFixture(typeof(int))]
    public class GenericFixtureWithOneAttribute<X> : FixtureBase
    { }

    [TestFixture]
    [GenericFixture(typeof(int), typeof(IEnumerable<int>))]
    public class GenericFixtureWithOneAttribute<X, Y> : FixtureBase
    { }

    [TestFixture]
    [GenericFixture(typeof(int), typeof(IEnumerable<int>), typeof(IList<int>))]
    public class GenericFixtureWithOneAttribute<X, Y, Z> : FixtureBase
    { }

    [TestFixture]
    [GenericFixture(typeof(int))]
    [GenericFixture(typeof(long))]
    [GenericFixture(typeof(string))]
    public class GenericFixtureWithThreeAttributes<X> : FixtureBase
    { }

    [TestFixture]
    [GenericFixture(typeof(int), typeof(IEnumerable<int>))]
    [GenericFixture(typeof(long), typeof(IEnumerable<long>))]
    [GenericFixture(typeof(string), typeof(IEnumerable<string>))]
    public class GenericFixtureWithThreeAttributes<X, Y> : FixtureBase
    { }
}
