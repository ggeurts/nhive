namespace NHive.UnitTests
{
    using System;
    using System.Collections.Generic;
    using MbUnit.Framework;

    public class ReadOnlyCollectionTestArgs<T, TSize, THive>
        : HiveTestArgs<T, TSize, THive>
        where THive : IReadOnlyCollection<T, TSize>
        where TSize : struct, IConvertible
    {
        public ReadOnlyCollectionTestArgs(THive hive, IEnumerable<T> expectedItems)
            : base(hive, expectedItems)
        { }
    }

    public abstract class ReadOnlyCollectionTestsBase<T, TSize, THive>
        : HiveTestsBase<T, TSize, THive>
        where THive : IReadOnlyCollection<T, TSize>
        where TSize : struct, IConvertible
    {
        [Test]
        public void CountReturnsNumberOfItemsInHive(ReadOnlyCollectionTestArgs<T, TSize, THive> args)
        {
            Assert.AreEqual(args.ExpectedItems.Count, args.Hive.Count);
        }

        [Test]
        public void TryGetCountAlwaysReturnsTrue(ReadOnlyCollectionTestArgs<T, TSize, THive> args)
        {
            TSize count;
            Assert.IsTrue(args.Hive.TryGetCount(out count));
        }

        [Test]
        public void ContainsReturnsTrueIfItemInHive(ReadOnlyCollectionTestArgs<T, TSize, THive> args)
        {
            int itemIndex = 0;
            foreach (T expectedItem in args.ExpectedItems)
            {
                Assert.IsTrue(args.Hive.Contains(expectedItem),
                    "Contains '{0}' (item #{1})", expectedItem, itemIndex);
                itemIndex++;
            }
        }

        [Test]
        public void ContainsReturnsFalseIfItemNotInHive(ReadOnlyCollectionTestArgs<T, TSize, THive> args)
        {
            if (!args.ExpectedItems.Contains(default(T)))
            {
                Assert.IsFalse(args.Hive.Contains(default(T)));
            }
        }
    }
}
