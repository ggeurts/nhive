namespace NHive.Collections
{
    using NHive.Collections.Base;
    using NHive.Core.Size;
using System.Collections.Generic;

    public class HashSet32<T>
        : HashSetBase<T, int, Int32Operations>
    {
        public HashSet32()
            : base()
        { }

        public HashSet32(IEqualityComparer<T> itemEqualityComparer)
            : base(itemEqualityComparer)
        { }

        /// <summary>
        /// Create a hash set with external item equalityComparer and default fill threshold (66%)
        /// </summary>
        /// <param name="capacity">Initial table size (rounded to power of 2, at least 16)</param>
        /// <param name="itemEqualityComparer">The external item equalityComparer</param>
        public HashSet32(int capacity, IEqualityComparer<T> itemEqualityComparer)
            : base(capacity, itemEqualityComparer) 
        { }

        /// <summary>
        /// Create a hash set with external item equalityComparer.
        /// </summary>
        /// <param name="capacity">Initial table size (rounded to power of 2, at least 16)</param>
        /// <param name="fillFactor">Fill threshold (in range 10% to 90%)</param>
        /// <param name="itemEqualityComparer">The external item equalityComparer</param>
        public HashSet32(int capacity, double fillFactor, IEqualityComparer<T> itemEqualityComparer)
            : base(capacity, fillFactor, itemEqualityComparer)
        { }
    }

    public class HashSet64<T>
        : HashSetBase<T, long, Int64Operations>
    {
        public HashSet64()
            : base()
        { }

        public HashSet64(IEqualityComparer<T> itemEqualityComparer)
            : base(itemEqualityComparer)
        { }

        /// <summary>
        /// Create a hash set with external item equalityComparer and default fill threshold (66%)
        /// </summary>
        /// <param name="capacity">Initial table size (rounded to power of 2, at least 16)</param>
        /// <param name="itemEqualityComparer">The external item equalityComparer</param>
        public HashSet64(long capacity, IEqualityComparer<T> itemEqualityComparer)
            : base(capacity, itemEqualityComparer) 
        { }

        /// <summary>
        /// Create a hash set with external item equalityComparer.
        /// </summary>
        /// <param name="capacity">Initial table size (rounded to power of 2, at least 16)</param>
        /// <param name="fillFactor">Fill threshold (in range 10% to 90%)</param>
        /// <param name="itemEqualityComparer">The external item equalityComparer</param>
        public HashSet64(long capacity, double fillFactor, IEqualityComparer<T> itemEqualityComparer)
            : base(capacity, fillFactor, itemEqualityComparer)
        { }
    }
}
