using System;
using System.Runtime.Serialization;

namespace GoogleAPI_Library.Exceptions
{
    /// <summary>
    /// Represents errors that occur during request to Google sheets.
    /// </summary>
    [Serializable]
    public class GoogleSheetsException : Exception
    {
        public GoogleSheetsException() { }

        public GoogleSheetsException(string message) : base(message) { }

        public GoogleSheetsException(string message, Exception inner) : base(message, inner) { }

        protected GoogleSheetsException(
            SerializationInfo info,
            StreamingContext context) : base(info, context) { }
    }
}