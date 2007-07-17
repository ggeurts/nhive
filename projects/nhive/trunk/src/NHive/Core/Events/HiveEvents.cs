namespace NHive.Core.Events
{
    using System;

    /// <summary>
    /// Types of events that can be raised by collections.
    /// </summary>
    [Flags]
    public enum HiveEvents
    {
        /// <summary>No collection event types.</summary>
        None = 0x0,

        /// <summary>Item has been added to the collection. Insert, enqueue and push operations
        /// are also considered as item additions.</summary>
        Added = 0x1,

        /// <summary>Item has been removed from the collection. Dequeue and pop operations
        /// are also considered as item removals.</summary>
        Removing = 0x4,

        /// <summary>All collection event types.</summary>
        All = Added | Removing
    }
}