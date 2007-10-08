namespace NHive.NUnitExtensions
{
    using System;
    using NUnit.Core;

    public interface IGenericFixture
    {
        Type FixtureType { get; }
        void Add(Type constructedFixtureType);
    }
}
