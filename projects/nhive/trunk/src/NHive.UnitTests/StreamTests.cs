namespace NHive.UnitTests
{
    using MbUnit.Framework;
    using NHive.UnitTests.Factories;

    [TypeFixture(
       typeof(CharStreamTestArgs))]
    [ProviderFactory(
        typeof(StreamFactory),
        typeof(CharStreamTestArgs))]
    public class CharStream_IInputIteratableTests
        : InputIteratableTestsBase<char, int, CharStream, CharStream.Iterator>
    { }
}
