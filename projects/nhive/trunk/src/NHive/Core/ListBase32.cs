namespace NHive.Core
{
    using System.Collections.Generic;
    using NHive.Core.Size;

    public abstract class ListBase32<T>
        : ListBase<T, int, Int32Operations>, IList<T>
    {
        #region Constructor(s)

        protected ListBase32(IEqualityComparer<T> itemEqualityComparer)
            : base(itemEqualityComparer)
        { }

        #endregion
    }
}
