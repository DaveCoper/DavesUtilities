using System;
using System.Runtime.Serialization;

namespace DavesUtilities.Reflection
{
    [Serializable]
    internal class TragetPropertiesAreMissingException : Exception
    {
        public TragetPropertiesAreMissingException()
        {
        }

        public TragetPropertiesAreMissingException(string message) : base(message)
        {
        }

        public TragetPropertiesAreMissingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TragetPropertiesAreMissingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}