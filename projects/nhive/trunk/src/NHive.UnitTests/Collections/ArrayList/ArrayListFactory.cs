namespace NHive.UnitTests.Factories
{
    using MbUnit.Framework;
    using System.Collections.Generic;
    using NHive.Base;
    using NHive.Base.Size;

    public class ArrayList32TestArgs<T>
        : ListTestArgs<T, int, ArrayList<T>>
        , IRandomAccessIteratableTestArgs<T, int, ArrayList<T>, ArrayList<T>.Iterator>
        , IInputIteratableTestArgs<T, int, ArrayList<T>, ArrayList<T>.Iterator>
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
