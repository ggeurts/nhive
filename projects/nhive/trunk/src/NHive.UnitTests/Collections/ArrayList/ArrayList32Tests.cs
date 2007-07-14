namespace NHive.UnitTests
{
    using System;
    using System.Collections.Generic;
    using MbUnit.Framework;
    using NHive.Base.Size;
    using NHive.UnitTests;

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

    internal class ArrayList32TestArgs<T>
        : ListTestArgs<T, int, ArrayList<T>>
        , IRandomAccessIteratableTestArgs<T, int, ArrayList<T>, ArrayList<T>.Iterator>
    {
        public ArrayList32TestArgs(ArrayList<T> hive, IEnumerable<T> expectedItems)
            : base(hive, expectedItems)
        { }
    }

    internal class ArrayList32Factory
    {
        [Factory]
        public ArrayList32TestArgs<string> EmptyStringArrayList
        {
            get { return CreateTestArgs<string>(); }
        }

        [Factory]
        public ArrayList32TestArgs<string> NonEmptyStringArrayList
        {
            get { return CreateTestArgs("#1", "#two", "#tres", "#vier"); }
        }

        private ArrayList32TestArgs<T> CreateTestArgs<T>(params T[] items)
        {
            ArrayList<T> list = new ArrayList<T>();
            list.AddRange(items);
            return new ArrayList32TestArgs<T>(list, items);
        }
    }
}
