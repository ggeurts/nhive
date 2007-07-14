namespace NHive.UnitTests.SizeOperations
{
    using MbUnit.Framework;
    using NHive.Base.Size;

    [TestFixture]
    public class Int32OperationsTests
    {
        [Test]
        public void ConvertToInt64()
        {
            long value = new Int32Operations().ToInt64(1);
        }

        [Test]
        public void ConvertFromInt64()
        {
            int value = new Int32Operations().From((long) 1);
        }
    }
}
