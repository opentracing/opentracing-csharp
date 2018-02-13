using System;

namespace OpenTracing
{
    /// <summary>
    /// This exception should be used when the provided format value is unknown or disallowed by the
    /// <see cref="ITracer"/>
    /// </summary>
    public class UnsupportedFormatException : OpenTracingException
    {
        public UnsupportedFormatException()
        {
        }

        public UnsupportedFormatException(string message)
            : base(message)
        {
        }

        public UnsupportedFormatException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
