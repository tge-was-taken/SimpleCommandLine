using System;

namespace TGE.SimpleCommandLine
{
    [Serializable]
    public class MissingRequiredOptionException : Exception
    {
        public string Option { get; set; }

        public MissingRequiredOptionException() { }
        public MissingRequiredOptionException( string message ) : base( message ) { }
        public MissingRequiredOptionException( string message, Exception inner ) : base( message, inner ) { }
        protected MissingRequiredOptionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context ) : base( info, context ) { }
    }
}
