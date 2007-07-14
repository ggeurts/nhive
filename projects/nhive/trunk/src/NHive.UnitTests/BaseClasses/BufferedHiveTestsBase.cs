namespace NHive.UnitTests
{
    using System;
    using System.Collections.Generic;
    using MbUnit.Framework;

    public class BufferedHiveTestArgs<T, TSize, THive>
        : HiveTestArgs<T, TSize, THive>
        where THive : IBufferedHive<T, TSize>
        where TSize : struct, IConvertible
    {
        public BufferedHiveTestArgs(THive hive, IEnumerable<T> expectedItems)
            : base(hive, expectedItems)
        { }
    }

    public abstract class BufferedHiveTestsBase<T, TSize, THive>
        : HiveTestsBase<T, TSize, THive>
        where THive : IBufferedHive<T, TSize>
        where TSize : struct, IConvertible
    {
        [Test]
        public void CountReturnsNumberOfItemsInHive(BufferedHiveTestArgs<T, TSize, THive> args)
        {
            Assert.AreEqual(args.ExpectedItems.Count, args.Hive.Count);
        }

        [Test]
        public void TryGetCountAlwaysReturnsTrue(BufferedHiveTestArgs<T, TSize, THive> args)
        {
            TSize count;
            Assert.IsTrue(args.Hive.TryGetCount(out count));
        }

        [Test]
        public void ContainsReturnsTrueIfItemInHive(BufferedHiveTestArgs<T, TSize, THive> args)
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
        public void ContainsReturnsFalseIfItemNotInHive(BufferedHiveTestArgs<T, TSize, THive> args)
        {
            if (!args.ExpectedItems.Contains(default(T)))
            {
                Assert.IsFalse(args.Hive.Contains(default(T)));
            }
        }
    }
}
