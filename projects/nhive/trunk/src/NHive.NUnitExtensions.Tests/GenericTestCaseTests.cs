namespace NHive.NUnitExtensions.Tests
{
    using System;
    using System.Reflection;
    using NUnit.Core;
    using NUnit.Core.Filters;
    using NUnit.Framework;
    using NUnit.Framework.SyntaxHelpers;
    using System.Threading;

    [TestFixture]
    public class GenericTestCaseTests
    {
        private GenericTestCase _testCase;

        [SetUp]
        public void SetUp()
        {
            MethodInfo testMethod = typeof(FakeTestFixture<>).GetMethod("FakeTestMethod");
            _testCase = new GenericTestCase(testMethod);
        }

        [Test]
        public void CreateFromTestMethod()
        {
            Assert.That(_testCase, Is.Not.Null);
        }

        [Test]
        public void NameIsTestMethodName()
        {
            Assert.That(_testCase.TestName.Name, Is.EqualTo("FakeTestMethod"));
        }

        [Test]
        public void IsTestSuite()
        {
            Assert.That(_testCase.IsSuite);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void AddThowsArgumentNullExceptionIfTestIsNull()
        {
            _testCase.Add(null);
        }

        [Test]
        public void CountZeroTestCasesWhenNoTestsPresent()
        {
            Assert.That(_testCase.CountTestCases(TestFilter.Empty), Is.EqualTo(0));
        }

        [Test]
        public void CountAllTestCasesWhenUsingEmptyFilter()
        {
            _testCase.Add(CreateTestCase<int>());
            _testCase.Add(CreateTestCase<long>());
            _testCase.Add(CreateTestCase<string>());

            Assert.That(_testCase.CountTestCases(TestFilter.Empty), Is.EqualTo(3));
        }

        [Test]
        public void CountMatchingTestCasesWhenUsingNonEmptyFilter()
        {
            Test test2 = CreateTestCase<long>();
            _testCase.Add(CreateTestCase<int>());
            _testCase.Add(test2);
            _testCase.Add(CreateTestCase<string>());

            Assert.That(_testCase.CountTestCases(new NameFilter(test2.TestName)), Is.EqualTo(1));
        }

        [Test, ExpectedException(typeof(NotSupportedException))]
        public void RunAllThrowsNotSupportedException()
        {
            _testCase.Run(NullListener.NULL);
        }

        [Test, ExpectedException(typeof(NotSupportedException))]
        public void RunWithFilterThrowsNotSupportedException()
        {
            _testCase.Run(NullListener.NULL, TestFilter.Empty);
        }

        private static Test CreateTestCase<T>()
        {
            MethodInfo testMethod = typeof(FakeTestFixture<T>).GetMethod("FakeTestMethod");
            return TestCaseBuilder.BuildFrom(testMethod);
        }

        private class FakeTestFixture<T>
        {
            [Test]
            public void FakeTestMethod(T arg)
            { }
        }
    }
}
