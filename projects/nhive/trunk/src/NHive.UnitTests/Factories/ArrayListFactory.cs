namespace NHive.UnitTests.Factories
{
    using MbUnit.Framework;
using System.Collections.Generic;

    public class ArrayListTestArgs<T>
        : ListTestArgs<T, int, ArrayList<T>>
        , IRandomAccessIterableTestArgs<T, int, ArrayList<T>, ArrayList<T>.Iterator>
    {
        public ArrayListTestArgs(ArrayList<T> hive, IEnumerable<T> expectedItems)
            : base(hive, expectedItems)
        { }
    }

    internal class ArrayListFactory
    {
        [Factory]
        public ArrayListTestArgs<string> EmptyStringArrayList
        {
            get { return CreateTestArgs<string>(); }
        }

        [Factory]
        public ArrayListTestArgs<string> NonEmptyStringArrayList
        {
            get { return CreateTestArgs("#1", "#two", "#tres", "#vier"); }
        }

        private ArrayListTestArgs<T> CreateTestArgs<T>(params T[] items)
        {
            ArrayList<T> list = new ArrayList<T>();
            list.AddRange(items);
            return new ArrayListTestArgs<T>(list, items);
        }
    }
}
