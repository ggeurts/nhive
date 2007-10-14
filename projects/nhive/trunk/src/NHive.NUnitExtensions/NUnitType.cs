using System;
using System.Reflection;

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
            private static Assembly _assembly;

            /// <summary>
            /// NUnit.Core assembly.
            /// </summary>
            private static Assembly NUnitAssembly
            {
                get
                {
                    if (_assembly == null)
                    {
                        _assembly = GetLoadedAssembly("NUnit.Core");
                    }
                    return _assembly;
                }
            }
            /// <summary>
            /// Late bound <see cref="NUnit.Core.TestFixtureBuilder"/> type.
            /// </summary>
            public static readonly Type TestFixtureBuilder = 
                NUnitAssembly.GetType("NUnit.Core.TestFixtureBuilder", true);
        }

        public static class Framework
        {
            /// <summary>
            /// Late bound <see cref="NUnit.Framework.TestFixtureAttribute"/> type.
            /// </summary>
            public static readonly Type TestFixtureAttribute = 
                Type.GetType("NUnit.Framework.TestFixtureAttribute, NUnit.Framework", true);
        }

        private static Assembly GetLoadedAssembly(string simpleName)
        {
            Assembly result = null;
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (StringComparer.InvariantCultureIgnoreCase.Equals(assembly.GetName().Name, simpleName))
                {
                    result = assembly;
                    break;
                }
            }

            if (result == null)
            {
                throw new ArgumentException(string.Format(
                    "AppDomain does not contains ssembly with name '{0}'.",
                    simpleName));
            }

            return result;
        }
    }
}
