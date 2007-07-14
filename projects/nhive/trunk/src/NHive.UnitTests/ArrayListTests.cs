namespace NHive.UnitTests
{
    using System;
    using System.Collections.Generic;
    using MbUnit.Framework;
    using NHive.Base;
    using NHive.Base.Size;
    using NHive.UnitTests.Factories;

    [TestFixture]
    public class ArrayListConstructorTests
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

    [TypeFixture(
        typeof(ArrayListTestArgs<string>))]
    [ProviderFactory(
        typeof(ArrayListFactory),
        typeof(ArrayListTestArgs<string>))]
    public abstract class ArrayListTestsBase<T, TSize, TSizeOperations>
        : ListTestsBase<T, TSize, ArrayListBase<T, TSize, TSizeOperations>>
        where TSize: struct, IConvertible
        where TSizeOperations: ISizeOperations<TSize>, new()
    {
        [Test]
        public void InsertIncreasesCapacityIfNoSpareCapacityExists
            (ListTestArgs<T, TSize, ArrayListBase<T, TSize, TSizeOperations>> args)
        {
            for (int capacityIncreaseCount = 0; capacityIncreaseCount < 3; capacityIncreaseCount++)
            {
                TSize initialCapacity = args.Hive.Capacity;
                for (TSize itemIndex = Size.Zero
                    ; Size.Compare(itemIndex, initialCapacity) <= 0
                    ; Size.Increment(ref itemIndex)
                    )
                {
                    args.Hive.Insert(GetRandomIndex(args.Hive.Count), CreateRandomItem());
                }
                Assert.GreaterEqualThan(
                    Size.ToInt64(args.Hive.Capacity), Size.ToInt64(initialCapacity),
                    "Capacity increase #{0}", capacityIncreaseCount);
            }
        }

        [Test]
        public void InsertRangeIncreasesCapacityIfNoSpareCapacityExists
            (ListTestArgs<T, TSize, ArrayListBase<T, TSize, TSizeOperations>> args)
        {
            for (int capacityIncreaseCount = 0; capacityIncreaseCount < 3; capacityIncreaseCount++)
            {
                TSize initialCapacity = args.Hive.Capacity;
                args.Hive.InsertRange(Size.Zero, CreateRange(Size.ToInt64(initialCapacity) + 1));
                Assert.GreaterEqualThan(
                    Size.ToInt64(args.Hive.Capacity), 
                    Size.ToInt64(initialCapacity),
                    "Capacity increase #{0}", capacityIncreaseCount);
            }
        }
    }

    [TypeFixture(
        typeof(ArrayListTestArgs<string>))]
    [ProviderFactory(
        typeof(ArrayListFactory), 
        typeof(ArrayListTestArgs<string>))]
    public class ArrayListIteratorTests
        : RandomAccessIteratableTestsBase<string, int, ArrayList<string>, ArrayList<string>.Iterator>
    { }
}
