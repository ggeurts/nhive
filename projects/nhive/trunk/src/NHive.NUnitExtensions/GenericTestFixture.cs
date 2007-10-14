namespace NHive.NUnitExtensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using NUnit.Core;
    using NHive.NUnitExtensions.Framework;
    using NHive.NUnitExtensions.Proxies;

    // Must inherit from TestSuite rather than Test to workaround bugs in NUnit GUI.
    public class GenericTestFixture : TestSuite, IGenericFixture
    {
        #region Fields

        private Dictionary<string, GenericTestCase> _testMethodMap =
            new Dictionary<string, GenericTestCase>();
        private List<Test> _constructedFixtures = new List<Test>();

        #endregion

        #region Constructor(s)

        public GenericTestFixture(Type fixtureType)
            : base(fixtureType)
        {
            if (!fixtureType.IsGenericTypeDefinition)
            {
                throw new ArgumentException("Type must be a generic type definition.", "type");
            }

            if (!fixtureType.IsDefined(typeof(GenericFixturePatternAttribute), false))
            {
                throw new ArgumentException(
                    string.Format("Generic type must define one or more {0} attributes.",
                        typeof(GenericFixtureAttribute).Name),
                    "type");
            }

            ConstructChildFixtures();
        }

        private void ConstructChildFixtures()
        {
            object[] atts = this.FixtureType.GetCustomAttributes(
                typeof(GenericFixturePatternAttribute), false);
            foreach (GenericFixturePatternAttribute att in atts)
            {
                att.BuildFixtures(this);
            }
        }

        #endregion

        #region IGenericFixture Members

        void IGenericFixture.Add(Type constructedFixtureType)
        {
            Test fixture = (Test) TestFixtureBuilderProxy.BuildFromType(constructedFixtureType);
            if (fixture != null)
            {
                fixture.TestName.Name = FormatTypeName(constructedFixtureType);
                _constructedFixtures.Add(fixture);
                foreach (Test test in fixture.Tests)
                {
                    AddTestCase(test);
                }
            }
        }

        private void AddTestCase(Test test)
        {
            GenericTestCase testCase;

            if (!_testMethodMap.TryGetValue(test.TestName.Name, out testCase))
            {
                string testMethodName = GetTestMethodName(test.TestName);
                MethodInfo testMethod = this.FixtureType.GetMethod(
                    testMethodName, BindingFlags.Instance | BindingFlags.Public);

                testCase = (testMethod == null)
                    ? new GenericTestCase(test.TestName.Name)
                    : new GenericTestCase(testMethod);
                _testMethodMap.Add(testCase.TestName.Name, testCase);
            }

            testCase.Add(test);
        }

        private string GetTestMethodName(TestName testName)
        {
            string name = testName.Name;
            int lastNameSeparatorIndex = name.LastIndexOf('.');
            return lastNameSeparatorIndex < 0
                ? name
                : name.Substring(lastNameSeparatorIndex + 1);
        }

        private static string FormatTypeName(Type type)
        {
            if (!type.IsGenericType)
            {
                return type.Name;
            }

            StringBuilder sb = new StringBuilder()
                .Append(type.Name.Substring(0, type.Name.IndexOf('`')));

            Type[] genericArgs = type.GetGenericArguments();
            if (genericArgs != null && genericArgs.Length > 0)
            {
                sb.Append('<').Append(FormatTypeName(genericArgs[0]));
                for (int i = 1; i < genericArgs.Length; i++)
                {
                    sb.Append(',').Append(FormatTypeName(genericArgs[i]));
                }
                sb.Append('>');
            }

            return sb.ToString();
        }

        #endregion

        #region Test implementation

        public override bool IsSuite
        {
            get { return true; }
        }

        public override string TestType
        {
            get { return "Generic Test Suite"; }
        }

        public override System.Collections.IList Tests
        {
            get { return _constructedFixtures; }
        }

        public override int CountTestCases(ITestFilter filter)
        {
            if (filter.Match(this))
            {
                filter = TestFilter.Empty;
            }

            int count = 0;
            foreach (Test childTest in _testMethodMap.Values)
            {
                count += childTest.CountTestCases(filter);
            }
            return count;
        }

        public override TestResult Run(EventListener listener, ITestFilter filter)
        {
            return base.Run(listener, TranslateFilter(filter));
        }

        private ITestFilter TranslateFilter(ITestFilter filter)
        {
            if (filter.IsEmpty)
            {
                return filter;
            }
            if (filter.Match(this))
            {
                return TestFilter.Empty;
            }

            TestIdFilter childFilter = new TestIdFilter();
            foreach (ITest childTest in _testMethodMap.Values)
            {
                if (filter.Match(childTest))
                {
                    childFilter.Add(childTest.Tests);
                }
            }
            return childFilter;
        }

        protected override void CreateUserFixture()
        {
            // Cannot create instances of this.FixtureType, because it is
            // a generic type definition. Therefore we must override the
            // default TestSuite behaviour with an implementation that 
            // does nothing.
        }

        #endregion


    }
}
