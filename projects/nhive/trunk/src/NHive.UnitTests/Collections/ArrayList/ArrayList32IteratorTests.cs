namespace NHive.UnitTests
{
    using System;
    using NHive.Base.Size;
    using MbUnit.Framework;
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
}
