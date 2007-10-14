namespace NHive.NUnitExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using NUnit.Core;

    /// <summary>
    /// A test suite that contains all test cases that were constructed 
    /// from the same generic test method. This test suite implementation supports
    /// the counting and introspection of its test cases, but does not 
    /// support the actual running of the test cases, because the running
    /// of test cases and the associated setup/reardown logic is the responsibility 
    /// of the test suite(s) that originally created the test cases.
    /// </summary>
    public class GenericTestMethod : Test
    {
        #region Fields

        /// <summary>
        /// List of test methods in closed generic types that derive
        /// from one test method in a generic test fixture definition.
        /// </summary>
        private List<Test> _constructedTestMethods = new List<Test>();

        #endregion

        #region Constructor(s)

        public GenericTestMethod(MethodInfo method)
            : base(method)
        { }

        public GenericTestMethod(string name)
            : base(name)
        { }

        #endregion

        #region Public operations

        public void Add(Test constructedTestMethod)
        {
            if (constructedTestMethod == null)
            {
                throw new ArgumentNullException("child");
            }
            _constructedTestMethods.Add(constructedTestMethod);
        }

        #endregion

        #region Test method overrides

        public override bool IsSuite
        {
            get { return true; }
        }

        public override int CountTestCases(ITestFilter filter)
        {
            if (filter.Match(this))
            {
                return _constructedTestMethods.Count;
            }
            
            int count = 0;
            foreach (Test constructedTestMethod in _constructedTestMethods)
            {
                count += constructedTestMethod.CountTestCases(filter);
            }
            return count; 
        }

        public override TestResult Run(EventListener listener, ITestFilter filter)
        {
            throw new NotSupportedException();
        }

        public override TestResult Run(EventListener listener)
        {
            throw new NotSupportedException();
        }

        public override string TestType
        {
            get { return "Generic Test Case"; }
        }

        public override System.Collections.IList Tests
        {
            get { return _constructedTestMethods; }
        }

        #endregion
    }
}
