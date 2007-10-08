using System;
using System.Collections.Generic;
using System.Text;

namespace NHive.NUnitExtensions
{
    /// <summary>
    /// Static class with late bound <see cref="Type"/> instances for
    /// NUnit 2.4.x types to support the development of NUnit addins that
    /// are not bound to a specific version of the NUnit assemblies.
    /// </summary>
    internal static class NUnitType
    {
        public static class Core
        {
            public static readonly Type TestFixtureBuilder = 
                Type.GetType("NUnit.Core.TestFixtureBuilder, NUnit.Core");
            public static readonly Type TestSuite = 
                Type.GetType("NUnit.Core.TestSuite, NUnit.Core");
        }

        public static class Framework
        {
            public static readonly Type TestFixtureAttribute = 
                Type.GetType("NUnit.Framework.TestFixtureAttribute, NUnit.Framework");
        }
    }
}
