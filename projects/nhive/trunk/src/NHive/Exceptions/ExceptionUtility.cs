namespace NHive
{
    using System;
    
    internal static class ExceptionUtility
    {
        public static void ThrowIfInvalidIndex(int index, int maxIndex)
        {
            if (index < 0 || index >= maxIndex)
            {
                throw new ArgumentOutOfRangeException(string.Format(
                    "Index must be in range '0..{0}'.", maxIndex));
            }
        }
    }
}
