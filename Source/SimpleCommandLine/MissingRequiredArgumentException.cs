using System;

namespace SimpleCommandLine
{
    [Serializable]
    public class MissingRequiredArgumentException : Exception
    {
        public string Option { get; set; }

        public MissingRequiredArgumentException() { }
        public MissingRequiredArgumentException( string message ) : base( message ) { }
        public MissingRequiredArgumentException( string message, Exception inner ) : base( message, inner ) { }
        protected MissingRequiredArgumentException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context ) : base( info, context ) { }
    }
}
