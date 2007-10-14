namespace NHive.NUnitExtensions
{
    using System;
    using NUnit.Core;
    using NUnit.Core.Extensibility;
    using NHive.NUnitExtensions.Framework;

    /// <summary>
    /// NUnit addin that builds test fixtures from generic test fixture definitions.
    /// A generic test fixture definition is a generic type definition that is decoared
    /// with one or more <see cref="GenericFixtureAttribute"/>s.
    /// </summary>
    public class GenericFixtureBuilder : ISuiteBuilder
    {
        #region ISuiteBuilder Members

        public bool CanBuildFrom(Type type)
        {
            return type.IsGenericTypeDefinition
                && type.IsDefined(typeof(GenericFixturePatternAttribute), false)
                && type.IsDefined(NUnitType.Framework.TestFixtureAttribute, false);
        }

        public Test BuildFrom(Type type)
        {
            return new GenericTestFixture(type);
        }

        #endregion
    }
}
