namespace NHive.UnitTests
{
    using System;
    using System.Collections.Generic;
    using MbUnit.Framework;

    public interface IInputIteratableTestArgs<T, TSize, THive, TIterator>
        where THive : IInputIteratable<T, TSize, TIterator>
        where TIterator : struct, IInputIterator<T, TSize, TIterator>
        where TSize: struct, IConvertible
    {
        THive Hive { get; }
        IList<T> ExpectedItems { get; }
    }

    public abstract class InputIteratableTestsBase<T, TSize, THive, TIterator>
        where THive : IInputIteratable<T, TSize, TIterator>
        where TIterator : struct, IInputIterator<T, TSize, TIterator>
        where TSize : struct, IConvertible
    {
        [Test]
        public void HiveIsParentOfBeginIterator(
            IInputIteratableTestArgs<T, TSize, THive, TIterator> args)
        {
            Assert.AreEqual(args.Hive, args.Hive.Begin.Parent);
        }

        [Test]
        public void HiveIsParentOfEndIterator(
            IInputIteratableTestArgs<T, TSize, THive, TIterator> args)
        {
            Assert.AreEqual(args.Hive, args.Hive.End.Parent);
        }

        [Test]
        public void BeginAndEndIteratorsHaveSameType(
            IInputIteratableTestArgs<T, TSize, THive, TIterator> args)
        {
            Assert.AreEqual(args.Hive.Begin.GetType(), args.Hive.End.GetType());
        }

        [Test]
        public void IteratorIsValueType(
            IInputIteratableTestArgs<T, TSize, THive, TIterator> args)
        {
            Assert.IsTrue(typeof(TIterator).IsValueType);
        }

        [Test]
        public void IterateFromBeginToEnd(
            IInputIteratableTestArgs<T, TSize, THive, TIterator> args)
        {
            IEnumerator<T> expectedItems = args.ExpectedItems.GetEnumerator();
            for (TIterator i = args.Hive.Begin; !i.Equals(args.Hive.End); i.Increment())
            {
                if (!expectedItems.MoveNext())
                {
                    Assert.Fail("Iterator returns too many items.");
                }
                else
                {
                    Assert.AreEqual(expectedItems.Current, i.Read());
                }
            }

            if (expectedItems.MoveNext())
            {
                Assert.Fail("Iterator returns not all items.");
            }
        }
    }
}
