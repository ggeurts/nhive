namespace NHive.NUnitExtensions.Proxies
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Proxy for <see cref="NUnit.Core.TestFixtureBuilder"/> that provides access
    /// to <see cref="NUnit.Core.TestFixtureBuilder"/> operatios without requiring
    /// an explicit dependency (a specific version) of the NUnit.Core assembly.
    /// </summary>
    internal static class TestFixtureBuilderProxy
    {
        /// <summary>
        /// Indicates whether NUnit test fixture can be built from a specific 
        /// ficture type. See <see cref="NUnit.Core.TestFixtureBuilder.CanBuildFrom()"/>
        /// for more info.
        /// </summary>
        public static readonly Func<bool, Type> CanBuildFrom = 
            CreateDelegate<bool, Type>("CanBuildFrom");

        /// <summary>
        /// Build NUnit test fixture from a specific fixture type. 
        /// See <see cref="NUnit.Core.TestFixtureBuilder.BuildFrom(System.Type)"/>
        /// for more info.
        /// </summary>
        public static readonly Func<object, Type> BuildFromType =
            CreateDelegate<object, Type>("BuildFrom");

        /// <summary>
        /// Builds NUnit test fixture from a specific test fixture instance. 
        /// See <see cref="NUnit.Core.TestFixtureBuilder.BuildFrom(System.Object)"/>
        /// for more info.
        /// </summary>
        public static readonly Func<object, object> BuildFromFixture =
            CreateDelegate<object, object>("BuildFrom");

        private static Func<R, X> CreateDelegate<R, X>(string methodName)
        {
            MethodInfo methodInfo = NUnitType.Core.TestFixtureBuilder.GetMethod(
                methodName, new Type[] { typeof(X) });
            return (Func<R, X>) Delegate.CreateDelegate(
                typeof(Func<R, X>), methodInfo);
        }
    }
}
