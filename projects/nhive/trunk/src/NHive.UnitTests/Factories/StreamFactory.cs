namespace NHive.UnitTests.Factories
{
    using System.Collections.Generic;
    using MbUnit.Framework;

    public class CharStreamTestArgs 
        : HiveTestArgs<char, int, CharStream>
        , IInputIteratableTestArgs<char, int, CharStream, CharStream.Iterator>
    {
        public CharStreamTestArgs(CharStream hive, IEnumerable<char> expectedItems)
            : base(hive, expectedItems)
        { }
    }

    internal class StreamFactory
    {
        [Factory]
        public CharStreamTestArgs EmptyCharStream
        {
            get { return CreateTestInput(string.Empty); }
        }

        [Factory]
        public CharStreamTestArgs NonEmptyCharStream
        {
            get { return CreateTestInput("Just a bunch of characters..."); }
        }

        private CharStreamTestArgs CreateTestInput(string text)
        {
            return new CharStreamTestArgs(new CharStream(text), text.ToCharArray());
        }
    }
}
