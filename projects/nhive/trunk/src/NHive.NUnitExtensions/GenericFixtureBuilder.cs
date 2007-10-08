namespace NHive.NUnitExtensions
{
    using System;
    using System.Reflection;
    using NUnit.Core.Extensibility;
    using NUnit.Core;
    using NHive.NUnitExtensions.Framework;
    using NHive.NUnitExtensions.Proxies;

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
                && type.IsDefined(typeof(GenericFixtureAttribute), false)
                && type.IsDefined(NUnitType.Framework.TestFixtureAttribute, false);
        }

        public Test BuildFrom(Type type)
        {
            return new GenericFixture(type).BuildFixtures();
        }

        #endregion

        private class GenericFixture : IGenericFixture
        {
            private Type _fixtureType;
            private TestSuiteProxy _suiteProxy;

            public GenericFixture(Type fixtureType)
            {
                if (!fixtureType.IsGenericTypeDefinition)
                {
                    throw new ArgumentException("Type must be a generic type definition.", "type");
                }

                if (!fixtureType.IsDefined(typeof(GenericFixtureAttribute), false))
                {
                    throw new ArgumentException(
                        string.Format("Generic type must define one or more {0} attributes.",
                            typeof(GenericFixtureAttribute).Name),
                        "type");
                }

                if (!fixtureType.IsDefined(NUnitType.Framework.TestFixtureAttribute, false))
                {
                    throw new ArgumentException(
                        string.Format("Generic type must have a {0} attribute.",
                            NUnitType.Framework.TestFixtureAttribute.Name),
                        "type");
                }

                _fixtureType = fixtureType;
                _suiteProxy = new TestSuiteProxy(fixtureType.Name);
            }

            #region IGenericFixture Members

            public Type FixtureType
            {
                get { return _fixtureType; }
            }

            public void Add(Type constructedFixtureType)
            {
                Test fixture = (Test)TestFixtureBuilderProxy.BuildFromType(constructedFixtureType);
                _suiteProxy.Add(fixture);
            }

            #endregion

            public Test BuildFixtures()
            {
                object[] atts = _fixtureType.GetCustomAttributes(typeof(GenericFixturePatternAttribute), false);
                foreach (GenericFixturePatternAttribute att in atts)
                {
                    att.BuildFixtures(this);
                }
                return _suiteProxy.InnerTestSuite;
            }
        }
    }
}
