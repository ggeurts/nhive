namespace NHive.UnitTests
{
    using System;
    using System.Collections.Generic;
    using MbUnit.Framework;

    public class CollectionTestArgs<T, TSize, THive>
        : HiveTestArgs<T, TSize, THive>
        where THive : ICollection<T, TSize>
        where TSize : struct, IConvertible
    {
        public CollectionTestArgs(THive hive, IEnumerable<T> expectedItems)
            : base(hive, expectedItems)
        { }
    }

    //TODO: Add tests to see whether added items are contained in collection.
    //TODO: Add tests to see whether removed items are no longer contained in collection.
    //TODO: Add clear tests.
    public abstract class CollectionTestsBase<T, TSize, THive>
        : BufferedHiveTestsBase<T, TSize, THive>
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
