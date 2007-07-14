namespace NHive
{
    using System.Collections.Generic;
    using NHive.Base;
    using NHive.Base.Size;

    public class ArrayList<T> : ArrayListBase<T, int, Int32Operations>
    {
        public ArrayList()
            : base()
        { }

        public ArrayList(int capacity)
            : base(capacity)
        { }

       public ArrayList(int capacity, IEqualityComparer<T> itemEqualityComparer)
            : base(capacity, itemEqualityComparer)
        { }
    }

    public class ArrayList64<T> : ArrayListBase<T, long, Int64Operations>
    {
        public ArrayList64()
            : base()
        { }

        public ArrayList64(int capacity)
            : base(capacity)
        { }

        public ArrayList64(int capacity, IEqualityComparer<T> itemEqualityComparer)
            : base(capacity, itemEqualityComparer)
        { }
    }
}
