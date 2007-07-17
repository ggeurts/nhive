namespace NHive
{
    using System;

    public interface IHiveBehaviour
    {
        HiveSpeed CountSpeed { get; }
        HiveSpeed ContainsSpeed { get; }
        HiveProperties Properties { get; }
    }

    /// <summary>
    /// The symbolic characterization of the speed of lookups for a collection.
    /// The values may refer to worst-case, amortized and/or expected asymptotic 
    /// complexity with regards to the collection size.
    /// </summary>
    public enum HiveSpeed : short
    {
        /// <summary>
        /// Counting the collection with the <code>Count property</code> may not return
        /// (for a synthetic and potentially infinite collection).
        /// </summary>
        PotentiallyInfinite = 1,

        /// <summary>
        /// Lookup operations like <code>Contains(T item)</code> or the <code>Count</code>
        /// property may take time O(n), where n is the size of the collection.
        /// </summary>
        Linear = 2,

        /// <summary>
        /// Lookup operations like <code>Contains(T item)</code> or the <code>Count</code>
        /// property take time O(log n), where n is the size of the collection.
        /// </summary>
        Log = 3,

        /// <summary>
        /// Lookup operations like <code>Contains(T item)</code> or the <code>Count</code>
        /// property take time O(1),
        /// </summary>
        Constant = 4
    }

    [Flags]
    public enum HiveProperties
    {
        Unspecified = 0,
        ReadOnly = 0x1,
        Fixed = 0x2,
        Sorted = 0x10,
        Persistent = 0x20,
        ThreadSafe = 0x100
    }
}
