namespace NHive.NUnitExtensions
{
    using System;
    using System.Collections.Generic;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public abstract class GenericFixturePatternAttribute : Attribute
    {
        public abstract void BuildFixtures(IGenericFixture genericFixture);
    }
}
