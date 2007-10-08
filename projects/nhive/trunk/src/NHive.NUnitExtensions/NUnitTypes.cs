using System;
using System.Collections.Generic;
using System.Text;

namespace NHive.NUnitExtensions
{
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
