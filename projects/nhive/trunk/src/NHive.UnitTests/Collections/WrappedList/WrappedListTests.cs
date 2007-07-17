namespace NHive.UnitTests
{
    using System;
    using System.Collections.Generic;
    using MbUnit.Framework;
    using NHive.Collections;

    [TypeFixture(
        typeof(WrappedListTestArgs<string>))]
    [ProviderFactory(
        typeof(WrappedListFactory),
        typeof(WrappedListTestArgs<string>))]
    public class WrappedListTests 
        : ListTestsBase<string, int, WrappedList<string>>
    {
        private static string[] _items = new string[] { "item 1", "item 2", "item 3" };
        private static Random _random = new Random(0);

        protected override string CreateRandomItem()
        {
            return _items[_random.Next(_items.Length - 1)];
        }
    }

    [TypeFixture(
        typeof(WrappedListTestArgs<string>))]
    [ProviderFactory(
        typeof(WrappedListFactory), 
        typeof(WrappedListTestArgs<string>))]
    public class WrappedListIteratorTests
        : RandomAccessIteratableTestsBase<string, int, WrappedList<string>, WrappedList<string>.Iterator>
    { }

    internal class WrappedListTestArgs<T>
        : ListTestArgs<T, int, WrappedList<T>>
        , IRandomAccessIteratableTestArgs<T, int, WrappedList<T>, ArrayList32<T>.Iterator>
    {
        public WrappedListTestArgs(WrappedList<T> hive, IEnumerable<T> expectedItems)
            : base(hive, expectedItems)
        { }
    }

    internal class WrappedListFactory
    {
        [Factory]
        public WrappedListTestArgs<string> EmptyStringList
        {
            get { return CreateWrappedListTestArgs<string>(); }
        }

        [Factory]
        public WrappedListTestArgs<string> NonEmptyStringList
        {
            get { return CreateWrappedListTestArgs("#1", "#two", "#tres", "#vier"); }
        }

        [Factory]
        public WrappedListTestArgs<string> EmptyStringArray
        {
            get { return CreateWrappedArrayTestArgs<string>(); }
        }

        [Factory]
        public WrappedListTestArgs<string> NonEmptyStringArray
        {
            get { return CreateWrappedArrayTestArgs("#1", "#two", "#tres", "#vier"); }
        }

        private WrappedListTestArgs<T> CreateWrappedListTestArgs<T>(params T[] items)
        {
            WrappedList<T> wrappedList = new WrappedList<T>(
                new List<T>(items));
            return new WrappedListTestArgs<T>(wrappedList, items);
        }

        private WrappedListTestArgs<T> CreateWrappedArrayTestArgs<T>(params T[] items)
        {
            WrappedList<T> wrappedList = new WrappedList<T>(items);
            return new WrappedListTestArgs<T>(wrappedList, items);
        }
    }
}
