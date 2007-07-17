namespace NHive
{
    using System;
    using System.Runtime.Serialization;
    
    [Serializable]
    public class HiveException : Exception
    {
        public HiveException() 
        { }
        
        public HiveException(string message) 
            : base(message) 
        { }
        
        public HiveException(string message, Exception inner) 
            : base(message, inner) 
        { }
        
        protected HiveException(SerializationInfo info, StreamingContext context)
            : base(info, context) 
        { }
    }
}
