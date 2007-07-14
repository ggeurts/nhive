namespace NHive.UnitTests.Factories
{
    public class ArrayList32TestArgs<T>
        : ListTestArgs<T, int, ArrayList<T>>
        , IRandomAccessIteratableTestArgs<T, int, ArrayList<T>, ArrayList<T>.Iterator>
        , IInputIteratableTestArgs<T, int, ArrayList<T>, ArrayList<T>.Iterator>
    {
        public ArrayList32TestArgs(ArrayList<T> hive, IEnumerable<T> expectedItems)
            : base(hive, expectedItems)
        { }
    }
}
