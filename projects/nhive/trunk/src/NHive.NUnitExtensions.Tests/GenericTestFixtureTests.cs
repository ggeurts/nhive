namespace NHive.NUnitExtensions.Tests
{
    using System;
    using NUnit.Core;
    using NUnit.Core.Filters;
    using NUnit.Framework;
    using NUnit.Framework.SyntaxHelpers;
    using NHive.NUnitExtensions.Framework;
    using GenericTestsAssembly;

    [TestFixture]
    public class GenericTestFixtureTests
    {
        [Test]
        public void CreateFromType()
        {
            GenericTestFixture fixture = new GenericTestFixture(
                typeof(GenericFixtureWithOneAttribute<>));
            Assert.That(fixture, Is.Not.Null);
        }

        [Test]
        public void NameIsTestClassName()
        {
            GenericTestFixture fixture = new GenericTestFixture(
                typeof(GenericFixtureWithOneAttribute<>));
            Assert.That(fixture.TestName.Name, Is.EqualTo("GenericFixtureWithOneAttribute`1"));
        }

        [Test]
        public void IsTestSuite()
        {
            GenericTestFixture fixture = new GenericTestFixture(
                typeof(GenericFixtureWithOneAttribute<>));
            Assert.That(fixture.IsSuite);
        }

        [Test]
        public void CountZeroTestCasesWhenNoTestsPresent()
        {
            GenericTestFixture fixture = new GenericTestFixture(
                typeof(EmptyGenericFixture<>));
            Assert.That(fixture.CountTestCases(TestFilter.Empty), Is.EqualTo(0));
        }

        [Test]
        public void CountAllTestCasesWhenUsingEmptyFilter()
        {
            GenericTestFixture fixture = new GenericTestFixture(
                typeof(GenericFixtureWithThreeAttributes<>));
            Assert.That(fixture.CountTestCases(TestFilter.Empty), Is.EqualTo(6));
        }

        [Test]
        public void CountAllTestCasesWhenMatchingFilter()
        {
            Type fixtureType = typeof(GenericFixtureWithThreeAttributes<>);
            GenericTestFixture fixture = new GenericTestFixture(fixtureType);
            Assert.That(fixture.CountTestCases(new SimpleNameFilter(fixtureType.FullName)), Is.EqualTo(6));
        }

        [Test]
        public void CountMatchingTestCasesWhenUsingNonEmptyFilter()
        {
            GenericTestFixture fixture = new GenericTestFixture(
                typeof(GenericFixtureWithThreeAttributes<>));
            Test test2 = (Test) fixture.Tests[1];
            Assert.That(fixture.CountTestCases(new NameFilter(test2.TestName)), Is.EqualTo(2));
        }

    }
}
