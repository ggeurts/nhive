namespace NHive.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using NHive.Base;
    using NHive.Base.Size;

    public class CharStream : StreamedHiveBase<char, int, Int32Operations>
    {
        private const int BEGIN_KEY = 0;
        private const int END_KEY = -1;

        private TextReader _reader;
        private int _key = BEGIN_KEY;

        public CharStream(string text)
            : base(EqualityComparer<char>.Default)
        {
            _reader = new StringReader(text);
        }

        protected override bool OnTryRead(out char value)
        {
            if (_key != END_KEY)
            {
                int nextChar = _reader.Read();
                if (nextChar >= 0)
                {
                    _key++;
                    value = (char) nextChar;
                    return true;
                }

                _key = END_KEY;
            }

            value = default(char);
            return false;
        }
    }
}
