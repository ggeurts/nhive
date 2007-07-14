namespace NHive.UnitTests
{
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
}
