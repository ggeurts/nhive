namespace NHive.UnitTests
{
    using System;
    using System.Collections.Generic;
    using MbUnit.Framework;
    using NHive.Base;
    using NHive.Base.Size;
    using NHive.UnitTests.Factories;

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

    public abstract class ArrayListTestsBase<T, TSize, TSizeOperations, THive>
        : ListTestsBase<T, TSize, THive>
        where TSize: struct, IConvertible
        where TSizeOperations: ISizeOperations<TSize>, new()
        where THive: ArrayListBase<T, TSize, TSizeOperations>
    {
        [Test]
        public void InsertIncreasesCapacityIfNoSpareCapacityExists
            (ListTestArgs<T, TSize, THive> args)
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
            (ListTestArgs<T, TSize, THive> args)
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
        typeof(ArrayList32TestArgs<string>))]
    [ProviderFactory(
        typeof(ArrayList32Factory),
        typeof(ArrayList32TestArgs<string>))]
    public class ArayList32Tests : ArrayListTestsBase<string, int, Int32Operations, ArrayList<string>>
    {
        private static string[] _items = new string[] { "item 1", "item 2", "item 3" };
        private static Random _random = new Random(0);

        protected override string CreateRandomItem()
        {
            return _items[_random.Next(_items.Length - 1)];
        }
    }

    [TypeFixture(
        typeof(ArrayList32TestArgs<string>))]
    [ProviderFactory(
        typeof(ArrayList32Factory), 
        typeof(ArrayList32TestArgs<string>))]
    public class ArrayList32IteratorTests
        : RandomAccessIteratableTestsBase<string, int, ArrayList<string>, ArrayList<string>.Iterator>
    { }
}
