namespace NHive.NUnitExtensions.Framework
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Specifies type arguments for a generic test fixture. A generic test fixture
    /// is a generic type definitions that is decorated with one or more 
    /// <see cref="GenericFixtureAttribute" /> attributes. For each 
    /// <see cref="GenericFixtureAttribute" /> one closed generic
    /// test fixture type will be generated.
    /// </summary>
    public class GenericFixtureAttribute : GenericFixturePatternAttribute
    {
        public readonly Type[] GenericTypeArgs;

        public GenericFixtureAttribute(params Type[] genericTypeArgs)
        {
            this.GenericTypeArgs = genericTypeArgs ?? Type.EmptyTypes;
        }

        public override void BuildFixtures(IGenericFixture fixture)
        {
            fixture.Add(fixture.FixtureType.MakeGenericType(this.GenericTypeArgs));
        }
    }
}
