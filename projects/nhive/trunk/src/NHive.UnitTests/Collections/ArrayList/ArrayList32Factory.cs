namespace NHive.UnitTests
{
    using MbUnit.Framework;

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
