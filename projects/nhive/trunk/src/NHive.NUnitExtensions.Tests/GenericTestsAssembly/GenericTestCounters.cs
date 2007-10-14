namespace GenericTestsAssembly
{
    using System;
    using System.Threading;

    public static class GenericTestCounters
    {
        public static readonly Counter SetUp = new Counter(0);
        public static readonly Counter TearDown = new Counter(0);
        public static readonly Counter Test = new Counter(0);

        public static void ResetCounters()
        {
            Test.Reset();
            SetUp.Reset();
            TearDown.Reset();
        }

        public class Counter
        {
            private int _value;

            public Counter(int initialValue)
            {
                _value = initialValue;
            }

            public int Value
            {
                get { return Thread.VolatileRead(ref _value); }
            }

            public void Increment()
            {
                Interlocked.Increment(ref _value);
            }

            public void Reset()
            {
                Thread.VolatileWrite(ref _value, 0);
            }
        }
    }
}
