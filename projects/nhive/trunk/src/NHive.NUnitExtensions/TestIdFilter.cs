namespace NHive.NUnitExtensions
{
    using System;
    using NUnit.Core;
    using System.Collections;
    using System.Collections.Generic;

    public class TestIdFilter : TestFilter
    {
        private readonly List<TestID> _testIDs = new List<TestID>();

        public TestIdFilter()
            : base()
        { }

        public TestIdFilter(IEnumerable tests)
        {
            Add(tests);
        }

        public void Add(IEnumerable tests)
        {
            foreach (ITest test in tests)
            {
                _testIDs.Add(test.TestName.TestID);
            }
        }

        public override bool Match(ITest test)
        {
            return _testIDs.Contains(test.TestName.TestID);
        }
    }
}
