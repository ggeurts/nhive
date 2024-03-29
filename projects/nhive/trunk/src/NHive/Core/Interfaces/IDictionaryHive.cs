namespace NHive
{
    using System;
    using System.Collections.Generic;

    public interface IDictionaryHive<K, V, TSize> : IReadOnlyCollection<KeyValuePair<K, V>, TSize>
        where TSize : struct, IConvertible
    { }
}
