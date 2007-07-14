namespace NHive.UnitTests
{
    using System;
    using MbUnit.Framework;

    [TestFixture]
    public class ArrayList32ConstructorTests
    {
        private Random _random = new Random();

        [Test]
        public void CreateListWithDefaultCapacity()
        {
            ArrayList32<int> list = new ArrayList32<int>();
            Assert.AreEqual(8, list.Capacity);
        }

        [Test]
        public void CreateListWithNonDefaultCapacity()
        {
            ArrayList32<int> list = new ArrayList32<int>(20);
            Assert.GreaterEqualThan(list.Capacity, 20);
        }

        [Test]
        public void CreateListWithDefaultCapacityIfCapacityIsLessThanDefault()
        {
            ArrayList32<int> list = new ArrayList32<int>(4);
            Assert.AreEqual(8, list.Capacity);
        }

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CreateListWithZeroCapacityThrowsArgumentOutOfRangeException()
        {
            ArrayList32<int> list = new ArrayList32<int>(0);
        }

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CreateListWithNegativeCapacityThrowsArgumentOutOfRangeException()
        {
            ArrayList32<int> list = new ArrayList32<int>(-1);
        }
    }
}
