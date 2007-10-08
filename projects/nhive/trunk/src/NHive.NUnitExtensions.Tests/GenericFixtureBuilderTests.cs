namespace NHive.NUnitExtensions.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using NUnit.Core;
    using NUnit.Core.Extensibility;
    using NUnit.Framework;
    using NUnit.Framework.SyntaxHelpers;

    [TestFixture]
    public class GenericFixtureBuilderTests
    {
        private ISuiteBuilder _suiteBuilder;

        [SetUp]
        public void SetUp()
        {
            _suiteBuilder = new GenericFixtureBuilder();
        }

        #region CanBuildFrom tests

        [Test]
        public void CannotBuildFromNonGenericType()
        {
            Assert.That(!_suiteBuilder.CanBuildFrom(typeof(NonGenericFixture)));
        }

        [Test]
        public void CannotBuildFromGenericTypeWithoutGenericFixtureAttribute()
        {
            Assert.That(!_suiteBuilder.CanBuildFrom(typeof(GenericFixtureWithoutAttribute<>)));
        }

        [Test]
        public void CanBuildFromGenericTypeDefinitionWithOneTypeArgAndOneOrMoreFixtureAttributes()
        {
            Assert.That(_suiteBuilder.CanBuildFrom(typeof(GenericFixtureWithOneAttribute<>)), "One attribute");
            Assert.That(_suiteBuilder.CanBuildFrom(typeof(GenericFixtureWithThreeAttributes<>)), "Multiple attributes");
        }

        [Test]
        public void CanBuildFromGenericTypeDefinitionWithOneOrMoreTypeArgumentsAndOneFixtureAttribute()
        {
            Assert.That(_suiteBuilder.CanBuildFrom(typeof(GenericFixtureWithOneAttribute<>)), "One type argument");
            Assert.That(_suiteBuilder.CanBuildFrom(typeof(GenericFixtureWithOneAttribute<,>)), "Two type arguments");
            Assert.That(_suiteBuilder.CanBuildFrom(typeof(GenericFixtureWithOneAttribute<,,>)), "Three type arguments");
        }

        [Test]
        public void CanBuildFromGenericTypeDefinitionWithMultipleTypeArgumentsAndMultipleFixtureAttributes()
        {
            Assert.That(_suiteBuilder.CanBuildFrom(typeof(GenericFixtureWithThreeAttributes<,>)));
        }

        #endregion

        #region BuildFrom tests

        [Test, ExpectedException(typeof(ArgumentException))]
        public void BuildFromNonGenericTypeThrowsArgumentException()
        {
            _suiteBuilder.BuildFrom(typeof(NonGenericFixture));
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void BuildFromGenericTypeWithoutGenericFixtureAttributeThrowsArgumentException()
        {
            _suiteBuilder.BuildFrom(typeof(GenericFixtureWithoutAttribute<>));
        }

        [Test]
        public void BuildFromGenericTypeWithOneGenericFixtureAttributeReturnsTestSuite()
        {
            ITest buildResult = _suiteBuilder.BuildFrom(typeof(GenericFixtureWithOneAttribute<>));
            Assert.That(buildResult, Is.Not.Null, "Build result is not null.");
            Assert.That(buildResult.IsSuite, "Build result is test suite.");
        }

        [Test]
        public void BuildFromGenericTypeReturnsTestSuiteWithSameNameAsGenericType()
        {
            ITest buildResult1 = _suiteBuilder.BuildFrom(typeof(GenericFixtureWithOneAttribute<>));
            ITest buildResult3 = _suiteBuilder.BuildFrom(typeof(GenericFixtureWithOneAttribute<,,>));
            Assert.That(buildResult1.TestName.Name, Is.EqualTo("GenericFixtureWithOneAttribute`1"),
                "Generic type with one type argument");
            Assert.That(buildResult3.TestName.Name, Is.EqualTo("GenericFixtureWithOneAttribute`3"),
                "Generic type with three type arguments");
        }

        [Test]
        public void BuildFromGenericTypeReturnsTestSuiteWithOneTestPerGenericFixtureAttribute()
        {
            ITest buildResult1 = _suiteBuilder.BuildFrom(typeof(GenericFixtureWithOneAttribute<>));
            ITest buildResult3 = _suiteBuilder.BuildFrom(typeof(GenericFixtureWithThreeAttributes<>));
            Assert.That(buildResult1.Tests.Count, Is.EqualTo(1),
                "Build result for type with one attribute.");
            Assert.That(buildResult3.Tests.Count, Is.EqualTo(3),
                "Build result for type with three attributes.");
        }

        #endregion

        #region Generic Fixture execution tests

        [Test]
        public void RunGenericFixtureWithOneAttribute()
        {
            FixtureBase.ResetCounters();
            Test suite = _suiteBuilder.BuildFrom(typeof(GenericFixtureWithOneAttribute<>));
            TestResult testResult = suite.Run(NullListener.NULL);
            Assert.That(testResult.IsSuccess, "All tests are successful.");
            Assert.That(FixtureBase.Count, Is.EqualTo(1), "Execute 1 Test");
            Assert.That(FixtureBase.SetUpCount, Is.EqualTo(1), "Execute SetUp 1 time");
            Assert.That(FixtureBase.TearDownCount, Is.EqualTo(1), "Execute TearDown 1 time");
        }

        [Test]
        public void RunGenericFixtureWithThreeAttributes()
        {
            FixtureBase.ResetCounters();
            Test suite = _suiteBuilder.BuildFrom(typeof(GenericFixtureWithThreeAttributes<>));
            TestResult testResult = suite.Run(NullListener.NULL);
            Assert.That(testResult.IsSuccess, "All tests are successful.");
            Assert.That(FixtureBase.Count, Is.EqualTo(3), "Execute 3 tests");
            Assert.That(FixtureBase.SetUpCount, Is.EqualTo(3), "Execute SetUp 3 times");
            Assert.That(FixtureBase.TearDownCount, Is.EqualTo(3), "Execute TearDown 3 times");
        }

        #endregion

        #region Helper classes - Generic test fixtures

        public class FixtureBase
        {
            private static int _setupCount = 0;
            private static int _teardownCount = 0;
            private static int _count = 0;

            public static int Count
            {
                get { return Thread.VolatileRead(ref _count); }
            }

            public static int SetUpCount
            {
                get { return Thread.VolatileRead(ref _setupCount); }
            }

            public static int TearDownCount
            {
                get { return Thread.VolatileRead(ref _teardownCount); }
            }

            public static void ResetCounters()
            {
                Thread.VolatileWrite(ref _count, 0);
                Thread.VolatileWrite(ref _setupCount, 0);
                Thread.VolatileWrite(ref _teardownCount, 0);
            }

            [SetUp]
            public void Setup()
            {
                Interlocked.Increment(ref _setupCount);
            }

            [TearDown]
            public void TearDown()
            {
                Interlocked.Increment(ref _teardownCount);
            }

            [Test]
            public void Success()
            {
                Interlocked.Increment(ref _count);
            }
        }

        [TestFixture]
        [GenericFixture(typeof(int))]
        public class NonGenericFixture: FixtureBase
        { }

        public class GenericFixtureWithoutAttribute<T> : FixtureBase
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

        #endregion
    }
}
