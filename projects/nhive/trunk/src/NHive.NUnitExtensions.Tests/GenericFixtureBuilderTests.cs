namespace NHive.NUnitExtensions.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using NUnit.Core;
    using NUnit.Core.Extensibility;
    using NUnit.Core.Filters;
    using NUnit.Framework;
    using NUnit.Framework.SyntaxHelpers;
    using NHive.NUnitExtensions.Framework;
    using GenericTestsAssembly;
    using System.Reflection;

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
            Assert.That(!_suiteBuilder.CanBuildFrom(
                typeof(NonGenericFixture)));
        }

        [Test]
        public void CannotBuildFromGenericTypeWithoutGenericFixtureAttribute()
        {
            Assert.That(!_suiteBuilder.CanBuildFrom(
                typeof(GenericFixtureWithoutAttribute<>)));
        }

        [Test]
        public void CanBuildFromGenericTypeDefinitionWithOneTypeArgAndOneOrMoreFixtureAttributes()
        {
            Assert.That(_suiteBuilder.CanBuildFrom(
                typeof(GenericFixtureWithOneAttribute<>)), "One attribute");
            Assert.That(_suiteBuilder.CanBuildFrom(
                typeof(GenericFixtureWithThreeAttributes<>)), "Multiple attributes");
        }

        [Test]
        public void CanBuildFromGenericTypeDefinitionWithOneOrMoreTypeArgumentsAndOneFixtureAttribute()
        {
            Assert.That(_suiteBuilder.CanBuildFrom(
                typeof(GenericFixtureWithOneAttribute<>)), "One type argument");
            Assert.That(_suiteBuilder.CanBuildFrom(
                typeof(GenericFixtureWithOneAttribute<,>)), "Two type arguments");
            Assert.That(_suiteBuilder.CanBuildFrom(
                typeof(GenericFixtureWithOneAttribute<,,>)), "Three type arguments");
        }

        [Test]
        public void CanBuildFromGenericTypeDefinitionWithMultipleTypeArgumentsAndMultipleFixtureAttributes()
        {
            Assert.That(_suiteBuilder.CanBuildFrom(
                typeof(GenericFixtureWithThreeAttributes<,>)));
        }

        #endregion

        #region BuildFrom tests

        [Test, ExpectedException(typeof(ArgumentException))]
        public void BuildFromNonGenericTypeThrowsArgumentException()
        {
            _suiteBuilder.BuildFrom(
                typeof(NonGenericFixture));
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void BuildFromGenericTypeWithoutGenericFixtureAttributeThrowsArgumentException()
        {
            _suiteBuilder.BuildFrom(
                typeof(GenericFixtureWithoutAttribute<>));
        }

        [Test]
        public void BuildFromGenericTypeWithOneGenericFixtureAttributeReturnsTestSuite()
        {
            ITest buildResult = _suiteBuilder.BuildFrom(
                typeof(GenericFixtureWithOneAttribute<>));
            Assert.That(buildResult, Is.Not.Null, "Build result is not null.");
            Assert.That(buildResult.IsSuite, "Build result is test suite.");
        }

        [Test]
        public void BuildFromGenericTypeReturnsTestSuiteWithSameNameAsGenericType()
        {
            ITest buildResult1 = _suiteBuilder.BuildFrom(
                typeof(GenericFixtureWithOneAttribute<>));
            ITest buildResult3 = _suiteBuilder.BuildFrom(
                typeof(GenericFixtureWithOneAttribute<,,>));
            Assert.That(buildResult1.TestName.Name, Is.EqualTo("GenericFixtureWithOneAttribute`1"),
                "Generic type with one type argument");
            Assert.That(buildResult3.TestName.Name, Is.EqualTo("GenericFixtureWithOneAttribute`3"),
                "Generic type with three type arguments");
        }

        [Test]
        public void BuildFromGenericTypeReturnsTestSuiteWithOneTestPerGenericFixtureAttribute()
        {
            ITest buildResult1 = _suiteBuilder.BuildFrom(typeof
                (GenericFixtureWithOneAttribute<>));
            ITest buildResult3 = _suiteBuilder.BuildFrom(
                typeof(GenericFixtureWithThreeAttributes<>));
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
            GenericTestCounters.ResetCounters();
            Test suite = _suiteBuilder.BuildFrom(
                typeof(GenericFixtureWithOneAttribute<>));
            TestResult testResult = suite.Run(NullListener.NULL);
            Assert.That(testResult.IsSuccess, "All tests are successful.");
            Assert.That(GenericTestCounters.Test.Value, Is.EqualTo(2), "Executes 2 Tests");
            Assert.That(GenericTestCounters.SetUp.Value, Is.EqualTo(2), "Executed SetUp 1 time");
            Assert.That(GenericTestCounters.TearDown.Value, Is.EqualTo(2), "Executed TearDown 1 time");
        }

        [Test]
        public void RunGenericFixtureWithThreeAttributes()
        {
            GenericTestCounters.ResetCounters();
            Test suite = _suiteBuilder.BuildFrom(typeof(GenericFixtureWithThreeAttributes<>));
            TestResult testResult = suite.Run(NullListener.NULL);
            Assert.That(testResult.IsSuccess, "All tests are successful.");
            Assert.That(GenericTestCounters.Test.Value, Is.EqualTo(6), "Executed 6 tests");
            Assert.That(GenericTestCounters.SetUp.Value, Is.EqualTo(6), "Execute SetUp 3 times");
            Assert.That(GenericTestCounters.TearDown.Value, Is.EqualTo(6), "Execute TearDown 3 times");
        }

        [Test]
        public void RunGenericFixtureWithFilterForOneTestMethod()
        {
            GenericTestCounters.ResetCounters();

            Type fixtureType = typeof(GenericFixtureWithThreeAttributes<>);
            MethodInfo testMethod = fixtureType.GetMethod("Success");
            
            Test suite = _suiteBuilder.BuildFrom(fixtureType);
            TestFilter filter = new SimpleNameFilter(
                testMethod.ReflectedType.FullName + "." + testMethod.Name);

            TestResult testResult = suite.Run(NullListener.NULL, filter);
            Assert.That(testResult.IsSuccess, "All tests are successful.");
            Assert.That(GenericTestCounters.Test.Value, Is.EqualTo(3), "Executed 3 tests");
            Assert.That(GenericTestCounters.SetUp.Value, Is.EqualTo(3), "Execute SetUp 3 times");
            Assert.That(GenericTestCounters.TearDown.Value, Is.EqualTo(3), "Execute TearDown 3 times");
        }

        #endregion
    }
}
