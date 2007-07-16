namespace NHive.UnitTests
{
    using System;
    using System.Collections.Generic;
    using MbUnit.Framework;

    public abstract class ListTestArgs<T, TSize, THive>
        : BufferedHiveTestArgs<T, TSize, THive>
        where THive: IList<T, TSize>
        where TSize : struct, IConvertible
    {
        public ListTestArgs(THive hive, IEnumerable<T> expectedItems)
            : base(hive, expectedItems)
        { }
    }

    public abstract class ListTestsBase<T, TSize, THive>
        : BufferedHiveTestsBase<T, TSize, THive>
        where THive: IList<T, TSize>
        where TSize : struct, IConvertible
    {
        private Random _random = new Random();

        #region Contains, IndexOf tests

        [Test]
        public void IndexOfReturnsIndexOfItemIfItemInHive(ListTestArgs<T, TSize, THive> args)
        {
            TSize itemIndex = Size.Zero;
            foreach (T expectedItem in args.ExpectedItems)
            {
                Assert.AreEqual(itemIndex, args.Hive.IndexOf(expectedItem), "Item #{0}", itemIndex);
                Size.Increment(ref itemIndex);
            }
        }

        [Test]
        public void IndexOfReturnsMinusOneIfItemNotInHive(ListTestArgs<T, TSize, THive> args)
        {
            if (!args.ExpectedItems.Contains(default(T)))
            {
                Assert.AreEqual(-1, args.Hive.IndexOf(default(T)));
            }
        }

        #endregion

        #region Add, AddRange tests

        [Test]
        public void AddAtEnd(ListTestArgs<T, TSize, THive> args)
        {
            IgnoreTestIfHiveIsReadOnly(args.Hive);

            T lastItem = CreateRandomItem();
            args.Hive.Add(lastItem);

            TSize itemIndex = Size.Zero;
            foreach (T expectedItem in args.ExpectedItems)
            {
                Assert.AreEqual(expectedItem, args.Hive[(TSize)(object) itemIndex], "item[{0}]", itemIndex);
                Size.Increment(ref itemIndex);
            }
            Assert.AreEqual(lastItem, args.Hive[itemIndex], "item[{0}]", itemIndex);
        }

        [Test]
        public void AddIncrementsCount(ListTestArgs<T, TSize, THive> args)
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
        public void AddIncrementsRevision(ListTestArgs<T, TSize, THive> args)
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
        public void AddRangeAtEnd(ListTestArgs<T, TSize, THive> args)
        {
            IgnoreTestIfHiveIsReadOnly(args.Hive);

            List<T> range = new List<T>(CreateRange(3)); 
            args.Hive.AddRange(range);

            TSize itemIndex = Size.Zero;
            foreach (T expectedItem in args.ExpectedItems)
            {
                Assert.AreEqual(expectedItem, args.Hive[itemIndex], "item[{0}]", itemIndex);
                Size.Increment(ref itemIndex);
            }
            foreach (T rangeItem in range)
            {
                Assert.AreEqual(rangeItem, args.Hive[itemIndex], "item[{0}]", itemIndex);
                Size.Increment(ref itemIndex);
            }
        }

        [Test]
        public void AddRangeIncrementsCount(ListTestArgs<T, TSize, THive> args)
        {
            const int RANGE_SIZE = 3;

            IgnoreTestIfHiveIsReadOnly(args.Hive);

            TSize initialCount = args.Hive.Count;
            args.Hive.AddRange(CreateRange(RANGE_SIZE));
            Assert.AreEqual(Size.Add(initialCount, RANGE_SIZE), args.Hive.Count);
        }

        [Test]
        public void AddRangeIncrementsRevisionWhenRangeIsNotEmpty(ListTestArgs<T, TSize, THive> args)
        {
            const int RANGE_SIZE = 3;

            IgnoreTestIfHiveIsReadOnly(args.Hive);

            long initialRevision = args.Hive.Revision;
            args.Hive.AddRange(CreateRange(RANGE_SIZE));
            Assert.Greater(args.Hive.Revision, initialRevision);
        }

        [Test]
        public void AddRangeDoesNotIncrementRevisionWhenRangeIsEmpty(ListTestArgs<T, TSize, THive> args)
        {
            IgnoreTestIfHiveIsReadOnly(args.Hive);

            long initialRevision = args.Hive.Revision;
            args.Hive.AddRange(CreateRange(0));
            Assert.AreEqual(args.Hive.Revision, initialRevision);
        }

        #endregion

        #region Insert, InsertRange tests

        [Test]
        public void InsertAtBegin(ListTestArgs<T, TSize, THive> args)
        {
            IgnoreTestIfHiveIsReadOnly(args.Hive);

            T firstItem = CreateRandomItem();
            args.Hive.Insert((TSize)(object) 0, firstItem);

            TSize itemIndex = Size.Zero;
            Assert.AreEqual(firstItem, args.Hive[itemIndex], "item[{0}]", itemIndex);
            
            Size.Increment(ref itemIndex);
            foreach (T expectedItem in args.ExpectedItems)
            {
                Assert.AreEqual(expectedItem, args.Hive[itemIndex], "item[{0}]", itemIndex);
                Size.Increment(ref itemIndex);
            }
        }

        [Test]
        public void InsertAtEnd(ListTestArgs<T, TSize, THive> args)
        {
            IgnoreTestIfHiveIsReadOnly(args.Hive);

            T lastItem = CreateRandomItem();
            args.Hive.Insert(args.Hive.Count, lastItem);

            TSize itemIndex = Size.Zero;
            foreach (T expectedItem in args.ExpectedItems)
            {
                Assert.AreEqual(expectedItem, args.Hive[itemIndex], "item[{0}]", itemIndex);
                Size.Increment(ref itemIndex);
            }
            Assert.AreEqual(lastItem, args.Hive[itemIndex], "item[{0}]", itemIndex);
        }

        [Test]
        public void InsertInMiddle(ListTestArgs<T, TSize, THive> args)
        {
            IgnoreTestIfHiveIsReadOnly(args.Hive);

            long itemCount = Size.ToInt64(args.Hive.Count);
            if (itemCount < 2) return;

            TSize insertIndex = Size.Add(GetRandomIndex(Size.Subtract(args.Hive.Count, 2)), 1);
            T insertedItem = CreateRandomItem();
            args.Hive.Insert(insertIndex, insertedItem);

            TSize itemIndex = Size.Zero;
            foreach (T expectedItem in args.ExpectedItems)
            {
                Assert.AreEqual(expectedItem, args.Hive[itemIndex], "item[{0}]", itemIndex);
                Size.Increment(ref itemIndex);

                if (Size.Equals(itemIndex, insertIndex))
                {
                    Assert.AreEqual(insertedItem, args.Hive[itemIndex], "item[{0}]", itemIndex);
                    Size.Increment(ref itemIndex);
                }
            }
        }

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InsertThrowsArgumentOutOfRangeExceptionIfIndexIsLessThanZero(ListTestArgs<T, TSize, THive> args)
        {
            IgnoreTestIfHiveIsReadOnly(args.Hive);
            args.Hive.Insert(Size.Const(-1), CreateRandomItem());
        }

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InsertThrowsArgumentOutOfRangeExceptionIfIndexIsGreaterThanCount(ListTestArgs<T, TSize, THive> args)
        {
            IgnoreTestIfHiveIsReadOnly(args.Hive);
            args.Hive.Insert(Size.Add(args.Hive.Count, 1), CreateRandomItem());
        }

        [Test]
        public void InsertIncrementsCount(ListTestArgs<T, TSize, THive> args)
        {
            IgnoreTestIfHiveIsReadOnly(args.Hive);

            for (int i = 0; i < 3; i++)
            {
                TSize initialCount = args.Hive.Count;
                args.Hive.Insert(GetRandomIndex(args.Hive.Count), CreateRandomItem());
                Assert.AreEqual(Size.Add(initialCount, 1), args.Hive.Count, "Insert #{0}", i);
            }
        }

        [Test]
        public void InsertIncrementsRevision(ListTestArgs<T, TSize, THive> args)
        {
            IgnoreTestIfHiveIsReadOnly(args.Hive);

            for (int i = 0; i < 3; i++)
            {
                long revision = args.Hive.Revision;
                args.Hive.Insert(GetRandomIndex(args.Hive.Count), CreateRandomItem());
                Assert.GreaterThan(args.Hive.Revision, revision, "Insert #{0}", i);
            }
        }

        [Test]
        public void InsertRangeAtBegin(ListTestArgs<T, TSize, THive> args)
        {
            IgnoreTestIfHiveIsReadOnly(args.Hive);

            List<T> range = new List<T>(CreateRange(3));
            args.Hive.InsertRange(Size.Zero, range);

            TSize itemIndex = Size.Zero;
            foreach (T rangeItem in range)
            {
                Assert.AreEqual(rangeItem, args.Hive[itemIndex], "item[{0}]", itemIndex);
                Size.Increment(ref itemIndex);
            }
            foreach (T expectedItem in args.ExpectedItems)
            {
                Assert.AreEqual(expectedItem, args.Hive[itemIndex], "item[{0}]", itemIndex);
                Size.Increment(ref itemIndex);
            }
        }

        [Test]
        public void InsertRangeAtEnd(ListTestArgs<T, TSize, THive> args)
        {
            IgnoreTestIfHiveIsReadOnly(args.Hive);

            List<T> range = new List<T>(CreateRange(3));
            args.Hive.InsertRange(args.Hive.Count, range);

            TSize itemIndex = Size.Zero;
            foreach (T expectedItem in args.ExpectedItems)
            {
                Assert.AreEqual(expectedItem, args.Hive[itemIndex], "item[{0}]", itemIndex);
                Size.Increment(ref itemIndex);
            }
            foreach (T rangeItem in range)
            {
                Assert.AreEqual(rangeItem, args.Hive[itemIndex], "item[{0}]", itemIndex);
                Size.Increment(ref itemIndex);
            }
        }

        [Test]
        public void InsertRangeInMiddle(ListTestArgs<T, TSize, THive> args)
        {
            IgnoreTestIfHiveIsReadOnly(args.Hive);
            if (Size.Compare(args.Hive.Count, Size.Const(2)) < 0) return;

            List<T> range = new List<T>(CreateRange(3));
            TSize insertIndex = Size.Add(GetRandomIndex(Size.Subtract(args.Hive.Count, 2)), 1);
            args.Hive.InsertRange(insertIndex, range);

            TSize itemIndex = Size.Zero;
            foreach (T expectedItem in args.ExpectedItems)
            {
                Assert.AreEqual(expectedItem, args.Hive[itemIndex], "item[{0}]", itemIndex);
                Size.Increment(ref itemIndex);

                if (Size.Equals(itemIndex,  insertIndex))
                {
                    foreach (T rangeItem in range)
                    {
                        Assert.AreEqual(rangeItem, args.Hive[itemIndex], "item[{0}]", itemIndex);
                        Size.Increment(ref itemIndex);
                    }
                }
            }
        }

        [Test]
        public void InsertRangeIncrementsCount(ListTestArgs<T, TSize, THive> args)
        {
            const int RANGE_SIZE = 3;

            IgnoreTestIfHiveIsReadOnly(args.Hive);

            TSize initialCount = args.Hive.Count;
            args.Hive.InsertRange(Size.Zero, CreateRange(RANGE_SIZE));
            Assert.AreEqual(Size.Add(initialCount, RANGE_SIZE), args.Hive.Count);
        }

        [Test]
        public void InsertRangeIncrementsRevisionWhenRangeIsNotEmpty(ListTestArgs<T, TSize, THive> args)
        {
            const int RANGE_SIZE = 3;

            IgnoreTestIfHiveIsReadOnly(args.Hive);

            long initialRevision = args.Hive.Revision;
            args.Hive.InsertRange(Size.Zero, CreateRange(RANGE_SIZE));
            Assert.Greater(args.Hive.Revision, initialRevision);
        }

        [Test]
        public void InsertRangeDoesNotIncrementRevisionWhenRangeIsEmpty(ListTestArgs<T, TSize, THive> args)
        {
            IgnoreTestIfHiveIsReadOnly(args.Hive);

            long initialRevision = args.Hive.Revision;
            args.Hive.InsertRange(Size.Zero, CreateRange(0));
            Assert.AreEqual(args.Hive.Revision, initialRevision);
        }

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InsertRangeThrowsArgumentOutOfRangeExceptionIfIndexIsLessThanZero(ListTestArgs<T, TSize, THive> args)
        {
            IgnoreTestIfHiveIsReadOnly(args.Hive);
            args.Hive.InsertRange(Size.Const(-1), CreateRange(1));
        }

        [Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InsertRangeThrowsArgumentOutOfRangeExceptionIfIndexIsGreaterThanCount(ListTestArgs<T, TSize, THive> args)
        {
            IgnoreTestIfHiveIsReadOnly(args.Hive);
            args.Hive.InsertRange(Size.Add(args.Hive.Count, 1), CreateRange(1));
        }

        #endregion

        #region Remove, RemoveRange tests

        [Test]
        public void RemoveFromEnd(ListTestArgs<T, TSize, THive> args)
        {
            IgnoreTestIfHiveIsReadOnly(args.Hive);

            for (int itemIndex = args.ExpectedItems.Count - 1; itemIndex >= 0; itemIndex--)
            {
                T expectedItem = args.ExpectedItems[itemIndex];
                args.Hive.Remove(expectedItem);
                Assert.AreEqual(Size.Const(-1), args.Hive.IndexOf(expectedItem), "item[{0}]", itemIndex);
            }
        }

        [Test]
        public void RemoveFromBegin(ListTestArgs<T, TSize, THive> args)
        {
            IgnoreTestIfHiveIsReadOnly(args.Hive);

            TSize itemIndex = Size.Zero;
            foreach (T expectedItem in args.ExpectedItems)
            {
                args.Hive.Remove(expectedItem);
                Assert.AreEqual(Size.Const(-1), args.Hive.IndexOf(expectedItem), "item[{0}]", itemIndex);
                Size.Increment(ref itemIndex);
            }
        }

        [Test]
        public void RemoveDecrementsCount(ListTestArgs<T, TSize, THive> args)
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
        public void RemoveIncrementsRevision(ListTestArgs<T, TSize, THive> args)
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

        protected TSize GetRandomIndex(TSize listItemCount)
        {
            return Size.FromInt32(
                _random.Next((int)
                    Math.Min(int.MaxValue, Size.ToInt64(listItemCount))));
        }

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
