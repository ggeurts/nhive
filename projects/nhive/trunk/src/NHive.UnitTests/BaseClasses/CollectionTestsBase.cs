namespace NHive.UnitTests
{
    using System;
    using System.Collections.Generic;
    using MbUnit.Framework;

    public class CollectionTestArgs<T, TSize, THive>
        : ReadOnlyCollectionTestArgs<T, TSize, THive>
        where THive : ICollection<T, TSize>
        where TSize : struct, IConvertible
    {
        public CollectionTestArgs(THive hive, IEnumerable<T> expectedItems)
            : base(hive, expectedItems)
        { }
    }

    // TODO: Add clear tests.
    public abstract class CollectionTestsBase<T, TSize, THive>
        : ReadOnlyCollectionTestsBase<T, TSize, THive>
        where THive : ICollection<T, TSize>
        where TSize : struct, IConvertible
    {
        #region Add, AddRange tests

        [Test]
        public void AddIncrementsCount(CollectionTestArgs<T, TSize, THive> args)
        {
            IgnoreTestIfHiveIsReadOnly(args.Hive);

            for (int i = 0; i < 3; i++)
            {
                TSize initialCount = args.Hive.Count;
                args.Hive.Add(CreateRandomItem());
                Assert.AreEqual(Size.Add(initialCount, 1), args.Hive.Count, "Add #{0}", i);
            }
        }

        [Test]
        public void AddIncrementsRevision(CollectionTestArgs<T, TSize, THive> args)
        {
            IgnoreTestIfHiveIsReadOnly(args.Hive);

            for (int i = 0; i < 3; i++)
            {
                long revision = args.Hive.Revision;
                args.Hive.Add(CreateRandomItem());
                Assert.GreaterThan(args.Hive.Revision, revision, "Add #{0}", i);
            }
        }

        [Test]
        public void AddRangeIncrementsCount(CollectionTestArgs<T, TSize, THive> args)
        {
            const int RANGE_SIZE = 3;

            IgnoreTestIfHiveIsReadOnly(args.Hive);

            TSize initialCount = args.Hive.Count;
            args.Hive.AddRange(CreateRange(RANGE_SIZE));
            Assert.AreEqual(Size.Add(initialCount, RANGE_SIZE), args.Hive.Count);
        }

        [Test]
        public void AddRangeIncrementsRevisionWhenRangeIsNotEmpty(CollectionTestArgs<T, TSize, THive> args)
        {
            const int RANGE_SIZE = 3;

            IgnoreTestIfHiveIsReadOnly(args.Hive);

            long initialRevision = args.Hive.Revision;
            args.Hive.AddRange(CreateRange(RANGE_SIZE));
            Assert.Greater(args.Hive.Revision, initialRevision);
        }

        [Test]
        public void AddRangeDoesNotIncrementRevisionWhenRangeIsEmpty(CollectionTestArgs<T, TSize, THive> args)
        {
            IgnoreTestIfHiveIsReadOnly(args.Hive);

            long initialRevision = args.Hive.Revision;
            args.Hive.AddRange(CreateRange(0));
            Assert.AreEqual(args.Hive.Revision, initialRevision);
        }

        #endregion

        #region Remove, RemoveRange tests

        [Test]
        public void RemoveDecrementsCount(CollectionTestArgs<T, TSize, THive> args)
        {
            IgnoreTestIfHiveIsReadOnly(args.Hive);

            TSize itemIndex = Size.Zero;
            foreach (T expectedItem in args.ExpectedItems)
            {
                TSize initialCount = args.Hive.Count;
                args.Hive.Remove(expectedItem);
                Assert.AreEqual(Size.Subtract(initialCount, 1), args.Hive.Count, "item[{0}]", itemIndex);
                Size.Decrement(ref itemIndex);
            }
        }

        [Test]
        public void RemoveIncrementsRevision(CollectionTestArgs<T, TSize, THive> args)
        {
            IgnoreTestIfHiveIsReadOnly(args.Hive);

            TSize itemIndex = Size.Zero;
            foreach (T expectedItem in args.ExpectedItems)
            {
                long revision = args.Hive.Revision;
                args.Hive.Remove(expectedItem);
                Assert.GreaterThan(args.Hive.Revision, revision, "Remove #{0}", itemIndex);
                Size.Decrement(ref itemIndex);
            }
        }

        #endregion

        #region Contains tests

        [Test]
        public void ContainsIsTrueForAddedItem(CollectionTestArgs<T, TSize, THive> args)
        {
            IgnoreTestIfHiveIsReadOnly(args.Hive);

            T item = CreateRandomItem();
            args.Hive.Add(item);
            Assert.IsTrue(args.Hive.Contains(item));
        }

        [Test]
        public void ContainsIsFalseForRemovedItem(CollectionTestArgs<T, TSize, THive> args)
        {
            IgnoreTestIfHiveIsReadOnly(args.Hive);
            if (args.ExpectedItems.Count == 0)
            {
                Assert.Ignore("Collection does not contain any items to remove");
            }

            // TODO: This test is assuming set semantics at the moment!
            T item = args.ExpectedItems[0];
            args.Hive.Remove(item);
            Assert.IsFalse(args.Hive.Contains(item));
        }

        #endregion

        protected abstract T CreateRandomItem();

        protected virtual IEnumerable<T> CreateRange(long rangeSize)
        {
            for (int i = 0; i < rangeSize; i++)
            {
                yield return CreateRandomItem();
            }
        }
    }
}
