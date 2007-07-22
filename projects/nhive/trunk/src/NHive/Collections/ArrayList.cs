namespace NHive.Collections
{
    using System.Collections.Generic;
    using NHive.Collections.Base;
    using NHive.Core.Size;

    public class ArrayList32<T> 
        : ArrayListBase<T, int, Int32Operations>
        , IList<T>
    {
        public ArrayList32()
            : base()
        { }

        public ArrayList32(int capacity)
            : base(capacity)
        { }

       public ArrayList32(int capacity, IEqualityComparer<T> itemEqualityComparer)
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
