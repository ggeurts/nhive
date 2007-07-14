namespace NHive
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// An exception thrown by enumerators, range views etc. when accessed after 
    /// the underlying collection has been modified.
    /// </summary>
    [Serializable]
    public class HiveModifiedException : HiveException
    {
        public HiveModifiedException() 
        { }
        
        protected HiveModifiedException(SerializationInfo info, StreamingContext context)
            : base(info, context) 
        { }
    }
}
