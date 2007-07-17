namespace NHive.UnitTests
{
    using System;
    using System.Collections.Generic;
    using MbUnit.Framework;

    public class HiveTestArgs<T, TSize, THive>
        where THive : IHive<T, TSize>
        where TSize : struct, IConvertible
    {
        private readonly THive _hive;
        private readonly List<T> _expectedItems;

        public HiveTestArgs(THive hive, IEnumerable<T> expectedItems)
        {
            _hive = hive;
            _expectedItems = new List<T>(expectedItems);
        }

        public THive Hive
        {
            get { return _hive; }
        }

        public IList<T> ExpectedItems
        {
            get { return _expectedItems; }
        }
    }

    public abstract class HiveTestsBase<T, TSize, THive>
        where THive : IHive<T, TSize>
        where TSize : struct, IConvertible
    {
        protected readonly ISizeOperations<TSize> Size = SizeOperations<TSize>.Default;

        [Test]
        public void IsEmptyWhenHiveContainsNoItems
            (HiveTestArgs<T, TSize, THive> args)
        {
            Assert.AreEqual(args.ExpectedItems.Count == 0, args.Hive.IsEmpty);
        }

        [Test]
        public void TryGetCountReturnsMinusOneWhenNumberOfItemsInHiveIsUnknown
            (HiveTestArgs<T, TSize, THive> args)
        {
            TSize count;
            if (!args.Hive.TryGetCount(out count))
            {
                Assert.AreEqual(-1, count);
            }
        }

        [Test]
        public void TryGetCountReturnsItemCountWhenNumberOfItemsInHiveIsKnown
            (HiveTestArgs<T, TSize, THive> args)
        {
            TSize count;
            if (args.Hive.TryGetCount(out count))
            {
                Assert.AreEqual(args.ExpectedItems.Count, count);
            }
        }

        [Test]
        public void GetPropertyReturnsDefaultIfPropertyTypeIsNotSupported
            (HiveTestArgs<T, TSize, THive> args)
        {
            Assert.AreEqual(default(bool), args.Hive.GetProperty<bool>(), "GetProperty<bool>()");
            Assert.AreEqual(default(object), args.Hive.GetProperty<object>(), "GetProperty<object>()");
        }

        protected static void IgnoreTestIfHiveIsReadOnly(IHive<T> hive)
        {
            if (hive.IsReadOnly)
            {
                Assert.Ignore("Not applicable to read-only collections.");
            }
        }
    }
}
