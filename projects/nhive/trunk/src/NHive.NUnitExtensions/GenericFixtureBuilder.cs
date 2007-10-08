namespace NHive.NUnitExtensions
{
    using System;
    using System.Reflection;
    using NUnit.Core.Extensibility;
    using NUnit.Core;
    using NHive.NUnitExtensions.Proxies;

    public class GenericFixtureBuilder: ISuiteBuilder
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
            ThrowIfInvalidGenericFixtureType(type);
            TestSuiteProxy suiteProxy = new TestSuiteProxy(type.Name);

            object[] atts = type.GetCustomAttributes(typeof(GenericFixtureAttribute), false);
            foreach (GenericFixtureAttribute att in atts)
            {
                Type constructedType = type.MakeGenericType(att.GenericTypeArguments);
                if (TestFixtureBuilderProxy.CanBuildFrom(constructedType))
                {
                    Test fixture = (Test) TestFixtureBuilderProxy.BuildFromType(constructedType);
                    suiteProxy.Add(fixture);
                }
            }

            return suiteProxy.InnerTestSuite;
        }

        private static void ThrowIfInvalidGenericFixtureType(Type type)
        {
            if (!type.IsGenericTypeDefinition)
            {
                throw new ArgumentException("Type must be a generic type definition.", "type");
            }

            if (!type.IsDefined(typeof(GenericFixtureAttribute), false))
            {
                throw new ArgumentException(
                    string.Format("Generic type must define one or more {0} attributes.",
                        typeof(GenericFixtureAttribute).Name),
                    "type");
            }

            if (!type.IsDefined(NUnitType.Framework.TestFixtureAttribute, false))
            {
                throw new ArgumentException(
                    string.Format("Generic type must have a {0} attribute.",
                        NUnitType.Framework.TestFixtureAttribute.Name),
                    "type");
            }
        }

        private object CreateFixture(Type fixtureType)
        {
            ConstructorInfo ctor = fixtureType.GetConstructor(Type.EmptyTypes);
            return ctor.Invoke(null);
        }

        #endregion
    }
}
