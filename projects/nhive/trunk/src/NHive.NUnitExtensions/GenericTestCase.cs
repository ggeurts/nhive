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
    public class GenericTestCase : Test
    {
        #region Fields

        private List<Test> _childTests = new List<Test>();

        #endregion

        #region Constructor(s)

        public GenericTestCase(MethodInfo method)
            : base(method)
        { }

        public GenericTestCase(string name)
            : base(name)
        { }

        #endregion

        #region Public operations

        public void Add(Test child)
        {
            if (child == null)
            {
                throw new ArgumentNullException("child");
            }
            _childTests.Add(child);
        }

        #endregion

        #region Test method overrides

        public override bool IsSuite
        {
            get { return true; }
        }

        public override int CountTestCases(ITestFilter filter)
        {
            if (filter.Match(this)) return _childTests.Count;
            
            int count = 0;
            foreach (Test childTest in _childTests)
            {
                count += childTest.CountTestCases(filter);
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
            get { return _childTests; }
        }

        #endregion
    }
}
