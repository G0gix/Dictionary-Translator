using System;

namespace CoreLibrary.Logger.Exceptions
{
    [Serializable]
    public class LoggerException : Exception
    {
        public LoggerException() { }
        public LoggerException(string message) : base(message) { }
        public LoggerException(string message, Exception inner) : base(message, inner) { }
        protected LoggerException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
