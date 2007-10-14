namespace NHive.NUnitExtensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using NUnit.Core;

    /// <summary>
    /// NUnit <see cref="ITestFilter"/> implementation that selects tests
    /// by their <see cref="NUnit.Core.TestID"/>.
    /// </summary>
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
