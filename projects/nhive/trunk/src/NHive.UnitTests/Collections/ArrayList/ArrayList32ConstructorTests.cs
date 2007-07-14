namespace NHive.UnitTests
{
    [TestFixture]
    public class ArrayList32ConstructorTests
    {
        private Random _random = new Random();

        [Test]
        public void CreateListWithDefaultCapacity()
        {
            ArrayList<int> list = new ArrayList<int>();
            Assert.AreEqual(8, list.Capacity);
        }

        [Test]
        public void CreateListWithNonDefaultCapacity()
        {
            ArrayList<int> list = new ArrayList<int>(20);
            Assert.GreaterEqualThan(list.Capacity, 20);
        }

        [Test]
        public void CreateListWithDefaultCapacityIfCapacityIsLessThanDefault()
        {
            ArrayList<int> list = new ArrayList<int>(4);
            Assert.AreEqual(8, list.Capacity);
        }

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CreateListWithZeroCapacityThrowsArgumentOutOfRangeException()
        {
            ArrayList<int> list = new ArrayList<int>(0);
        }

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CreateListWithNegativeCapacityThrowsArgumentOutOfRangeException()
        {
            ArrayList<int> list = new ArrayList<int>(-1);
        }
    }
}
