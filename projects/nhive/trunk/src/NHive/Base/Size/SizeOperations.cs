namespace NHive.Base.Size
{
    using System;

    /// <summary>
    /// Utility class that provides access to default <see cref="ISizeOperations{TSize}"/>
    /// implementation for type <typeparamref name="TSize"/>.
    /// </summary>
    /// <typeparam name="TSize">A signed or unsigned ordinal type that is used to measure the number 
    /// of items in a data structure or operation.</typeparam>
    public static class SizeOperations<TSize>
        where TSize: struct, IConvertible
    {
        private static ISizeOperations<TSize> _defaultInstance;

        public static ISizeOperations<TSize> Default
        {
            get 
            {
                if (_defaultInstance == null)
                {
                    switch (Convert.GetTypeCode(typeof(TSize)))
                    {
                        case TypeCode.Int32:
                            _defaultInstance = (ISizeOperations<TSize>) (object) new Int32Operations();
                            break;
                        case TypeCode.Int64:
                            _defaultInstance = (ISizeOperations<TSize>)(object) new Int64Operations();
                            break;
                        default:
                            throw new NotImplementedException(string.Format(
                                "Type '{0}' is not (yet) supported as size.", typeof(TSize).FullName));
                    }
                }
                return _defaultInstance;
            }
        }
    }
}
